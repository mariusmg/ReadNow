using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using ReadNow.Portable;
using ReadNow.Views;

namespace ReadNow.Presenters
{
	internal class VideosViewPresenter : BasePresenter
	{
		private List<PocketVideoBinding> bindings = null;
		private VideosView page;

		public VideosViewPresenter(VideosView page)
		{
			this.page = page;
		}

		public void Display()
		{
			try
			{
				page.listBoxVideos.ItemsSource = null;

				page.listBoxVideos.ItemTemplate = Application.Current.Resources["videosTemplate"] as DataTemplate;

				CreateBindings();

				if (ApplicationContext.Videos.Count == 0)
				{
					page.stackPanelNoItems.Visibility = Visibility.Visible;
					page.listBoxVideos.Visibility = Visibility.Visible;
				}
				else if (page.listBoxVideos.Visibility == Visibility.Collapsed)
				{
					page.stackPanelNoItems.Visibility = Visibility.Collapsed;
					page.listBoxVideos.Visibility = Visibility.Visible;
				}
			}
			catch (Exception ex)
			{
				(new TabletLogger()).Log(ex);
			}
		}


		private void CreateBindings()
		{
			bindings = new List<PocketVideoBinding>();

			foreach (PocketItem item in ApplicationContext.Videos)
			{
				try
				{
					if (item.Videos.Count == 0)
					{
						continue;
					}

					foreach (KeyValuePair<string, PocketVideo> pair in item.Videos)
					{
						PocketVideoBinding p = new PocketVideoBinding();
						p.Video = pair.Value;
						p.ImageUrl = item.Images.Count > 0 ? item.Images.FirstOrDefault().Value.Url : "";
						p.Item = item;
						bindings.Add(p);
					}
				}
				catch
				{
					continue;
				}
			}

			page.listBoxVideos.ItemsSource = bindings;
		}


		public void DisplayItems()
		{
			Display();
			RefreshItems();
		}


		public async void RefreshItems()
		{
			try
			{
				page.progressBar.Visibility = Visibility.Visible;

				ItemResponse itemResponse = await (new PocketIntegration(new TabletLogger(), new TabletHttpUtility())).RefreshVideos(ApplicationContext.AppSettings.Token, ApplicationContext.AppSettings.Since);

				//nothing
				if (itemResponse.Item == null)
				{
					return;
				}

				if ((new EntityDiff()).HasChanges(itemResponse.Item.Values.ToList(), ApplicationContext.Videos.ToList()))
				{
					IOrderedEnumerable<PocketItem> orderedEnumerable = itemResponse.Item.Values.ToList().OrderBy(item => item.SortId);

					ApplicationContext.Videos.Clear();

					foreach (PocketItem item in orderedEnumerable)
					{
						ApplicationContext.Videos.Add(item);
					}

					await (new BusinessObject(new Storage())).PersistItems(ItemType.Video);

					Display();
				}
			}
			catch (Exception ex)
			{
				new TabletLogger().Log(ex);
			}
			finally
			{
				page.progressBar.Visibility = Visibility.Collapsed;
			}
		}


		public async Task Remove(PocketItem pi)
		{
			page.progressBar.Visibility = Visibility.Visible;

			try
			{
				ActionModelResult response =
					await (new PocketIntegration(new TabletLogger(), new TabletHttpUtility())).DeleteItem(ApplicationContext.AppSettings.Token, pi.Id);

				if (response.Results[0] && response.Status == 1)
				{
					//remove from list 
					ApplicationContext.Videos.Remove(ApplicationContext.Videos.FirstOrDefault(item => item.Id == pi.Id));

					await (new BusinessObject(new Storage())).PersistItems(ItemType.Video);
				}
				else
				{
					await (new MessageDialog("Operation failed. Please try again")).ShowAsync();
				}
			}
			catch (Exception ex)
			{
				new TabletLogger().Log(ex);
				(new MessageDialog("Operation failed. Please try again")).ShowAsync();
			}
			finally
			{
				page.progressBar.Visibility = Visibility.Collapsed;
			}
		}
	}
}