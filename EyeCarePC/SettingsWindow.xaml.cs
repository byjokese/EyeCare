using System;
using System.Windows;

namespace EyeCarePC {
	/// <summary>
	/// Interaction logic for SettingsWindow.xaml
	/// </summary>
	public partial class SettingsWindow : Window {
		public SettingsWindow() {
			InitializeComponent();
			//Import Settings
			Properties.Settings.Default.Reload();
			generalDisable.IsChecked = Properties.Settings.Default.disabled;
			fullscreenDisable.IsChecked = Properties.Settings.Default.disableOnFullScreen;
			forceBreaks.IsChecked = Properties.Settings.Default.forceNoClose;
			playAudios.IsChecked = Properties.Settings.Default.audios;
			RunAtStart.IsChecked = Properties.Settings.Default.runStartUp;

			ShortHours.Text = Properties.Settings.Default.shortBreaks.Hours.ToString();
			ShortMinutes.Text = Properties.Settings.Default.shortBreaks.Minutes.ToString();
			ShortSeconds.Text = Properties.Settings.Default.shortBreaks.Seconds.ToString();
			if(Properties.Settings.Default.shortBreakDuration.Seconds != 00) {
				ShortDuration.Text = Properties.Settings.Default.shortBreakDuration.Seconds.ToString();
				ShortDurationUnits.SelectedIndex = 0;
			} else if (Properties.Settings.Default.shortBreakDuration.Minutes != 00) {
				ShortDuration.Text = Properties.Settings.Default.shortBreakDuration.Minutes.ToString();
				ShortDurationUnits.SelectedIndex = 1;
			}
			LongHours.Text = Properties.Settings.Default.longBreaks.Hours.ToString();
			LongMinutes.Text = Properties.Settings.Default.longBreaks.Minutes.ToString();
			LongSeconds.Text = Properties.Settings.Default.longBreaks.Seconds.ToString();
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
			Properties.Settings.Default.disabled = (bool)generalDisable.IsChecked;
			Properties.Settings.Default.disableOnFullScreen = (bool)fullscreenDisable.IsChecked;
			Properties.Settings.Default.forceNoClose = (bool)forceBreaks.IsChecked;
			Properties.Settings.Default.audios = (bool)playAudios.IsChecked;
			Properties.Settings.Default.runStartUp = (bool)RunAtStart.IsChecked;
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
			Properties.Settings.Default.shortBreaks = new TimeSpan(shortHours, shortMinutes, shortSeconds);
			//Short Duration
			TimeSpan shortDurationSpan = new TimeSpan(0,0,20); //Default Vale in case of error.
			Int32 shortDurationInt;
			if (!Int32.TryParse(ShortDuration.Text, out shortDurationInt)) {
				InputError.Content = "Check Sort break Duration, must be a valid number";
				return;
			}
			switch (ShortDurationUnits.SelectedIndex) {
				case 0:
					shortDurationSpan = new TimeSpan(0,0, shortDurationInt);
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
			Properties.Settings.Default.longBreaks = new TimeSpan(longHours, longMinutes, longSeconds);
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
			//Set the form to default values
			generalDisable.IsChecked = false;
			fullscreenDisable.IsChecked = true;
			forceBreaks.IsChecked = false;
			playAudios.IsChecked = false;
			RunAtStart.IsChecked = false;
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

		private void setupRunAtStartUp(){

			IWshRuntimeLibrary.WshShell wsh = new IWshRuntimeLibrary.WshShell();
			IWshRuntimeLibrary.IWshShortcut shortcut = wsh.CreateShortcut(
				Environment.GetFolderPath(Environment.SpecialFolder.Startup) + @"\program.lnk") as IWshRuntimeLibrary.IWshShortcut;
			shortcut.Arguments = "";
			shortcut.TargetPath = Environment.CurrentDirectory + @"\program.exe";
			shortcut.WindowStyle = 1;
			shortcut.Description = "program";
			shortcut.WorkingDirectory = Environment.CurrentDirectory + @"\";
			//shortcut.IconLocation = "specify icon location";
			shortcut.Save();
		}
	}
}
