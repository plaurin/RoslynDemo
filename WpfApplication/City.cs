using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfApplication
{
	public class City : NotifiableBase
	{
		private int temp;
		private object calc;

		public string Name { get; set; }
		public string Country { get; set; }
		public string Forecast { get; set; }

		public int Temp
		{
			get { return this.temp; }
			set
			{
				if (this.temp != value)
				{
					this.temp = value;
					this.OnPropertyChanged();
				}
			}
		}

		public object Calc
		{
			get { return this.calc; }
			set
			{
				if (this.calc != value)
				{
					this.calc = value;
					this.OnPropertyChanged();
				}
			}
		}
	}
}
