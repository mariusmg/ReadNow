namespace ReadNow.Portable
{
	public interface IHttpUtility
	{
		string UrlEncode(string input);
		string UrlDecode(string input);
	}
}