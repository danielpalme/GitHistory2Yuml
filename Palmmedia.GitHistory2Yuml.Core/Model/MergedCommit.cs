using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Palmmedia.GitHistory2Yuml.Core.Model
{
    /// <summary>
    /// Represents a merged commit (linear commits could be merged into a <see cref="MergedCommit"/>.
    /// </summary>
    internal class MergedCommit : ICommit
    {
        /// <summary>
        /// The corresponding commits.
        /// </summary>
        private readonly List<Commit> commits;

        /// <summary>
        /// Initializes a new instance of the <see cref="MergedCommit"/> class.
        /// </summary>
        /// <param name="commits">The commits.</param>
        public MergedCommit(List<Commit> commits)
        {
            if (commits == null)
            {
                throw new ArgumentNullException("commits");
            }

            this.commits = commits;
        }

        /// <summary>
        /// Gets the short identifier.
        /// </summary>
        /// <value>
        /// The short identifier.
        /// </value>
        public string ShortId
        {
            get
            {
                if (this.commits.Count == 1)
                {
                    return this.commits[0].ShortId;
                }
                else
                {
                    return string.Format(
                        "{0} ... {1}",
                        this.commits[this.commits.Count - 1].ShortId,
                        this.commits[0].ShortId);
                }
            }
        }

        /// <summary>
        /// Gets the formatted commit message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message
        {
            get
            {
                return string.Join("\\n", this.commits.Select(c => c.Message).Reverse());
            }
        }

        /// <summary>
        /// Gets the name of the branch (if available).
        /// </summary>
        /// <value>
        /// The name of the branch.
        /// </value>
        public string BranchName
        {
            get
            {
                return this.commits.Select(c => c.BranchName).FirstOrDefault(c => c != null);
            }
        }

        /// <summary>
        /// Gets the parents of the commit.
        /// </summary>
        /// <value>
        /// The parents.
        /// </value>
        public ICollection<ICommit> Parents
        {
            get
            {
                var result = new Collection<ICommit>();

                foreach (var item in this.commits[0].Parents
                    .Cast<Commit>()
                    .Select(c => c.MergedCommit))
                {
                    result.Add(item);
                }

                return result;
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.ShortId;
        }
    }
}
