using System;
using System.Linq;
using System.Threading.Tasks;

namespace ReadNow.Portable
{
	public class BusinessObject
	{
		private const string VIEWER_THEME = "<style> body {background-color:{0};color:{1};font-size:{2}px} </style>";

		private IFileSystemOperations fileSystem;

		public BusinessObject(IFileSystemOperations fs)
		{
			fileSystem = fs;
		}

		public async Task RemoveUserData()
		{
			await fileSystem.RemoveFile(Constants.DATA_VERSION_FILE);
			await fileSystem.RemoveFile(Constants.APP_SETTINGS);
			await fileSystem.RemoveFile(Constants.ARTICLES);
			await fileSystem.RemoveFile(Constants.IMAGES);
			await fileSystem.RemoveFile(Constants.VIDEOS);
		}

		public async Task PersistItems(ItemType type)
		{
			if (type == ItemType.Article)
			{
				await fileSystem.PersistType(Constants.ARTICLES, ApplicationContext.Articles.ToList());
			}
			else if (type == ItemType.Image)
			{
				await fileSystem.PersistType(Constants.IMAGES, ApplicationContext.Images.ToList());
			}
			else if (type == ItemType.Video)
			{
				await fileSystem.PersistType(Constants.VIDEOS, ApplicationContext.Videos.ToList());
			}
		}

		public Task PersistSettings()
		{
			return fileSystem.PersistType(Constants.APP_SETTINGS, ApplicationContext.AppSettings);
		}

		public Task PersistDataVersion()
		{
			return fileSystem.Persist(Constants.DATA_VERSION_FILE, Constants.DATA_VERSION.ToString());
		}


		public async Task<string> GetArticleTextFormattedForDisplay(string id, string url, IHttpUtility http, ILogger logger)
		{
			string result = String.Empty;

			try
			{

				//try to load it from disk
				try
				{
					result = await fileSystem.LoadContentFromFile(Constants.ARTICLE_FILENAME + id);

					if (!string.IsNullOrEmpty(result))
					{
						return FormatTextBeforeDisplay(result);
					}
				}
				catch
				{
					//ignore it. file doesn't exists.
				}

				//try to load it from pocket
				try
				{
					result = await (new PocketArticleViewIntegration(http)).GetText(url);

					if (!string.IsNullOrEmpty(result))
					{
						await SaveArticleText(result, id, logger);

						return FormatTextBeforeDisplay(result);
					}
				}
				catch (Exception ex)
				{
					logger.Log(ex);
				}
			}
			catch (Exception ex)
			{
				logger.Log(ex);
				throw;
			}

			throw new InvalidOperationException();
		}



		private async Task SaveArticleText(string content, string id, ILogger log)
		{
			try
			{
				await fileSystem.Persist(Constants.ARTICLE_FILENAME + id, content);
			}
			catch (Exception ex)
			{
				log.Log(ex);
			}
		}


		private string FormatTextBeforeDisplay(string input)
		{
			string final;

			if (ApplicationContext.AppSettings.DarkTheme)
			{
				final = VIEWER_THEME.Replace("{0}", "black");
				final = final.Replace("{1}", "white");
				final = final.Replace("{2}", ApplicationContext.AppSettings.ArticleFontSize);
			}
			else
			{
				final = VIEWER_THEME.Replace("{0}", "white");
				final = final.Replace("{1}", "black");
				final = final.Replace("{2}", ApplicationContext.AppSettings.ArticleFontSize);
			}

			return final + input;
		}
	}
}