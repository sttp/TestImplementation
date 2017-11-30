using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.Core.Data
{
    public abstract class MetadataFunctions
    {
        public abstract string FunctionName { get; }
        public abstract void Execute(SttpValue[] values);
        public abstract void PropagateType(SttpValueTypeCode[] codes);
    }

    public class FuncMultiply : MetadataFunctions
    {
        public override string FunctionName => "MUL";

        private int[] m_inputs;
        private int m_output;

        public FuncMultiply(int[] inputs, int output)
        {
            m_inputs = inputs;
            m_output = output;
        }

        public override void Execute(SttpValue[] values)
        {
            if (values[0].IsNull)
            {
                values[m_output] = new SttpValue();
                return;
            }
            double firstValue = values[m_inputs[0]].AsDouble;
            for (int x = 1; x < m_inputs.Length; x++)
            {
                if (values[m_inputs[x]].IsNull)
                {
                    values[m_output] = new SttpValue();
                    return;
                }
                firstValue *= values[m_inputs[x]].AsDouble;
            }
            values[m_output] = new SttpValue(firstValue);
        }

        public override void PropagateType(SttpValueTypeCode[] codes)
        {
            codes[m_output] = SttpValueTypeCode.Double;
        }
    }

    public class FuncEquals : MetadataFunctions
    {
        public override string FunctionName => "EQU";

        private int[] m_inputs;
        private int m_output;

        public FuncEquals(int[] inputs, int output)
        {
            m_inputs = inputs;
            m_output = output;
        }

        public override void Execute(SttpValue[] values)
        {
            if (values[0].IsNull)
            {
                values[m_output] = new SttpValue();
                return;
            }
            SttpValue firstValue = values[m_inputs[0]];
            for (int x = 1; x < m_inputs.Length; x++)
            {
                if (values[m_inputs[x]].IsNull)
                {
                    values[m_output] = new SttpValue();
                    return;
                }
                if (!firstValue.Equals(values[m_inputs[x]]))
                {
                    values[m_output] = new SttpValue(false);
                    return;
                }
            }
            values[m_output] = new SttpValue(true);
        }

        public override void PropagateType(SttpValueTypeCode[] codes)
        {
            codes[m_output] = SttpValueTypeCode.Bool;
        }
    }
}
