using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp
{
    //public class SttpQueryInputDirectColumn
    //{
    //    public string ColumnName;
    //    public string Variable;
    //}




    //public class SttpProcedure
    //{
    //    public string Function;
    //    public string[] InputVariables;
    //    public string OutputVariables;
    //}

    //public class SttpOutputVariables
    //{
    //    public string Variable;
    //    public string ColumnName;
    //    public SttpValueTypeCode ColumnType;
    //}

    public class SttpQueryLiteral
    {
        public string Variable;
        public SttpValue Value;

        public SttpQueryLiteral(string variable, SttpValue value)
        {
            Variable = variable;
            Value = value;
        }
    }

    /// <summary>
    /// The STTP based query expression object
    /// </summary>
    public class SttpCompiledQueryExpression
    {
        private class IndirectColumn
        {
            public List<JoinPath> JoinPath = new List<JoinPath>();
            public string ColumnName;

            public IndirectColumn(string expression)
            {
                int positionOfInnerJoin = expression.IndexOf("->");
                int positionOfLeftJoin = expression.IndexOf("?>");

                while (positionOfLeftJoin >= 0 || positionOfInnerJoin >= 0)
                {
                    bool isInnerJoin;
                    if (positionOfLeftJoin < 0)
                    {
                        isInnerJoin = true;
                    }
                    else if (positionOfInnerJoin < 0)
                    {
                        isInnerJoin = false;
                    }
                    else if (positionOfLeftJoin < positionOfInnerJoin)
                    {
                        isInnerJoin = false;
                    }
                    else
                    {
                        isInnerJoin = true;
                    }

                    if (isInnerJoin)
                    {
                        string joinColumn = expression.Substring(0, positionOfInnerJoin).Trim();
                        string joinTable = expression.Substring(positionOfInnerJoin + 2, expression.IndexOf(".") - positionOfInnerJoin - 2).Trim();
                        JoinPath.Add(new JoinPath(joinColumn, joinTable, JoinType.Inner));
                        expression = expression.Substring(expression.IndexOf(".") + 1).Trim();
                    }
                    else
                    {
                        string joinColumn = expression.Substring(0, positionOfLeftJoin).Trim();
                        string joinTable = expression.Substring(positionOfInnerJoin + 2, expression.IndexOf(".") - positionOfLeftJoin - 2).Trim();
                        JoinPath.Add(new JoinPath(joinColumn, joinTable, JoinType.Left));
                        expression = expression.Substring(expression.IndexOf(".") + 1).Trim();
                    }

                    positionOfInnerJoin = expression.IndexOf("->");
                    positionOfLeftJoin = expression.IndexOf("?>");
                }

                ColumnName = expression.Trim();
            }
        }

        private class JoinPath
        {
            public string JoinedColumn;
            public string JoinedTable;
            public JoinType JoinType;

            public JoinPath(string column, string table, JoinType type = JoinType.Inner)
            {
                JoinedColumn = column;
                JoinedTable = table;
                JoinType = type;
            }
        }

        private enum JoinType
        {
            Left,
            Inner,
        }

        private class CompiledLine
        {
            public string AssignmentVariable;
            public string Expression;
            public string ColumnName;
            public CompiledFunction ExpressionIsFunction;
            public IndirectColumn ExpressionIsIndirectColumn;

            public CompiledLine(string line)
            {
                if (line.Contains("="))
                {
                    AssignmentVariable = line.Substring(0, line.IndexOf("=")).Trim();
                    line = line.Substring(line.IndexOf("=") + 1).Trim();
                }
                if (line.Contains(">>"))
                {
                    ColumnName = line.Substring(line.IndexOf(">>") + 2).Trim();
                    line = line.Substring(0, line.IndexOf(">>")).Trim();
                }
                Expression = line;

                if (Expression.Contains("("))
                {
                    ExpressionIsFunction = new CompiledFunction(Expression);
                }
                else if (Expression.Contains("->") || Expression.Contains("?>"))
                {
                    ExpressionIsIndirectColumn = new IndirectColumn(Expression);
                }
            }
        }

        private class CompiledFunction
        {
            public string FunctionName;
            public List<string> Args = new List<string>();
            public List<CompiledFunction> ArgsAsFunctions = new List<CompiledFunction>();
            public List<IndirectColumn> ArgsAsIndirectColumn = new List<IndirectColumn>();

            public CompiledFunction(string expression)
            {
                StringBuilder sb = new StringBuilder();

                int pCount = 0;
                bool isDone = false;
                foreach (var c in expression)
                {
                    if (isDone)
                    {
                        sb.Append(c);
                    }
                    else
                    {
                        switch (c)
                        {
                            case '(':
                                if (pCount == 0)
                                {
                                    FunctionName = ToTrimString(sb);
                                    sb.Clear();
                                }
                                else
                                {
                                    sb.Append('(');
                                }
                                pCount++;
                                break;
                            case ',':
                                if (pCount == 1)
                                {
                                    Args.Add(ToTrimString(sb));
                                    sb.Clear();
                                }
                                else
                                {
                                    sb.Append(',');
                                }
                                break;
                            case ')':
                                if (pCount == 1)
                                {
                                    Args.Add(ToTrimString(sb));
                                    sb.Clear();
                                    isDone = true;
                                }
                                else
                                {
                                    sb.Append(')');
                                }
                                pCount--;
                                if (pCount < 0)
                                    throw new Exception("Too many close Parentheses");
                                break;
                            default:
                                sb.Append(c);
                                break;

                        }
                    }
                }

                if (pCount != 0)
                    throw new Exception("Parentheses don't line up.");
                if (!isDone)
                    throw new Exception("Missing Parentheses");
                if (ToTrimString(sb).Length > 0)
                    throw new Exception("Unknown characters after the function");

                foreach (var arg in Args)
                {
                    if (arg.Contains("("))
                    {
                        ArgsAsFunctions.Add(new CompiledFunction(arg));
                        ArgsAsIndirectColumn.Add(null);
                    }
                    else if (arg.Contains("->") || arg.Contains("?>"))
                    {
                        ArgsAsIndirectColumn.Add(new IndirectColumn(arg));
                        ArgsAsFunctions.Add(null);
                    }
                    else
                    {
                        ArgsAsFunctions.Add(null);
                        ArgsAsIndirectColumn.Add(null);
                    }
                }
            }
        }

        public Dictionary<string, SttpValue> Literals = new Dictionary<string, SttpValue>();
        public HashSet<string> Variables = new HashSet<string>();

        public SttpCompiledQueryExpression(SttpQueryExpression queryExpression)
        {
            var expr = new Dictionary<string, List<string>>(queryExpression.Sections);
            Literals = new Dictionary<string, SttpValue>(queryExpression.Literals);

            var withSection = new List<CompiledLine>();
            var whereSection = new List<CompiledLine>();
            var selectSection = new List<CompiledLine>();

            if (expr.ContainsKey("WITH"))
            {
                foreach (var row in expr["WITH"])
                {
                    withSection.Add(new CompiledLine(row));
                }
            }
            if (expr.ContainsKey("WHERE"))
            {
                foreach (var row in expr["WHERE"])
                {
                    whereSection.Add(new CompiledLine(row));
                }
            }
            if (expr.ContainsKey("SELECT"))
            {
                foreach (var row in expr["SELECT"])
                {
                    selectSection.Add(new CompiledLine(row));
                }
            }

            //Error if there are any duplicates.

            //Define all sequence of functions.

            //Define all outputs.

            foreach (var item in queryExpression.Sections)
            {
                switch (item.Key)
                {
                    case "SELECT":
                        ParseSelect(item.Value);
                        break;
                    case "FROM":

                    case "WHERE":

                        break;
                    default:
                        throw new Exception("Unknown keyword " + item.Key);
                }
            }
        }

        private void FindColumns(Dictionary<string, List<string>> expr, string keyword)
        {
            if (expr.ContainsKey(keyword))
            {
                foreach (var line in expr[keyword])
                {
                    if (line.Contains("="))
                    {
                        Variables.Add(line.Split('=')[0].Trim());
                    }
                }
            }
        }

        private void FindVariables(Dictionary<string, List<string>> expr, string keyword)
        {
            if (expr.ContainsKey(keyword))
            {
                foreach (var line in expr[keyword])
                {
                    if (line.Contains("="))
                    {
                        Variables.Add(line.Split('=')[0].Trim());
                    }
                }
            }
        }

        private void ParseSelect(List<string> lines)
        {
            for (var x = 0; x < lines.Count; x++)
            {
                string line = lines[x];
                string variableName = null;

                if (line.Contains(">>"))
                {
                    variableName = line.Substring(0, line.LastIndexOf(">>"));
                    line = line.Substring(line.LastIndexOf(">>") + 2);
                }

                if (line.Contains(">>"))
                    throw new Exception(">> is only permitted once per line at the end of the line.");
                if (line.Contains("("))
                    throw new Exception("SELECT cannot contain functions");

                if (line.Contains("->") || line.Contains("?>"))
                {
                    //This is an indirect column.
                }
                else
                {

                }
            }
        }

        private class SttpFunction
        {
            public string FunctionName;
            public List<string> Args = new List<string>();
            public string VariableOutput;
        }

        private static readonly char[] TrimChars = ", \t\r\n".ToCharArray();

        private static string ToTrimString(StringBuilder sb)
        {
            if (sb.Length == 0)
                return string.Empty;
            return sb.ToString().Trim(TrimChars);
        }

        private class Records
        {
            public bool IsFunction;

            public string FunctionName;
            public string OutputVariable;
            public List<Records> Arguments = new List<Records>();
        }

    }
}
