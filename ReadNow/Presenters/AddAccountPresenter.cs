using System;
using Windows.Security.Authentication.Web;
using Windows.System;
using ReadNow.Portable;
using ReadNow.Views;

namespace ReadNow.Presenters
{
	public class AddAccountPresenter : BasePresenter
	{
		private const string REDIRECT_URI = App.POCKETNOW_PROTOCOL + "?code=";

		private AddAccount page;

		public AddAccountPresenter(AddAccount page)
		{
			this.page = page;
		}

		public async void AddAccountWithPopup()
		{
			page.buttonLogin.IsEnabled = false;

			try
			{
				TokenResponse tr = await (new PocketIntegration(new TabletLogger(), new TabletHttpUtility())).GetRequestToken();

				string url = string.Format("https://getpocket.com/auth/authorize?request_token={0}&webauthenticationbroker=1&redirect_uri={1}", tr.Code, REDIRECT_URI + tr.Code);

				WebAuthenticationResult result = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, new Uri(url), new Uri(REDIRECT_URI + tr.Code));

				if (result.ResponseStatus != WebAuthenticationStatus.Success)
				{
					return;
				}

				string code = result.ResponseData.Substring(REDIRECT_URI.Length + 1);

				(new InternalNavigation()).NavigateToLogin(code);
			}
			catch (Exception ex)
			{
				new TabletLogger().Log(ex);
			}
			finally
			{
				page.buttonLogin.IsEnabled = true;
			}
		}

		public async void AddAccountWithBrowser()
		{
			try
			{
				TokenResponse tr = await (new PocketIntegration(new TabletLogger(), new TabletHttpUtility())).GetRequestToken();
				string url = string.Format("https://getpocket.com/auth/authorize?request_token={0}&redirect_uri=" + REDIRECT_URI + tr.Code, tr.Code);

				//open browser
				await Launcher.LaunchUriAsync(new Uri(url, UriKind.Absolute));
			}
			catch (Exception ex)
			{
				new TabletLogger().Log(ex);

				ShowError("Failed to login");
			}
		}

	}
}