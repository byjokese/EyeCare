using System;
using System.IO;
using System.Windows;
using File = System.IO.File;

namespace EyeCarePC {
	/// <summary>
	/// Interaction logic for SettingsWindow.xaml
	/// </summary>
	public partial class SettingsWindow : Window {
		public SettingsWindow() {
			InitializeComponent();
			//Import Settings
			Properties.Settings.Default.Reload();
			GeneralDisable.IsChecked = Properties.Settings.Default.disabled;
			FullscreenDisable.IsChecked = Properties.Settings.Default.disableOnFullScreen;
			ForceBreaks.IsChecked = Properties.Settings.Default.forceNoClose;
			PlayAudios.IsChecked = Properties.Settings.Default.audios;
			RunAtStart.IsChecked = Properties.Settings.Default.runStartUp;
			MultiMonitor.IsChecked = Properties.Settings.Default.showOnAllMonitors;

			ShortHours.Text = Properties.Settings.Default.shortBreaksInterval.Hours.ToString();
			ShortMinutes.Text = Properties.Settings.Default.shortBreaksInterval.Minutes.ToString();
			ShortSeconds.Text = Properties.Settings.Default.shortBreaksInterval.Seconds.ToString();
			if (Properties.Settings.Default.shortBreakDuration.Seconds != 00) {
				ShortDuration.Text = Properties.Settings.Default.shortBreakDuration.Seconds.ToString();
				ShortDurationUnits.SelectedIndex = 0;
			} else if (Properties.Settings.Default.shortBreakDuration.Minutes != 00) {
				ShortDuration.Text = Properties.Settings.Default.shortBreakDuration.Minutes.ToString();
				ShortDurationUnits.SelectedIndex = 1;
			}
			LongHours.Text = Properties.Settings.Default.longBreaksInterval.Hours.ToString();
			LongMinutes.Text = Properties.Settings.Default.longBreaksInterval.Minutes.ToString();
			LongSeconds.Text = Properties.Settings.Default.longBreaksInterval.Seconds.ToString();
			if (Properties.Settings.Default.longBreakDuration.Seconds != 00) {
				LongDuration.Text = Properties.Settings.Default.longBreakDuration.Seconds.ToString();
				LongDurationUnits.SelectedIndex = 0;
			} else if (Properties.Settings.Default.longBreakDuration.Minutes != 00) {
				LongDuration.Text = Properties.Settings.Default.longBreakDuration.Minutes.ToString();
				LongDurationUnits.SelectedIndex = 1;
			}
		}

		private void Grid_Loaded(object sender, RoutedEventArgs e) {

		}

		protected override void OnClosing(System.ComponentModel.CancelEventArgs e) {
			InputError.Content = ""; //Clear Error messages as no save has been done in wrong input.		
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e) {
			InputError.Content = "";//Clear previous Error Messeges
			Properties.Settings.Default.disabled = (bool)GeneralDisable.IsChecked;
			Properties.Settings.Default.disableOnFullScreen = (bool)FullscreenDisable.IsChecked;
			Properties.Settings.Default.forceNoClose = (bool)ForceBreaks.IsChecked;
			Properties.Settings.Default.audios = (bool)PlayAudios.IsChecked;
			Properties.Settings.Default.runStartUp = (bool)RunAtStart.IsChecked;
			Properties.Settings.Default.showOnAllMonitors = (bool)MultiMonitor.IsChecked;
			//Update Run on Start preference
			if ((bool)RunAtStart.IsChecked)
				SetupRunAtStartUp();
			else
				RemoveRunAtStartUp();
			//Short break interval
			Int32 shortHours, shortMinutes, shortSeconds;
			if (!Int32.TryParse(ShortHours.Text, out shortHours)) {
				InputError.Content = "Check Hours in Sort break Interval, must be a valid number";
				ShortHours.Focus();
				return;
			}
			if (!Int32.TryParse(ShortMinutes.Text, out shortMinutes)) {
				InputError.Content = "Check Minutes in Sort break Interval, must be a valid number";
				ShortMinutes.Focus();
				return;
			}
			if (!Int32.TryParse(ShortSeconds.Text, out shortSeconds)) {
				InputError.Content = "Check Seconds in Sort break Interval, must be a valid number";
				ShortSeconds.Focus();
				return;
			}
			Properties.Settings.Default.shortBreaksInterval = new TimeSpan(shortHours, shortMinutes, shortSeconds);
			//Short Duration
			TimeSpan shortDurationSpan = new TimeSpan(0, 0, 20); //Default Vale in case of error.
			Int32 shortDurationInt;
			if (!Int32.TryParse(ShortDuration.Text, out shortDurationInt)) {
				InputError.Content = "Check Sort break Duration, must be a valid number";
				return;
			}
			switch (ShortDurationUnits.SelectedIndex) {
				case 0:
					shortDurationSpan = new TimeSpan(0, 0, shortDurationInt);
					break;
				case 1:
					shortDurationSpan = new TimeSpan(0, shortDurationInt, 0);
					break;
			}
			Properties.Settings.Default.shortBreakDuration = shortDurationSpan;
			//Long break interval
			Int32 longHours, longMinutes, longSeconds;
			if (!Int32.TryParse(LongHours.Text, out longHours)) {
				InputError.Content = "Check Hours in Long break Interval, must be a valid number";
				LongHours.Focus();
				return;
			}
			if (!Int32.TryParse(LongMinutes.Text, out longMinutes)) {
				InputError.Content = "Check Minutes in Long break Interval, must be a valid number";
				ShortSeconds.Focus();
				return;
			}
			if (!Int32.TryParse(LongSeconds.Text, out longSeconds)) {
				InputError.Content = "Check Seconds in Long break Interval, must be a valid number";
				LongSeconds.Focus();
				return;
			}
			Properties.Settings.Default.longBreaksInterval = new TimeSpan(longHours, longMinutes, longSeconds);
			//Long Duration
			TimeSpan longDurationSpan = new TimeSpan(0, 3, 0); //Default Vale in case of error.
			Int32 longDurationInt;
			if (!Int32.TryParse(LongDuration.Text, out longDurationInt)) {
				InputError.Content = "Check Long break Duration, must be a valid number";
				return;
			}
			switch (LongDurationUnits.SelectedIndex) {
				case 0:
					longDurationSpan = new TimeSpan(0, 0, longDurationInt);
					break;
				case 1:
					longDurationSpan = new TimeSpan(0, longDurationInt, 0);
					break;
				case 2:
					longDurationSpan = new TimeSpan(longDurationInt, 0, 0);
					break;
			}
			Properties.Settings.Default.longBreakDuration = longDurationSpan;
			//Save All Settings
			Properties.Settings.Default.Save();
		}

		private void RestoreButton_Click(object sender, RoutedEventArgs e) {
			InputError.Content = "";//Clear previous Error Messeges
									//Set the UI to default values
			GeneralDisable.IsChecked = false;
			FullscreenDisable.IsChecked = true;
			ForceBreaks.IsChecked = false;
			PlayAudios.IsChecked = false;
			RunAtStart.IsChecked = false;
			MultiMonitor.IsChecked = true;
			ShortHours.Text = "0";
			ShortMinutes.Text = "40";
			ShortSeconds.Text = "0";
			ShortDuration.Text = "20";
			ShortDurationUnits.SelectedIndex = 0;
			LongHours.Text = "0";
			LongMinutes.Text = "40";
			LongSeconds.Text = "0";
			LongDuration.Text = "3";
			LongDurationUnits.SelectedIndex = 1;

			Properties.Settings.Default.Reset(); //Reset Settings											 
			Properties.Settings.Default.Save(); //Save All Settings
		}

		private void SetupRunAtStartUp() {
			//create shortcut to file in startup
			IWshRuntimeLibrary.WshShell wsh = new IWshRuntimeLibrary.WshShell();
			IWshRuntimeLibrary.IWshShortcut shortcut = wsh.CreateShortcut(
				Environment.GetFolderPath(Environment.SpecialFolder.Startup) + @"\EyeCare.lnk") as IWshRuntimeLibrary.IWshShortcut;
			shortcut.Arguments = "";
			shortcut.TargetPath = Environment.CurrentDirectory + @"\EyeCare.exe";
			shortcut.WindowStyle = 1;
			shortcut.Description = "EyeCare Shortcut";
			shortcut.WorkingDirectory = Environment.CurrentDirectory + @"\";
			shortcut.IconLocation = Path.Combine(Environment.CurrentDirectory, "/Resource/favicon.ico");
			shortcut.Save();
		}

		private void RemoveRunAtStartUp() {
			var startUpFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
			if (File.Exists(Path.Combine(startUpFolder, @"\EyeCare.lnk"))) {
				File.Delete(Path.Combine(startUpFolder, @"\EyeCare.lnk"));
			}
		}
	}
}
