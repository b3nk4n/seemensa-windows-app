using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace SeeMensaWindows.Views
{
    public sealed partial class AboutFlyout : UserControl
    {
        public AboutFlyout()
        {
            this.InitializeComponent();

            this.loadVersion();
        }

        private void BackClicked(object sender, RoutedEventArgs e)
        {
            if (this.Parent.GetType() == typeof(Popup))
            {
                ((Popup)this.Parent).IsOpen = false;
            }

            SettingsPane.Show();
        }

        /// <summary>
        /// Loads the current version from assembly.
        /// </summary>
        private void loadVersion()
        {
            //AssemblyName an = new AssemblyName(Assembly.GetExecutingAssembly().FullName);
            //this.tbVersion.Text = new StringBuilder().Append(an.Version.Major)
            //                                         .Append('.')
            //                                         .Append(an.Version.Minor)
            //                                         .ToString();
            this.tbVersion.Text = "1.0";
        }
    }
}
