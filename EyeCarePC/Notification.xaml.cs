using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EyeCarePC {
	/// <summary>
	/// Interaction logic for Notification.xaml
	/// </summary>
	public partial class Notification : Window {
		public string NotificationTitle { get; set; }
		public string NotificationDescription { get; set; }
		public Notification() {
			InitializeComponent();
			this.DataContext = this;
			this.WindowStartupLocation = WindowStartupLocation.Manual;
			this.Top = SystemParameters.WorkArea.Right - this.Height;
			this.Left = SystemParameters.WorkArea.Bottom - this.Width;
			this.Top = 1500;
			this.Left = 1200;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e) {
			//this.Top = 1500;
			//this.Left = 1200;
		}
	}
}
