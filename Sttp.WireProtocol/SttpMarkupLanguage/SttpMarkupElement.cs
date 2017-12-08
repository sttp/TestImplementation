using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp
{
    public class SttpMarkupElement
    {
        public readonly string ElementName;
        public readonly SttpMarkupCompatiblity ElementCompatibility;

        public List<SttpMarkupElement> ChildElements = new List<SttpMarkupElement>();
        public List<SttpMarkupValue> ChildValues = new List<SttpMarkupValue>();

        public SttpMarkupElement(SttpMarkupReader reader)
        {
            if (reader.NodeType != SttpMarkupNodeType.Element)
                throw new Exception("Expecting an Element type for the current node.");
            ElementName = reader.ElementName;
            ElementCompatibility = reader.ElementCompatibility;

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case SttpMarkupNodeType.Element:
                        ChildElements.Add(new SttpMarkupElement(reader));
                        break;
                    case SttpMarkupNodeType.Value:
                        ChildValues.Add(new SttpMarkupValue(reader));
                        break;
                    case SttpMarkupNodeType.EndElement:
                        return;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }



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

        public void ErrorIfNotHandled()
        {
            foreach (var item in ChildElements)
            {
                if (item.ElementCompatibility != SttpMarkupCompatiblity.Unknown)
                    item.ErrorIfNotHandled();
            }

            foreach (var item in ChildValues)
            {
                if (item.ValueCompatibility != SttpMarkupCompatiblity.Unknown && !item.Handled)
                {
                    throw new Exception("Unknown value not properly read.");
                }
            }
        }

    }
}
