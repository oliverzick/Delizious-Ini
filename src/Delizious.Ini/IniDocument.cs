namespace Delizious.Ini
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Represents an INI document.
    /// </summary>
    public sealed class IniDocument
    {
        private readonly IIniDocument iniDocument;

        private IniDocument(IIniDocument iniDocument)
        {
            this.iniDocument = iniDocument;
        }

        /// <summary>
        /// Loads an INI document from the given <paramref name="textReader"/>.
        /// The <paramref name="textReader"/> is only used to read the INI document from and is not kept in the returned <see cref="IniDocument"/> instance.
        /// </summary>
        /// <param name="textReader">
        /// The <see cref="TextReader"/> to read the INI document from.
        /// </param>
        /// <returns>
        /// A new <see cref="IniDocument"/> instance that represents the INI document read from the given <paramref name="textReader"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="textReader"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="PersistenceException">
        /// The INI document could not be loaded from the given <paramref name="textReader"/>.
        /// Inspect <see cref="Exception.InnerException"/> for detailed error and the reason for the exception.
        /// </exception>
        public static IniDocument LoadFrom(TextReader textReader)
        {
            if (textReader is null)
            {
                throw new ArgumentNullException(nameof(textReader));
            }

            return new IniDocument(IniParserAdapter.LoadFrom(textReader));
        }

        /// <summary>
        /// Saves the INI document to the given <paramref name="textWriter"/>.
        /// </summary>
        /// <param name="textWriter">
        /// The <see cref="TextWriter"/> to save the INI document to.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="textWriter"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="PersistenceException">
        /// The INI document could not be saved to the given <paramref name="textWriter"/>.
        /// Inspect <see cref="Exception.InnerException"/> for detailed error and the reason for the exception.
        /// </exception>
        public void SaveTo(TextWriter textWriter)
        {
            if (textWriter is null)
            {
                throw new ArgumentNullException(nameof(textWriter));
            }

            this.iniDocument.SaveTo(textWriter);
        }

        /// <summary>
        /// Enumerates the names of all contained sections.
        /// </summary>
        /// <returns>
        /// An enumerable collection of section names for all the sections contained in the current <see cref="IniDocument"/>.
        /// </returns>
        public IEnumerable<SectionName> EnumerateSections()
            => this.iniDocument.EnumerateSections();

        /// <summary>
        /// Enumerates all the properties contained in the specified section.
        /// </summary>
        /// <param name="sectionName">
        /// The name of the section to enumerate the contained properties.
        /// </param>
        /// <returns>
        /// An enumerable collection of property keys for all the properties contained in the specified section of the current <see cref="IniDocument"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sectionName"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="SectionNotFoundException">
        /// The section specified by the <paramref name="sectionName"/> does not exist.
        /// </exception>
        public IEnumerable<PropertyKey> EnumerateProperties(SectionName sectionName)
        {
            if (sectionName is null)
            {
                throw new ArgumentNullException(nameof(sectionName));
            }

            return this.EnumerateProperties(sectionName, PropertyEnumerationMode.Fail());
        }

        /// <summary>
        /// <para>
        /// Enumerates all the properties contained in the specified section. The mode specifies the behavior in case the section does not exist.
        /// </para>
        /// <para>
        /// When mode is <see cref="PropertyEnumerationMode.Fail"/> and the section does not exist,
        /// throws a <see cref="SectionNotFoundException"/> 
        /// </para>
        /// <para>
        /// When mode is <see cref="PropertyEnumerationMode.Fallback"/> and the section does not exist,
        /// returns an empty collection of property keys.
        /// </para>
        /// </summary>
        /// <param name="sectionName">
        /// The name of the section to enumerate the contained properties.
        /// </param>
        /// <param name="mode">
        /// The mode that specifies how to enumerate when the section does not exist.
        /// </param>
        /// <returns>
        /// An enumerable collection of property keys for all the properties contained in the specified section of the current <see cref="IniDocument"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="sectionName"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="mode"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="SectionNotFoundException">
        /// <paramref name="mode"/> is <see cref="PropertyEnumerationMode.Fail"/> and the specified section does not exist.
        /// </exception>
        public IEnumerable<PropertyKey> EnumerateProperties(SectionName sectionName, PropertyEnumerationMode mode)
        {
            if (sectionName is null)
            {
                throw new ArgumentNullException(nameof(sectionName));
            }

            if (mode is null)
            {
                throw new ArgumentNullException(nameof(mode));
            }

            return mode.Transform(new PropertyEnumerationModeTransformation(this.iniDocument)).EnumerateProperties(sectionName);
        }

        private sealed class PropertyEnumerationModeTransformation : IPropertyEnumerationModeTransformation<IPropertyEnumerator>
        {
            private readonly IPropertyEnumerator propertyEnumerator;

            public PropertyEnumerationModeTransformation(IPropertyEnumerator propertyEnumerator)
            {
                this.propertyEnumerator = propertyEnumerator;
            }

            public IPropertyEnumerator Fail()
                => this.propertyEnumerator;

            public IPropertyEnumerator Fallback()
                => new FallbackPropertyEnumerator(this.propertyEnumerator);

            private sealed class FallbackPropertyEnumerator : IPropertyEnumerator
            {
                private readonly IPropertyEnumerator propertyEnumerator;

                public FallbackPropertyEnumerator(IPropertyEnumerator propertyEnumerator)
                {
                    this.propertyEnumerator = propertyEnumerator;
                }

                public IEnumerable<PropertyKey> EnumerateProperties(SectionName sectionName)
                {
                    try
                    {
                        return this.propertyEnumerator.EnumerateProperties(sectionName);
                    }
                    catch (SectionNotFoundException)
                    {
                        return Enumerable.Empty<PropertyKey>();
                    }
                }
            }
        }

        /// <summary>
        /// Reads the property contained in the specified section.
        /// </summary>
        /// <param name="sectionName">
        /// The name of the section containing the property.
        /// </param>
        /// <param name="propertyKey">
        /// The key of the property to read the value.
        /// </param>
        /// <returns>
        /// The value of the property. If the property does exist but has no value, <see cref="string.Empty"/> is returned.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sectionName"/> or <paramref name="propertyKey"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="SectionNotFoundException">
        /// The section specified by the <paramref name="sectionName"/> does not exist.
        /// </exception>
        /// <exception cref="PropertyNotFoundException">
        /// The property specified by the <paramref name="propertyKey"/> does not exist.
        /// </exception>
        public PropertyValue ReadProperty(SectionName sectionName, PropertyKey propertyKey)
        {
            if (sectionName is null)
            {
                throw new ArgumentNullException(nameof(sectionName));
            }

            if (propertyKey is null)
            {
                throw new ArgumentNullException(nameof(propertyKey));
            }

            return this.iniDocument.ReadProperty(sectionName, propertyKey);
        }

        /// <summary>
        /// Updates the value of the property contained in the section.
        /// </summary>
        /// <param name="sectionName">
        /// The name of the section containing the property.
        /// </param>
        /// <param name="propertyKey">
        /// The key of the property to update.
        /// </param>
        /// <param name="newPropertyValue">
        /// The new value of the property.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sectionName"/> or <paramref name="propertyKey"/> or <paramref name="newPropertyValue"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="SectionNotFoundException">
        /// The section specified by the <paramref name="sectionName"/> does not exist.
        /// </exception>
        /// <exception cref="PropertyNotFoundException">
        /// The property specified by the <paramref name="propertyKey"/> does not exist.
        /// </exception>
        public void UpdateProperty(SectionName sectionName, PropertyKey propertyKey, PropertyValue newPropertyValue)
        {
            if (sectionName is null)
            {
                throw new ArgumentNullException(nameof(sectionName));
            }

            if (propertyKey is null)
            {
                throw new ArgumentNullException(nameof(propertyKey));
            }

            if (newPropertyValue is null)
            {
                throw new ArgumentNullException(nameof(newPropertyValue));
            }

            this.iniDocument.WriteProperty(sectionName, propertyKey, newPropertyValue);
        }
    }
}
