using System.Collections.Generic;
using Newtonsoft.Json;

namespace ReadNow.Portable
{
	public class JsonSerializer
	{
		public string JsonSerialize(Dictionary<string, string> items)
		{
			return JsonConvert.SerializeObject(items);
		}

		public string JsonSerialize<T>(T t)
		{
			return JsonConvert.SerializeObject(t);
		}

		public T JsonDeserialize<T>(string input) where T : class
		{
			return JsonConvert.DeserializeObject(input, typeof (T)) as T;
		}

		public T[] JsonDeserializeArray<T>(string input) where T : class
		{
			return JsonConvert.DeserializeObject(input, typeof (T)) as T[];
		}
	}
}