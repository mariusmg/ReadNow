using System;
using System.Threading.Tasks;

namespace ReadNow.Portable
{
	public interface ILogger
	{
		void Log(string message);
		void Log(Exception ex);
		Task<string> GetLoggedDataForToday();
	}
}