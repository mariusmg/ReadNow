// The Settings Flyout item template is documented at http://go.microsoft.com/fwlink/?LinkId=273769
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ReadNow;
using ReadNow.Portable;

namespace ReadNow.Views.Settings
{
	public sealed partial class NewSettingsFlyout : SettingsFlyout
	{

		private bool isSetFromCode = false;

		public NewSettingsFlyout()
		{
			InitializeComponent();

			toogleSwitchIsDark.IsOn = ApplicationContext.AppSettings.DarkTheme;

			isSetFromCode = true;

			Dictionary<int, int> d = new Dictionary<int, int>();
			d.Add(0, 20);
			d.Add(1, 22);
			d.Add(3, 24);
			d.Add(4, 26);
			d.Add(5, 30);

			comboxboxFontSize.SelectedIndex = d.FirstOrDefault(pair => pair.Value == Convert.ToInt32(ApplicationContext.AppSettings.ArticleFontSize)).Key;

			comboxboxFontSize.SelectionChanged += Selector_OnSelectionChanged;

			Unloaded += Settings_Unloaded;
		}

		private void Settings_Unloaded(object sender, RoutedEventArgs e)
		{
			comboxboxFontSize.SelectionChanged -= Selector_OnSelectionChanged;
		}

		private async void ToggleSwitch_OnToggled(object sender, RoutedEventArgs e)
		{

			if (isSetFromCode == false)
			{
				return;
			}

			ApplicationContext.AppSettings.DarkTheme = toogleSwitchIsDark.IsOn;

			try
			{
				await (new BusinessObject(new Storage())).PersistSettings();

				SaveTheme();

				MessageDialog ms = new MessageDialog("Please restart the app to see the new theme");
				ms.ShowAsync();
			}
			catch (Exception ex)
			{
				new TabletLogger().Log(ex);
			}
		}

		private void SaveTheme()
		{
			ApplicationDataContainer settings = ApplicationData.Current.RoamingSettings;

			if (settings.Values.ContainsKey("currentTheme"))
			{
				settings.Values.Remove("currentTheme");
			}

			settings.Values.Add("currentTheme", ApplicationContext.AppSettings.DarkTheme ? "dark" : "light");
		}

		private async void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ApplicationContext.AppSettings.ArticleFontSize = (comboxboxFontSize.SelectedValue as ComboBoxItem).Content.ToString();

			try
			{
				await (new BusinessObject(new Storage())).PersistSettings();
			}
			catch (Exception ex)
			{
				new TabletLogger().Log(ex);
			}
		}
	}
}