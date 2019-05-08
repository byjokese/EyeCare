﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
using System.Xml;
using Application = System.Windows.Application;
using ContextMenu = System.Windows.Forms.ContextMenu;
using MenuItem = System.Windows.Forms.MenuItem;

namespace EyeCarePC {
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application {

		[DllImport("user32.dll")]
		private static extern IntPtr GetForegroundWindow();
		[DllImport("user32.dll")]
		private static extern IntPtr GetDesktopWindow();
		[DllImport("user32.dll")]
		private static extern IntPtr GetShellWindow();
		[DllImport("user32.dll", SetLastError = true)]
		private static extern int GetWindowRect(IntPtr hwnd, out RECT rc);

		public struct RECT {
			public int Left;
			public int Top;
			public int Right;
			public int Bottom;
		}
		public enum BreakType { SHORT, LONG };

		private IntPtr desktopHandle;
		private IntPtr shellHandle;

		private const String APP_ID = "By-Jokese.EyeCare";

		public NotifyIcon notifyIcon;
		private SettingsWindow settingsWindow;
		private EyeCareWindow eyeCareWindow;
		private List<EyeCareWindow> eyeCareWindows;
		private DispatcherTimer shortEyeCareWindowTimmer;
		private DispatcherTimer longEyeCareWindowTimmer;
		private SoundPlayer soundPlayer;

		public App() {
			//Get the handles for Shell and the Desktop as they wont change unless someting nasting comes from Shell.
			desktopHandle = GetDesktopWindow();
			shellHandle = GetShellWindow();

			eyeCareWindows = new List<EyeCareWindow>();
			settingsWindow = new SettingsWindow();
			eyeCareWindow = new EyeCareWindow(BreakType.LONG, true);
			soundPlayer = new SoundPlayer();

			#region NotifyIcon Setup
			notifyIcon = new NotifyIcon();
			notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetEntryAssembly().ManifestModule.Name);
			notifyIcon.Visible = true;
			notifyIcon.DoubleClick +=
				delegate (object sender, EventArgs args) {
					settingsWindow.Show();
					settingsWindow.WindowState = WindowState.Normal;
				};
			notifyIcon.MouseDown +=
				delegate (object sender, MouseEventArgs args) {
					notifyIcon.ContextMenu.MenuItems[0].Checked = EyeCarePC.Properties.Settings.Default.disabled;
				};
			ContextMenu contextMenu = new ContextMenu();
			MenuItem menuItem1 = new MenuItem {
				Text = "&Open Settings"
			};
			menuItem1.Click += new EventHandler(this.MenuItem1_Clicked);
			MenuItem closeMenu = new MenuItem {
				Text = "E&xit"
			};
			MenuItem disableMenu = new MenuItem {
				Text = "&Disable"
			};
			disableMenu.Checked = EyeCarePC.Properties.Settings.Default.disabled;
			disableMenu.Click += new EventHandler(this.DisableMenu_Clicked);
			closeMenu.Click += new EventHandler(this.MenuItemClose_Clicked);
			contextMenu.MenuItems.Add(disableMenu);
			contextMenu.MenuItems.Add(menuItem1);
			contextMenu.MenuItems.Add(closeMenu);
			notifyIcon.ContextMenu = contextMenu;
			#endregion
			#region Timmers Setup
			//Short Brake
			shortEyeCareWindowTimmer = new DispatcherTimer();
			shortEyeCareWindowTimmer.Interval = EyeCarePC.Properties.Settings.Default.shortBreaksInterval;//new TimeSpan(0, 0, 30);
			shortEyeCareWindowTimmer.Tick += (_, a) => {
				if (!(EyeCarePC.Properties.Settings.Default.disabled || IsUserOnFullScreenMode() || IsEyeCareAlreadyOpen())) {
					PrepareAndShowWindows(BreakType.SHORT);
				} else {
					//Toast notification??
				}
			};
			//Long Brake
			longEyeCareWindowTimmer = new DispatcherTimer();
			longEyeCareWindowTimmer.Interval = EyeCarePC.Properties.Settings.Default.longBreaksInterval;//new TimeSpan(0, 1, 0);
			longEyeCareWindowTimmer.Tick += (_, a) => {
				if (!(EyeCarePC.Properties.Settings.Default.disabled || IsUserOnFullScreenMode() || IsEyeCareAlreadyOpen())) {
					if (EyeCarePC.Properties.Settings.Default.audios) {
						PrepareAndShowWindows(BreakType.LONG);
					}
					eyeCareWindow.Show();
				} else {
					//Toast notification??
				}
			};
			//Start All Timmers
			shortEyeCareWindowTimmer.Start();
			longEyeCareWindowTimmer.Start();
			#endregion
		}

		private void PrepareAndShowWindows(BreakType breakType) {
			//Clear previous Windows
			eyeCareWindows.Clear();
			foreach (Screen screen in Screen.AllScreens) {
				//Play audios only on Primary Window to avoid audio repetition
				if (screen.Primary) {
					eyeCareWindow = new EyeCareWindow(breakType, false);//Primary Window does not get muted
					eyeCareWindow.WindowStartupLocation = WindowStartupLocation.Manual;
					eyeCareWindow.Left = screen.WorkingArea.Left + (screen.Bounds.Width / 2) - (eyeCareWindow.Width / 2);
					eyeCareWindow.Top = screen.WorkingArea.Top + (screen.Bounds.Height / 2) - (eyeCareWindow.Height / 2);
					eyeCareWindow.WindowState = WindowState.Normal;
					eyeCareWindows.Add(eyeCareWindow);
					eyeCareWindow.Show();
					if (EyeCarePC.Properties.Settings.Default.audios) {
						PlayStartAudio(BreakType.SHORT);
					}
					//With Multimonitor disabed, skip the rest of the screens
					if (!EyeCarePC.Properties.Settings.Default.showOnAllMonitors)
						break;
				} else {
					EyeCareWindow otherEyeCare = new EyeCareWindow(breakType, true);//Mute all other windows
					otherEyeCare.WindowStartupLocation = WindowStartupLocation.Manual;
					otherEyeCare.Show();
					otherEyeCare.Left = screen.WorkingArea.Left + (screen.Bounds.Width / 2) - (otherEyeCare.Width / 2);
					otherEyeCare.Top = screen.WorkingArea.Top + (screen.Bounds.Height / 2) - (otherEyeCare.Height / 2);
					otherEyeCare.WindowState = WindowState.Normal;
					eyeCareWindows.Add(otherEyeCare);
				}
			}
		}

		private bool IsUserOnFullScreenMode() {
			//Get the dimensions of the active window
			IntPtr hWnd = GetForegroundWindow();
			RECT appBounds;
			Rectangle screenBounds;
			bool runningFullScreen = false;
			if (hWnd != null && !hWnd.Equals(IntPtr.Zero)) {
				//Check we haven't picked up the desktop or the shell
				if (!(hWnd.Equals(desktopHandle) || hWnd.Equals(shellHandle))) {
					GetWindowRect(hWnd, out appBounds);
					//Determine if window is fullscreen
					screenBounds = Screen.FromHandle(hWnd).Bounds;
					if ((appBounds.Bottom - appBounds.Top) == screenBounds.Height && (appBounds.Right - appBounds.Left) == screenBounds.Width) {
						runningFullScreen = true;
					}
				}
			}
			return runningFullScreen;
		}
		private bool IsEyeCareAlreadyOpen() {
			if (eyeCareWindow == null)
				return false;
			return (eyeCareWindow.IsActive) ? true : false;
		}
		private void PlayStartAudio(BreakType breakType) {
			switch (breakType) {
				case BreakType.SHORT:
					soundPlayer.Stream = EyeCarePC.Properties.Resources.eyes_start;
					break;
				case BreakType.LONG:
					soundPlayer.Stream = EyeCarePC.Properties.Resources.eyes_start_long; //TODO Change audio to long break
					break;
			}
			soundPlayer.Load();
			if (soundPlayer.IsLoadCompleted) {
				soundPlayer.Play();
			}
		}

		private void DisableMenu_Clicked(object sender, EventArgs e) {
			notifyIcon.ContextMenu.MenuItems[0].Checked = !notifyIcon.ContextMenu.MenuItems[0].Checked;
			//Save new Status
			EyeCarePC.Properties.Settings.Default.disabled = notifyIcon.ContextMenu.MenuItems[0].Checked;
			EyeCarePC.Properties.Settings.Default.Save();
		}
		private void MenuItem1_Clicked(object sender, EventArgs e) {
			settingsWindow = new SettingsWindow();
			settingsWindow.Show();
		}
		private void MenuItemClose_Clicked(object sender, EventArgs e) {
			System.Environment.Exit(0);
		}
	}
}
