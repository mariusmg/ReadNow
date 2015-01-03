using System.Threading.Tasks;

namespace ReadNow.Portable
{
	public class PocketArticleViewIntegration : Integration
	{
		private const string ENDPOINT = "http://text.readitlater.com/v3beta/text?";
		private IHttpUtility httpUtility;

		public PocketArticleViewIntegration(IHttpUtility utility)
		{
			httpUtility = utility;
		}

		public async Task<string> GetText(string url)
		{
			string formattedUrl = ENDPOINT + ("consumer_key=" + PocketIntegration.APP_KEY);

			formattedUrl += ("&url=" + url);
			formattedUrl += ("&output=json");
			formattedUrl += ("&images=1");

			httpUtility.UrlEncode(formattedUrl);

			//consumer_key - your consumer_key (you can get one here: http://getpocket.com/developer/)

			//url - the url you want to parse
			//images - (0 or 1) #see note on images below
			//videos - (0 or 1) #see note on videos below
			//refresh - (0 or 1) If you need to get the latest version, use refresh.  Leave refresh off to make use of any caches of the item.
			//output - set this to 'json'

			PocketArticleViewResponse ar = (new JsonSerializer().JsonDeserialize<PocketArticleViewResponse>(await DoGetRequest(formattedUrl)));
			return ar.Text;
		}
	}
}