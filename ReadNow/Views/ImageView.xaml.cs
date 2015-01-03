using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using ReadNow.Portable;
using ReadNow.Presenters;

namespace ReadNow.Views
{
	public sealed partial class ImageView : BasePage
	{
		private ImageViewPresenter presenter = null;

		public ImageView()
		{
			InitializeComponent();

			presenter = new ImageViewPresenter(this);
		}

		private void ButtonAppBarRefresh_OnClick(object sender, RoutedEventArgs e)
		{
			HideAppBars();
			presenter.RefreshItems();
		}

		private async void ButtonAppBarDelete_OnClick(object sender, RoutedEventArgs e)
		{
			if (listBoxItems.SelectedItem == null)
			{
				return;
			}

			PocketItem item = (listBoxItems.SelectedItem as PocketItem);

			if (item == null)
			{
				return;
			}

			MessageDialog md = new MessageDialog("Remove \"" + (string.IsNullOrEmpty(item.GivenTitle) ? item.ResolvedTitle : item.GivenTitle) + "\" ?");
			md.Commands.Add(new UICommand("Yes"));
			md.Commands.Add(new UICommand("No"));

			IUICommand command = await md.ShowAsync();

			if (command.Label == "Yes")
			{
				await presenter.Remove(item);
				HideAppBars();
			}
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
		
			base.OnNavigatedTo(e);

			presenter.Display();

			presenter.RefreshItems();
		}
		
		private void ListBoxItems_OnTapped(object sender, TappedRoutedEventArgs e)
		{
			if (listBoxItems.SelectedItem == null)
			{
				return;
			}

			PocketItem item = listBoxItems.SelectedItem as PocketItem;

			(new InternalNavigation()).NavigateToImageContentViewer(item);

			e.Handled = true;
		}

		private void ListBoxItems_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			BottomAppBar.IsOpen = true;
		}

		private void ButtonAppBarArticles_OnClick(object sender, RoutedEventArgs e)
		{
			NavigateToArticleView();
		}

		private void ButtonAppBarVideos_OnClick(object sender, RoutedEventArgs e)
		{
			NavigateToVideoView();
		}
	}
}