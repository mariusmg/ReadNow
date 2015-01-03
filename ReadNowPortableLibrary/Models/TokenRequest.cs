using System.Runtime.Serialization;

namespace ReadNow.Portable
{
	[DataContract()]
	public class TokenRequest
	{
		[DataMember(Name = "consumer_key")]
		public string Key
		{
			get;
			set;
		}

		[DataMember(Name = "redirect_uri")]
		public string RedirectUri
		{
			get;
			set;
		}

	}
}