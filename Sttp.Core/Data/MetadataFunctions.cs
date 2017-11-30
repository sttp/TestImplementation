using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.Core.Data
{
    public abstract class MetadataFunctions
    {
        private readonly int[] m_inputs;
        private readonly int m_output;
        private readonly SttpValue[] m_values;

        protected MetadataFunctions(int[] inputs, int output, SttpValue[] values)
        {
            m_inputs = inputs;
            m_output = output;
            m_values = values;
        }

        protected int InputCount => m_inputs.Length;

        protected SttpValue Inputs(int index)
        {
            return m_values[m_inputs[0]];
        }

        protected SttpValue Output
        {
            set
            {
                m_values[m_output] = value;
            }
        }

        protected void ValidInputsEquals(int inputCount)
        {
            if (InputCount != inputCount)
                throw new Exception("Input mismatch");
        }

        protected void ValidInputsAtLeast(int fewestInputCount)
        {
            if (InputCount < fewestInputCount)
                throw new Exception("Input mismatch");
        }

        protected bool IfAnyNullAssignNull()
        {
            for (int x = 0; x < InputCount; x++)
            {
                if (Inputs(x).IsNull)
                {
                    Output = new SttpValue();
                    return true;
                }
            }
            return false;
        }

        public abstract void Execute();

        public void PropagateType(SttpValueTypeCode[] codes)
        {
            SttpValueTypeCode[] myCodes = new SttpValueTypeCode[InputCount];
            for (int x = 0; x < InputCount; x++)
            {
                myCodes[x] = codes[m_inputs[x]];
            }
            codes[m_output] = GetReturnType(myCodes);
        }

        protected abstract SttpValueTypeCode GetReturnType(SttpValueTypeCode[] codes);
    }

    public class FuncMultiply : MetadataFunctions
    {
        public FuncMultiply(int[] inputs, int output, SttpValue[] values)
            : base(inputs, output, values)
        {
            ValidInputsAtLeast(2);
        }

        public override void Execute()
        {
            if (IfAnyNullAssignNull())
                return;

            double firstValue = Inputs(0).AsDouble;
            for (int x = 1; x < InputCount; x++)
            {
                firstValue *= Inputs(x).AsDouble;
            }
            Output = new SttpValue(firstValue);
        }

        protected override SttpValueTypeCode GetReturnType(SttpValueTypeCode[] codes)
        {
            return SttpValueTypeCode.Double;
        }
    }

    public class FuncEquals : MetadataFunctions
    {
        public FuncEquals(int[] inputs, int output, SttpValue[] values)
            : base(inputs, output, values)
        {
            ValidInputsEquals(2);
        }

        public override void Execute()
        {
            if (IfAnyNullAssignNull())
                return;

            Output = new SttpValue(Inputs(0).Equals(Inputs(1)));
        }

        protected override SttpValueTypeCode GetReturnType(SttpValueTypeCode[] codes)
        {
            return SttpValueTypeCode.Bool;
        }
    }

    public class FuncNotEquals : MetadataFunctions
    {
        public FuncNotEquals(int[] inputs, int output, SttpValue[] values)
            : base(inputs, output, values)
        {
            ValidInputsEquals(2);
        }

        public override void Execute()
        {
            if (IfAnyNullAssignNull())
                return;

            Output = new SttpValue(!Inputs(0).Equals(Inputs(1)));
        }

        protected override SttpValueTypeCode GetReturnType(SttpValueTypeCode[] codes)
        {
            return SttpValueTypeCode.Bool;
        }
    }

    public class FuncNot : MetadataFunctions
    {
        public FuncNot(int[] inputs, int output, SttpValue[] values)
            : base(inputs, output, values)
        {
            ValidInputsEquals(1);
        }

        public override void Execute()
        {
            if (IfAnyNullAssignNull())
                return;

            Output = new SttpValue(!Inputs(0).AsBool);
        }

        protected override SttpValueTypeCode GetReturnType(SttpValueTypeCode[] codes)
        {
            return SttpValueTypeCode.Bool;
        }
    }

    public class FuncOr : MetadataFunctions
    {
        public FuncOr(int[] inputs, int output, SttpValue[] values)
            : base(inputs, output, values)
        {
            ValidInputsAtLeast(2);
        }

        public override void Execute()
        {
            for (int x = 0; x < InputCount; x++)
            {
                if (!Inputs(x).IsNull && Inputs(x).AsBool)
                {
                    Output = new SttpValue(true);
                    return;
                }
            }

            if (IfAnyNullAssignNull())
                return;

            Output = new SttpValue(false);
        }

        protected override SttpValueTypeCode GetReturnType(SttpValueTypeCode[] codes)
        {
            return SttpValueTypeCode.Bool;
        }
    }

    public class FuncAnd : MetadataFunctions
    {
        public FuncAnd(int[] inputs, int output, SttpValue[] values)
            : base(inputs, output, values)
        {
            ValidInputsAtLeast(2);
        }

        public override void Execute()
        {
            if (IfAnyNullAssignNull())
                return;
            for (int x = 0; x < InputCount; x++)
            {
                if (!Inputs(x).AsBool)
                {
                    Output = new SttpValue(false);
                    return;
                }
            }

            Output = new SttpValue(true);
        }

        protected override SttpValueTypeCode GetReturnType(SttpValueTypeCode[] codes)
        {
            return SttpValueTypeCode.Bool;
        }

       
    }

    public class FuncLessThan : MetadataFunctions
    {
        public FuncLessThan(int[] inputs, int output, SttpValue[] values)
            : base(inputs, output, values)
        {
            ValidInputsEquals(2);
        }

        public override void Execute()
        {
            if (IfAnyNullAssignNull())
                return;

            Output = new SttpValue(Inputs(0).AsDouble < Inputs(1).AsDouble);
        }

        protected override SttpValueTypeCode GetReturnType(SttpValueTypeCode[] codes)
        {
            return SttpValueTypeCode.Bool;
        }
    }
    public class FuncLessThanOrEqual : MetadataFunctions
    {
        public FuncLessThanOrEqual(int[] inputs, int output, SttpValue[] values)
            : base(inputs, output, values)
        {
            ValidInputsEquals(2);
        }

        public override void Execute()
        {
            if (IfAnyNullAssignNull())
                return;

            Output = new SttpValue(Inputs(0).AsDouble <= Inputs(1).AsDouble);
        }

        protected override SttpValueTypeCode GetReturnType(SttpValueTypeCode[] codes)
        {
            return SttpValueTypeCode.Bool;
        }
    }
    public class FuncGreaterThan : MetadataFunctions
    {
        public FuncGreaterThan(int[] inputs, int output, SttpValue[] values)
            : base(inputs, output, values)
        {
            ValidInputsEquals(2);
        }

        public override void Execute()
        {
            if (IfAnyNullAssignNull())
                return;

            Output = new SttpValue(Inputs(0).AsDouble > Inputs(1).AsDouble);
        }

        protected override SttpValueTypeCode GetReturnType(SttpValueTypeCode[] codes)
        {
            return SttpValueTypeCode.Bool;
        }
    }
    public class FuncGreaterThanOrEqual : MetadataFunctions
    {
        public FuncGreaterThanOrEqual(int[] inputs, int output, SttpValue[] values)
            : base(inputs, output, values)
        {
            ValidInputsEquals(2);
        }

        public override void Execute()
        {
            if (IfAnyNullAssignNull())
                return;

            Output = new SttpValue(Inputs(0).AsDouble >= Inputs(1).AsDouble);
        }

        protected override SttpValueTypeCode GetReturnType(SttpValueTypeCode[] codes)
        {
            return SttpValueTypeCode.Bool;
        }
    }
}
