using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;

namespace ReadNow.Portable
{
	public class ApplicationInitialization
	{
		private IFileSystemOperations fileSystem;


		public ApplicationInitialization(IFileSystemOperations operations)
		{
			this.fileSystem = operations;
		}

		//private Storage fileSystem = new Storage();

		public async Task LoadCachedData()
		{
			List<PocketItem> articles = null;
			try
			{
				articles = await fileSystem.LoadPersistedType<List<PocketItem>>(Constants.ARTICLES);
			}
			catch
			{
			}

			List<PocketItem> images = null;
			try
			{
				images = await fileSystem.LoadPersistedType<List<PocketItem>>(Constants.IMAGES);
			}
			catch
			{
			}

			List<PocketItem> videos = null;
			
			try
			{
				videos = await fileSystem.LoadPersistedType<List<PocketItem>>(Constants.VIDEOS);
			}
			catch
			{

			}

			ApplicationContext.Articles = new ObservableCollection<PocketItem>();
			ApplicationContext.Images = new ObservableCollection<PocketItem>();
			ApplicationContext.Videos = new ObservableCollection<PocketItem>();

			if (articles != null)
			{
				articles = articles.OrderBy(item => item.SortId).ToList();

				foreach (PocketItem article in articles)
				{
					ApplicationContext.Articles.Add(article);
				}

			}

			if (images != null)
			{
				images = images.OrderBy(item => item.SortId).ToList();

				foreach (PocketItem img in images)
				{
					ApplicationContext.Images.Add(img);
				}
			}

			if (videos != null)
			{
				videos = videos.OrderBy(item => item.SortId).ToList();

				foreach (PocketItem v in videos)
				{
					ApplicationContext.Videos.Add(v);
				}
			}
		}
	}
}