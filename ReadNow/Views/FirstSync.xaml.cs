using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using ReadNow.Presenters;

namespace ReadNow.Views
{
	public sealed partial class FirstSync : Page
	{
		private FirstSyncPresenter presenter;

		public FirstSync()
		{
			InitializeComponent();

			presenter = new FirstSyncPresenter(this);
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			presenter.DoUserLogin(e.Parameter.ToString());
		}
	}
}