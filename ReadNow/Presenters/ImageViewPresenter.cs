using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using ReadNow.Portable;
using ReadNow.Views;

namespace ReadNow.Presenters
{
	public class ImageViewPresenter
	{
		private ImageView page;

		public ImageViewPresenter(ImageView page)
		{
			this.page = page;
		}

		public void Display()
		{

			try
			{

				page.listBoxItems.Items.Clear();
				page.listBoxItems.ItemsSource = ApplicationContext.Images;

				if (ApplicationContext.Images.Count == 0)
				{
					page.stackPanelNoItems.Visibility = Visibility.Visible;
					page.listBoxItems.Visibility = Visibility.Visible;
				}
				else if (page.listBoxItems.Visibility == Visibility.Collapsed)
				{
					page.stackPanelNoItems.Visibility = Visibility.Collapsed;
					page.listBoxItems.Visibility = Visibility.Visible;
				}

			}
			catch (Exception ex)
			{
				(new TabletLogger()).Log(ex);
			}
		}


		public async void RefreshItems()
		{
			try
			{
				page.progressBar.Visibility = Visibility.Visible;

				ItemResponse itemResponse = await(new PocketIntegration(new TabletLogger(), new TabletHttpUtility())).RefreshImages(ApplicationContext.AppSettings.Token, ApplicationContext.AppSettings.Since);

				if (itemResponse.Item == null)
				{
					return;
				}

				if ((new EntityDiff()).HasChanges(itemResponse.Item.Values.ToList(), ApplicationContext.Images.ToList()))
				{
					IOrderedEnumerable<PocketItem> orderedEnumerable = itemResponse.Item.Values.ToList().OrderBy(item => item.SortId);

					ApplicationContext.Images.Clear();

					foreach (PocketItem item in orderedEnumerable)
					{
						ApplicationContext.Images.Add(item);
					}

					await(new BusinessObject(new Storage())).PersistItems(ItemType.Image);

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
				ActionModelResult response = await(new PocketIntegration(new TabletLogger(), new TabletHttpUtility())).DeleteItem(ApplicationContext.AppSettings.Token, pi.Id);

				if (response.Results[0] && response.Status == 1)
				{
					//remove from list 
					ApplicationContext.Images.Remove(ApplicationContext.Images.FirstOrDefault(item => item.Id == pi.Id));

					await(new BusinessObject(new Storage())).PersistItems(ItemType.Image);
				}
				else
				{
					await(new MessageDialog("Operation failed. Please try again")).ShowAsync();
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