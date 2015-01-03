using System.Collections.ObjectModel;

namespace ReadNow.Portable
{
	public static class ApplicationContext
	{
		public static ApplicationSettings AppSettings
		{
			get;
			set;
		}

		public static ObservableCollection<PocketItem> Articles
		{
			get;
			set;
		}

		public static ObservableCollection<PocketItem> Images
		{
			get;
			set;
		}

		public static ObservableCollection<PocketItem> Videos
		{
			get;
			set;
		}
	}
}