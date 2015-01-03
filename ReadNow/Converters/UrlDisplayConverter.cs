using System;
using System.Net;
using Windows.UI.Xaml.Data;
using ReadNow.Portable;

namespace ReadNow.Converters
{
	public class UrlDisplayConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			PocketItem item = value as PocketItem;

			if (item == null)
			{
				return string.Empty;
			}

			if (!string.IsNullOrEmpty(item.ResolvedTitle))
			{
				return WebUtility.UrlDecode(item.ResolvedTitle);
			}


			if (!string.IsNullOrEmpty(item.GivenTitle))
			{
				return WebUtility.UrlDecode(item.GivenTitle);
			}

			return string.Empty;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			return null;
		}
	}
}