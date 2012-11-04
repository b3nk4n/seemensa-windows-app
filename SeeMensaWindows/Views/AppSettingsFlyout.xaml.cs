using SeeMensaWindows.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace SeeMensaWindows.Views
{
    public sealed partial class AppSettingsFlyout : UserControl
    {
        public AppSettingsFlyout()
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

        private void SelectMensaClicked(object sender, RoutedEventArgs e)
        {
            MainViewModel.ResetSelectedMensa();
            
            Frame rootFrame = Window.Current.Content as Frame;

            rootFrame.Navigate(typeof(ItemsPage), "AllMensas");

            this.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }
    }
}
