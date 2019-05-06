using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace EyeCarePC {
	/// <summary>
	/// Interaction logic for EyeCareShortWindow.xaml
	/// </summary>
	
	public partial class EyeCareShortWindow : Window {
		private enum Movement { LEFT, RIGHT, UP, DOWN, CENTER };
		private Movement LastMove { get; set; }
		private int MovementCount { get; set; }
		private TimeSpan CountDown { get; set; }
		private Thread animationThreath;
		private SoundPlayer soundLeftRight;
		private SoundPlayer soundUpDown;
		private SoundPlayer soundEnd;
		private App.BreakType Breaktype { get; set; }

		public EyeCareShortWindow(App.BreakType breakType) {
			InitializeComponent();
			Breaktype = breakType;
			CloseBtn.Visibility = (Properties.Settings.Default.forceNoClose) ? Visibility.Hidden : Visibility.Visible;
			switch (Breaktype) {
				case App.BreakType.SHORT:
					LongBreakImage.Visibility = Visibility.Collapsed;
					//Get Break Duration
					CountDown = Properties.Settings.Default.shortBreakDuration;
					//Audio Setup
					if (Properties.Settings.Default.audios) {
						soundLeftRight = new SoundPlayer(Properties.Resources.eyes_left_right);
						soundUpDown = new SoundPlayer(Properties.Resources.eyes_up_down);
						soundEnd = new SoundPlayer(Properties.Resources.eyes_end_short);
						soundLeftRight.Load();
						soundUpDown.Load();
						soundEnd.Load();
					}
				break;
				case App.BreakType.LONG:
					EyeCanvas.Visibility = Visibility.Collapsed;
					//Get Break Duration
					CountDown = Properties.Settings.Default.longBreakDuration;
					//Audio Setup
					if (Properties.Settings.Default.audios) {
						soundEnd = new SoundPlayer(Properties.Resources.eyes_end_long);
						soundEnd.Load();
					}
					break;
			};
			TimeLeft.Content = CountDown;
			LastMove = Movement.LEFT;
			MovementCount = 1;
			CenterEyes();
		}

		private void CloseButton_Click(object sender, RoutedEventArgs e) {
			this.Close();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e) {
			animationThreath = new Thread(StartAnimation);
			animationThreath.Start();
		}

		private void Countdown(int count, TimeSpan interval, Action<int> ts) {
			//Thread.Sleep(2000);	
			var dt = new System.Windows.Threading.DispatcherTimer();
			dt.Interval = interval;
			dt.Tick += (_, a) => {
				if (count-- == 0) {
					soundEnd.Play();
					dt.Stop();
					this.Close();
				}	
				else
					ts(count);
			};
			ts(count);
			dt.Start();		
		}

		private void StartAnimation() {
			Dispatcher.Invoke(() => {
				CenterEyes();
				switch (Breaktype) {
					case App.BreakType.SHORT:				
						description.Text = "Move your eyes Left to Right";
						LastMove = Movement.LEFT;
						soundLeftRight.Play();
						Countdown((int)CountDown.TotalSeconds, TimeSpan.FromSeconds(1), cur => TimeLeft.Content = cur.ToString());
						MoveEyesLefFromCenter();
						break;
					case App.BreakType.LONG:
						description.Text = "Get up and relax for the rest of the break. Look out the window or take a small walk.";
						Countdown((int)CountDown.TotalSeconds, TimeSpan.FromSeconds(1), cur => TimeLeft.Content = cur.ToString());
						break;
				};			
			});		
		}
		private void CenterEyes() {
			Dispatcher.Invoke(() => {
				double canvasWidth = EyeCanvas.ActualWidth;
				double canvasHeight = EyeCanvas.ActualHeight;
				//Center Eyes
				double eyeContainerWidth = RightEye.Width + LeftEye.Width + 20d;
				double leftEyeLeftMargin = ((canvasWidth / 2d) - (LeftEye.Width / 2d)) - (eyeContainerWidth / 4d);
				double leftEyeToptMargin = ((canvasHeight / 2d) - (LeftEye.Height / 2d));
				double rightEyeLeftMargin = ((canvasWidth / 2d) - (RightEye.Width / 2d)) + (eyeContainerWidth / 4d);
				double rightEyeToptMargin = ((canvasHeight / 2d) - (LeftEye.Height / 2d));
				Canvas.SetLeft(LeftEye, leftEyeLeftMargin);
				Canvas.SetTop(LeftEye, leftEyeToptMargin);
				Canvas.SetLeft(RightEye, rightEyeLeftMargin);
				Canvas.SetTop(RightEye, rightEyeToptMargin);

				//Center Pupils		
				double leftPupilLeftMargin = Canvas.GetLeft(LeftEye) + (LeftEye.Width / 2d) - (LeftPupil.Width / 2d);
				double leftPupilTopMargin = Canvas.GetTop(LeftEye) + (LeftEye.Height / 2d) - (LeftPupil.Height / 2d);
				double rightPupilLeftMargin = Canvas.GetLeft(RightEye) + (RightEye.Width / 2d) - (RightPupil.Width / 2d);
				double rightPupilTopMargin = Canvas.GetTop(RightEye) + (RightEye.Height / 2d) - (RightPupil.Height / 2d);
				Canvas.SetLeft(LeftPupil, leftPupilLeftMargin);
				Canvas.SetTop(LeftPupil, leftPupilTopMargin);
				Canvas.SetLeft(RightPupil, rightPupilLeftMargin);
				Canvas.SetTop(RightPupil, rightPupilTopMargin);
			});
		}
		private void MoveEyesLefFromCenter() {
			Dispatcher.Invoke(() => {
				double toValueLeft = Canvas.GetLeft(LeftPupil) - (LeftEye.Width / 2d) + (LeftPupil.Width / 2d);
				double toValueRight = Canvas.GetLeft(RightPupil) - (RightEye.Width / 2d) + (RightPupil.Width / 2d);
				var moveAnimXLeft = new DoubleAnimation(Canvas.GetLeft(LeftPupil), toValueLeft, new Duration(TimeSpan.FromSeconds(0.5)));
				var moveAnimXRight = new DoubleAnimation(Canvas.GetLeft(RightPupil), toValueRight, new Duration(TimeSpan.FromSeconds(0.5)));
				moveAnimXLeft.Completed += new EventHandler(MoveCompleted);
				LeftPupil.BeginAnimation(Canvas.LeftProperty, moveAnimXLeft);
				RightPupil.BeginAnimation(Canvas.LeftProperty, moveAnimXRight);
			});
		}
		private void MoveEyesUpFromCenter() {
			Dispatcher.Invoke(() => {
				double toValueLeft = Canvas.GetTop(LeftPupil) - (LeftEye.Height / 2d) + (LeftPupil.Height / 2d);
				double toValueRight = Canvas.GetTop(RightPupil) - (RightEye.Height / 2d) + (RightPupil.Height / 2d);
				var moveAnimYLeft = new DoubleAnimation(Canvas.GetTop(LeftPupil), toValueLeft, new Duration(TimeSpan.FromSeconds(0.5)));
				var moveAnimYRight = new DoubleAnimation(Canvas.GetTop(RightPupil), toValueRight, new Duration(TimeSpan.FromSeconds(0.5)));
				moveAnimYLeft.Completed += new EventHandler(MoveCompleted);
				LeftPupil.BeginAnimation(Canvas.TopProperty, moveAnimYLeft);
				RightPupil.BeginAnimation(Canvas.TopProperty, moveAnimYRight);
			});
		}
		private void MoveEyesFromLeftToCenter() {
			Dispatcher.Invoke(() => {
				double toValueLeft = Canvas.GetLeft(LeftPupil) + (LeftEye.Width / 2d) - (LeftPupil.Width / 2d);
				double toValueRight = Canvas.GetLeft(RightPupil) + (RightEye.Width / 2d) - (RightPupil.Width / 2d);
				var moveAnimXLeft = new DoubleAnimation(Canvas.GetLeft(LeftPupil), toValueLeft, new Duration(TimeSpan.FromSeconds(0.5)));
				var moveAnimXRight = new DoubleAnimation(Canvas.GetLeft(RightPupil), toValueRight, new Duration(TimeSpan.FromSeconds(0.5)));
				moveAnimXLeft.Completed += new EventHandler(MoveCompleted);
				LeftPupil.BeginAnimation(Canvas.LeftProperty, moveAnimXLeft);
				RightPupil.BeginAnimation(Canvas.LeftProperty, moveAnimXRight);
			});
		}
		private void MoveEyesLeft() {
			Dispatcher.Invoke(() => {
				double toValueLeft = Canvas.GetLeft(LeftPupil) - (LeftEye.Width) + LeftPupil.Width;
				double toValueRight = Canvas.GetLeft(RightPupil) - (RightEye.Width) + RightPupil.Width;
				var moveAnimXLeft = new DoubleAnimation(Canvas.GetLeft(LeftPupil), toValueLeft, new Duration(TimeSpan.FromSeconds(1)));
				var moveAnimXRight = new DoubleAnimation(Canvas.GetLeft(RightPupil), toValueRight, new Duration(TimeSpan.FromSeconds(1)));
				moveAnimXLeft.Completed += new EventHandler(MoveCompleted);
				LeftPupil.BeginAnimation(Canvas.LeftProperty, moveAnimXLeft);
				RightPupil.BeginAnimation(Canvas.LeftProperty, moveAnimXRight);
			});
		}
		private void MoveEyesRight() {
			Dispatcher.Invoke(() => {
				double toValueLeft = Canvas.GetLeft(LeftPupil) + (LeftEye.Width) - (LeftPupil.Width);
				double toValueRight = Canvas.GetLeft(RightPupil) + (RightEye.Width) - (RightPupil.Width);
				var moveAnimXLeft = new DoubleAnimation(Canvas.GetLeft(LeftPupil), toValueLeft, new Duration(TimeSpan.FromSeconds(1)));
				var moveAnimXRight = new DoubleAnimation(Canvas.GetLeft(RightPupil), toValueRight, new Duration(TimeSpan.FromSeconds(1)));
				moveAnimXLeft.Completed += new EventHandler(MoveCompleted);
				LeftPupil.BeginAnimation(Canvas.LeftProperty, moveAnimXLeft);
				RightPupil.BeginAnimation(Canvas.LeftProperty, moveAnimXRight);
			});
		}
		private void MoveEyesUp() {
			Dispatcher.Invoke(() => {
				double toValueLeft = Canvas.GetTop(LeftPupil) - (LeftEye.Height) + LeftPupil.Height;
				double toValueRight = Canvas.GetTop(RightPupil) - (RightEye.Height) + RightPupil.Height;
				var moveAnimYLeft = new DoubleAnimation(Canvas.GetTop(LeftPupil), toValueLeft, new Duration(TimeSpan.FromSeconds(1)));
				var moveAnimYRight = new DoubleAnimation(Canvas.GetTop(RightPupil), toValueRight, new Duration(TimeSpan.FromSeconds(1)));
				moveAnimYLeft.Completed += new EventHandler(MoveCompleted);
				LeftPupil.BeginAnimation(Canvas.TopProperty, moveAnimYLeft);
				RightPupil.BeginAnimation(Canvas.TopProperty, moveAnimYRight);
			});
		}
		private void MoveEyesDown() {
			Dispatcher.Invoke(() => {
				double toValueLeft = Canvas.GetTop(LeftPupil) + (LeftEye.Height) - (LeftPupil.Height);
				double toValueRight = Canvas.GetTop(RightPupil) + (RightEye.Height) - (RightPupil.Height);
				var moveAnimYLeft = new DoubleAnimation(Canvas.GetTop(LeftPupil), toValueLeft, new Duration(TimeSpan.FromSeconds(1)));
				var moveAnimYRight = new DoubleAnimation(Canvas.GetTop(RightPupil), toValueRight, new Duration(TimeSpan.FromSeconds(1)));
				moveAnimYLeft.Completed += new EventHandler(MoveCompleted);
				LeftPupil.BeginAnimation(Canvas.TopProperty, moveAnimYLeft);
				RightPupil.BeginAnimation(Canvas.TopProperty, moveAnimYRight);
			});
		}

		private void MoveCompleted(object sender, EventArgs e) {
			Dispatcher.Invoke(() => {
				if((string)TimeLeft.Content == "0") {
					return;
				}
				if(MovementCount == 9) {
					soundUpDown.Play();
					MoveEyesFromLeftToCenter();
					LastMove = Movement.CENTER;
					description.Text = "Move your eyes Up and Down";
				} else {
					if (LastMove == Movement.LEFT) {
						MoveEyesRight();
						LastMove = Movement.RIGHT;
					} else if (LastMove == Movement.RIGHT) {
						MoveEyesLeft();
						LastMove = Movement.LEFT;
					} else if (LastMove == Movement.CENTER) {
						MoveEyesUpFromCenter();			
						LastMove = Movement.UP;
					} else if (LastMove == Movement.UP) {
						MoveEyesDown();
						LastMove = Movement.DOWN;
					} else if (LastMove == Movement.DOWN) {
						MoveEyesUp();
						LastMove = Movement.UP;
					}
				}				
				MovementCount++;
			});
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			if (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.F4)) {
				e.Cancel = true;
			}
		}
	}
}
