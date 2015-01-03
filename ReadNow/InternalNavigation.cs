using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ReadNow.Portable;
using ReadNow.Views;

namespace ReadNow
{
	public class InternalNavigation
	{
		public void NavigateToLogin(string code)
		{
			NavigateTo<FirstSync>(code);
		}

		public void NavigateToAddAccount()
		{
			NavigateTo<AddAccount>();
		}


		public void NavigateToVideosList()
		{
			NavigateTo<VideosView>();
		}

		public void NavigateToSendErrorLog()
		{
			NavigateTo<SendErrorLog>();
		}


		public void NavigateToMain()
		{
			NavigateTo<ArticleView>();
		}


		public void NavigateToVideoViewer(PocketVideoBinding item)
		{
			NavigateTo<VideoContentViewer>(item);
		}


		public void NavigateToViewer(PocketItem item)
		{
			NavigateTo<ArticleViewer>(item);
		}


		public void NavigateToImageContentViewer(PocketItem item)
		{
			NavigateTo<ImageContentViewer>(item);
		}

		
		public void NavigateToImageView()
		{
			NavigateTo<ImageView>();
		}

		public void NavigateToArticleView()
		{
			NavigateTo<ArticleView>();
		}

		public void NavigateToSearchResults(object searchTerm)
		{
			NavigateTo<SearchResults>(searchTerm);
		}


		public void NavigateToAddItem(object parameter = null)
		{
			NavigateTo<AddItem>(parameter);
		}

		public bool NavigateBack()
		{
			Frame rootFrame = Window.Current.Content as Frame;

			if (rootFrame.CanGoBack)
			{
				rootFrame.GoBack();
				return true;
			}

			return false;
		}

		private void NavigateTo<T>(object param = null)
		{
			Frame rootFrame = Window.Current.Content as Frame;

			if (rootFrame != null)
			{
				rootFrame.Navigate(typeof (T), param);
			}
			else
			{
				rootFrame = new Frame();
				Window.Current.Content = rootFrame;
				rootFrame.Navigate(typeof (T), param);
			}
		}
	}
}