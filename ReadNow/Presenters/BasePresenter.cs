using System;
using Windows.UI.Popups;

namespace ReadNow.Presenters
{
	public class BasePresenter
	{
		public async void ShowError(string message)
		{
			await (new MessageDialog(message)).ShowAsync();
		}
	}
}