using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CTP
{
    /// <summary>
    /// Represents an element of an <see cref="CtpMarkup"/> file with all of it's children.
    /// 
    /// This class is created by <see cref="CtpMarkupReader.ReadEntireElement"/> and makes it easier to 
    /// parse an <see cref="CtpMarkup"/> object.
    /// </summary>
    public class CtpMarkupElement
    {
        /// <summary>
        /// The name of the element.
        /// </summary>
        public readonly string ElementName;

        /// <summary>
        /// The list of all of the child elements. Can be empty.
        /// </summary>
        public List<CtpMarkupElement> ChildElements = new List<CtpMarkupElement>();
        /// <summary>
        /// The list of all of the child values. Can be empty.
        /// </summary>
        public List<CtpMarkupValue> ChildValues = new List<CtpMarkupValue>();

        /// <summary>
        /// Creates a <see cref="CtpMarkupElement"/> from a <see cref="CtpMarkupReader"/> advancing the position 
        /// of the reader to the end of the current element.
        /// </summary>
        /// <param name="reader"></param>
        public CtpMarkupElement(CtpMarkupReader reader)
        {
            if (reader.NodeType == CtpMarkupNodeType.StartOfDocument)
            {
                ElementName = reader.RootElement;
                IterateNodes(reader);
                return;
            }
            while (reader.NodeType == CtpMarkupNodeType.EndElement)
            {
                reader.Read();
            }
            if (reader.NodeType != CtpMarkupNodeType.Element)
                throw new Exception("Expecting an Element type for the current node.");
            ElementName = reader.ElementName;

            IterateNodes(reader);
        }

        /// <summary>
        /// Iterates all of the nodes in a recursive manor to build a complete object of child elements.
        /// </summary>
        /// <param name="reader">the reader to parse.</param>
        private void IterateNodes(CtpMarkupReader reader)
        {
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case CtpMarkupNodeType.Element:
                        ChildElements.Add(new CtpMarkupElement(reader));  //Recursion occurs here.
                        break;
                    case CtpMarkupNodeType.Value:
                        ChildValues.Add(new CtpMarkupValue(reader));
                        break;
                    case CtpMarkupNodeType.EndElement:
                    case CtpMarkupNodeType.EndOfDocument:
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
        public CtpMarkupElement GetElement(string elementName)
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
        public CtpValue GetValue(string valueName, bool setHandled = true)
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
            return CtpValue.Null;
        }

        /// <summary>
        /// Gets every occurrence of a value with the specified value name. Optionally does not mark each value as handled.
        /// </summary>
        /// <param name="valueName">The name of the value</param>
        /// <param name="setHandled">allows to not automatically set the value as handled. </param>
        /// <returns></returns>
        public IEnumerable<CtpValue> ForEachValue(string valueName, bool setHandled = true)
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
