using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sttp.Codec;

namespace Sttp
{
    public enum SttpConnectionStringCompatiblity
    {
        /// <summary>
        /// Indicates that this option must be interpreted. If the option is not recognized, the request must be denied. 
        /// This also requires that if the option is properly interpreted, it must be enforced, otherwise the request must be denied.
        /// </summary>
        RequiredAndEnfored,
        /// <summary>
        /// Indicates that this option must be interpreted. If the option is not recognized, the request must be denied.
        /// However, the option can be ignored if desired, but only by an entity that recognizes the option.
        /// </summary>
        RequiredAndDesired,
        /// <summary>
        /// Indicates that if the server does not recognized this item. It can be safely ignored.
        /// </summary>
        Optional,
    }

    public class SttpConnectionString
    {
        public List<SttpConnectionStringElement> Values;

        public SttpConnectionString(PayloadReader rd)
        {
            Values = rd.ReadSttpConnectionStringElement();
        }

        public void Save(PayloadWriter wr)
        {
            wr.Write(Values);
        }

        public bool TryGetValue(string recordName, out SttpValue value)
        {
            foreach (var item in Values)
            {
                if (item.RecordName == recordName)
                {
                    value = item.Value;
                    return true;
                }
            }
            value = null;
            return false;
        }

        public bool HasValue(string syntax, string sttpquerystatement)
        {
            foreach (var item in Values)
            {
                if (item.RecordName == syntax)
                {
                    if (sttpquerystatement == item.Value.AsString)
                        return true;
                }
            }
            return false;
        }
    }

    public class SttpConnectionStringElement
    {
        public string RecordName;
        public SttpConnectionStringCompatiblity Requirement;
        public SttpValue Value;

        public SttpConnectionStringElement(string recordName, SttpConnectionStringCompatiblity requirement, SttpValue value)
        {
            RecordName = recordName;
            Requirement = requirement;
            Value = value;
        }

        public SttpConnectionStringElement(PayloadReader rd)
        {
            RecordName = rd.ReadString();
            Requirement = (SttpConnectionStringCompatiblity)rd.ReadByte();
            Value = rd.ReadSttpValue();
        }

        public void Save(PayloadWriter wr)
        {
            wr.Write(RecordName);
            wr.Write((byte)Requirement);
            wr.Write(Value);
        }

    }

}
