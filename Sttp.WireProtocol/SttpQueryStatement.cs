using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sttp.Codec;

namespace Sttp
{
    public class SttpQueryColumn
    {
        public int TableIndex;
        public string ColumnName;
        public int Variable;

        public SttpQueryColumn(int tableIndex, string columnName, int variable)
        {
            TableIndex = tableIndex;
            ColumnName = columnName;
            Variable = variable;
        }
    }

    public class SttpQueryJoinedTable
    {
        public int TableIndex;
        public SttpQueryJoinPath[] JoinPath;

        public SttpQueryJoinedTable(int tableIndex, SttpQueryJoinPath[] joinPath)
        {
            TableIndex = tableIndex;
            JoinPath = joinPath;
        }
    }

    public class SttpQueryJoinPath
    {
        public string JoinedColumn;
        public string JoinedTable;
        public JoinType JoinType;

        public SttpQueryJoinPath(string column, string table, JoinType type = JoinType.Inner)
        {
            JoinedColumn = column;
            JoinedTable = table;
            JoinType = type;
        }
    }

    public enum JoinType
    {
        Left,
        Inner,
    }

    public class SttpQueryLiterals
    {
        public SttpValue Value;
        public int Variable;

        public SttpQueryLiterals(SttpValue value, int variable)
        {
            Value = value;
            Variable = variable;
        }
    }

    public class SttpProcedureStep
    {
        public string Function;
        public int[] InputVariables;
        public int OutputVariable;

        public SttpProcedureStep(string function, int[] inputVariables, int outputVariable)
        {
            Function = function;
            InputVariables = inputVariables;
            OutputVariable = outputVariable;
        }
    }

    public class SttpOutputColumns
    {
        public int Variable;
        public string ColumnName;
    }

    /// <summary>
    /// The STTP based query expression object
    /// </summary>
    public class SttpQueryStatement
    {
        public string DirectTable;
        public List<SttpQueryJoinedTable> JoinedTables = new List<SttpQueryJoinedTable>();
        public List<SttpQueryLiterals> Literals = new List<SttpQueryLiterals>();
        public List<SttpQueryColumn> ColumnInputs = new List<SttpQueryColumn>();
        public List<SttpProcedureStep> Procedure = new List<SttpProcedureStep>();
        public List<SttpOutputColumns> Outputs = new List<SttpOutputColumns>();
        public List<int> GroupByVariables = new List<int>();
        public string WhereBooleanVariable = null;
        public List<SttpProcedureStep> HavingProcedure = new List<SttpProcedureStep>();
        public string HavingBooleanVariable = null;

        public int Limit = -1;

        public SttpQueryStatement(PayloadReader reader)
        {
            throw new NotImplementedException();
        }

        public void Save(PayloadWriter payloadWriter)
        {
            throw new NotImplementedException();
        }
    }
}
