using System.Net;
using ReadNow.Portable;

namespace ReadNow.BackgroundTasks
{
	internal sealed class TabletHttpUtility : IHttpUtility
	{
		public string UrlEncode(string input)
		{
			return WebUtility.UrlEncode(input);
		}

		public string UrlDecode(string input)
		{
			return WebUtility.UrlDecode(input);
		}
	}
}