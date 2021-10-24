using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ArcGauge2
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			gauge.GetRealValue = (value) => value * 100;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			DispatcherTimer timer = new DispatcherTimer();
			timer.Interval = new TimeSpan(0, 0, 3);
			timer.Tick += Timer_Tick;
			timer.Start();
		}

		private void Timer_Tick(object sender, EventArgs e)
		{
			Random r = new Random();
			int n = r.Next(gauge.Min, gauge.Max);
			gauge.Value = n;
		}

		private void btnTickGap10_Click(object sender, RoutedEventArgs e)
		{
			gauge.TickGap = 10;
		}

		private void btnTickGap8_Click(object sender, RoutedEventArgs e)
		{
			gauge.TickGap = 8;
		}

		private void btnTickGap20_Click(object sender, RoutedEventArgs e)
		{
			gauge.TickGap = 20;
		}
	}
}
