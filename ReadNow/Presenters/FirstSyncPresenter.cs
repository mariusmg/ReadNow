using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using ReadNow.Portable;
using ReadNow.Views;

namespace ReadNow.Presenters
{
	public class FirstSyncPresenter : BasePresenter
	{
		private FirstSync page;

		public FirstSyncPresenter(FirstSync page)
		{
			this.page = page;
		}

		public async void DoUserLogin(string code)
		{
			AuthorizeResponse ar = null;

			TabletLogger logger = new TabletLogger();
			TabletHttpUtility http = new TabletHttpUtility();

			//authorize the token
			try
			{
				ar = await (new PocketIntegration(logger, http)).DoAuthorization(code);
			}
			catch (WebException wex)
			{
				new TabletLogger().Log(wex);
			}
			catch (Exception ex)
			{
				new TabletLogger().Log(ex);
			}

			//load articles and persist items
			try
			{
				page.textBlockCurrentOperation.Text = "Downloading articles...";

				ItemResponse item = await (new PocketIntegration(logger, http)).GetAllItems(ar.AccessToken, ItemType.Article);

				ApplicationContext.Articles = new ObservableCollection<PocketItem>();

				if (item.Item != null)
				{
					PocketItem[] sortedArticles = item.Item.Values.OrderBy(pocketItem => pocketItem.SortId).ToArray();

					foreach (PocketItem p in sortedArticles)
					{
						ApplicationContext.Articles.Add(p);
					}

					//persist items
					await (new Storage()).PersistType(Constants.ARTICLES, item.Item.Values);
				}

				page.textBlockCurrentOperation.Text = "Downloading images...";
				ItemResponse itemImage = await (new PocketIntegration(logger, http)).GetAllItems(ar.AccessToken, ItemType.Image);
				ApplicationContext.Images = new ObservableCollection<PocketItem>();

				if (itemImage.Item != null)
				{
					PocketItem[] sortedImages = itemImage.Item.Values.OrderBy(pocketItem => pocketItem.SortId).ToArray();

					foreach (PocketItem p in sortedImages)
					{
						ApplicationContext.Images.Add(p);
					}

					//persist items
					await (new Storage()).PersistType(Constants.IMAGES, itemImage.Item.Values);
				}

				page.textBlockCurrentOperation.Text = "Downloading videos...";
				ItemResponse itemVideo = await (new PocketIntegration(logger, http)).GetAllItems(ar.AccessToken, ItemType.Video);
				ApplicationContext.Videos = new ObservableCollection<PocketItem>();

				if (itemVideo.Item != null)
				{
					PocketItem[] sortedVideo = itemVideo.Item.Values.OrderBy(pocketItem => pocketItem.SortId).ToArray();

					foreach (PocketItem p in sortedVideo)
					{
						ApplicationContext.Videos.Add(p);
					}

					//persist items
					await (new Storage()).PersistType(Constants.VIDEOS, itemVideo.Item.Values);
				}

				ApplicationSettings settings = new ApplicationSettings();
				settings.Since = item.Since;
				settings.Token = ar.AccessToken;
				settings.DarkTheme = false;
				settings.ArticleFontSize = "22";

				ApplicationContext.AppSettings = settings;

				Storage io = new Storage();

				await (new BusinessObject(io)).PersistSettings();

				await (new BusinessObject(io)).PersistDataVersion();

				(new InternalNavigation()).NavigateToMain();
			}
			catch (Exception ex)
			{
				new TabletLogger().Log(ex);
			}
		}
	}
}