using System.Runtime.Serialization;

namespace ReadNow.Portable
{
	[DataContract]
	public class PocketVideo
	{

		[DataMember(Name = "item_id")]
		public string ItemId
		{
			get;
			set;
		}


		[DataMember(Name = "video_id")]
		public string VideoId
		{
			get;
			set;
		}


		[DataMember(Name = "src")]
		public string Url
		{
			get;
			set;
		}


		[DataMember(Name = "vid")]
		public string Vid
		{
			get;
			set;
		}


		[DataMember(Name = "width")]
		public int Width
		{
			get;
			set;
		}

		[DataMember(Name = "height")]
		public int Heigth
		{
			get;
			set;
		}

		[DataMember(Name = "type")]
		public string Type
		{
			get;
			set;
		}


	}
}