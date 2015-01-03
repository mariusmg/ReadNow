using System;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Background;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using ReadNow.Portable;
using ReadNow.Presenters;
using ReadNow.Views.Settings;

namespace ReadNow.Views
{
	public sealed partial class ArticleView : BasePage
	{
		private const string BACKGROUND_TASK_NAME = "PocketNow Background Downloader";

		private ArticleViewPresenter presenter;

		public ArticleView()
		{
			InitializeComponent();
			presenter = new ArticleViewPresenter(this);
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

		private void ButtonAppBarRefresh_OnClick(object sender, RoutedEventArgs e)
		{
			HideAppBars();
			presenter.RefreshItems();
		}

		public override void OnFullScreen()
		{
			listBoxItems.ItemTemplate = Application.Current.Resources["articleTemplate"] as DataTemplate;
		}

		public override void OnSmallSize()
		{
			listBoxItems.ItemTemplate = Application.Current.Resources["articleTemplateSmall"] as DataTemplate;
		}

		private void ListBoxItems_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			BottomAppBar.IsOpen = true;
		}

		private void ListBoxItems_OnTapped(object sender, TappedRoutedEventArgs e)
		{
			if (listBoxItems.SelectedItem == null)
			{
				return;
			}

			PocketItem item = listBoxItems.SelectedItem as PocketItem;

			(new InternalNavigation()).NavigateToViewer(item);

			e.Handled = true;
		}

		private void OnTaskCompleted(BackgroundTaskRegistration sender, BackgroundTaskCompletedEventArgs args)
		{
		}

		private void OnTaskProgress(BackgroundTaskRegistration sender, BackgroundTaskProgressEventArgs args)
		{
		}

		public T GetParentDataContext<T>(FrameworkElement e) where T : class
		{
			if (e == null)
			{
				return null;
			}

			if (e.DataContext is T)
			{
				return e.DataContext as T;
			}
			return GetParentDataContext<T>(e.Parent as FrameworkElement);
		}

		private void ButtonFavorite_OnClick(object sender, RoutedEventArgs e)
		{
			if (listBoxItems.SelectedItem == null)
			{
				return;
			}

			PocketItem item = listBoxItems.SelectedItem as PocketItem;
		}

		protected override async void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			presenter.DisplayItems();

			//start the background task
			try
			{
				RegisterBackgroundTask();
			}
			catch (Exception ex)
			{
				(new TabletLogger()).Log(ex);
			}
		}

		private void RegisterBackgroundTask()
		{
			try
			{
				KeyValuePair<Guid, IBackgroundTaskRegistration> keyValuePair = BackgroundTaskRegistration.AllTasks.FirstOrDefault(pair => pair.Value.Name == BACKGROUND_TASK_NAME);

				if (keyValuePair.Value == null)
				{
					BackgroundTaskBuilder taskBuilder = new BackgroundTaskBuilder();

					taskBuilder.Name = BACKGROUND_TASK_NAME;

					taskBuilder.TaskEntryPoint = "PocketNowBackgroundTasks.ItemDownloader";

					taskBuilder.SetTrigger(new MaintenanceTrigger(15, false));

					taskBuilder.SetTrigger(new SystemTrigger(SystemTriggerType.InternetAvailable, false));

					BackgroundTaskRegistration task = taskBuilder.Register();

					task.Completed += OnTaskCompleted;
					task.Progress += OnTaskProgress;
				}
				else
				{
					keyValuePair.Value.Progress += OnTaskProgress;

					keyValuePair.Value.Completed += OnTaskCompleted;
				}
			}
			catch (Exception ex)
			{
				(new TabletLogger()).Log(ex);
			}
		}

		private void ButtonAppBarImages_OnClick(object sender, RoutedEventArgs e)
		{
			NavigateToImageView();
		}

		private void ButtonAppBarVideos_OnClick(object sender, RoutedEventArgs e)
		{
			NavigateToVideoView();
		}

		private void ButtonSettings_OnClick(object sender, RoutedEventArgs e)
		{
			NewSettingsFlyout settings = new NewSettingsFlyout();
			settings.Title = "Settings";
			settings.ShowIndependent();
		}

		private void ButtonSwitchContent_OnClick(object sender, RoutedEventArgs e)
		{
			this.TopAppBar.IsOpen = true;
			this.BottomAppBar.IsOpen = true;
		}
	}
}