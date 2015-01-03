using System.Threading.Tasks;

namespace ReadNow.Portable
{
	public class VoidsoftApiIntegration : Integration
	{
		private const string LOG_URL = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";

		public Task SendErrorLog(string data)
		{
			return DoPostRequest(LOG_URL, data, true);
		}
	}
}