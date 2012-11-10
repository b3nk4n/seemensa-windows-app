using SeeMensaWindows.Common;
using SeeMensaWindows.Common.DataModel;
using SeeMensaWindows.Common.Storage;
using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel.Background;

// The Split App template is documented at http://go.microsoft.com/fwlink/?LinkId=234228

namespace SeeMensaWindows
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        static MainViewModel _mainViewModel = MainViewModel.GetInstance;

        /// <summary>
        /// Initializes the singleton Application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            this.Resuming += OnResuming;
        }

        private void RegisterBackgroundTasks()
        {
            BackgroundTaskBuilder builder = new BackgroundTaskBuilder();

            // Friendly string name identifying the background task
            builder.Name = "seeMENSA Live Tile";
            // Class name
            builder.TaskEntryPoint = "TileBackground.TileBackgroundAgent";

            IBackgroundTrigger trigger = new MaintenanceTrigger(15, false);
            builder.SetTrigger(trigger);
            IBackgroundCondition condition = new SystemCondition(SystemConditionType.InternetAvailable);
            builder.AddCondition(new SystemCondition(SystemConditionType.InternetAvailable));

            IBackgroundTaskRegistration task = builder.Register();
            //You have the option of implementing these events to do something upon completion
            //task.Progress += task_Progress;
            //task.Completed += task_Completed;
        }


        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            // Load the stored settings
            await AppStorage.Load();

            RegisterBackgroundTasks();

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();
                //Associate the frame with a SuspensionManager key                                
                SuspensionManager.RegisterFrame(rootFrame, "AppFrame");

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // Restore the saved session state only when appropriate
                    try
                    {
                        await SuspensionManager.RestoreAsync();
                    }
                    catch (SuspensionManagerException)
                    {
                        //Something went wrong restoring state.
                        //Assume there is no state and continue
                    }
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }
            if (rootFrame.Content == null)
            {
                if (_mainViewModel.IsMensaSelected)
                {
                    // Redirect to the split page, if a mensa was already selected
                    rootFrame.Navigate(typeof(SplitPage), _mainViewModel.SelectedMensaId);
                }
                else
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    if (!rootFrame.Navigate(typeof(ItemsPage), "AllMensas"))
                    {
                        throw new Exception("Failed to create initial page");
                    }
                }
            }
            // Ensure the current window is active
            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            await SuspensionManager.SaveAsync();
            deferral.Complete();
        }

        private async void OnResuming(object sender, object e)
        {
            await AppStorage.Load();
        }
    }
}
