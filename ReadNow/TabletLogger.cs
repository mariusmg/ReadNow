using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using ReadNow.Portable;

namespace ReadNow
{
	public class TabletLogger : ILogger
	{
		private static ConcurrentStack<string> logCache = new ConcurrentStack<string>();

		public static bool isLocked = false;

		private static string GetLogFileName()
		{
			DateTime dt = DateTime.Now;

			string logFile = "log_" + dt.Day.ToString() + dt.Month.ToString() + dt.Year.ToString() + ".txt";

			return logFile;
		}

		public async Task<string> GetLoggedDataForToday()
		{
			Stream stream = null;
			try
			{
				string logFileName = GetLogFileName();

				StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(logFileName);

				stream = await file.OpenStreamForReadAsync();

				return (new StreamReader(stream)).ReadToEnd();
			}
			catch (Exception)
			{
				return string.Empty;
			}
			finally
			{
				if (stream != null)
				{
					stream.Dispose();
				}
			}
		}

		public void Log(Exception ex, string message)
		{
			Log("Message :" + message + Environment.NewLine + "Exception " + ex.GetType() + Environment.NewLine + "Message: " + ex.Message + Environment.NewLine + "Stacktrace: " + ex.StackTrace);
		}

		public void Log(Exception ex)
		{
			Log("Exception " + ex.GetType() + Environment.NewLine + "Message: " + ex.Message + Environment.NewLine + "Stacktrace: " + ex.StackTrace);
		}

		public async void Log(string error)
		{
			Debug.WriteLine(error);

			try
			{
				if (isLocked == false)
				{
					isLocked = true;
				}
				else
				{
					logCache.Push(error);
					return;
				}

				string fileName = GetLogFileName();

				Stream stream = null;

				StorageFile file = null;

				try
				{
					file = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
				}
				catch
				{
				}

				try
				{
					if (file != null)
					{
						stream = await ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync(fileName, CreationCollisionOption.OpenIfExists);
					}
					else
					{
						file = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName);

						stream = await file.OpenStreamForWriteAsync();
					}

					StringBuilder builder = new StringBuilder();

					string cachedString;

					while (logCache.TryPop(out cachedString))
					{
						builder.Append(cachedString);
						builder.Append(Environment.NewLine);
					}

					builder.Append(Environment.NewLine);
					builder.Append(Environment.NewLine);
					builder.Append(DateTime.Now.ToString());
					builder.Append(Environment.NewLine);
					builder.Append(error);

					StreamWriter writer = new StreamWriter(stream);

					writer.Write(builder.ToString());
					writer.Flush();
				}
				catch
				{
				}
				finally
				{
					if (stream != null)
					{
						stream.Dispose();
					}
				}
			}
			finally
			{
				isLocked = false;
			}
		}
	}
}