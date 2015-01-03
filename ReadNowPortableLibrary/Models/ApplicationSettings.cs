using System.Runtime.Serialization;

namespace ReadNow.Portable
{
	[DataContract]
	public class ApplicationSettings
	{
		[DataMember]
		public string Token
		{
			get;
			set;
		}

		[DataMember]
		public string UserName
		{
			get;
			set;
		}

		[DataMember]
		public string Since
		{
			get;
			set;
		}

		//[DataMember]
		//public bool IsDarkArticleView
		//{
		//	get;
		//	set;
		//}

		[DataMember]
		public string ArticleFontSize
		{
			get;
			set;
		}


		[IgnoreDataMember]
		public bool DarkTheme
		{
			get;
			set;
		}
	}
}