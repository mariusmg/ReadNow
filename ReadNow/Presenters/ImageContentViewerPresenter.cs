using System;
using Windows.UI.Xaml.Media.Imaging;
using ReadNow.Portable;
using ReadNow.Views;

namespace ReadNow.Presenters
{
	public class ImageContentViewerPresenter
	{
		private ImageContentViewer page;

		public ImageContentViewerPresenter(ImageContentViewer p)
		{
			page = p;
		}

		public void Display(PocketItem i)
		{
			page.image.Source = new BitmapImage(new Uri(i.GivenUrl, UriKind.RelativeOrAbsolute));
		}
	}
}