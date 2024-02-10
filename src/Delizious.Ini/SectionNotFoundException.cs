namespace Delizious.Ini
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;

    /// <summary>
    /// The exception that is thrown when a section cannot be found.
    /// </summary>
    [Serializable]
    public sealed class SectionNotFoundException : IniException
    {
        internal SectionNotFoundException(SectionName sectionName)
            : base(ExceptionMessages.CouldNotFindSectionInIniDocument)
        {
            this.SectionName = sectionName;
        }

        [ExcludeFromCodeCoverage]
        private SectionNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Gets the name of the section that was not found.
        /// </summary>
        public SectionName SectionName { get; }
    }
}
