using SeeMensaWindows.Common.DataModel;
using SeeMensaWindows.Common.LiveTile;
using System;

namespace SeeMensaWindows.Common.Helpers
{
    public static class SeeMensaLiveTileHelper
    {
        private const int MAX_TILE_PAGES = 5;

        /// <summary>
        /// Updates the live tile with the data of the given mensa.
        /// </summary>
        /// <param name="mensa">The mensa data.</param>
        public static void UpdateLiveTile(MensaItemViewModel mensa)
        {
            if (mensa.Days.Count > 0)
            {
                var liveTileManager = new LiveTileManager(Windows.UI.Notifications.TileTemplateType.TileWideText09, Windows.UI.Notifications.TileTemplateType.TileSquareText02, true);

                var todayMeals = mensa.Days[0].Meals; // TODO: select the correct day, not just the meals of today! Probably the mensa is already closed, so show the meals of tomorrow!

                for (int i = 0; (i < todayMeals.Count) && i < MAX_TILE_PAGES; i++)
                {
                    liveTileManager.Tiles.Add(new LiveTileData(todayMeals[i].Category, todayMeals[i].Title, null));
                }

                liveTileManager.Update();
            }
        }
    }
}
