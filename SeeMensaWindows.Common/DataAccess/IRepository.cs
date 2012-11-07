using System;
using System.Threading.Tasks;

namespace SeeMensaWindows.Common.DataAccess
{
    /// <summary>
    /// Interface for retrieving data from the web asynchronously.
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Gets the data asynchronously.
        /// </summary>
        /// <param name="sourceUri">
        /// The uri of the data source.
        /// </param>
        /// <returns>
        /// The loaded data.
        /// </returns>
        Task<string> GetDataAsync(Uri sourceUri);
    }
}
