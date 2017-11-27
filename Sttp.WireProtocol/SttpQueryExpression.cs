using System;
using System.Collections.Generic;

namespace Sttp
{
    public class SttpQueryInputDirectColumn
    {
        public string ColumnName;
        public int VariableIndex;
    }

    public class SttpQueryInputIndirectColumn
    {
        public List<SttpQueryJoinPath> JoinPath;
        public string ColumnName;
        public int VariableIndex;
    }

    public class SttpQueryJoinPath
    {
        public string JoinedColumn;
        public string JoinedTable;
        public JoinType JoinType;
    }

    public enum JoinType
    {
        Left,
        Inner,
    }

    public class SttpQueryInputValue
    {
        public SttpValue Value;
        public int VariableIndex;
    }

    public class SttpProcedure
    {
        public string Function;
        public int[] VariableIndexInputs;
        public int OutputVariableIndex;
    }

    public class SttpOutputVariables
    {
        public int VariableIndex;
        public string ColumnName;
        public SttpValueTypeCode ColumnType;
    }

    /// <summary>
    /// The STTP based query expression object
    /// </summary>
    public class SttpQueryExpression
    {
        public string BaseTable;
        public List<SttpQueryInputDirectColumn> DirectColumnInputs = new List<SttpQueryInputDirectColumn>();
        public List<SttpQueryInputIndirectColumn> IndirectColumnInputs = new List<SttpQueryInputIndirectColumn>();
        public List<SttpQueryInputValue> ValueInputs = new List<SttpQueryInputValue>();
        public List<SttpProcedure> Procedures = new List<SttpProcedure>();
        public List<SttpOutputVariables> Outputs = new List<SttpOutputVariables>();
        public int? WhereBooleanVariableIndex;

        public void DefineDirectColumn(string columnName, int variableIndex)
        {
            DirectColumnInputs.Add(new SttpQueryInputDirectColumn() { ColumnName = columnName, VariableIndex = variableIndex });
        }
        public void DefineIndirectColumn(List<SttpQueryJoinPath> joinPath, string columnName, int variableIndex)
        {
            IndirectColumnInputs.Add(new SttpQueryInputIndirectColumn() { JoinPath = joinPath, ColumnName = columnName, VariableIndex = variableIndex });
        }

        public void DefineValue(int variableIndex, SttpValue value)
        {
            ValueInputs.Add(new SttpQueryInputValue() { VariableIndex = variableIndex, Value = value });
        }

        public void DefineProcedures(string function, int[] variableIndexInputs, int outputVariableIndex)
        {
            Procedures.Add(new SttpProcedure() { Function = function, VariableIndexInputs = variableIndexInputs, OutputVariableIndex = outputVariableIndex });
        }

        public void DefineOutputs(int variableIndex, string columnName, SttpValueTypeCode columnType)
        {
            Outputs.Add(new SttpOutputVariables() { VariableIndex = variableIndex, ColumnName = columnName, ColumnType = columnType });
        }


        public static SttpQueryExpression ParseSql(string sql)
        {
            //Return Something
            return null;
        }

        public static SttpQueryExpression ParseFilterExpression(string text)
        {
            //This is the expression filter that GPA uses right now.
            return null;
        }
    }
}
