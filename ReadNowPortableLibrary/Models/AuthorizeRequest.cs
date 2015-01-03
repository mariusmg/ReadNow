using System.Runtime.Serialization;

namespace ReadNow.Portable
{
	[DataContract()]
	public class AuthorizeRequest
	{
		[DataMember(Name = "consumer_key")]
		public string ConsumerKey
		{
			get;
			set;
		}

		[DataMember(Name = "code")]
		public string Code
		{
			get;
			set;
		}
	}
}