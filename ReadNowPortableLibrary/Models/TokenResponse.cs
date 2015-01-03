using System.Runtime.Serialization;

namespace ReadNow.Portable
{
	[DataContract()]
	public class TokenResponse
	{
		[DataMember(Name = "code")]
		public string Code
		{
			get;
			set;
		}


		[DataMember(Name = "state")]
		public object State
		{
			get;
			set;
		}

	}
}