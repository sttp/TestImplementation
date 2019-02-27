using System;
using System.Collections.Generic;
using System.Text;
using CTP;

namespace Sttp.Codec
{
    public class MetadataColumn
        : CommandObject<MetadataColumn>
    {
        /// <summary>
        /// The name of the column
        /// </summary>
        [CommandField()]
        public string Name { get; private set; }
        /// <summary>
        /// The type of this column
        /// </summary>
        public CtpTypeCode TypeCode { get; private set; }

        [CommandField("TypeCode")]
        private string TypeCodeInternal
        {
            get
            {
                switch (TypeCode)
                {
                    case CtpTypeCode.Null:
                        return "Null";
                    case CtpTypeCode.Integer:
                        return "Integer";
                    case CtpTypeCode.Single:
                        return "Single";
                    case CtpTypeCode.Double:
                        return "Double";
                    case CtpTypeCode.Numeric:
                        return "Numeric";
                    case CtpTypeCode.CtpTime:
                        return "CtpTime";
                    case CtpTypeCode.Boolean:
                        return "Boolean";
                    case CtpTypeCode.Guid:
                        return "Guid";
                    case CtpTypeCode.String:
                        return "String";
                    case CtpTypeCode.CtpBuffer:
                        return "CtpBuffer";
                    case CtpTypeCode.CtpCommand:
                        return "CtpDocument";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            set
            {
                switch (value)
                {
                    case "Null":
                        TypeCode = CtpTypeCode.Null;
                        break;
                    case "Integer":
                        TypeCode = CtpTypeCode.Integer;
                        break;
                    case "Single":
                        TypeCode = CtpTypeCode.Single;
                        break;
                    case "Double":
                        TypeCode = CtpTypeCode.Double;
                        break;
                    case "Numeric":
                        TypeCode = CtpTypeCode.Numeric;
                        break;
                    case "CtpTime":
                        TypeCode = CtpTypeCode.CtpTime;
                        break;
                    case "Boolean":
                        TypeCode = CtpTypeCode.Boolean;
                        break;
                    case "Guid":
                        TypeCode = CtpTypeCode.Guid;
                        break;
                    case "String":
                        TypeCode = CtpTypeCode.String;
                        break;
                    case "CtpBuffer":
                        TypeCode = CtpTypeCode.CtpBuffer;
                        break;
                    case "CtpDocument":
                        TypeCode = CtpTypeCode.CtpCommand;
                        break;
                    default:
                        TypeCode = (CtpTypeCode)Enum.Parse(typeof(CtpTypeCode), value);
                        break;
                }
            }

        }

        public MetadataColumn(string name, CtpTypeCode typeCode)
        {
            TypeCode = typeCode;
            Name = name;
        }

        //Exists to support CtpSerializable
        private MetadataColumn()
        { }

    }
}