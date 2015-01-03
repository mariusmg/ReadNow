using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ReadNow.Portable
{
	[DataContract]
	public class PocketItem
	{
		[DataMember(Name = "item_id")]
		public string Id
		{
			get;
			set;
		}
		
		[DataMember(Name = "sort_id")]
		public int SortId
		{
			get;
			set;
		}

		[DataMember(Name = "resolved_id")]
		public string ResolvedId
		{
			get;
			set;
		}

		[DataMember(Name = "given_url")]
		public string GivenUrl
		{
			get;
			set;
		}

		[DataMember(Name = "resolved_url")]
		public string ResolvedUrl
		{
			get;
			set;
		}

		[DataMember(Name = "given_title")]
		public string GivenTitle
		{
			get;
			set;
		}

		[DataMember(Name = "resolved_title")]
		public string ResolvedTitle
		{
			get;
			set;
		}

		[DataMember(Name = "is_article")]
		public string IsArticle
		{
			get;
			set;
		}

		[DataMember(Name = "has_video")]
		public string HasVideo
		{
			get;
			set;
		}

		[DataMember(Name = "has_image")]
		public string HasImage
		{
			get;
			set;
		}

		[DataMember(Name = "excerpt")]
		public string Excerpt
		{
			get;
			set;
		}

		[DataMember(Name = "word_count")]
		public int WordCount
		{
			get;
			set;
		}

		[DataMember(Name = "images")]
		public Dictionary<string, PocketImage> Images
		{
			get;
			set;
		}

		[DataMember(Name = "videos")]
		public Dictionary<string, PocketVideo> Videos
		{
			get;
			set;
		}

		[DataMember(Name = "favorite")]
		public int Favorite
		{
			get;
			set;
		}

		[DataMember(Name = "status")]
		public int Status
		{
			get;
			set;
		}
	}
}