using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using ReadNow.Portable;
using ReadNow.Presenters;

namespace ReadNow.Views
{
	public sealed partial class ArticleViewer : BasePage
	{
		private PocketItem item;
		private ArticleContentViewerPresenter presenter;

		public ArticleViewer()
		{
			InitializeComponent();

			presenter = new ArticleContentViewerPresenter(this);
		}

		private void ButtonBack_OnClick(object sender, RoutedEventArgs e)
		{
			(new InternalNavigation()).NavigateBack();
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

			item = e.Parameter as PocketItem;
			presenter.Display(item);
		}
	}
}