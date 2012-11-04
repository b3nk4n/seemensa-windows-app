using SeeMensaWindows.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace SeeMensaWindows.Helpers
{
    class SettingsPaneManager
    {
        private const int SettingsWidth = 346;
        private static Popup _settingsPopup;

        public static void RegisterSettings(IList<SettingsCommand> appCommands)
        {
            appCommands.Clear();

            appCommands.Add(new SettingsCommand("visit", "Get seeMENSA for Windows Phone", async a =>
            {
                await Launcher.LaunchUriAsync(new Uri("http://bsautermeister.de/seemensa"));
            }));

            appCommands.Add(
                new SettingsCommand("settings", "Mensa Settings",
                a =>
                {
                    _settingsPopup = new Popup();
                    _settingsPopup.Closed += OnPopupClosed;
                    Window.Current.Activated += OnWindowActivated;

                    _settingsPopup.IsLightDismissEnabled = true;
                    _settingsPopup.Width = SettingsWidth;
                    _settingsPopup.Height = Window.Current.Bounds.Height;

                    var mypane = new AppSettingsFlyout();
                    mypane.Width = SettingsWidth;
                    mypane.Height = Window.Current.Bounds.Height;
                    _settingsPopup.Child = mypane;
                    _settingsPopup.SetValue(Canvas.LeftProperty, Window.Current.Bounds.Width - SettingsWidth);
                    _settingsPopup.SetValue(Canvas.TopProperty, 0);
                    _settingsPopup.IsOpen = true;
                }));

            appCommands.Add(
                new SettingsCommand("help", "Help",
                a =>
                {
                    _settingsPopup = new Popup();
                    _settingsPopup.Closed += OnPopupClosed;
                    Window.Current.Activated += OnWindowActivated;

                    _settingsPopup.IsLightDismissEnabled = true;
                    _settingsPopup.Width = SettingsWidth;
                    _settingsPopup.Height = Window.Current.Bounds.Height;

                    var mypane = new HelpFlyout();
                    mypane.Width = SettingsWidth;
                    mypane.Height = Window.Current.Bounds.Height;
                    _settingsPopup.Child = mypane;
                    _settingsPopup.SetValue(Canvas.LeftProperty, Window.Current.Bounds.Width - SettingsWidth);
                    _settingsPopup.SetValue(Canvas.TopProperty, 0);
                    _settingsPopup.IsOpen = true;
                }));

            appCommands.Add(
                new SettingsCommand("about", "About seeMENSA",
                a =>
                {
                    _settingsPopup = new Popup();
                    _settingsPopup.Closed += OnPopupClosed;
                    Window.Current.Activated += OnWindowActivated;

                    _settingsPopup.IsLightDismissEnabled = true;
                    _settingsPopup.Width = SettingsWidth;
                    _settingsPopup.Height = Window.Current.Bounds.Height;

                    var mypane = new AboutFlyout();
                    mypane.Width = SettingsWidth;
                    mypane.Height = Window.Current.Bounds.Height;
                    _settingsPopup.Child = mypane;
                    _settingsPopup.SetValue(Canvas.LeftProperty, Window.Current.Bounds.Width - SettingsWidth);
                    _settingsPopup.SetValue(Canvas.TopProperty, 0);
                    _settingsPopup.IsOpen = true;
                }));
        }

        private static void OnWindowActivated(object sender, Windows.UI.Core.WindowActivatedEventArgs e)
        {
            if (e.WindowActivationState == Windows.UI.Core.CoreWindowActivationState.Deactivated)
            {
                _settingsPopup.IsOpen = false;
            }
        }

        private static void OnPopupClosed(object sender, object e)
        {
            Window.Current.Activated -= OnWindowActivated;
        }
    }
}
