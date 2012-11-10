using SeeMensaWindows.Common.DataModel;
using SeeMensaWindows.Common.Storage;
using System;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace SeeMensaWindows.Views
{
    public sealed partial class AppSettingsFlyout : UserControl
    {
        static MainViewModel _mainViewModel = MainViewModel.GetInstance;

        public AppSettingsFlyout()
        {
            this.InitializeComponent();

            loadPriceTypes();

            Unloaded += AppSettingsFlyout_Unloaded;

            rbStudent.Checked += PriceRadioButtonChecked;
            rbGuest.Checked += PriceRadioButtonChecked;
            rbEmployee.Checked += PriceRadioButtonChecked;
            rbPupil.Checked += PriceRadioButtonChecked;
        }

        void AppSettingsFlyout_Unloaded(object sender, RoutedEventArgs e)
        {
            AppStorage.Save();
        }

        private void loadPriceTypes()
        {
            switch (_mainViewModel.PriceType)
            {
                case PriceType.Student:
                    rbStudent.IsChecked = true;
                    break;

                case PriceType.Guest:
                    rbGuest.IsChecked = true;
                    break;

                case PriceType.Employee:
                    rbEmployee.IsChecked = true;
                    break;

                case PriceType.Pupil:
                    rbPupil.IsChecked = true;
                    break;
            }
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
            _mainViewModel.ResetSelectedMensa();
            
            Frame rootFrame = Window.Current.Content as Frame;

            rootFrame.Navigate(typeof(ItemsPage), "AllMensas");

            this.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private void PriceRadioButtonChecked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;

            if (rb != null)
            {
                _mainViewModel.PriceType = (PriceType)Enum.Parse(typeof(PriceType), (string)rb.Tag, true);

                tbSettingsInformation.Text = "Die Änderungen werden beim nächsten Start der Anwendung übernommen.";
            }
            
        }
    }
}
