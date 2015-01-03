using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ReadNow.Portable
{
	[DataContract]
	public class ActionModelResult
	{
		[DataMember(Name = "action_results")]
		public List<bool> Results
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