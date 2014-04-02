using System;
using System.Collections.Generic;
using System.Text;
using Palmmedia.GitHistory2Yuml.Core.Model;

namespace Palmmedia.GitHistory2Yuml.Core
{
    /// <summary>
    /// Creates Yuml graphs from GIT History.
    /// </summary>
    public class YumlGraphBuilder : IYumlGraphBuilder
    {
        /// <summary>
        /// Creates the Yuml graph.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <returns>
        /// The Yuml graph.
        /// </returns>
        public string CreateYumlGraph(string directory)
        {
            if (directory == null)
            {
                throw new ArgumentNullException("directory");
            }

            IEnumerable<Commit> commits = GitHistory.GetCommits(directory);
            return CreateYumlGraph(commits);
        }

        /// <summary>
        /// Creates the merged Yuml graph.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <returns>
        /// The merged Yuml graph.
        /// </returns>
        public string CreateMergedYumlGraph(string directory)
        {
            if (directory == null)
            {
                throw new ArgumentNullException("directory");
            }

            IEnumerable<Commit> commits = GitHistory.GetCommits(directory);
            IEnumerable<MergedCommit> mergedCommits = GitHistory.GetMergedCommits(commits);

            return CreateYumlGraph(mergedCommits);
        }

        /// <summary>
        /// Converts the commits into a Yuml graph.
        /// </summary>
        /// <param name="commits">The commits.</param>
        /// <returns>The Yuml graph.</returns>
        private static string CreateYumlGraph(IEnumerable<ICommit> commits)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var commit in commits)
            {
                if (commit.Parents.Count > 0)
                {
                    foreach (var parent in commit.Parents)
                    {
                        sb.AppendFormat(
                            "[{0}|{1}{2}{3}]->[{4}],",
                            commit.ShortId,
                            commit.BranchName != null ? "Branch: " + commit.BranchName + "|" : null,
                            commit.Message,
                            commit.BranchName != null ? "{bg:cornsilk}" : null,
                            parent.ShortId);
                    }
                }
                else
                {
                    sb.AppendFormat(
                        "[{0}|{1}{2}],",
                        commit.ShortId,
                        commit.BranchName != null ? "Branch: " + commit.BranchName + "|" : null,
                        commit.Message,
                        commit.BranchName != null ? "{bg:cornsilk}" : null);
                }
            }

            return sb.ToString();
        }
    }
}
