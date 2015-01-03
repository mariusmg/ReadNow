using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ReadNow.Portable;

namespace PocketNowPortableLibrary
{
	public class MassArticleContentDownloader
	{
		public async Task CacheArticlesContent(IList<PocketItem> items, IFileSystemOperations fs, IHttpUtility http, ILogger logger)
		{
			foreach (PocketItem pi in ApplicationContext.Articles)
			{
				try
				{
					//try
					//{
					//	//see if the article is already cached
					//	await fs.LoadContentFromFile(Constants.ARTICLE_FILENAME + pi.Id);

					//	continue;
					//}
					//catch
					//{

					//}

					await (new BusinessObject(fs)).GetArticleTextFormattedForDisplay(pi.Id, pi.ResolvedId ?? pi.GivenUrl, http, logger);
				}
				catch (Exception ex)
				{
					continue;
				}
			}
		}
	}
}