using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Storage;
using ReadNow.Portable;

namespace ReadNow.BackgroundTasks
{
	public sealed class ItemDownloader : IBackgroundTask
	{
		private static List<PocketItem> articles = null;

		public async void Run(IBackgroundTaskInstance taskInstance)
		{
			BackgroundTaskDeferral backgroundTaskDeferral = null;

			taskInstance.Canceled += (sender, reason) =>
			{
			};

			try
			{
				backgroundTaskDeferral = taskInstance.GetDeferral();

				try
				{
					await LoadPersistedType(Constants.ARTICLES);
				}
				catch
				{
					return;
				}

				foreach (PocketItem item in articles)
				{
					try
					{
						await GetArticleTextFormattedForDisplay(item.Id, item.ResolvedUrl ?? item.GivenUrl);
					}
					catch (Exception ex)
					{
						continue;
					}
				}
			}
			catch (Exception ex)
			{
				//(new TabletLogger()).Log(ex);
			}
			finally
			{
				if (backgroundTaskDeferral != null)
				{
					backgroundTaskDeferral.Complete();
				}
			}
		}

		public IAsyncAction GetArticleTextFormattedForDisplayAction(string id, string url)
		{
			return GetArticleTextFormattedForDisplay(id, url).AsAsyncAction();
		}

		internal async Task GetArticleTextFormattedForDisplay(string id, string url)
		{
			string result = String.Empty;

			Stream stream = null;

			try
			{
				stream = await GetStreamForReading(Constants.ARTICLE_FILENAME + id);
				return;
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

			//try to load it from pocket
			try
			{
				result = await (new PocketArticleViewIntegration(new TabletHttpUtility())).GetText(url);

				if (!string.IsNullOrEmpty(result))
				{
					await SaveArticleText(result, id);
				}
			}
			catch (Exception ex)
			{
			}
		}


		private async Task SaveArticleText(string content, string id)
		{
			try
			{
				Stream stream = null;

				try
				{
					stream = await GetStreamForPersistance(Constants.ARTICLE_FILENAME + id);

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
			catch (Exception ex)
			{
			}
		}

		private async Task<Stream> GetStreamForPersistance(string fileName)
		{
			StorageFolder userFolder = ApplicationData.Current.LocalFolder;

			StorageFile file = await userFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

			return await file.OpenStreamForWriteAsync();
		}

		public IAsyncAction LoadPersistedTypeAction(string fileName)
		{
			return LoadPersistedType(fileName).AsAsyncAction();
		}

		internal async Task LoadPersistedType(string fileName)
		{
			Stream stream = null;

			try
			{
				StorageFolder userFolder = ApplicationData.Current.LocalFolder;

				StorageFile file = await userFolder.GetFileAsync(fileName);

				stream = await file.OpenStreamForReadAsync();

				JsonSerializer js = new JsonSerializer();

				StreamReader reader = new StreamReader(stream);

				articles = js.JsonDeserialize<List<PocketItem>>(reader.ReadToEnd());
			}
			finally
			{
				if (stream != null)
				{
					stream.Dispose();
				}
			}
		}

		private async Task<Stream> GetStreamForReading(string fileName)
		{
			StorageFolder userFolder = ApplicationData.Current.LocalFolder;

			StorageFile file = await userFolder.GetFileAsync(fileName);

			return await file.OpenStreamForReadAsync();
		}
	}
}