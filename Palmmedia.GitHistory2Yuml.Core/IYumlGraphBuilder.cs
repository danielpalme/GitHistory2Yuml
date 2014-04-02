namespace Palmmedia.GitHistory2Yuml.Core
{
    /// <summary>
    /// Interface to create Yuml graphs from GIT History.
    /// </summary>
    public interface IYumlGraphBuilder
    {
        /// <summary>
        /// Creates the Yuml graph.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <returns>The Yuml graph.</returns>
        string CreateYumlGraph(string directory);

        /// <summary>
        /// Creates the merged Yuml graph.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <returns>The merged Yuml graph.</returns>
        string CreateMergedYumlGraph(string directory);
    }
}
