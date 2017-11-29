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

    //public class SttpQueryInputIndirectColumn
    //{
    //    public SttpQueryJoinPath[] JoinPath;
    //    public string ColumnName;
    //    public string Variable;
    //}

    //public class SttpQueryJoinPath
    //{
    //    public string JoinedColumn;
    //    public string JoinedTable;
    //    public JoinType JoinType;

    //    public static SttpQueryJoinPath Create(string column, string table, JoinType type = JoinType.Inner)
    //    {
    //        return new SttpQueryJoinPath() { JoinedColumn = column, JoinedTable = table, JoinType = type };
    //    }
    //}

    //public enum JoinType
    //{
    //    Left,
    //    Inner,
    //}

    //public class SttpQueryParameter
    //{
    //    public SttpValue Value;
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

    /// <summary>
    /// The STTP based query expression object
    /// </summary>
    public class SttpQueryExpression
    {
        public Dictionary<string, List<string>> Sections;
        public Dictionary<string, SttpValue> Literals;

        public SttpQueryExpression(string expression, Dictionary<string, SttpValue> literals = null)
        {
            if (literals != null)
            {
                Literals = new Dictionary<string, SttpValue>(literals);
            }
            else
            {
                Literals = new Dictionary<string, SttpValue>();
            }
            Sections = new Dictionary<string, List<string>>();

            expression = ExtractLiterals(expression);

            ExtractSections(expression);

        }

        /// <summary>
        /// This method will extract all literals and assign them a new variable name.
        /// </summary>
        private string ExtractLiterals(string expression)
        {
            int param = 1;

            StringBuilder sb = new StringBuilder();
            StringBuilder sbLiteral = new StringBuilder();

            bool isLiteral = false;
            for (var x = 0; x < expression.Length; x++)
            {
                char c = expression[x];
                char peek = ' ';
                if (x + 1 < expression.Length)
                    peek = expression[x + 1];

                if (c == '#')
                {
                    isLiteral = !isLiteral;
                    if (!isLiteral)
                    {
                        //Only on closing, if the next char is a # is this an escape character. 
                        if (peek == '#')
                        {
                            sbLiteral.Append('#');
                        }
                        else
                        {
                            SttpValue value = ParseLiteral(ToTrimString(sbLiteral));
                            while (Literals.ContainsKey($"{{{param}}}"))
                            {
                                param++;
                            }
                            Literals.Add($"{{{param}}}", value);
                            sbLiteral.Clear();
                            sb.Append($"{{{param}}}");
                        }
                    }
                }
                else if (isLiteral)
                {
                    sbLiteral.Append(c);
                }
                else
                {
                    switch (c)
                    {
                        default:
                            sb.Append(c);
                            break;
                    }
                }
            }
            if (isLiteral)
                throw new Exception("Literal Mismatch");
            if (sb.Length == 0)
                return string.Empty;
            return sb.ToString();
        }


        /// <summary>
        /// This method will extract all sections and occurs after extracting literals.
        /// </summary>
        /// <param name="expression"></param>
        private void ExtractSections(string expression)
        {
            StringBuilder sb = new StringBuilder();
            List<string> args = new List<string>();

            string functionName = null;
            int pCount = 0;
            foreach (var c in expression)
            {
                switch (c)
                {
                    case '(':
                        if (pCount == 0)
                        {
                            functionName = ToTrimString(sb);
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
                            args.Add(ToTrimString(sb));
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
                            args.Add(ToTrimString(sb));
                            sb.Clear();

                            if (Sections.ContainsKey(functionName))
                            {
                                Sections[functionName].AddRange(args);
                            }
                            else
                            {
                                Sections[functionName] = args;
                            }
                            args = new List<string>();
                            functionName = null;
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

            if (pCount != 0)
                throw new Exception("Parentheses don't line up.");
            if (ToTrimString(sb).Length > 0)
                throw new Exception("Unknown characters after all functions");
        }

        private string TrimArgument(string arg)
        {
            StringBuilder sb = new StringBuilder();
            bool isQuoted = false;
            bool hasSpace = false;
            foreach (var c in arg)
            {
                if (c == '\'')
                {
                    isQuoted = !isQuoted;
                    sb.Append(c);
                }
                else if (isQuoted)
                {
                    sb.Append(c);
                }
                else
                {
                    switch (c)
                    {
                        case '\r':
                        case '\n':
                        case '\t':
                        case ' ':
                            if (!hasSpace)
                            {
                                sb.Append(' ');
                                hasSpace = true;
                            }
                            break;
                        default:
                            hasSpace = false;
                            sb.Append(c);
                            break;

                    }
                }
            }

            if (sb.Length == 0)
                return string.Empty;
            return sb.ToString().Trim();
        }

        private SttpValue ParseLiteral(string literal)
        {
            //ToDo: Actually attempt to parse the literal
            return (SttpValue)literal;
        }

        private static readonly char[] TrimChars = ", \t\r\n".ToCharArray();

        private string ToTrimString(StringBuilder sb)
        {
            if (sb.Length == 0)
                return string.Empty;
            return TrimArgument(sb.ToString().Trim(TrimChars));
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (Literals.Count > 0)
            {
                sb.AppendLine($"LITERALS ( {string.Join("\n,", Literals.Select(x => $"#{x.Value.AsTypeString}# >> {x.Key}"))} )");
            }

            foreach (var item in Sections)
            {
                sb.AppendLine($"{item.Key}({string.Join("\n,", item.Value)})");
            }
            if (sb.Length == 0)
                return string.Empty;
            return sb.ToString();
        }

        //public List<SttpQueryInputDirectColumn> DirectColumnInputs = new List<SttpQueryInputDirectColumn>();
        //public List<SttpQueryInputIndirectColumn> IndirectColumnInputs = new List<SttpQueryInputIndirectColumn>();
        //public List<SttpProcedure> Procedures = new List<SttpProcedure>();
        //public List<SttpOutputVariables> Outputs = new List<SttpOutputVariables>();
        //public List<SttpQueryParameter> Parameters = new List<SttpQueryParameter>();

        //public string WhereBooleanVariable;
        //public string FromTable;
        //public long? Limit;
        //private int UndefinedExpressions = 1;

        //public SttpQueryExpression(string expression)
        //{
        //    string[] lines = expression.Split("\r\n;".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        //    foreach (var line in lines)
        //    {
        //        if (line.StartsWith("SELECT"))
        //        {
        //            ParseExpression(line);
        //        }
        //        if (line.StartsWith("FROM"))
        //        {
        //            FromTable = line.Substring(4).Trim();
        //        }
        //        if (line.StartsWith("WHERE"))
        //        {
        //            if (line.Contains(">>"))
        //                throw new Exception("Cannot contain >> in the WHERE clause");

        //            string subExpr = line.Substring(5).Trim();
        //            if (IsExpression(subExpr))
        //            {
        //                WhereBooleanVariable = "{Condition}";
        //                ParseExpression("SELECT! " + subExpr + " >> {Condition}");
        //            }
        //            else
        //            {
        //                WhereBooleanVariable = subExpr;
        //            }
        //        }
        //        if (line.StartsWith("LIMIT"))
        //        {
        //            Limit = long.Parse(line.Substring(5).Trim());
        //        }
        //    }
        //}

        //public bool IsExpression(string expr)
        //{
        //    return expr.IndexOfAny(":(".ToCharArray()) >= 0;
        //}
        //public bool IsFunction(string expr)
        //{
        //    return expr.IndexOfAny("(".ToCharArray()) >= 0;
        //}
        //public bool IsColumnRedirect(string expr)
        //{
        //    return expr.IndexOfAny(":".ToCharArray()) >= 0;
        //}

        //private void ParseExpression(string selectLine)
        //{
        //    bool suppressOutput;
        //    string line = selectLine;
        //    if (line.StartsWith("SELECT!"))
        //    {
        //        suppressOutput = true;
        //        line = line.Substring(7).Trim();
        //    }
        //    else
        //    {
        //        suppressOutput = false;
        //        line = line.Substring(6).Trim();
        //    }

        //    string variableName = null;
        //    if (line.Contains(">>"))
        //    {
        //        variableName = line.Substring(line.IndexOf(">>") + 2).Trim();
        //        line = line.Substring(0, line.IndexOf(">>")).Trim();
        //    }

        //    ParseExpression(suppressOutput, line, variableName);
        //}

        //private void ParseExpression(bool suppressOutput, string line, string variableName)
        //{
        //    if (IsFunction(line))
        //    {
        //        ParseFunction(line, out string functionName, out string[] parameters);
        //        if (variableName == null)
        //            variableName = $"{{Expr{UndefinedExpressions++}}}";

        //        DefineProcedures(functionName, parameters, variableName);
        //    }
        //    else if (IsColumnRedirect(line))
        //    {
        //        List<SttpQueryJoinPath> join = new List<SttpQueryJoinPath>();
        //        string[] parts = line.Split(':');
        //        if (variableName == null)
        //            variableName = parts.Last();

        //        for (int x = 0; x < parts.Length - 1; x++)
        //        {
        //            string part = parts[x];
        //            if (part.Contains("->"))
        //            {
        //                join.Add(SttpQueryJoinPath.Create(part.Substring(0, part.IndexOf("->")).Trim(), part.Substring(part.IndexOf("->") + 2).Trim(), JoinType.Inner));
        //            }
        //            else if (part.Contains("?>"))
        //            {
        //                join.Add(SttpQueryJoinPath.Create(part.Substring(0, part.IndexOf("?>")).Trim(), part.Substring(part.IndexOf("?>") + 2).Trim(), JoinType.Left));
        //            }
        //            else
        //            {
        //                throw new Exception("Syntax error in column redirection, Requires -> or ?>");
        //            }
        //        }
        //        DefineIndirectColumn(variableName, parts.Last(), join.ToArray());
        //    }
        //    else //Either a variable or a column
        //    {
        //        if (variableName == null)
        //            variableName = line;
        //        DefineDirectColumn(variableName, line);
        //    }
        //}

        //private void ParseFunction(string line, out string functionName, out string[] parameters)
        //{
        //    StringBuilder sb = new StringBuilder();

        //    functionName = null;
        //    List<string> plist = new List<string>();

        //    int pCount = 0;
        //    bool done = false;
        //    foreach (var c in line)
        //    {
        //        if (done)
        //        {
        //            sb.Append(c);
        //        }
        //        else
        //        {
        //            switch (c)
        //            {
        //                case '(':
        //                    if (pCount == 0)
        //                    {
        //                        functionName = sb.ToString().Trim();
        //                        sb.Clear();
        //                    }
        //                    else
        //                    {
        //                        sb.Append('(');
        //                    }
        //                    pCount++;
        //                    break;
        //                case ',':
        //                    if (pCount == 1)
        //                    {
        //                        plist.Add(sb.ToString().Trim());
        //                        sb.Clear();
        //                    }
        //                    else
        //                    {
        //                        sb.Append(',');
        //                    }
        //                    break;
        //                case ')':
        //                    if (pCount == 1)
        //                    {
        //                        plist.Add(sb.ToString().Trim());
        //                        sb.Clear();
        //                        done = true;
        //                    }
        //                    else
        //                    {
        //                        sb.Append(')');
        //                    }
        //                    pCount--;
        //                    break;
        //                default:
        //                    sb.Append(c);
        //                    break;

        //            }
        //        }
        //    }
        //    if (pCount != 0)
        //        throw new Exception("Parentheses don't line up.");
        //    if (string.IsNullOrEmpty(functionName))
        //        throw new Exception("Missing function name");
        //    if (sb.Length > 0 && sb.ToString().Trim().Length != 0)
        //        throw new Exception("Extra chars at the end of the string");

        //    parameters = plist.ToArray();
        //}







        //public void DefineParameter(string variable, SttpValue value)
        //{
        //    Parameters.Add(new SttpQueryParameter() { Variable = variable, Value = value });
        //}
        //public void DefineDirectColumn(string variable, string columnName)
        //{
        //    DirectColumnInputs.Add(new SttpQueryInputDirectColumn() { ColumnName = columnName, Variable = variable });
        //}
        //public void DefineIndirectColumn(string variable, string columnName, params SttpQueryJoinPath[] joinPath)
        //{
        //    IndirectColumnInputs.Add(new SttpQueryInputIndirectColumn() { JoinPath = joinPath, ColumnName = columnName, Variable = variable });
        //}



        //public void DefineProcedures(string function, string[] inputVariables, string outputVariables)
        //{
        //    Procedures.Add(new SttpProcedure() { Function = function, InputVariables = inputVariables, OutputVariables = outputVariables });
        //}

        //public void DefineOutputs(string variable, string columnName, SttpValueTypeCode columnType)
        //{
        //    Outputs.Add(new SttpOutputVariables() { Variable = variable, ColumnName = columnName, ColumnType = columnType });
        //}

        //public override string ToString()
        //{
        //    StringBuilder sb = new StringBuilder();

        //    sb.AppendLine($"TABLE {FromTable}");
        //    foreach (var column in DirectColumnInputs)
        //    {
        //        sb.AppendLine($"COLUMN {column.Variable}: [{column.ColumnName}]");
        //    }
        //    foreach (var column in IndirectColumnInputs)
        //    {
        //        sb.Append($"COLUMN {column.Variable}: ");
        //        foreach (var link in column.JoinPath)
        //        {
        //            if (link.JoinType == JoinType.Inner)
        //            {
        //                sb.Append($"[{link.JoinedColumn}].[{link.JoinedTable}].");

        //            }
        //            else if (link.JoinType == JoinType.Left)
        //            {
        //                sb.Append($"[{link.JoinedColumn}]?.[{link.JoinedTable}].");
        //            }
        //            else
        //            {
        //                throw new Exception("Join type invalid");
        //            }
        //        }
        //        sb.AppendLine($"[{column.ColumnName}]");
        //    }
        //    foreach (var input in Parameters)
        //    {
        //        sb.AppendLine($"VALUE {input.Variable}: {input.Value.AsTypeString}");
        //    }
        //    foreach (var step in Procedures)
        //    {
        //        sb.AppendLine($"PROCEDURE ([{String.Join("],[", step.OutputVariables)}]): {step.Function}([{String.Join("],[", step.InputVariables)}])");
        //    }
        //    foreach (var output in Outputs)
        //    {
        //        sb.AppendLine($"OUTPUT ({output.ColumnType})[{output.Variable}] AS [{output.ColumnName}]");
        //    }
        //    sb.AppendLine($"WHERE {WhereBooleanVariable}");

        //    return sb.ToString();
        //}

        //public static SttpQueryExpression ParseSql(string sql)
        //{
        //    int indexSelect = sql.IndexOf("SELECT ", StringComparison.CurrentCultureIgnoreCase);
        //    int indexfrom = sql.IndexOf(" FROM ", StringComparison.CurrentCultureIgnoreCase);
        //    int indexwhere = sql.IndexOf(" WHERE ", StringComparison.CurrentCultureIgnoreCase);

        //    if (indexwhere >= 0)
        //        throw new Exception("Cannot parse a WHERE statement yet");

        //    if (indexfrom < 0)
        //        throw new Exception("must have a FROM statement");

        //    if (indexSelect < 0)
        //        throw new Exception("must have a SELECT statement");


        //    string selectStatement = sql.Substring(indexSelect + 7, indexfrom - (indexSelect + 7)).Trim();
        //    string fromStatement = sql.Substring(indexfrom + 6).Trim();
        //    string[] columns = selectStatement.Split(',');

        //    var rv = new SttpQueryExpression();
        //    rv.FromTable = fromStatement;
        //    for (var index = 0; index < columns.Length; index++)
        //    {
        //        rv.DefineDirectColumn(index.ToString(), columns[index].Trim());
        //        rv.DefineOutputs(index.ToString(), columns[index].Trim(), SttpValueTypeCode.Null);
        //    }
        //    return rv;
        //}

        //public static SttpQueryExpression ParseFilterExpression(string text)
        //{
        //    //This is the expression filter that GPA uses right now.
        //    return null;
        //}

        //internal void DefineDirectColumn(string v, SttpValue sttpValue)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
