namespace CarSalesApp.UI
{
	using System;
	using System.Globalization;
	using System.Windows.Data;
	using System.Windows.Media;

	public class GreaterThan25MillionConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is decimal amount && amount > 25_000_000)
				return new SolidColorBrush(Colors.LightGreen);

			return new SolidColorBrush(Colors.Transparent);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
			=> throw new NotImplementedException();
	}
}
