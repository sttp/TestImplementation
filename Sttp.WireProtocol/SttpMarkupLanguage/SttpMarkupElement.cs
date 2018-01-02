using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp
{
    /// <summary>
    /// Represents an element of an <see cref="SttpMarkup"/> file with all of it's children.
    /// 
    /// This class is created by <see cref="SttpMarkupReader.ReadEntireElement"/> and makes it easier to 
    /// parse an <see cref="SttpMarkup"/> object.
    /// </summary>
    public class SttpMarkupElement
    {
        /// <summary>
        /// The name of the element.
        /// </summary>
        public readonly string ElementName;

        /// <summary>
        /// The list of all of the child elements. Can be empty.
        /// </summary>
        public List<SttpMarkupElement> ChildElements = new List<SttpMarkupElement>();
        /// <summary>
        /// The list of all of the child values. Can be empty.
        /// </summary>
        public List<SttpMarkupValue> ChildValues = new List<SttpMarkupValue>();

        /// <summary>
        /// Creates a <see cref="SttpMarkupElement"/> from a <see cref="SttpMarkupReader"/> advancing the position 
        /// of the reader to the end of the current element.
        /// </summary>
        /// <param name="reader"></param>
        public SttpMarkupElement(SttpMarkupReader reader)
        {
            if (reader.NodeType == SttpMarkupNodeType.StartOfDocument)
            {
                ElementName = reader.RootElement;
                IterateNodes(reader);
                return;
            }
            while (reader.NodeType == SttpMarkupNodeType.EndElement)
            {
                reader.Read();
            }
            if (reader.NodeType != SttpMarkupNodeType.Element)
                throw new Exception("Expecting an Element type for the current node.");
            ElementName = reader.ElementName;

            IterateNodes(reader);
        }

        /// <summary>
        /// Iterates all of the nodes in a recursive manor to build a complete object of child elements.
        /// </summary>
        /// <param name="reader">the reader to parse.</param>
        private void IterateNodes(SttpMarkupReader reader)
        {
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case SttpMarkupNodeType.Element:
                        ChildElements.Add(new SttpMarkupElement(reader));  //Recursion occurs here.
                        break;
                    case SttpMarkupNodeType.Value:
                        ChildValues.Add(new SttpMarkupValue(reader));
                        break;
                    case SttpMarkupNodeType.EndElement:
                    case SttpMarkupNodeType.EndOfDocument:
                        return;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// Gets the first occurrence of an element with the supplied name. Throws an exception if the element cannot be found.
        /// </summary>
        /// <param name="elementName">the case sensitive name of the element.</param>
        /// <returns></returns>
        public SttpMarkupElement GetElement(string elementName)
        {
            foreach (var value in ChildElements)
            {
                if (value.ElementName == elementName)
                {
                    return value;
                }
            }
            throw new Exception("Element Not Found");
        }

        /// <summary>
        /// Gets the first occurrence of a value with the specified value name. Optionally does not marks the value as handled.
        /// </summary>
        /// <param name="valueName">The name of the value</param>
        /// <param name="setHandled">allows to not automatically set the value as handled. </param>
        /// <returns></returns>
        public SttpValue GetValue(string valueName, bool setHandled = true)
        {
            foreach (var value in ChildValues)
            {
                if (value.ValueName == valueName)
                {
                    if (setHandled)
                        value.Handled = true;
                    return value.Value;
                }
            }
            throw new Exception("Item Not Found");
        }

        /// <summary>
        /// Gets every occurrence of a value with the specified value name. Optionally does not mark each value as handled.
        /// </summary>
        /// <param name="valueName">The name of the value</param>
        /// <param name="setHandled">allows to not automatically set the value as handled. </param>
        /// <returns></returns>
        public IEnumerable<SttpValue> ForEachValue(string valueName, bool setHandled = true)
        {
            foreach (var value in ChildValues)
            {
                if (value.ValueName == valueName)
                {
                    if (setHandled)
                        value.Handled = true;
                    yield return value.Value;
                }
            }
        }

        /// <summary>
        /// Checks all children elements and children values to see if their values were read as part of the parsing. 
        /// This will throw an error for any SttpMarkupValue that exists but was not read. This assists in improperly interpreting all
        /// of the options present.
        /// </summary>
        public void ErrorIfNotHandled()
        {
            foreach (var item in ChildElements)
            {
                item.ErrorIfNotHandled();
            }

            foreach (var item in ChildValues)
            {
                if (!item.Handled)
                {
                    throw new Exception("Unknown value not properly read.");
                }
            }
        }

        /// <summary>
        /// Element: {ElementName} Children: {ChildElements.Count} Values: {ChildValues.Count}
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Element: {ElementName} Children: {ChildElements.Count} Values: {ChildValues.Count}";
        }
    }
}
