using System.Runtime.Serialization;

namespace ReadNow.Portable
{

	[DataContract()]
	public class DiffBotResponse
	{
		[DataMember(Name = "url")]
		public string Url
		{
			get;
			set;
		}

		[DataMember(Name = "title")]
		public string Title
		{
			get;
			set;
		}


		[DataMember(Name = "author")]
		public string Author
		{
			get;
			set;
		}


		[DataMember(Name = "date")]
		public string Date
		{
			get;
			set;
		}



		[DataMember(Name = "html")]
		public string Text
		{
			get;
			set;
		}

	}
}