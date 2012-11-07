using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using SeeMensaWindows.Common.Storage;
using SeeMensaWindows.Common.DataModel;
using SeeMensaWindows.Common.LiveTile;
using SeeMensaWindows.Common.DataAccess;

namespace TileBackground
{
    public sealed class TileBackgroundAgent : IBackgroundTask
    {
        LiveTileManager _liveTileManager;

        /// <summary>
        /// The entry point of the background worker.
        /// </summary>
        /// <param name="taskInstance">
        /// The task instance.
        /// </param>
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();
            
            // Loads the data.
            await AppStorage.Load();

            if (MainViewModel.IsMensaSelected)
            {
                var mensa = MainViewModel.GetMensa(MainViewModel.SelectedMensaId);

                await CheckLoadXml(mensa);

                UpdateLiveTile(mensa);
            }
            
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

            if (delay.Days >= 7)
            {
                var xml = await DownloadAsync(mensa.InterfaceUriDe);

                mensa.ParseXml(xml);
            }
        }

        /// <summary>
        /// LoadsUpdates the live tile.
        /// </summary>
        private void UpdateLiveTile(MensaItemViewModel mensa)
        {
            if (MainViewModel.IsMensaSelected)
            {
                if (mensa.Days.Count > 0)
                {
                    _liveTileManager = new LiveTileManager(Windows.UI.Notifications.TileTemplateType.TileWideText09, Windows.UI.Notifications.TileTemplateType.TileSquareText02, true);

                    var todayMeals = mensa.Days[0].Meals;

                    for (int i = 0; (i < todayMeals.Count) && i < 5; i++)
                    {
                        _liveTileManager.Tiles.Add(new LiveTileData(todayMeals[i].Category, mensa.LastUpdate.ToString(), null));
                    }

                    _liveTileManager.Update();
                }
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
