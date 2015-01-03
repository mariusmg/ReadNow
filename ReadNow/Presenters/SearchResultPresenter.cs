using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using ReadNow.Portable;
using ReadNow.Views;

namespace ReadNow.Presenters
{
	public class SearchResultPresenter
	{
		private SearchResults page;

		public SearchResultPresenter(SearchResults page)
		{
			this.page = page;
		}

		public async void Search(string searchTerm)
		{
			if (ApplicationContext.Articles == null)
			{
				await (new ApplicationInitialization(new Storage())).LoadCachedData();
			}

			List<PocketItem> items = ApplicationContext.Articles.Where(item => item.ResolvedTitle.IndexOf(searchTerm, System.StringComparison.CurrentCultureIgnoreCase) > -1).Take(200).ToList();

			page.gridWithProgress.Visibility = Visibility.Collapsed;

			if (items.Count == 0)
			{
				page.gridNoResults.Visibility = Visibility.Visible;

				return;
			}

			page.listViewResults.ItemsSource = items;
			page.gridContent.Visibility = Visibility.Visible;

		}
	}
}