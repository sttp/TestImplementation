using System.Collections.Generic;

namespace Sttp
{
    public class SttpQueryTable
    {
        public string TableName;
        public int TableIndex;
    }

    public class SttpQueryColumnInputs
    {
        public int TableIndex;
        public string ColumnName;
        public int VariableIndex;
    }

    public class SttpQueryValueInputs
    {
        public SttpValue Value;
        public int VariableIndex;
    }

    public class SttpOutputVariables
    {
        public int VariableIndex;
        public string ColumnName;
        public SttpValueTypeCode ColumnType;
    }

    public class SttpQueryJoinedTables
    {
        public int ColumnVariableIndexLeft;
        public int ColumnVariableIndexRight;
        public JoinType JoinType; 
    }

    public enum JoinType
    {
        Left,
        Right,
        Inner,
        Outer,
    }

    public class SttpProcedure
    {
        public string Function;
        public int[] ParameterAliasInputs;
        public int VariableIndex;
    }

    /// <summary>
    /// The STTP based query expression object
    /// </summary>
    public class SttpQueryExpression
    {
        public List<SttpQueryTable> Tables = new List<SttpQueryTable>();
        public List<SttpQueryColumnInputs> ColumnInputs = new List<SttpQueryColumnInputs>();
        public List<SttpQueryValueInputs> ValueInputs = new List<SttpQueryValueInputs>();
        public List<SttpQueryJoinedTables> Joins = new List<SttpQueryJoinedTables>();
        public List<SttpProcedure> Procedures = new List<SttpProcedure>();
        public List<SttpOutputVariables> Outputs = new List<SttpOutputVariables>();

        public void DefineTable(string tableName, int tableIndex)
        {
            Tables.Add(new SttpQueryTable() { TableName = tableName, TableIndex = tableIndex });
        }
        public void DefineColumnInputs(int tableIndex, string columnName, int variableIndex)
        {
            ColumnInputs.Add(new SttpQueryColumnInputs() { TableIndex = tableIndex, ColumnName = columnName, VariableIndex = variableIndex});
        }

        public void DefineJoins(int columnVariableIndexLeft, int columnVariableIndexRight, JoinType joinType)
        {
            Joins.Add(new SttpQueryJoinedTables() { ColumnVariableIndexLeft = columnVariableIndexLeft, ColumnVariableIndexRight = columnVariableIndexRight, JoinType = joinType });
        }

        public void DefineValueInputs(int variableIndex, SttpValue value)
        {
            ValueInputs.Add(new SttpQueryValueInputs() { VariableIndex = variableIndex, Value = value });
        }

        public void DefineProcedures(string function, int[] parameters, int variableIndex)
        {
            Procedures.Add(new SttpProcedure() { Function = function, ParameterAliasInputs = parameters, VariableIndex = variableIndex });
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
