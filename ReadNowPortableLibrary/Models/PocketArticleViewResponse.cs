using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ReadNow.Portable
{
	[DataContract()]
	public class PocketArticleViewResponse
	{

		[DataMember(Name = "resolved_id")]
		public string Id
		{
			get;
			set;
		}

		[DataMember(Name = "article")]
		public string Text
		{
			get;
			set;
		}


		[DataMember(Name = "images")]
		public Dictionary<string, PocketArticleViewImage> Images
		{
		
			get;
			set;
		}
	}



	[DataContract()]
	public class PocketArticleViewImage
	{

		[DataMember(Name = "src")]
		public string Source
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
		
	}

}