using SeeMensaWindows.Common.DataModel;
using SeeMensaWindows.Helpers;
using System;
using System.Collections.Generic;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml.Controls;

// The Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234233

namespace SeeMensaWindows
{
    /// <summary>
    /// A page that displays a collection of item previews.  In the Split Application this page
    /// is used to display and select one of the available groups.
    /// </summary>
    public sealed partial class ItemsPage : SeeMensaWindows.Common.LayoutAwarePage
    {
        static MainViewModel _mainViewModel = MainViewModel.GetInstance;

        public ItemsPage()
        {
            this.InitializeComponent();

            // App settings in charms bar
            SettingsPane.GetForCurrentView().CommandsRequested += App_CommandsRequested;
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            var mainViewModel = _mainViewModel.GetMensas((String)navigationParameter);
            this.DefaultViewModel["Days"] = mainViewModel;
        }

        /// <summary>
        /// Invoked when an item is clicked.
        /// </summary>
        /// <param name="sender">The GridView (or ListView when the application is snapped)
        /// displaying the item clicked.</param>
        /// <param name="e">Event data that describes the item clicked.</param>
        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            var mensaId = ((MensaItemViewModel)e.ClickedItem).UniqueId;
            _mainViewModel.SelectMensa(mensaId);
            this.Frame.Navigate(typeof(SplitPage), mensaId);
        }

        #region Charms settings

        private void App_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            SettingsPaneManager.RegisterSettings(args.Request.ApplicationCommands);
        }

        #endregion
    }
}
