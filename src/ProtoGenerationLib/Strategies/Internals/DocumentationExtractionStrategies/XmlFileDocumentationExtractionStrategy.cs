using ProtoGenerationLib.Strategies.Abstracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using XmlDocExtractionLib;

namespace ProtoGenerationLib.Strategies.Internals.DocumentationExtractionStrategies
{
    /// <summary>
    /// A strategy for extracting documentation from
    /// xml documentation file.
    /// </summary>
    public class XmlFileDocumentationExtractionStrategy : IDocumentationExtractionStrategy
    {
        /// <summary>
        /// An ordered enumerable that tells how to construct to contents
        /// of a member documentation as a string.<br/>
        /// Default to summary remarks and returns.<br/><br/>
        /// <example>
        /// Example:<br/>
        /// If the value is {("summary", string.Empty), ("remarks", "Remarks:")},
        /// then the representation of the xml will be:
        /// "summary contents<br/>
        /// Remarks:
        /// remarks contents"
        /// </example>
        /// </summary>
        public List<(string ElementName, string Prefix)> DocumentationOrder { get; }

        /// <summary>
        /// A context for the extraction of xml documentations.
        /// </summary>
        private XmlExtractionContext xmlExtractionContext;

        #region Constructors

        /// <summary>
        /// Creates new instance of the <see cref="XmlFileDocumentationExtractionStrategy"/> class.
        /// </summary>
        /// <param name="dllsAndXmlDirectoryPath">Path to the directory containing all the dlls and xmls for the documentation extraction context.</param>
        /// <param name="documentationStructure"><inheritdoc cref="DocumentationOrder" path="/node()"/></param>
        public XmlFileDocumentationExtractionStrategy(string dllsAndXmlDirectoryPath, IEnumerable<(string ElementName, string Prefix)>? documentationStructure = null) : this()
        {
            foreach (var xmlPath in Directory.GetFiles(dllsAndXmlDirectoryPath, "*.xml", SearchOption.TopDirectoryOnly))
            {
                // Search for match dll file.
                var dllPath = xmlPath.Replace(".xml", ".dll");
                if (File.Exists(dllPath))
                {
                    var assembly = Assembly.LoadFrom(dllPath);
                    xmlExtractionContext.AddAssembly(assembly, xmlPath);
                }
            }
        }

        /// <summary>
        /// Creates new instance of the <see cref="XmlFileDocumentationExtractionStrategy"/> class.
        /// </summary>
        /// <param name="assemblyToXmlFilePath">A mapping between assembly to its corresponding xml documentation file path.</param>
        /// <param name="documentationStructure"><inheritdoc cref="DocumentationOrder" path="/node()"/></param>
        public XmlFileDocumentationExtractionStrategy(Dictionary<Assembly, string> assemblyToXmlFilePath, IEnumerable<(string ElementName, string Prefix)>? documentationStructure = null) : this()
        {
            foreach (var kvp in assemblyToXmlFilePath)
            {
                xmlExtractionContext.AddAssembly(kvp.Key, kvp.Value);
            }
        }

        /// <summary>
        /// Creates new instance of the <see cref="XmlFileDocumentationExtractionStrategy"/> class.
        /// </summary>
        /// <param name="documentationStructure"><inheritdoc cref="DocumentationOrder" path="/node()"/></param>
        private XmlFileDocumentationExtractionStrategy(IEnumerable<(string ElementName, string Prefix)>? documentationStructure = null)
        {
            xmlExtractionContext = new XmlExtractionContext();
            DocumentationOrder = documentationStructure?.ToList() ??
                                 new List<(string, string)>
                                 {
                                     ("summary", string.Empty),
                                     ("remarks", $"Remarks:{Environment.NewLine}"),
                                     ("returns", $"Returns:{Environment.NewLine}"),
                                 };
        }

        #endregion Constructors

        /// <inheritdoc/>
        public bool TryGetBaseTypeFieldDocumentation(Type subClassType, Type baseType, out string documentation)
        {
            documentation = $"A field containing all the data of the {subClassType.Name} base class.";
            return true;
        }

        /// <inheritdoc/>
        public bool TryGetEnumValueDocumentation(Type enumType, int enumValue, out string documentation)
        {
            if (!enumType.IsEnum)
                throw new ArgumentException($"{nameof(enumType)}: {enumType.Name} is not an enum.");

            documentation = string.Empty;
            var xmldoc = enumType.GetEnumValueXmlDocumentation(enumValue, xmlExtractionContext, resolveInheritdoc: true);

            if (xmldoc is not null)
                documentation = GetMemberDocumentationContents(xmldoc);

            return documentation != string.Empty;
        }

        /// <inheritdoc/>
        public bool TryGetFieldDocumentation(FieldInfo fieldInfo, out string documentation)
        {
            return TryGetMemberDocumentation(fieldInfo, out documentation);
        }

        /// <inheritdoc/>
        public bool TryGetMethodDocumentation(MethodBase methodBase, out string documentation)
        {
            return TryGetMemberDocumentation(methodBase, out documentation);
        }

        /// <inheritdoc/>
        public bool TryGetMethodParameterDocumentation(MethodBase MethodBase, string parameterName, out string documentation)
        {
            documentation = string.Empty;
            var xmldoc = MethodBase.GetXmlDocumentation(xmlExtractionContext, resolveInheritdoc: true);
            var firstMatchedParam = xmldoc?.Elements("param").FirstOrDefault(x => x.Attribute("name").Value.Equals(parameterName));
            if (firstMatchedParam != null)
            {
                documentation = GetElementContents(firstMatchedParam);
            }

            return documentation != string.Empty;
        }

        /// <inheritdoc/>
        public bool TryGetPropertyDocumentation(PropertyInfo propertyInfo, out string documentation)
        {
            return TryGetMemberDocumentation(propertyInfo, out documentation);
        }

        /// <inheritdoc/>
        public bool TryGetTypeDocumentation(Type type, out string documentation)
        {
            return TryGetMemberDocumentation(type, out documentation);
        }

        /// <summary>
        /// Try getting the documentation of the given <paramref name="memberInfo"/>.
        /// </summary>
        /// <param name="memberInfo">The member whose documentation is requested.</param>
        /// <param name="documentation">
        /// The documentation if a documentation for the given
        /// <paramref name="memberInfo"/> was found otherwise <see cref="string.Empty"/>.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the documentation of the given <paramref name="memberInfo"/>
        /// was found, otherwise <see langword="false"/>.
        /// </returns>
        private bool TryGetMemberDocumentation(MemberInfo memberInfo, out string documentation)
        {
            documentation = string.Empty;
            var xmldoc = memberInfo.GetXmlDocumentation(xmlExtractionContext, resolveInheritdoc: true);

            if (xmldoc is not null)
                documentation = GetMemberDocumentationContents(xmldoc);

            return documentation != string.Empty;
        }

        /// <summary>
        /// Gets the contents of a member documentation from the given
        /// <paramref name="memberXml"/>.
        /// </summary>
        /// <param name="memberXml">The xml documentation of the member.</param>
        /// <returns>
        /// The text contents of a member documentation from the given
        /// <paramref name="memberXml"/>.
        /// </returns>
        private string GetMemberDocumentationContents(XElement memberXml)
        {
            var docs = string.Empty;

            foreach(var (elementName, prefix) in DocumentationOrder)
            {
                var elementContents = GetSpecificElementContents(memberXml, elementName);
                if(elementContents != string.Empty)
                {
                    if(docs != string.Empty)
                        docs += Environment.NewLine;

                    docs += prefix + elementContents;
                }
            }

            return docs;
        }

        /// <summary>
        /// Get the contents of a specific element of the given <paramref name="memberXml"/>.
        /// </summary>
        /// <param name="memberXml">The xml documentation of the member.</param>
        /// <param name="elementName">The name of the element whose contents are requested.</param>
        /// <returns>
        /// The contents of a specific element of the given <paramref name="memberXml"/>.
        /// </returns>
        private string GetSpecificElementContents(XElement memberXml, string elementName)
        {
            var specificElement = memberXml.Element(elementName);
            return GetElementContents(specificElement);
        }

        /// <summary>
        /// Gets all the text contents inside the given <paramref name="element"/>.
        /// </summary>
        /// <param name="element">The elements whose text contents is requested.</param>
        /// <returns>
        /// A string containing all the text contents inside the given <paramref name="element"/>
        /// after trimming each line.
        /// </returns>
        private string GetElementContents(XElement element)
        {
            if (element != null)
            {
                ReplaceRefTags(element);
                var elementContents = element.Value;
                if (elementContents is not null)
                {
                    var lines = elementContents.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                    var trimmedLines = new List<string>();
                    foreach (var line in lines)
                    {
                        var trimmedLine = line.Trim();
                        if (!string.IsNullOrEmpty(trimmedLine))
                            trimmedLines.Add(trimmedLine);
                    }

                    return string.Join(Environment.NewLine, trimmedLines);
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Replace all the ref tags like see or paramref
        /// with their refs.
        /// </summary>
        /// <param name="element">The element in which to replace ref tags.</param>
        private void ReplaceRefTags(XElement element)
        {
            var refElementsNames = new string[]
            {
                "paramref", "see", "seealso", "typeparamref"
            };

            foreach (var refElementName in refElementsNames)
            {
                foreach(var refElement in element.Descendants(refElementName).ToArray())
                {
                    var refAttribute = refElement.Attributes().FirstOrDefault(att => att.Value != null);
                    if (refAttribute != null)
                    {
                        var replacement = refAttribute.Value;
                        if (refAttribute.Name.LocalName.Equals("cref"))
                        {
                            // Take only the member name.
                            replacement = refAttribute.Value.Split('.').Last();
                        }
                        refElement.ReplaceWith(replacement);
                    }
                }
            }
        }
    }
}
