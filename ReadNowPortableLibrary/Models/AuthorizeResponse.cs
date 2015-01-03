using System.Runtime.Serialization;

namespace ReadNow.Portable
{
	[DataContract]
	public class AuthorizeResponse
	{
		[DataMember(Name = "username")]
		public string UserName
		{
			get;
			set;
		}

		[DataMember(Name = "access_token")]
		public string AccessToken
		{
			get;
			set;
		}
	}
}