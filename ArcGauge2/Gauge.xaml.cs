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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ArcGauge2
{
	/// <summary>
	/// Interaction logic for Gauge.xaml
	/// </summary>
	public partial class Gauge : UserControl
	{
		// Dependency Properties
		public static readonly DependencyProperty MinProperty = DependencyProperty.Register("Min", typeof(int), typeof(Gauge), new FrameworkPropertyMetadata(0, new PropertyChangedCallback(OnMinPropertyChanged)));
		public int Min
		{
			get { return (int)GetValue(MinProperty); }
			set { SetValue(MinProperty, value); }
		}
		private static void OnMinPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Gauge f = d as Gauge;
			f.model.Min = (int)e.NewValue;
			f.RenderGauge();
		}

		public static readonly DependencyProperty MaxProperty = DependencyProperty.Register("Max", typeof(int), typeof(Gauge), new FrameworkPropertyMetadata(0, new PropertyChangedCallback(OnMaxPropertyChanged)));
		public int Max
		{
			get { return (int)GetValue(MaxProperty); }
			set { SetValue(MaxProperty, value); }
		}
		private static void OnMaxPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Gauge f = d as Gauge;
			f.model.Max = (int)e.NewValue;
			f.RenderGauge();
		}

		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(int), typeof(Gauge), new FrameworkPropertyMetadata(0, new PropertyChangedCallback(OnValuePropertyChanged)));
		public int Value
		{
			get { return (int)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}
		private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Gauge f = d as Gauge;
			f.model.Value = (int)e.NewValue;
			f.RenderGauge();
		}

		public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(Gauge), new FrameworkPropertyMetadata(string.Empty, new PropertyChangedCallback(OnTitlePropertyChanged)));
		public string Title
		{
			get { return (string)GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}
		private static void OnTitlePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Gauge f = d as Gauge;
			f.model.Title = (string)e.NewValue;
		}

		public static readonly DependencyProperty UnitProperty = DependencyProperty.Register("Unit", typeof(string), typeof(Gauge), new FrameworkPropertyMetadata(string.Empty, new PropertyChangedCallback(OnUnitPropertyChanged)));
		public string Unit
		{
			get { return (string)GetValue(UnitProperty); }
			set { SetValue(UnitProperty, value); }
		}
		private static void OnUnitPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Gauge f = d as Gauge;
			f.model.Unit = (string)e.NewValue;
		}

		public static readonly DependencyProperty TickUnitProperty = DependencyProperty.Register("TickUnit", typeof(string), typeof(Gauge), new FrameworkPropertyMetadata(string.Empty, new PropertyChangedCallback(OnTickUnitPropertyChanged)));
		public string TickUnit
		{
			get { return (string)GetValue(TickUnitProperty); }
			set { SetValue(TickUnitProperty, value); }
		}
		private static void OnTickUnitPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Gauge f = d as Gauge;
			f.model.TickUnit = (string)e.NewValue;
		}

		private static readonly DependencyProperty ValueStringProperty = DependencyProperty.Register("ValueString", typeof(int), typeof(Gauge), new FrameworkPropertyMetadata(0, new PropertyChangedCallback(OnValueStringPropertyChanged)));
		private int ValueString
		{
			get { return (int)GetValue(ValueStringProperty); }
			set { SetValue(ValueStringProperty, value); }
		}
		private static void OnValueStringPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Gauge f = d as Gauge;
			f.model.ValueString = (int)e.NewValue;
		}

		public static readonly DependencyProperty TickGapProperty = DependencyProperty.Register("TickGap", typeof(int), typeof(Gauge), new FrameworkPropertyMetadata(0, new PropertyChangedCallback(OnTickGapPropertyChanged)));
		public int TickGap
		{
			get { return (int)GetValue(TickGapProperty); }
			set { SetValue(TickGapProperty, value); }
		}
		private static void OnTickGapPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Gauge f = d as Gauge;
			f.RenderTicks();
		}

		private const double FULLRANGE_ANGLE = 270;
		private const double START_ANGLE_OFFSET = 90;
		private const double START_NIDDLE_ANGLE = 225;
		private const double START_TICK_ANGLE = 135;

		public delegate int RealValueDelegate(int value);
		public RealValueDelegate GetRealValue { get; set; }

		private Style tickStyle;

		private GaugeModel model;

		public Gauge()
		{
			InitializeComponent();

			tickStyle = this.Resources["TickStyle"] as Style;

			model = this.Resources["model"] as GaugeModel;
		}

		private void RenderGauge()
		{
			if (GetRealValue == null) return;

			// Draw gauge
			int value = Value >= Max ? Max : Value <= Min ? Min : Value;
			int offset = 190;
			double angle = FULLRANGE_ANGLE / (Max - Min) * (Value - Min);
			double rad = (Math.PI / 180) * (angle + START_ANGLE_OFFSET);

			// x and y range => 0~380, width of arc is 380
			double x = offset * Math.Cos(rad) + offset;
			double y = offset * Math.Sin(rad) + offset;
			Point point = new Point(x, y);
			arcGauge.IsLargeArc = angle > 180;
			arcGauge.Point = point;

			// Rotate niddle using animation
			DoubleAnimation ani_gauge = new DoubleAnimation();
			Duration dur = new Duration(TimeSpan.FromMilliseconds(500));
			ExponentialEase acc = new ExponentialEase();
			acc.EasingMode = EasingMode.EaseOut;
			acc.Exponent = 5;
			ani_gauge.To = angle + START_NIDDLE_ANGLE;
			ani_gauge.Duration = dur;
			ani_gauge.EasingFunction = acc;
			Storyboard.SetTargetName(ani_gauge, "niddle");
			Storyboard.SetTargetProperty(ani_gauge, new PropertyPath(RotateTransform.AngleProperty));

			// Print value string using animation
			Int32Animation ani_value_str = new Int32Animation();
			ani_value_str.To = GetRealValue(value);
			ani_value_str.Duration = dur;
			ani_value_str.EasingFunction = acc;
			Storyboard.SetTarget(ani_value_str, this);
			Storyboard.SetTargetProperty(ani_value_str, new PropertyPath(Gauge.ValueStringProperty));

			Storyboard sb = new Storyboard();
			sb.Children.Add(ani_gauge);
			sb.Children.Add(ani_value_str);
			sb.Begin(this);
		}

		private void RenderTicks()
		{
			if ((Max - Min) < TickGap) return;

			tick_container.Children.Clear();

			// Set each gauge text
			int ticks = (Max - Min) / TickGap + 1;
			TextBlock[] textBlocks = new TextBlock[ticks];
			for(int i=0; i<ticks; i++)
			{
				textBlocks[i] = new TextBlock();
				textBlocks[i].Text = string.Format("{0:F0}", Min + (Max - Min) / (ticks - 1) * i);
				SetTickDefaultStyle(textBlocks[i]);
			}

			// Place each ticks using animation
			double tick_angle_offset = FULLRANGE_ANGLE / (textBlocks.Length - 1);
			int tick_start_offset = 100;
			int tick_dest_offset = 160;

			ExponentialEase acc = new ExponentialEase();
			acc.EasingMode = EasingMode.EaseOut;
			acc.Exponent = 1;
			Duration dur = new Duration(TimeSpan.FromMilliseconds(500));
			List<string> names = new List<string>();
			int delay = 100;
			Storyboard sb = new Storyboard();

			for(int i=0; i<textBlocks.Length; i++)
			{
				double angle = i * tick_angle_offset + START_TICK_ANGLE;
				double rad = (Math.PI / 180) * angle;
				double x = tick_dest_offset * Math.Cos(rad);
				double y = tick_dest_offset * Math.Sin(rad);
				double start_x = tick_start_offset * Math.Cos(rad);
				double start_y = tick_start_offset * Math.Sin(rad);

				// Set start location of each textblock
				TranslateTransform tsl = new TranslateTransform(start_x, start_y);
				textBlocks[i].RenderTransform = tsl;
				string tsl_name = "translate" + tsl.GetHashCode();
				RegisterName(tsl_name, tsl);
				names.Add(tsl_name);
				tick_container.Children.Add(textBlocks[i]);

				// Move to destination of ticks using animation
				DoubleAnimation trans_tick_x = new DoubleAnimation();
				trans_tick_x.To = x;
				trans_tick_x.BeginTime = TimeSpan.FromMilliseconds(i * delay);
				trans_tick_x.Duration = dur;
				trans_tick_x.EasingFunction = acc;
				Storyboard.SetTargetName(trans_tick_x, tsl_name);
				Storyboard.SetTargetProperty(trans_tick_x, new PropertyPath(TranslateTransform.XProperty));

				DoubleAnimation trans_tick_y = new DoubleAnimation();
				trans_tick_y.To = y;
				trans_tick_y.BeginTime = TimeSpan.FromMilliseconds(i * delay);
				trans_tick_y.Duration = dur;
				trans_tick_y.EasingFunction = acc;
				Storyboard.SetTargetName(trans_tick_y, tsl_name);
				Storyboard.SetTargetProperty(trans_tick_y, new PropertyPath(TranslateTransform.YProperty));

				sb.Children.Add(trans_tick_x);
				sb.Children.Add(trans_tick_y);
			}

			sb.Begin(this);
			sb.Completed += (sender, e) =>
			{
				foreach (string s in names)
				{
					UnregisterName(s);
				}
			};
		}

		private void SetTickDefaultStyle(TextBlock tb)
		{
			if (tb == null) return;

			tb.Style = tickStyle;
		}
	}
}
