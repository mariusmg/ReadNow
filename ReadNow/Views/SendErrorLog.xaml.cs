// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

using System;
using System.Collections.Generic;
using System.Text;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using ReadNow.Common;
using ReadNow.Portable;
using ReadNow.Presenters;

namespace ReadNow.Views
{
	public sealed partial class SendErrorLog : LayoutAwarePage
	{
		public SendErrorLog()
		{
			InitializeComponent();
		}

		private void ButtonLog_OnClick(object sender, RoutedEventArgs e)
		{
			try
			{
				string data = textBoxData.Text.Trim();

				if (string.IsNullOrEmpty(data))
				{
					return;
				}

				(new VoidsoftApiIntegration()).SendErrorLog(data);

				(new BasePresenter()).ShowError("The error log was successfully sent. Thank you");

				(new InternalNavigation()).NavigateBack();
			}
			catch (Exception ex)
			{
				new TabletLogger().Log(ex);

				(new BasePresenter()).ShowError("The error log could not be sent. Please try again later");
			}
		}

		protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
		{
		}

		protected override void SaveState(Dictionary<String, Object> pageState)
		{
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			LoadData();
		}

		private async void LoadData()
		{
			try
			{
				string data = await new TabletLogger().GetLoggedDataForToday();

				if (string.IsNullOrEmpty(data))
				{
					(new BasePresenter()).ShowError("No errors logged");
					(new InternalNavigation()).NavigateBack();
				}

				StringBuilder builder = new StringBuilder();

				EasClientDeviceInformation info = new EasClientDeviceInformation();
				builder.Append("OS " + info.OperatingSystem + Environment.NewLine);
				builder.Append("Device Name " + info.SystemProductName + Environment.NewLine);

				textBoxData.Text = builder + Environment.NewLine + Environment.NewLine + data;
			}
			catch (Exception ex)
			{
				new TabletLogger().Log(ex);
			}
		}
	}
}