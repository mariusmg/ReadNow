using System.Threading.Tasks;

namespace ReadNow.Portable
{
	public interface IFileSystemOperations
	{
		Task RemoveFile(string fileName);

		Task<string> LoadContentFromFile(string fileName);

		Task<T> LoadPersistedType<T>(string fileName) where T : class;

		Task Persist(string fileName, string content);

		Task PersistType<T>(string fileName, T t) where T : class;

	}
}