using System;
using Windows.UI.Xaml.Data;

namespace ReadNow.Converters
{
	public class UrlDomainConverter : IValueConverter
	{
		private const string WWW = "www.";
		private const string HTTP_WWW = "http://www.";
		private const string HTTPS_WWW = "http://www.";
		private const string HTTP = "https://";
		private const string HTTPS = "http://";

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (value == null)
			{
				return string.Empty;
			}
			
			if (string.IsNullOrEmpty(value.ToString()) || string.IsNullOrWhiteSpace(value.ToString()))
			{
				return string.Empty;
			}

			
			string url = value.ToString().ToLower();



			if (url.StartsWith(HTTP_WWW))
			{
				url = url.Remove(0, HTTP_WWW.Length);
			}
			else if (url.StartsWith(HTTPS_WWW))
			{
				url = url.Remove(0, HTTPS_WWW.Length);
			}
			else if (url.StartsWith(HTTPS))
			{
				url = url.Remove(0, HTTPS.Length);
			}
			if (url.StartsWith(WWW))
			{
				url = url.Remove(0, WWW.Length);
			}
			else if (url.StartsWith(HTTP))
			{
				url = url.Remove(0, HTTP.Length);
			}

			int index = url.IndexOf(@"/");

			if (index == -1)
			{
				return url;
			}


			return url.Substring(0, index);

		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}