using System;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace SeeMensaWindows.Views
{
    public sealed partial class HelpFlyout : UserControl
    {
        public HelpFlyout()
        {
            this.InitializeComponent();
        }

        private void BackClicked(object sender, RoutedEventArgs e)
        {
            if (this.Parent.GetType() == typeof(Popup))
            {
                ((Popup)this.Parent).IsOpen = false;
            }

            SettingsPane.Show();
        }

        private async void SupportLinkClicked_Click(object sender, RoutedEventArgs e)
        {
            var mailto = new Uri("mailto:?to=bensaute@htwg-konstanz.de&subject=seeMENSA for Windows Support");
            await Windows.System.Launcher.LaunchUriAsync(mailto);
        }
    }
}
