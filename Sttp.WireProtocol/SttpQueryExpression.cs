using System.Collections.Generic;

namespace Sttp
{
    public class SttpQueryTable
    {
        public string TableName;
        public int AliasName;
    }

    public class SttpQueryColumns
    {
        public int TableAlias;
        public string ColumnName;
        public int ColumnAlias;
        public int OutputAsColumnIndex;
        public string OutputAsColumnName;
    }

    public class SttpQueryJoinedTables
    {
        public int ColumnIndexLeft;
        public int ColumnIndexRight;
        public bool IsLeftJoin;
    }

    public class SttpQueryValues
    {
        public int VariableName;
        public SttpValue Value;
    }

    public class SttpProcedure
    {
        public string Function;
        public int[] Parameters;
        public int Results;
    }

    /// <summary>
    /// The STTP based query expression object
    /// </summary>
    public class SttpQueryExpression
    {
        public List<SttpQueryTable> Tables = new List<SttpQueryTable>();
        public List<SttpQueryColumns> Columns = new List<SttpQueryColumns>();
        public List<SttpQueryJoinedTables> Joins = new List<SttpQueryJoinedTables>();
        public List<SttpQueryValues> Values = new List<SttpQueryValues>();
        public List<SttpProcedure> Procedures = new List<SttpProcedure>();

        public void DefineTable(string tableName, int aliasName)
        {
            Tables.Add(new SttpQueryTable() { TableName = tableName, AliasName = aliasName });
        }
        public void DefineColumns(int tableAlias, string columnName, int columnAlias, int outputAsColumnIndex, string outputAsColumnName)
        {
            Columns.Add(new SttpQueryColumns() { TableAlias = tableAlias, ColumnName = columnName, ColumnAlias = columnAlias, OutputAsColumnIndex = outputAsColumnIndex, OutputAsColumnName = outputAsColumnName });
        }

        public void DefineJoins(int columnIndexLeft, int columnIndexRight, bool isLeftJoin)
        {
            Joins.Add(new SttpQueryJoinedTables() { ColumnIndexLeft = columnIndexLeft, ColumnIndexRight = columnIndexRight, IsLeftJoin = isLeftJoin });
        }

        public void DefineValues(int variableName, SttpValue value)
        {
            Values.Add(new SttpQueryValues() { VariableName = variableName, Value = value });
        }

        public void DefineProcedures(string function, int[] parameters, int results)
        {
            Procedures.Add(new SttpProcedure() { Function = function, Parameters = parameters, Results = results });
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
