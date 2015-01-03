using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ReadNow.Portable
{
	[DataContract]
	public class ItemResponse
	{
		[DataMember(Name = "list")]
		public Dictionary<string, PocketItem> Item
		{
			get;
			set;
		}

		[DataMember(Name = "status")]
		public string Status
		{
			get;
			set;
		}

		[DataMember(Name = "complete")]
		public string Complete
		{
			get;
			set;
		}

		[DataMember(Name = "since")]
		public string Since
		{
			get;
			set;
		}
	}

	
	[DataContract]
	public class ItemResponseAlternative
	{
		//[DataMember(Name = "list")]
		//public Dictionary<string, PocketItem> Item
		//{
		//	get;
		//	set;
		//}

		[DataMember(Name = "status")]
		public string Status
		{
			get;
			set;
		}

		[DataMember(Name = "complete")]
		public string Complete
		{
			get;
			set;
		}

		[DataMember(Name = "since")]
		public string Since
		{
			get;
			set;
		}

	}

}