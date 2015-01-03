using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using ReadNow.Portable;

namespace ReadNow
{
	public class Storage : IFileSystemOperations
	{
		public async Task RemoveFile(string fileName)
		{
			StorageFolder userFolder = ApplicationData.Current.LocalFolder;

			StorageFile storageFile = await userFolder.GetFileAsync(fileName);

			storageFile.DeleteAsync();
		}

		public async Task<string> LoadContentFromFile(string fileName)
		{
			Stream stream = null;

			try
			{
				stream = await GetStreamForReading(fileName);

				StreamReader reader = new StreamReader(stream);

				return await reader.ReadToEndAsync();
			}
			finally
			{
				if (stream != null)
				{
					stream.Dispose();
				}
			}
		}

		public async Task<T> LoadPersistedType<T>(string fileName) where T : class
		{
			Stream stream = null;

			try
			{
				stream = await GetStreamForReading(fileName);

				JsonSerializer js = new JsonSerializer();

				StreamReader reader = new StreamReader(stream);

				return js.JsonDeserialize<T>(reader.ReadToEnd());
			}
			finally
			{
				if (stream != null)
				{
					stream.Dispose();
				}
			}
		}

		public async Task Persist(string fileName, string content)
		{
			Stream stream = null;

			try
			{
				stream = await GetStreamForPersistance(fileName);

				StreamWriter sw = new StreamWriter(stream);

				sw.Write(content);

				sw.Flush();
			}
			finally
			{
				if (stream != null)
				{
					stream.Dispose();
				}
			}
		}

		public async Task PersistType<T>(string fileName, T t) where T : class
		{
			Stream stream = null;

			try
			{
				JsonSerializer js = new JsonSerializer();

				string json = js.JsonSerialize(t);

				stream = await GetStreamForPersistance(fileName);

				StreamWriter sw = new StreamWriter(stream);

				sw.Write(json);

				sw.Flush();
			}
			finally
			{
				if (stream != null)
				{
					stream.Dispose();
				}
			}
		}

		private async Task<Stream> GetStreamForPersistance(string fileName)
		{
			StorageFolder userFolder = ApplicationData.Current.LocalFolder;

			StorageFile file = await userFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

			return await file.OpenStreamForWriteAsync();
		}

		private async Task<Stream> GetStreamForReading(string fileName)
		{
			StorageFolder userFolder = ApplicationData.Current.LocalFolder;

			StorageFile file = await userFolder.GetFileAsync(fileName);

			return await file.OpenStreamForReadAsync();
		}
	}
}