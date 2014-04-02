using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Palmmedia.GitHistory2Yuml.Core
{
    /// <summary>
    /// Renders Yuml graphs using 'http://yuml.me/'.
    /// </summary>
    public class YumlGraphRenderer : IYumlGraphRenderer
    {
        /// <summary>
        /// Renders the Yuml graph.
        /// </summary>
        /// <param name="yumlGraph">The yuml graph.</param>
        /// <returns>The Yuml as PNG image.</returns>
        public async Task<byte[]> RenderYumlGraphAync(string yumlGraph)
        {
            if (yumlGraph == null)
            {
                throw new ArgumentNullException("yumlGraph");
            }

            var requestContent = new KeyValuePair<string, string>("dsl_text", yumlGraph);

            HttpClient client = new HttpClient();
            var response = await client.PostAsync(
                "http://yuml.me/diagram/plain/class/",
                new FormUrlEncodedContent(Enumerable.Repeat(requestContent, 1)));

            response.EnsureSuccessStatusCode();

            string imageId = await response.Content.ReadAsStringAsync();

            byte[] image = await client.GetByteArrayAsync("http://yuml.me/" + imageId);

            return image;
        }
    }
}
