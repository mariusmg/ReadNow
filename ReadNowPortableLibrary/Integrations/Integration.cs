using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace ReadNow.Portable
{
	public class Integration
	{
		public async Task<string> DoGetRequest(string url)
		{
			HttpWebRequest client = WebRequest.Create(url) as HttpWebRequest;
			
			client.Method = "GET";
	
			WebResponse webResponse = await client.GetResponseAsync();

			string response = new StreamReader(webResponse.GetResponseStream()).ReadToEnd();

			return response;
		}
		

		public async Task<string> DoPostRequest(string url)
		{
			HttpWebRequest client = WebRequest.Create(url) as HttpWebRequest;

			client.Method = "POST";

			WebResponse webResponse = await client.GetResponseAsync();

			string response = new StreamReader(webResponse.GetResponseStream()).ReadToEnd();

			return response;
		}

		
		public async Task<string> DoPostRequest(string url, string content, bool addSubjectHeader = false)
		{
			Stream inputStream = null;

			try
			{
				HttpWebRequest client = WebRequest.Create(url) as HttpWebRequest;

				client.Headers["X-Accept"] = "application/json";

				if (addSubjectHeader)
				{
					client.Headers["Subject"] = "Read Now Error Log";
				}

				client.ContentType = "application/json; charset=UTF-8";
				client.Method = "POST";

				inputStream = await client.GetRequestStreamAsync();

				StreamWriter sw = new StreamWriter(inputStream);
				sw.Write(content);
				sw.Flush();

				inputStream.Dispose();

				WebResponse webResponse = await client.GetResponseAsync();

				string response = new StreamReader(webResponse.GetResponseStream()).ReadToEnd();

				return response;
			}
			finally
			{
				if (inputStream != null)
				{
					inputStream.Dispose();
				}
			}
		}
	}
}