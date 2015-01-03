using Windows.Storage.Pickers.Provider;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using ReadNow.Portable;
using ReadNow.Presenters;

namespace ReadNow.Views
{
	public sealed partial class ImageContentViewer
	{
		private ImageContentViewerPresenter presenter;

		public ImageContentViewer()
		{
			InitializeComponent();

			presenter = new ImageContentViewerPresenter(this);
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			PocketItem item = e.Parameter as PocketItem;

			presenter.Display(item);

		}

		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			base.OnNavigatedFrom(e);
		}

		private void ButtonAppBarSave_OnClick(object sender, RoutedEventArgs e)
		{
			
		}
	}
}