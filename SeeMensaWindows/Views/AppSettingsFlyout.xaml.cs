using SeeMensaWindows.Common.DataModel;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace SeeMensaWindows.Views
{
    public sealed partial class AppSettingsFlyout : UserControl
    {
        public AppSettingsFlyout()
        {
            this.InitializeComponent();

            this.tsLiveTile.IsOn = MainViewModel.HasLiveTile;
        }

        private void BackClicked(object sender, RoutedEventArgs e)
        {
            if (this.Parent.GetType() == typeof(Popup))
            {
                ((Popup)this.Parent).IsOpen = false;
            }

            SettingsPane.Show();
        }

        private void SelectMensaClicked(object sender, RoutedEventArgs e)
        {
            MainViewModel.ResetSelectedMensa();
            
            Frame rootFrame = Window.Current.Content as Frame;

            rootFrame.Navigate(typeof(ItemsPage), "AllMensas");

            this.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private void tsLiveTileToggled(object sender, RoutedEventArgs e)
        {
            var toggle = sender as ToggleSwitch;

            if (toggle != null)
            {
                MainViewModel.HasLiveTile = toggle.IsOn;
            }
        }
    }
}
