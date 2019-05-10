using System;
using System.Windows;
using System.Windows.Threading;

namespace EyeCarePC {
	/// <summary>
	/// Interaction logic for Notification.xaml
	/// </summary>
	public partial class Notification : Window {
		public string NotificationTitle { get; set; }
		public string NotificationDescription { get; set; }
		public Notification() {
			InitializeComponent();
			DataContext = this;
			WindowStartupLocation = WindowStartupLocation.Manual;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e) {
			Left = System.Windows.SystemParameters.WorkArea.Width - Width - 30;
			Top = System.Windows.SystemParameters.WorkArea.Height - Height - 30;
			System.Media.SystemSounds.Beep.Play();
			//Close Timmer
			DispatcherTimer closeTimmer = new DispatcherTimer {
				Interval = new TimeSpan(0, 0, 6)
			};
			closeTimmer.Tick += (_, a) => {
				Close();
			};
		}

		private void CloseBtn_Click(object sender, RoutedEventArgs e) {
			Close();
		}
	}
}
