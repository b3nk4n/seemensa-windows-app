using SeeMensaWindows.Common.DataModel;
using SeeMensaWindows.Helpers;
using SeeMensaWindows.Common.Storage;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.ApplicationSettings;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SeeMensaWindows.Common.DataAccess;
using SeeMensaWindows.Common.Helpers;
using SeeMensaWindows.Common;
using Windows.UI.Popups;

// The Split Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234234

namespace SeeMensaWindows
{
    /// <summary>
    /// A page that displays a group title, a list of items within the group, and details for the
    /// currently selected item.
    /// </summary>
    public sealed partial class SplitPage : SeeMensaWindows.Common.LayoutAwarePage
    {
        static MainViewModel _mainViewModel = MainViewModel.GetInstance;

        public SplitPage()
        {
            this.InitializeComponent();
            this.Loaded += SplitPage_Loaded;

            // App settings in charms bar
            SettingsPane.GetForCurrentView().CommandsRequested += App_CommandsRequested;
        }

        void SplitPage_Loaded(object sender, RoutedEventArgs e)
        {
            EnsureOneItemIsSelected();
        }

        /// <summary>
        /// Updates the live tile.
        /// </summary>
        private void UpdateLiveTile()
        {
            var mensa = DefaultViewModel["Mensa"] as MensaItemViewModel;
            if (mensa != null && _mainViewModel.IsMensaSelected)
            {
                SeeMensaLiveTileHelper.UpdateLiveTile(mensa);
            }
        }

        /// <summary>
        /// Ensures than one item is selected
        /// </summary>
        private void EnsureOneItemIsSelected()
        {
            if (itemListView.Items.Count > 0 && itemListView.SelectedIndex < 0)
            {
                itemListView.SelectedIndex = 0;
            }
        }

        #region Page state management

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
            var mensa = _mainViewModel.GetMensa((String)navigationParameter);
            this.DefaultViewModel["Mensa"] = mensa;
            this.DefaultViewModel["Days"] = mensa.Days;

            if (pageState == null)
            {
                this.itemListView.SelectedItem = null;
                // When this is a new page, select the first item automatically unless logical page
                // navigation is being used (see the logical page navigation #region below.)
                if (!this.UsingLogicalPageNavigation() && this.itemsViewSource.View != null)
                {
                    this.itemsViewSource.View.MoveCurrentToFirst();
                }
            }
            else
            {
                // Restore the previously saved state associated with this page
                if (pageState.ContainsKey("SelectedItem") && this.itemsViewSource.View != null)
                {
                    var selectedItem = _mainViewModel.GetDay((String)pageState["SelectedItem"]);
                    this.itemsViewSource.View.MoveCurrentTo(selectedItem);
                }
            }

            // Refresh the mensas-meals after the default viewmodel has been set.
            if (!string.IsNullOrEmpty(mensa.Xml))
            {
                DateTime now = DateTime.Now;
                DateTime lastUpdate = mensa.LastUpdate;

                TimeSpan delay = now.Subtract(lastUpdate);

                if (delay.Days >= 5)
                {
                    this.refresh();
                }
                else
                {
                    // Settings must be saved, because the selected mensa must be stored.
                    AppStorage.Save();
                }
            }
            else
            {
                this.refresh();
            }
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
            if (this.itemsViewSource.View != null)
            {
                var selectedItem = (DayViewModel)this.itemsViewSource.View.CurrentItem;
                if (selectedItem != null)
                    pageState["SelectedItem"] = selectedItem.UniqueId;
            }
        }

        #endregion

        #region Logical page navigation

        // Visual state management typically reflects the four application view states directly
        // (full screen landscape and portrait plus snapped and filled views.)  The split page is
        // designed so that the snapped and portrait view states each have two distinct sub-states:
        // either the item list or the details are displayed, but not both at the same time.
        //
        // This is all implemented with a single physical page that can represent two logical
        // pages.  The code below achieves this goal without making the user aware of the
        // distinction.

        /// <summary>
        /// Invoked to determine whether the page should act as one logical page or two.
        /// </summary>
        /// <param name="viewState">The view state for which the question is being posed, or null
        /// for the current view state.  This parameter is optional with null as the default
        /// value.</param>
        /// <returns>True when the view state in question is portrait or snapped, false
        /// otherwise.</returns>
        private bool UsingLogicalPageNavigation(ApplicationViewState? viewState = null)
        {
            if (viewState == null) viewState = ApplicationView.Value;
            return viewState == ApplicationViewState.FullScreenPortrait ||
                viewState == ApplicationViewState.Snapped;
        }

        /// <summary>
        /// Invoked when an item within the list is selected.
        /// </summary>
        /// <param name="sender">The GridView (or ListView when the application is Snapped)
        /// displaying the selected item.</param>
        /// <param name="e">Event data that describes how the selection was changed.</param>
        void ItemListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Invalidate the view state when logical page navigation is in effect, as a change
            // in selection may cause a corresponding change in the current logical page.  When
            // an item is selected this has the effect of changing from displaying the item list
            // to showing the selected item's details.  When the selection is cleared this has the
            // opposite effect.
            if (this.UsingLogicalPageNavigation())
            {
                this.InvalidateVisualState();
            }

            ScrollMealListUp();
        }

        /// <summary>
        /// Invoked when the page's back button is pressed.
        /// </summary>
        /// <param name="sender">The back button instance.</param>
        /// <param name="e">Event data that describes how the back button was clicked.</param>
        protected override void GoBack(object sender, RoutedEventArgs e)
        {
            if (this.UsingLogicalPageNavigation() && itemListView.SelectedItem != null)
            {
                // When logical page navigation is in effect and there's a selected item that
                // item's details are currently displayed.  Clearing the selection will return
                // to the item list.  From the user's point of view this is a logical backward
                // navigation.
                this.itemListView.SelectedItem = null;
            }
            else
            {
                // When logical page navigation is not in effect, or when there is no selected
                // item, use the default back button behavior.
                //base.GoBack(sender, e);
                Frame.Navigate(typeof(ItemsPage), "AllMensas");
            }
        }

        /// <summary>
        /// Invoked to determine the name of the visual state that corresponds to an application
        /// view state.
        /// </summary>
        /// <param name="viewState">The view state for which the question is being posed.</param>
        /// <returns>The name of the desired visual state.  This is the same as the name of the
        /// view state except when there is a selected item in portrait and snapped views where
        /// this additional logical page is represented by adding a suffix of _Detail.</returns>
        protected override string DetermineVisualState(ApplicationViewState viewState)
        {
            // Update the back button's enabled state when the view state changes
            var logicalPageBack = this.UsingLogicalPageNavigation(viewState) && this.itemListView.SelectedItem != null;
            var physicalPageBack = this.Frame != null && this.Frame.CanGoBack;
            //this.DefaultViewModel["CanGoBack"] = logicalPageBack || physicalPageBack;
            this.DefaultViewModel["CanGoBack"] = true;

            ForceItemReload();

            // Determine visual states for landscape layouts based not on the view state, but
            // on the width of the window.  This page has one layout that is appropriate for
            // 1366 virtual pixels or wider, and another for narrower displays or when a snapped
            // application reduces the horizontal space available to less than 1366.
            if (viewState == ApplicationViewState.Filled ||
                viewState == ApplicationViewState.FullScreenLandscape)
            {
                EnsureOneItemIsSelected();

                var windowWidth = Window.Current.Bounds.Width;
                if (windowWidth >= 1366) return "FullScreenLandscapeOrWide";
                return "FilledOrNarrow";
            }

            // When in portrait or snapped start with the default visual state name, then add a
            // suffix when viewing details instead of the list
            var defaultStateName = base.DetermineVisualState(viewState);
            return logicalPageBack ? defaultStateName + "_Detail" : defaultStateName;
        }

        /// <summary>
        /// Forces a reload of the selected item.
        /// </summary>
        private void ForceItemReload()
        {
            itemListView.SelectionChanged -= ItemListView_SelectionChanged;
            
            if (itemListView.Items.Count > 0)
            {
                int selectedIndex = itemListView.SelectedIndex;
                itemListView.SelectedIndex = -1;
                itemListView.SelectedIndex = selectedIndex;
            }

            itemListView.SelectionChanged += ItemListView_SelectionChanged;
        }

        #endregion

        #region mycode

        /// <summary>
        /// Starts the refresh proccess by async downloading the xml string.
        /// </summary>
        private async void refresh()
        {
            var currentMensa = (MensaItemViewModel)DefaultViewModel["Mensa"];

            Uri uri = currentMensa.InterfaceUriDe;

            try
            {
                if (InternetAccessHelper.IsInternet())
                {
                    var xml = await DownloadAsync(uri);

                    currentMensa.ParseXml(xml);

                    currentMensa.LastUpdate = DateTime.Now;

                    EnsureOneItemIsSelected(); 
                }
                else
                {
                    currentMensa.ParseXml(string.Empty);
                    ShowNoDataDialog("Sie benötigen eine Internetverbindung, um die Daten aktualisieren zu können.", "Achtung");
                }
            }
            catch (Exception)
            {
                currentMensa.ParseXml(string.Empty);
                ShowNoDataDialog("Aktuell stehen für diese Mensa keine Daten zur Verfügung.", "Achtung");
            }

            UpdateLiveTile();

            AppStorage.Save();
        }

        /// <summary>
        /// Shows a modal dialog.
        /// </summary>
        /// <param name="msg">The message</param>
        /// <param name="title">The title.</param>
        private async void ShowNoDataDialog(string msg, string title)
        {
            MessageDialog md = new MessageDialog(msg, title);
            bool? result = null;
            md.Commands.Add(
               new UICommand("OK", new UICommandInvokedHandler((cmd) => result = true)));

            await md.ShowAsync();
            if (result == true)
            {
                // do something    
            }
        }

        /// <summary>
        /// Downloads the xml data async.
        /// </summary>
        /// <param name="uri">
        /// The data source uri.
        /// </param>
        /// <returns>
        /// The loaded data.
        /// </returns>
        public static async Task<string> DownloadAsync(Uri uri)
        {
            IRepository repo = new HttpWebLoader();
            return await repo.GetDataAsync(uri);
        }

        #endregion

        #region Charms settings

        /// <summary>
        /// Registers the app settings.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="args">
        /// The event args.
        /// </param>
        private void App_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            SettingsPaneManager.RegisterSettings(args.Request.ApplicationCommands);
        }

        #endregion

        private void RefreshClicked(object sender, RoutedEventArgs e)
        {
            refresh();
            ScrollDayListUp();
        }

        private void ScrollMealListUp()
        {
            this.itemDetail.ScrollToVerticalOffset(0);
        }

        private void ScrollDayListUp()
        {
            if (itemListView.Items.Count > 0)
                this.itemListView.ScrollIntoView(itemListView.Items[0], ScrollIntoViewAlignment.Leading);
        }
    }
}
