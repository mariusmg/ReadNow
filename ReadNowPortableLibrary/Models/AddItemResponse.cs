using System.Runtime.Serialization;

namespace ReadNow.Portable
{
	[DataContract]
	public class AddItemResponse
	{
		[DataMember(Name = "item")]
		public ItemAdded Item
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
	}


	[DataContract]
	public class ItemAdded
	{
		[DataMember(Name = "item_id")]
		public string Id
		{
			get;
			set;
		}
	}
}