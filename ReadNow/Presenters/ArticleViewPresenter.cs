using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using ReadNow.Portable;
using ReadNow.Views;

namespace ReadNow.Presenters
{
	public class ArticleViewPresenter
	{
		private ArticleView page;

		public ArticleViewPresenter(ArticleView page)
		{
			this.page = page;
		}

		public void Display()
		{
			ShowItemsList(true);

			page.listBoxItems.Items.Clear();

			page.listBoxItems.ItemTemplate = Application.Current.Resources["articleTemplate"] as DataTemplate;
			page.listBoxItems.ItemsSource = ApplicationContext.Articles;
		}

		public void DisplayItems()
		{
			Display();

			RefreshItems();
		}

		public async Task Remove(PocketItem pi)
		{
			page.progressBar.Visibility = Visibility.Visible;

			try
			{
				ActionModelResult response = await (new PocketIntegration(new TabletLogger(), new TabletHttpUtility())).DeleteItem(ApplicationContext.AppSettings.Token, pi.Id);

				if (response.Results[0] && response.Status == 1)
				{
					//remove from list 
					ApplicationContext.Articles.Remove(ApplicationContext.Articles.FirstOrDefault(item => item.Id == pi.Id));

					await (new BusinessObject(new Storage())).PersistItems(ItemType.Article);
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

		public async void RefreshItems()
		{
			try
			{
				page.progressBar.Visibility = Visibility.Visible;

				ItemResponse itemResponse = await (new PocketIntegration(new TabletLogger(), new TabletHttpUtility())).RefreshArticles(ApplicationContext.AppSettings.Token, ApplicationContext.AppSettings.Since);

				if (itemResponse.Item == null)
				{
					return;
				}

				if ((new EntityDiff()).HasChanges(itemResponse.Item.Values.ToList(), ApplicationContext.Articles.ToList()))
				{
					IOrderedEnumerable<PocketItem> orderedEnumerable = itemResponse.Item.Values.ToList().OrderBy(item => item.SortId);

					ApplicationContext.Articles.Clear();

					foreach (PocketItem item in orderedEnumerable)
					{
						ApplicationContext.Articles.Add(item);
					}

					await (new BusinessObject(new Storage())).PersistItems(ItemType.Article);

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

		private void ShowItemsList(bool show)
		{
			page.listBoxItems.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
			page.stackPanelNoItems.Visibility = show ? Visibility.Collapsed : Visibility.Visible;
		}
	}
}