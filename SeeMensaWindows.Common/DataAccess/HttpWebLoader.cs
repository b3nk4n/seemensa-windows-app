using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SeeMensaWindows.Common.DataAccess
{
    /// <summary>
    /// Class for loading data from the web.
    /// </summary>
    public class HttpWebLoader : IRepository
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
        public async Task<string> GetDataAsync(Uri sourceUri)
        {
            HttpClientHandler handler = new HttpClientHandler { UseDefaultCredentials = true, AllowAutoRedirect = true };
            HttpClient client = new HttpClient(handler);
            client.MaxResponseContentBufferSize = 196608;
            HttpResponseMessage response = await client.GetAsync(sourceUri);

            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
    }
}
