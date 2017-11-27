using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp
{
    public class SttpQueryInputDirectColumn
    {
        public string ColumnName;
        public string Variable;
    }

    public class SttpQueryInputIndirectColumn
    {
        public SttpQueryJoinPath[] JoinPath;
        public string ColumnName;
        public string Variable;
    }

    public class SttpQueryJoinPath
    {
        public string JoinedColumn;
        public string JoinedTable;
        public JoinType JoinType;

        public static SttpQueryJoinPath Create(string column, string table, JoinType type = JoinType.Inner)
        {
            return new SttpQueryJoinPath() { JoinedColumn = column, JoinedTable = table, JoinType = type };
        }
    }

    public enum JoinType
    {
        Left,
        Inner,
    }

    public class SttpQueryInputValue
    {
        public SttpValue Value;
        public string Variable;
    }

    public class SttpProcedure
    {
        public string Function;
        public string[] InputVariables;
        public string[] OutputVariables;
    }

    public class SttpOutputVariables
    {
        public string Variable;
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
        public string WhereBooleanVariable;

        public void DefineDirectColumn(string variable, string columnName)
        {
            DirectColumnInputs.Add(new SttpQueryInputDirectColumn() { ColumnName = columnName, Variable = variable });
        }
        public void DefineIndirectColumn(string variable, string columnName, params SttpQueryJoinPath[] joinPath)
        {
            IndirectColumnInputs.Add(new SttpQueryInputIndirectColumn() { JoinPath = joinPath, ColumnName = columnName, Variable = variable });
        }

        public void DefineValue(string variable, SttpValue value)
        {
            ValueInputs.Add(new SttpQueryInputValue() { Variable = variable, Value = value });
        }

        public void DefineProcedures(string function, string[] inputVariables, string[] outputVariables)
        {
            Procedures.Add(new SttpProcedure() { Function = function, InputVariables = inputVariables, OutputVariables = outputVariables });
        }

        public void DefineOutputs(string variable, string columnName, SttpValueTypeCode columnType)
        {
            Outputs.Add(new SttpOutputVariables() { Variable = variable, ColumnName = columnName, ColumnType = columnType });
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"TABLE {BaseTable}");
            foreach (var column in DirectColumnInputs)
            {
                sb.AppendLine($"COLUMN {column.Variable}: [{column.ColumnName}]");
            }
            foreach (var column in IndirectColumnInputs)
            {
                sb.Append($"COLUMN {column.Variable}: ");
                foreach (var link in column.JoinPath)
                {
                    if (link.JoinType == JoinType.Inner)
                    {
                        sb.Append($"[{link.JoinedColumn}].[{link.JoinedTable}].");

                    }
                    else if (link.JoinType == JoinType.Left)
                    {
                        sb.Append($"[{link.JoinedColumn}]?.[{link.JoinedTable}].");
                    }
                    else
                    {
                        throw new Exception("Join type invalid");
                    }
                }
                sb.AppendLine($"[{column.ColumnName}]");
            }
            foreach (var input in ValueInputs)
            {
                sb.AppendLine($"VALUE {input.Variable}: {input.Value.AsTypeString}");
            }
            foreach (var step in Procedures)
            {
                sb.AppendLine($"PROCEDURE ([{String.Join("],[", step.OutputVariables)}]): {step.Function}([{String.Join("],[", step.InputVariables)}])");
            }
            foreach (var output in Outputs)
            {
                sb.AppendLine($"OUTPUT ({output.ColumnType})[{output.Variable}] AS [{output.ColumnName}]");
            }
            sb.AppendLine($"WHERE {WhereBooleanVariable}");

            return sb.ToString();
        }

        public static SttpQueryExpression ParseSql(string sql)
        {
            int indexSelect = sql.IndexOf("SELECT ", StringComparison.CurrentCultureIgnoreCase);
            int indexfrom = sql.IndexOf(" FROM ", StringComparison.CurrentCultureIgnoreCase);
            int indexwhere = sql.IndexOf(" WHERE ", StringComparison.CurrentCultureIgnoreCase);

            if (indexwhere >= 0)
                throw new Exception("Cannot parse a WHERE statement yet");

            if (indexfrom < 0)
                throw new Exception("must have a FROM statement");

            if (indexSelect < 0)
                throw new Exception("must have a SELECT statement");


            string selectStatement = sql.Substring(indexSelect + 7, indexfrom - (indexSelect + 7)).Trim();
            string fromStatement = sql.Substring(indexfrom + 6).Trim();
            string[] columns = selectStatement.Split(',');

            var rv = new SttpQueryExpression();
            rv.BaseTable = fromStatement;
            for (var index = 0; index < columns.Length; index++)
            {
                rv.DefineDirectColumn(index.ToString(), columns[index].Trim());
                rv.DefineOutputs(index.ToString(), columns[index].Trim(), SttpValueTypeCode.Null);
            }
            return rv;
        }

        public static SttpQueryExpression ParseFilterExpression(string text)
        {
            //This is the expression filter that GPA uses right now.
            return null;
        }

        internal void DefineDirectColumn(string v, SttpValue sttpValue)
        {
            throw new NotImplementedException();
        }
    }
}
