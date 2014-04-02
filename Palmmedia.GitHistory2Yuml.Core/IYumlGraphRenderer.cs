using System.Threading.Tasks;

namespace Palmmedia.GitHistory2Yuml.Core
{
    /// <summary>
    /// Interface for Yuml graph rendering.
    /// </summary>
    public interface IYumlGraphRenderer
    {
        /// <summary>
        /// Renders the Yuml graph.
        /// </summary>
        /// <param name="yumlGraph">The yuml graph.</param>
        /// <returns>The Yuml as PNG image.</returns>
        Task<byte[]> RenderYumlGraphAync(string yumlGraph);
    }
}
