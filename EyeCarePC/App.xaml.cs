using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Threading;
using Application = System.Windows.Application;
using System.Windows.Threading;
using ContextMenu = System.Windows.Forms.ContextMenu;
using MenuItem = System.Windows.Forms.MenuItem;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Media;
using System.Media;

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

		public NotifyIcon notifyIcon;
		private SettingsWindow settingsWindow;
		private EyeCareShortWindow eyeCareShortWindow;
		private DispatcherTimer shortEyeCareWindowTimmer;
		private DispatcherTimer longEyeCareWindowTimmer;
		private SoundPlayer soundPlayer;

		public App() {
			//Get the handles for Shell and the Desktop as they wont change unless someting nasting comes from Shell.
			desktopHandle = GetDesktopWindow();
			shellHandle = GetShellWindow();

			settingsWindow = new SettingsWindow();
			eyeCareShortWindow = new EyeCareShortWindow(BreakType.LONG);
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
			shortEyeCareWindowTimmer.Interval = new TimeSpan(0, 0, 30); //TODO change on realese
			shortEyeCareWindowTimmer.Tick += (_, a) => {
				if(!(EyeCarePC.Properties.Settings.Default.disabled || IsUserOnFullScreenMode() || IsEyeCareAlreadyOpen())) {
					eyeCareShortWindow = new EyeCareShortWindow(BreakType.SHORT);
					if (EyeCarePC.Properties.Settings.Default.audios) {
						PlayStartAudio(BreakType.SHORT);
						//Wait for audio
						Thread.Sleep(2000);
					}					
					eyeCareShortWindow.Show();
				} else {
					//Toast notification??
				}
			};
			//Long Brake
			longEyeCareWindowTimmer = new DispatcherTimer();
			longEyeCareWindowTimmer.Interval = new TimeSpan(0,1,0);  //TODO change on realese
			longEyeCareWindowTimmer.Tick += (_, a) => {
				if (!(EyeCarePC.Properties.Settings.Default.disabled || IsUserOnFullScreenMode() || IsEyeCareAlreadyOpen())) {
					eyeCareShortWindow = new EyeCareShortWindow(BreakType.LONG);
					if (EyeCarePC.Properties.Settings.Default.audios) {
						PlayStartAudio(BreakType.LONG);
						//Wait for audio
						Thread.Sleep(2000);
					}
					eyeCareShortWindow.Show();
				} else {
					//Toast notification??
				}
			};
			//Start All Timmers
			shortEyeCareWindowTimmer.Start();
			longEyeCareWindowTimmer.Start();
			#endregion
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
			if (eyeCareShortWindow == null)
				return false;
			return (eyeCareShortWindow.IsActive) ? true : false;
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
