using System;
using System.Collections.Generic;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using ReadNow.Portable;
using ReadNow.Presenters;

namespace ReadNow.Views
{
	public sealed partial class VideosView : BasePage
	{
		private VideosViewPresenter presenter;

		public VideosView()
		{
			InitializeComponent();

			presenter = new VideosViewPresenter(this);

			Window.Current.SizeChanged += Current_SizeChanged;
		}

		private void ButtonAppBarArticles_OnClick(object sender, RoutedEventArgs e)
		{
			NavigateToArticleView();
		}

		private async void ButtonAppBarDelete_OnClick(object sender, RoutedEventArgs e)
		{
			if (listBoxVideos.SelectedItem == null)
			{
				return;
			}

			PocketVideoBinding item = (listBoxVideos.SelectedItem as PocketVideoBinding);

			if (item == null)
			{
				return;
			}

			MessageDialog md = new MessageDialog("Remove \"" + (string.IsNullOrEmpty(item.Item.GivenTitle) ? item.Item.ResolvedTitle : item.Item.GivenTitle) + "\" ?");
			md.Commands.Add(new UICommand("Yes"));
			md.Commands.Add(new UICommand("No"));

			IUICommand command = await md.ShowAsync();

			if (command.Label == "Yes")
			{
				try
				{
					await presenter.Remove(item.Item);
					HideAppBars();
					presenter.DisplayItems();
				}
				catch (Exception ex)
				{
					(new TabletLogger()).Log(ex);
				}
			}
		}

		private void ButtonAppBarImages_OnClick(object sender, RoutedEventArgs e)
		{
			NavigateToImageView();
		}

		private void ButtonAppBarRefresh_OnClick(object sender, RoutedEventArgs e)
		{
			try
			{
				presenter.RefreshItems();
			}
			catch (Exception ex)
			{
				(new TabletLogger()).Log(ex);
			}
		}

		private void Current_SizeChanged(object sender, WindowSizeChangedEventArgs e)
		{
		}

		private void ListBoxVideos_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			BottomAppBar.IsOpen = true;
		}

		private async void ListBoxVideos_OnTapped(object sender, TappedRoutedEventArgs e)
		{
			if (listBoxVideos.SelectedItem == null)
			{
				return;
			}

			try
			{
				PocketVideoBinding item = listBoxVideos.SelectedItem as PocketVideoBinding;
				await Launcher.LaunchUriAsync(new Uri(item.Video.Url), new LauncherOptions());
				e.Handled = true;
			}
			catch (Exception ex)
			{
				(new TabletLogger()).Log(ex);
			}
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			try
			{
				base.OnNavigatedTo(e);
				presenter.DisplayItems();
			}
			catch (Exception ex)
			{
				(new TabletLogger()).Log(ex);
			}
		}

		protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
		{
		}

		protected override void SaveState(Dictionary<String, Object> pageState)
		{
		}
	}
}