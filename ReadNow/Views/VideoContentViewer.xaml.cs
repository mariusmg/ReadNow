using System;
using System.Collections.Generic;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using ReadNow.Common;
using ReadNow.Portable;

namespace ReadNow.Views
{
	public sealed partial class VideoContentViewer : LayoutAwarePage
	{

		private PocketVideoBinding video;

		public VideoContentViewer()
		{
			InitializeComponent();
		}



		protected async override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			video = e.Parameter as PocketVideoBinding;

			await Launcher.LaunchUriAsync(new Uri(video.Video.Url), new LauncherOptions());

			//await Launcher.LaunchUriAsync(new Uri(video.Video.Url),new LauncherOptions(){ContentType = "video/mp4" });


			//mediaElement.Source = new Uri(video.Video.Url,UriKind.RelativeOrAbsolute);
			//mediaElement.Play();
		}

		protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
		{
		}

		protected override void SaveState(Dictionary<String, Object> pageState)
		{
		}

		private void MediaElement_OnMediaFailed(object sender, ExceptionRoutedEventArgs e)
		{
		}
	}
}