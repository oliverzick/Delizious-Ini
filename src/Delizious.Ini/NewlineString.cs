namespace Delizious.Ini
{
    using System;

    /// <summary>
    /// Represents a newline string that specifies a sequence of characters used to signify the end of a line.
    /// </summary>
    public sealed class NewlineString : IEquatable<NewlineString>
    {
        private readonly string newline;

        private NewlineString(string newline)
        {
            this.newline = newline;
        }

        /// <summary>
        /// The current newline string for this environment.
        /// </summary>
        public static NewlineString Environment
            => new NewlineString(System.Environment.NewLine);

        /// <summary>
        /// The newline string <c>"\n"</c> for Unix systems.
        /// </summary>
        public static NewlineString Unix
            => new NewlineString("\n");

        /// <summary>
        /// The newline string <c>"\r\n"</c> for Windows systems.
        /// </summary>
        public static NewlineString Windows
            => new NewlineString("\r\n");

        public static bool operator ==(NewlineString left, NewlineString right)
            => Equals(left, right);

        public static bool operator !=(NewlineString left, NewlineString right)
            => !(left == right);

        /// <inheritdoc/>
        public bool Equals(NewlineString other)
        {
            if (other is null)
            {
                return false;
            }

            return this.newline == other.newline;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
            => this.Equals(obj as NewlineString);

        /// <inheritdoc/>
        public override int GetHashCode()
            => this.newline.GetHashCode();

        /// <inheritdoc/>
        public override string ToString()
            => this.newline;
    }
}
