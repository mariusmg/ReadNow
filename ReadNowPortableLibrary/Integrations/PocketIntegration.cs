using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ReadNow.Portable
{
	public class PocketIntegration : Integration
	{
		private ILogger logger;
		private IHttpUtility utility;

		public PocketIntegration(ILogger log, IHttpUtility u)
		{
			logger = log;
			utility = u;
		}

		public const string APP_KEY = "xxxxxxxxxxxxxxxxxxxxxxxxxxx";  //get your own API key and replace here
		private const string REQUEST_TOKEN_URL = "https://getpocket.com/v3/oauth/request";
		private const string AUTHORIZE_URL = "https://getpocket.com/v3/oauth/authorize";
		private const string ITEMS_URL = "https://getpocket.com/v3/get";
		private const string ADD_ITEM_URL = "https://getpocket.com/v3/add";
		private const string MODIFY_ITEM_URL = "https://getpocket.com/v3/send?";

		public async Task<AuthorizeResponse> DoAuthorization(string code)
		{
			AuthorizeRequest ar = new AuthorizeRequest();
			ar.ConsumerKey = APP_KEY;
			ar.Code = code;

			JsonSerializer js = new JsonSerializer();
			return js.JsonDeserialize<AuthorizeResponse>(await DoPostRequest(AUTHORIZE_URL, js.JsonSerialize(ar)));
		}

		public async Task<TokenResponse> GetRequestToken()
		{
			TokenRequest r = new TokenRequest();
			r.Key = APP_KEY;
			r.RedirectUri = "pocketnow:authorizationFinished";

			JsonSerializer js = new JsonSerializer();
			return js.JsonDeserialize<TokenResponse>(await DoPostRequest(REQUEST_TOKEN_URL, js.JsonSerialize(r)));
		}

		public async Task<ItemResponse> RefreshArticles(string token, string since)
		{
			return await GetAllItems(token, ItemType.Article);
		}

		public async Task<ItemResponse> RefreshVideos(string token, string since)
		{
			return await GetAllItems(token, ItemType.Video);
		}

		public async Task<ItemResponse> RefreshImages(string token, string since)
		{
			return await GetAllItems(token, ItemType.Image);
		}

		public async Task<ItemResponse> GetAllItems(string token, ItemType type)
		{
			JsonSerializer js = new JsonSerializer();

			Dictionary<string, string> dict = new Dictionary<string, string>();
			dict.Add("consumer_key", APP_KEY);
			dict.Add("access_token", token);
			dict.Add("contentType", GetItemType(type));
			dict.Add("state", "all"); //by default grab all items
			dict.Add("detailType", "complete"); //by default grab all items

			ItemResponse ir = null;

			string result = await DoPostRequest(ITEMS_URL, js.JsonSerialize(dict));

			try
			{
				ir = js.JsonDeserialize<ItemResponse>(result);
			}
			catch (JsonSerializationException jsex)
			{
				logger.Log(jsex);
			}

			if (ir == null)
			{
				ItemResponseAlternative irr = (js.JsonDeserialize<ItemResponseAlternative>(result));

				ir = new ItemResponse();
				ir.Complete = irr.Complete;
				ir.Since = irr.Since;
				ir.Status = irr.Status;
			}

			return ir;
		}

		private string GetItemType(ItemType t)
		{
			if (t == ItemType.Article)
			{
				return "article";
			}
			return t == ItemType.Image ? "image" : "video";
		}

		public async Task<AddItemResponse> AddItem(string token, string url)
		{
			JsonSerializer js = new JsonSerializer();

			Dictionary<string, string> dict = new Dictionary<string, string>();
			dict.Add("consumer_key", APP_KEY);
			dict.Add("access_token", token);
			dict.Add("url", url);

			AddItemResponse ir = js.JsonDeserialize<AddItemResponse>(await DoPostRequest(ADD_ITEM_URL, js.JsonSerialize(dict)));
			return ir;
		}

		public async Task<ActionModelResult> DeleteItem(string token, string id)
		{
			//sample call https://getpocket.com/v3/send?actions=%5B%7B%22action%22%3A%22archive%22%2C%22time%22%3A1348853312%2C%22item_id%22%3A229279689%7D%5D&access_token=[ACCESS_TOKEN]&consumer_key=[CONSUMER_KEY]

			JsonSerializer js = new JsonSerializer();

			//array if actions to send
			ActionModel[] am = new ActionModel[1];
			am[0] = new ActionModel();
			am[0].Action = "delete";
			am[0].ItemId = id;

			string s = js.JsonSerialize(am);

			Dictionary<string, string> dict = new Dictionary<string, string>();
			dict.Add("consumer_key", APP_KEY);
			dict.Add("access_token", token);

			StringBuilder builder = new StringBuilder();

			foreach (KeyValuePair<string, string> pair in dict)
			{
				builder.Append(utility.UrlEncode(pair.Key) + "=" + utility.UrlEncode(pair.Value) + "&");
			}

			string encoded = builder.ToString();
			encoded = encoded.Remove(encoded.Length - 1, 1);

			ActionModelResult ir = js.JsonDeserialize<ActionModelResult>(await DoGetRequest(MODIFY_ITEM_URL + "actions=" + s + "&" + encoded));

			return ir;
		}
	}
}