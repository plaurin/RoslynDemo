using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication
{
	public static class DataUtil
	{
		public static ViewModel CreateViewModel()
		{
			var cities = new List<City>()
			{
				new City { Name = "Montreal", Country = "Canada", Forecast = "Sunny", Temp = 78 },
				new City { Name = "New York", Country = "United", Forecast = "Cloudy", Temp = 85 },
				new City { Name = "Paris", Country = "France", Forecast = "Raining", Temp = 84 },
				new City { Name = "Tokyo", Country = "Japan", Forecast = "Thunder storm", Temp = 67 }
			};

			return new ViewModel(cities, new ScriptEvaluator());
		}
	}
}
