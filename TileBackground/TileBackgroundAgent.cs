using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using SeeMensaWindows.Common.Storage;
using SeeMensaWindows.Common.DataModel;
using SeeMensaWindows.Common.LiveTile;
using SeeMensaWindows.Common.DataAccess;
using SeeMensaWindows.Common.Helpers;

namespace TileBackground
{
    public sealed class TileBackgroundAgent : IBackgroundTask
    {
        static MainViewModel _mainViewModel = MainViewModel.GetInstance;

        /// <summary>
        /// The entry point of the background worker.
        /// </summary>
        /// <param name="taskInstance">
        /// The task instance.
        /// </param>
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();

            try
            {
                // Loads the data.
                await AppStorage.Load();

                if (_mainViewModel.IsMensaSelected)
                {
                    var mensa = _mainViewModel.GetMensa(_mainViewModel.SelectedMensaId);

                    await CheckLoadXml(mensa);

                    UpdateLiveTile(mensa);
                }
            }
            catch (Exception) { }
            
            deferral.Complete();
        }

        /// <summary>
        /// Reloads the xml data from the web, if it is neccessary.
        /// </summary>
        /// <param name="mensa"></param>
        /// <returns></returns>
        private async Task CheckLoadXml(MensaItemViewModel mensa)
        {
            DateTime now = DateTime.Now;
            DateTime lastUpdate = mensa.LastUpdate;

            TimeSpan delay = now.Subtract(lastUpdate);

            //if (delay.Days >= 7) // TODO: just for testing!
            if (delay.Days >= 0)
            {
                var xml = await DownloadAsync(mensa.InterfaceUriDe);

                mensa.ParseXml(xml);

                mensa.LastUpdate = DateTime.UtcNow;

                AppStorage.Save();
            }
        }

        /// <summary>
        /// LoadsUpdates the live tile.
        /// </summary>
        private void UpdateLiveTile(MensaItemViewModel mensa)
        {
            if (_mainViewModel.IsMensaSelected)
            {
                SeeMensaLiveTileHelper.UpdateLiveTile(mensa);
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
        private static async Task<string> DownloadAsync(Uri uri)        
        {
            IRepository repo = new HttpWebLoader();
            return await repo.GetDataAsync(uri);
        }
    }
}
