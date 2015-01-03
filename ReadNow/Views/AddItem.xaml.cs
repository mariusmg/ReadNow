using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using ReadNow.Portable;

namespace ReadNow.Views
{
	public sealed partial class AddItem : Page
	{
		public AddItem()
		{
			InitializeComponent();
		}

		protected override async void OnNavigatedTo(NavigationEventArgs e)
		{
			if (e.Parameter == null)
			{
				return;
			}

			textBoxUrl.Text = e.Parameter.ToString();

			Uri uri = e.Parameter as Uri;

			ApplicationSettings settings = null;

			if (ApplicationContext.AppSettings == null)
			{
				try
				{
					settings = await (new Storage()).LoadPersistedType<ApplicationSettings>(Constants.APP_SETTINGS);
				}
				catch (Exception ex)
				{
					new TabletLogger().Log(ex);
					(new MessageDialog("To add items to Pocket, you must first log in.")).ShowAsync();
					return;
				}
			}
			else
			{
				settings = ApplicationContext.AppSettings;
			}

			AddItemResponse item = null;

			try
			{
				item = await (new PocketIntegration(new TabletLogger(), new TabletHttpUtility())).AddItem(settings.Token, uri.ToString());

				textboxAdded.Visibility = Visibility.Visible;

				progressBar.Visibility = Visibility.Collapsed;

			}
			catch (Exception ex)
			{
				(new MessageDialog("Server error occurred while adding item to Pocket.")).ShowAsync();
				new TabletLogger().Log(ex);
			}
		}
	}
}