using System.Runtime.Serialization;

namespace ReadNow.Portable
{
	[DataContract]
	public class ActionModel
	{
		[DataMember(Name = "action")]
		public string Action
		{
			get;
			set;
		}

		[DataMember(Name = "item_id")]
		public string ItemId
		{
			get;
			set;
		}
	}
}