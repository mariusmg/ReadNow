using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.DataTransfer.ShareTarget;
using Windows.Storage;
using Windows.System;
using Windows.UI.ApplicationSettings;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ReadNow.Portable;
using ReadNow.Presenters;
using ReadNow.Views;
using ReadNow.Views.Settings;

namespace ReadNow
{
	sealed partial class App : Application
	{
		public const string POCKETNOW_PROTOCOL = "pocketnow://";

		public App()
		{
			InitializeComponent();

			try
			{
				ApplicationDataContainer settings = ApplicationData.Current.RoamingSettings;

				if (settings.Values.ContainsKey("currentTheme") && (string) settings.Values["currentTheme"] == "dark")
				{
					RequestedTheme = ApplicationTheme.Dark;
				}
				else
				{
					RequestedTheme = ApplicationTheme.Light;
				}
			}
			catch
			{
				RequestedTheme = ApplicationTheme.Light;
			}

			Suspending += OnSuspending;
		}

		private void OnSuspending(object sender, SuspendingEventArgs e)
		{
			var deferral = e.SuspendingOperation.GetDeferral();
			//TODO: Save application state and stop any background activity
			deferral.Complete();
		}

		private async void StartApplication()
		{
			Storage s = new Storage();

			try
			{
				ApplicationSettings settings = await s.LoadPersistedType<ApplicationSettings>(Constants.APP_SETTINGS);

				ApplicationContext.AppSettings = settings;

				ApplicationContext.AppSettings.DarkTheme = (RequestedTheme == ApplicationTheme.Dark);
			}
			catch (Exception ex)
			{
				(new TabletLogger()).Log(ex);

				(new InternalNavigation()).NavigateToAddAccount();

				return;
			}

			await (new ApplicationInitialization(new Storage())).LoadCachedData();

			(new InternalNavigation()).NavigateToMain();
		}

		protected override void OnActivated(IActivatedEventArgs args)
		{
			if (args.Kind == ActivationKind.Protocol)
			{
				ProtocolActivatedEventArgs protocolArgs = args as ProtocolActivatedEventArgs;

				if (protocolArgs.Uri.ToString().ToLowerInvariant() == POCKETNOW_PROTOCOL.ToLowerInvariant())
				{
					//no request token.so just redirect to add account
					return;
				}

				//extract the request token from here
				string code = protocolArgs.Uri.ToString().Substring(POCKETNOW_PROTOCOL.Length);

				code = code.Replace("/", "");

				(new InternalNavigation()).NavigateToLogin(code);
			}
			else
			{
				base.OnActivated(args);
			}
		}

		protected override void OnSearchActivated(SearchActivatedEventArgs args)
		{
			(new InternalNavigation()).NavigateToSearchResults(args.QueryText);

			Window.Current.Activate();
		}

		protected override async void OnLaunched(LaunchActivatedEventArgs args)
		{
			Frame rootFrame = Window.Current.Content as Frame;

			// Do not repeat app initialization when the Window already has content,
			// just ensure that the window is active
			if (rootFrame == null)
			{
				// Create a Frame to act as the navigation context and navigate to the first page
				rootFrame = new Frame();

				if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
				{
					//TODO: Load state from previously suspended application
				}

				// Place the frame in the current Window
				Window.Current.Content = rootFrame;
			}

			if (rootFrame.Content == null)
			{
				StartApplication();
			}

			// Ensure the current window is active
			Window.Current.Activate();
		}

		protected override async void OnShareTargetActivated(ShareTargetActivatedEventArgs args)
		{
			base.OnShareTargetActivated(args);

			ShareOperation op = args.ShareOperation;

			DataPackageView dataPackageView = op.Data;

			Uri uri = null;

			try
			{
				uri = await dataPackageView.GetUriAsync();
			}
			catch (Exception ex)
			{
				new TabletLogger().Log(ex);
			}

			if (uri == null)
			{
				return;
			}

			var rootFrame = new Frame();
			rootFrame.Navigate(typeof (AddItem), uri);
			Window.Current.Content = rootFrame;
			Window.Current.Activate();
		}

		private void OnUiCommandInvokedHandler(IUICommand x)
		{
			NewSettingsFlyout settings = new NewSettingsFlyout();
			settings.Title = "Settings";
			settings.ShowIndependent();
		}

		protected override void OnWindowCreated(WindowCreatedEventArgs args)
		{
			SettingsCommand commandSettings = new SettingsCommand(BasePage.SETTINGS_COMMAND_ID, "Settings", OnUiCommandInvokedHandler);

			SettingsCommand commandLogOut = new SettingsCommand(BasePage.LOGOUT_COMMAND_ID, "Log out", async x =>
			{
				try
				{
					IUICommand uiCommand = await ShowQuestionMessage("Are you sure you want to log out ?");

					if (uiCommand.Label == BasePage.CANCEL)
					{
						return;
					}

					ApplicationContext.Articles.Clear();
					ApplicationContext.Images.Clear();
					ApplicationContext.Videos.Clear();

					ApplicationContext.AppSettings = null;

					try
					{
						await (new BusinessObject(new Storage())).RemoveUserData();
					}
					catch (Exception ex)
					{
						new TabletLogger().Log(ex);
					}

					(new InternalNavigation()).NavigateToAddAccount();
				}
				catch (Exception ex)
				{
					new TabletLogger().Log(ex);
				}
			});

			SettingsCommand commandFeedback = new SettingsCommand(BasePage.FEEDBACK_COMMAND_ID, "Feedback", async x =>
			{
				try
				{
					bool result = await Launcher.LaunchUriAsync(new Uri("mailto:gmarius@gmail.com", UriKind.RelativeOrAbsolute));

					if (result == false)
					{
						(new BasePresenter()).ShowError("Can't open default mail application. Please send a email to gmarius@gmail.com");
					}
				}
				catch (Exception ex)
				{
					new TabletLogger().Log(ex);
					(new BasePresenter()).ShowError("Can't open default mail application. Please send a email to gmarius@gmail.com");
				}
			});

			SettingsCommand commandErrorLog = new SettingsCommand(BasePage.FEEDBACK_COMMAND_ID, "Error Log", async x =>
			{
				try
				{
					(new InternalNavigation()).NavigateToSendErrorLog();
				}
				catch (Exception ex)
				{
					new TabletLogger().Log(ex);
				}
			});

			SettingsPane.GetForCurrentView().CommandsRequested += (s, e) =>
			{
				e.Request.ApplicationCommands.Add(commandSettings);
				e.Request.ApplicationCommands.Add(commandLogOut);
				e.Request.ApplicationCommands.Add(commandFeedback);
				e.Request.ApplicationCommands.Add(commandErrorLog);
			};

			base.OnWindowCreated(args);
		}

		public async Task<IUICommand> ShowQuestionMessage(string message)
		{
			MessageDialog dialog = new MessageDialog(message);
			dialog.Commands.Add(new UICommand("OK"));
			dialog.Commands.Add(new UICommand(BasePage.CANCEL));

			IUICommand uiCommand = await dialog.ShowAsync();
			return uiCommand;
		}
	}
}