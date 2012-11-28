using SeeMensaWindows.Common.DataModel;
using System;
using System.Collections.Generic;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace SeeMensaWindows.Common.LiveTile
{
    /// <summary>
    /// Manager class for simple live tiles.
    /// </summary>
    public class LiveTileManager
    {
        /// <summary>
        /// The wide tile type.
        /// </summary>
        public TileTemplateType WideTileType { get; set; }

        /// <summary>
        /// Tje sqiare tile type.
        /// </summary>
        public TileTemplateType SquareTileType { get; set; }

        /// <summary>
        /// The data of the active live tiles.
        /// </summary>
        public List<LiveTileData> Tiles {get; set; }

        /// <summary>
        /// Initializes a new instance of the LiveTileManager.
        /// </summary>
        /// <param name="wideTemplate">The wide tile type.</param>
        /// <param name="squareTemplate">The square tile type.</param>
        /// <param name="notificationQueueEnabled">Indicates whether the queue is enabled or not.</param>
        public LiveTileManager(TileTemplateType wideTemplate, TileTemplateType squareTemplate, bool notificationQueueEnabled)
        {
            Tiles = new List<LiveTileData>(8);
            WideTileType = wideTemplate;
            SquareTileType = squareTemplate;
            TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(notificationQueueEnabled);
        }

        /// <summary>
        /// Updates the live tiles.
        /// </summary>
        public void Update()
        {
            if (Tiles.Count > 0)
            {
                var updater = TileUpdateManager.CreateTileUpdaterForApplication();

                updater.Clear();

                for (int i = 0; i < Tiles.Count && i < 5; ++i)
                {
                    TileNotification otherTile = CreateTileNotification(Tiles[i]);
                    updater.Update(otherTile);
                }
            }
        }

        /// <summary>
        /// Creates a new tile notification.
        /// </summary>
        /// <param name="index">The index of the Tile.</param>
        /// <returns></returns>
        private TileNotification CreateTileNotification(LiveTileData tileData)
        {
            XmlDocument wideTile = TileUpdateManager.GetTemplateContent(WideTileType);
            SetTileData(wideTile, tileData);
            XmlDocument squareTile = TileUpdateManager.GetTemplateContent(SquareTileType);
            SetTileData(squareTile, tileData);
            IXmlNode visualSquare = wideTile.ImportNode(squareTile.GetElementsByTagName("binding").Item(0), true);
            wideTile.GetElementsByTagName("visual").Item(0).AppendChild(visualSquare);

            TileNotification tile = new TileNotification(wideTile);
            tile.Tag = tileData.Header;
            return tile;
        }

        /// <summary>
        /// Sets the xml data for a live tile.
        /// </summary>
        /// <param name="tileXml">The xml tile.</param>
        /// <param name="tileData">The tile data.</param>
        private void SetTileData(XmlDocument tileXml, LiveTileData tileData)
        {
            XmlNodeList text = tileXml.GetElementsByTagName("text");
            text.Item(0).AppendChild(tileXml.CreateTextNode(tileData.Header));
            text.Item(1).AppendChild(tileXml.CreateTextNode(tileData.Text));
            XmlNodeList images = tileXml.GetElementsByTagName("image");
            XmlElement image = (XmlElement)images.Item(0);
            if (image != null && !String.IsNullOrWhiteSpace(tileData.ImageSource))
            {
                image.SetAttribute("src", tileData.ImageSource);
            }
        }
    }
}
