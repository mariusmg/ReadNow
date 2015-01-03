using System.Runtime.Serialization;

namespace ReadNow.Portable
{

	[DataContract]
	public class PocketImage
	{

		[DataMember(Name = "item_id")]
		public string Id
		{
			get;
			set;
		}


		[DataMember(Name = "image_id")]
		public string ImageId
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

		[DataMember(Name = "width")]
		public int Width
		{
			get;
			set;
		}

		[DataMember(Name = "height")]
		public int Height
		{
			get;
			set;
		}


		[DataMember(Name = "caption")]
		public string Caption
		{
			get;
			set;
		}


		[DataMember(Name = "credit")]
		public string Credit
		{
			get;
			set;
		}


	}
}