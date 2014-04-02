using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Palmmedia.GitHistory2Yuml.Core.Model
{
    /// <summary>
    /// Represents a commit with child and parent relationships.
    /// </summary>
    internal class Commit : ICommit
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Commit" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="message">The message.</param>
        /// <param name="when">The date of the commit</param>
        public Commit(string id, string message, DateTimeOffset when)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            this.Id = id;
            this.Message = string.Format("{0}: {1}", this.ShortId, FormatMessage(message ?? string.Empty));
            this.When = when;
            this.Parents = new HashSet<ICommit>();
            this.Children = new HashSet<Commit>();
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; private set; }

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
                return this.Id.Substring(0, 7);
            }
        }

        /// <summary>
        /// Gets the formatted commit message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; private set; }

        /// <summary>
        /// Gets or sets the date of the commit.
        /// </summary>
        /// <value>
        /// The date of the commit.
        /// </value>
        public DateTimeOffset When { get; set; }

        /// <summary>
        /// Gets the parents of the commit.
        /// </summary>
        /// <value>
        /// The parents.
        /// </value>
        public ICollection<ICommit> Parents { get; private set; }

        /// <summary>
        /// Gets the children of the commit.
        /// </summary>
        /// <value>
        /// The children.
        /// </value>
        public ICollection<Commit> Children { get; private set; }

        /// <summary>
        /// Gets or sets the name of the branch (if available).
        /// </summary>
        /// <value>
        /// The name of the branch.
        /// </value>
        public string BranchName { get; set; }

        /// <summary>
        /// Gets or sets the corresponding <see cref="MergedCommit"/> the commit belongs to.
        /// </summary>
        /// <value>
        /// The merged commit.
        /// </value>
        public MergedCommit MergedCommit { get; set; }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Commit other = obj as Commit;

            if (other == null)
            {
                return false;
            }

            return this.Id.Equals(other.Id);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Id;
        }

        /// <summary>
        /// Formats the given commit message.
        /// All special characters are removed and line breaks are added to avoid long lines.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>The formatted message.</returns>
        private static string FormatMessage(string message)
        {
            string line = Regex.Replace(message, "[^\\w^\\s]", string.Empty);

            if (line.Length < 50)
            {
                return line;
            }

            string result = string.Empty;
            string currentLine = string.Empty;

            foreach (var word in line.Split(' '))
            {
                if (currentLine.Length == 0 || currentLine.Length + word.Length < 50)
                {
                    currentLine += word + " ";
                }
                else
                {
                    result += currentLine;
                    result += "\\n";
                    currentLine = word + " ";
                }
            }

            result += currentLine;

            return result;
        }
    }
}
