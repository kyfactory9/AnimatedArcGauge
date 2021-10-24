using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcGauge2
{
	public class GaugeModel : ViewModelBase.ViewModelBase
	{
		private int _max;
		public int Max
		{
			get { return _max; }
			set
			{
				_max = value;
				NotifyPropertyChanged("Max");
			}
		}

		private int _min;
		public int Min
		{
			get { return _min; }
			set
			{
				_min = value;
				NotifyPropertyChanged("Min");
			}
		}

		private int _value;
		public int Value
		{
			get { return _value; }
			set
			{
				_value = value;
				NotifyPropertyChanged("Value");
			}
		}

		private int _value_string;
		public int ValueString
		{
			get { return _value_string; }
			set
			{
				_value_string = value;
				NotifyPropertyChanged("ValueString");
			}
		}

		private string _title;
		public string Title
		{
			get { return _title; }
			set
			{
				_title = value;
				NotifyPropertyChanged("Title");
			}
		}

		private string _unit;
		public string Unit
		{
			get { return _unit; }
			set
			{
				_unit = value;
				NotifyPropertyChanged("Unit");
			}
		}

		private string _tick_unit;
		public string TickUnit
		{
			get { return _tick_unit; }
			set
			{
				_tick_unit = value;
				NotifyPropertyChanged("TickUnit");
			}
		}
	}
}
