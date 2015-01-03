using System.Runtime.Serialization;

namespace ReadNow.Portable
{
	[DataContract]
	public class AuthentificatedRequest
	{
		[DataMember(Name = "consumer_key")]
		public string ConsumerKey
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