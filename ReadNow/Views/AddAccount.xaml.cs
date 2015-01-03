using System;
using System.Linq;
using Windows.System;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using ReadNow.Presenters;

namespace ReadNow.Views
{
	public sealed partial class AddAccount : Page
	{
		public AddAccount()
		{
			InitializeComponent();
		}

		private void ButtonLogin_OnClick(object sender, RoutedEventArgs e)
		{
			(new AddAccountPresenter(this)).AddAccountWithPopup();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			SettingsPane.GetForCurrentView().CommandsRequested += AppCommandsRequested;
		}

		private void AppCommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
		{
			if (args.Request.ApplicationCommands.FirstOrDefault(command => command.Id.ToString() == BasePage.PRIVACY_POLICY_COMMAND_ID) == null)
			{
				SettingsCommand commandPrivacyPolicy = new SettingsCommand(BasePage.PRIVACY_POLICY_COMMAND_ID, "Privacy Policy",
					async x =>
					{
						try
						{
							await Launcher.LaunchUriAsync(new Uri(BasePage.PRIVACY_URL, UriKind.RelativeOrAbsolute));
						}
						catch (Exception ex)
						{
							new TabletLogger().Log(ex);
						}
					});

				args.Request.ApplicationCommands.Add(commandPrivacyPolicy);
			}
		}



	}
}