using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using ReadNow.Portable;
using ReadNow.Presenters;

namespace ReadNow.Views
{
	public sealed partial class SearchResults : Page
	{
		private SearchResultPresenter presenter;

		public SearchResults()
		{
			InitializeComponent();

			presenter = new SearchResultPresenter(this);
		}

		private void BackButton_OnClick(object sender, RoutedEventArgs e)
		{
			InternalNavigation ns = new InternalNavigation();
			if (ns.NavigateBack() == false)
			{
				ns.NavigateToMain();
			}
		}

		private void ListViewResults_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (listViewResults.SelectedItem == null)
			{
				return;
			}

			PocketItem item = listViewResults.SelectedItem as PocketItem;

			(new InternalNavigation()).NavigateToViewer(item);
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			presenter.Search(e.Parameter.ToString());
		}
	}
}