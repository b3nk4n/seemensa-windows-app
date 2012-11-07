
namespace SeeMensaWindows.Common.LiveTile
{
    /// <summary>
    /// Defines the data of a live tile.
    /// </summary>
    public class LiveTileData
    {
        /// <summary>
        /// Initializes an instance of a LiveTileData.
        /// </summary>
        /// <param name="header">The header of the live tile.</param>
        /// <param name="text">The content text of the live tile.</param>
        /// <param name="imageSource">The optional image source of a life tile.</param>
        public LiveTileData(string header, string text, string imageSource)
        {
            this.Header = header;
            this.Text = text;
            this.ImageSource = imageSource;
        }

        /// <summary>
        /// Gets the content header.
        /// </summary>
        public string Header { get; private set; }

        /// <summary>
        /// Gets the content text.
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// Gets the optional image source.
        /// </summary>
        public string ImageSource { get; private set; }
    }
}
