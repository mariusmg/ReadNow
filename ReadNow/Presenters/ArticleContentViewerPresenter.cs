using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using ReadNow.Converters;
using ReadNow.Portable;
using ReadNow.Views;

namespace ReadNow.Presenters
{
	public class ArticleContentViewerPresenter
	{
		private ArticleViewer page;

		public ArticleContentViewerPresenter(ArticleViewer page)
		{
			this.page = page;

			
		}

		public async Task Display(PocketItem item)
		{
			ShowProgressBar(true);

			page.textBlockTitle.Text = (new UrlDisplayConverter()).Convert(item, typeof(PocketItem), null, string.Empty).ToString();

			string result = String.Empty;

			try
			{
				 result = await	(new BusinessObject(new Storage()).GetArticleTextFormattedForDisplay(item.Id, item.ResolvedUrl ?? item.GivenUrl, new TabletHttpUtility(), new TabletLogger()));

				page.webViewer.NavigateToString(result);
			}
			catch (Exception ex)
			{
				(new TabletLogger()).Log(ex);				
			}
			finally
			{
				ShowProgressBar(false);
			}
		}

		private void ShowProgressBar(bool show)
		{
			page.textBlockLoading.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
			page.webViewer.Visibility = !show ? Visibility.Visible : Visibility.Collapsed;
		}

		private async Task ShowErrorMessage(string message)
		{
			await (new MessageDialog(message)).ShowAsync();

			(new InternalNavigation()).NavigateBack();
		}
	}
}