using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using ReadNow.Common;

namespace ReadNow.Views
{
	public class BasePage : LayoutAwarePage
	{
		public const string SETTINGS_COMMAND_ID = "settings";
		public const string LOGOUT_COMMAND_ID = "logout";
		public const string FEEDBACK_COMMAND_ID = "feedback";
		public const string ERROR_LOG_COMMAND_ID = "error_log";
		public const string PRIVACY_POLICY_COMMAND_ID = "privacy_policy";
		public const string CANCEL = "Cancel";
		public const string PRIVACY_URL = "http://voidsoft.ro/pocketnow/privacy.html";

		public BasePage()
		{
			this.SizeChanged += OnSizeChanged;
		}

		private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
		{

			if (ApplicationView.GetForCurrentView().IsFullScreen)
			{
				OnFullScreen();
			}
			else
			{
				OnSmallSize();
			}
		}


		public virtual void OnFullScreen()
		{
		}


		public virtual void OnSmallSize()
		{
			
		}



	protected void NavigateToArticleView()
		{
			(new InternalNavigation()).NavigateToArticleView();
		}

		protected void NavigateToVideoView()
		{
			(new InternalNavigation()).NavigateToVideosList();
		}

		protected void NavigateToImageView()
		{
			(new InternalNavigation()).NavigateToImageView();
		}

		protected void HideAppBars()
		{
			BottomAppBar.IsOpen = false;
		}

		public async Task<IUICommand> ShowQuestionMessage(string message)
		{
			MessageDialog dialog = new MessageDialog(message);
			dialog.Commands.Add(new UICommand("OK"));
			dialog.Commands.Add(new UICommand(CANCEL));

			IUICommand uiCommand = await dialog.ShowAsync();
			return uiCommand;
		}
	}
}