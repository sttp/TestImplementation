using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;
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

        public void GetFullOutputString(string linePrefix, StringBuilder builder)
        {
            builder.Append(linePrefix); builder.AppendLine($"({nameof(SttpQueryColumn)}) {Variable} = {TableIndex}.{ColumnName} ");
        }

        public void Save(SttpMarkupWriter writer)
        {
            using (writer.StartElement("Column"))
            {
                writer.WriteValue("TableIndex", TableIndex);
                writer.WriteValue("ColumnName", ColumnName);
                writer.WriteValue("Variable", Variable);
            }
        }
    }

    public class SttpQueryJoinedTable
    {
        public int ExistingTableIndex;
        public string ExistingForeignKeyColumn;
        public string ForeignTable;
        public int ForeignTableIndex;

        public SttpQueryJoinedTable(int existingTableIndex, string existingForeignKeyColumn, string foreignTable, int foreignTableIndex)
        {
            ExistingTableIndex = existingTableIndex;
            ExistingForeignKeyColumn = existingForeignKeyColumn;
            ForeignTable = foreignTable;
            ForeignTableIndex = foreignTableIndex;
        }

        public SttpQueryJoinedTable(PayloadReader rd)
        {
            ExistingTableIndex = rd.ReadInt32();
            ExistingForeignKeyColumn = rd.ReadString();
            ForeignTable = rd.ReadString();
            ForeignTableIndex = rd.ReadInt32();
        }

        public void Save(PayloadWriter wr)
        {
            wr.Write(ExistingTableIndex);
            wr.Write(ExistingForeignKeyColumn);
            wr.Write(ForeignTable);
            wr.Write(ForeignTableIndex);
        }

        public void GetFullOutputString(string linePrefix, StringBuilder builder)
        {
            builder.Append(linePrefix); builder.AppendLine($"({nameof(SttpQueryJoinedTable)}) ({ExistingTableIndex}) LEFT JOIN {ForeignTable} AS ({ForeignTableIndex}) ON ({ExistingTableIndex}).{ExistingForeignKeyColumn} = ({ForeignTableIndex}).[PrimaryKey]");
        }

        public void Save(SttpMarkupWriter writer)
        {
            using (writer.StartElement("JoinedTable"))
            {
                writer.WriteValue("ExistingTableIndex", ExistingTableIndex);
                writer.WriteValue("ExistingForeignKeyColumn", ExistingForeignKeyColumn);
                writer.WriteValue("ForeignTable", ForeignTable);
                writer.WriteValue("ForeignTableIndex", ForeignTableIndex);
            }
        }
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
        public SttpQueryLiterals(PayloadReader rd)
        {
            Variable = rd.ReadInt32();
            Value = rd.ReadSttpValue();
        }
        public void Save(PayloadWriter wr)
        {
            wr.Write(Variable);
            wr.Write(Value);
        }

        public void GetFullOutputString(string linePrefix, StringBuilder builder)
        {
            builder.Append(linePrefix); builder.AppendLine($"({nameof(SttpQueryLiterals)}) {Variable} = {Value.AsString} ");
        }

        public void Save(SttpMarkupWriter writer)
        {
            using (writer.StartElement("Literal"))
            {
                writer.WriteValue("Value", Value);
                writer.WriteValue("Variable", Variable);
            }
        }
        
    }

    public class SttpQueryProcedureStep
    {
        public string Function;
        public List<int> InputVariables;
        public int OutputVariable;

        public SttpQueryProcedureStep(string function, List<int> inputVariables, int outputVariable)
        {
            Function = function;
            InputVariables = inputVariables;
            OutputVariable = outputVariable;
        }

        public SttpQueryProcedureStep(PayloadReader rd)
        {
            Function = rd.ReadString();
            InputVariables = rd.ReadListInt();
            OutputVariable = rd.ReadInt32();
        }
        public void Save(PayloadWriter wr)
        {
            wr.Write(Function);
            wr.Write(InputVariables);
            wr.Write(OutputVariable);
        }

        public void GetFullOutputString(string linePrefix, StringBuilder builder)
        {
            builder.Append(linePrefix); builder.AppendLine($"({nameof(SttpQueryProcedureStep)}) {OutputVariable} = {Function}({string.Join(",", InputVariables)}) ");
        }

        public void Save(SttpMarkupWriter writer)
        {
            using (writer.StartElement("Procedure"))
            {
                writer.WriteValue("Function", Function);
                writer.WriteValue("OutputVariable", OutputVariable);

                using (writer.StartElement("InputVariables"))
                {
                    foreach (var x in InputVariables)
                    {
                        writer.WriteValue("Item", x);
                    }
                }
            }
        }
        
    }

    public class SttpQueryOutputColumns
    {
        public int Variable;
        public string ColumnName;

        public SttpQueryOutputColumns(int variable, string columnName)
        {
            Variable = variable;
            ColumnName = columnName;
        }

        public SttpQueryOutputColumns(PayloadReader rd)
        {
            Variable = rd.ReadInt32();
            ColumnName = rd.ReadString();
        }

        public void Save(PayloadWriter wr)
        {
            wr.Write(Variable);
            wr.Write(ColumnName);
        }

        public void GetFullOutputString(string linePrefix, StringBuilder builder)
        {
            builder.Append(linePrefix); builder.AppendLine($"({nameof(SttpQueryOutputColumns)}) {Variable} = {ColumnName} ");
        }

        public void Save(SttpMarkupWriter writer)
        {
            using (writer.StartElement("Output"))
            {
                writer.WriteValue("Variable", Variable);
                writer.WriteValue("ColumnName", ColumnName);
            }
        }

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
        public List<SttpQueryProcedureStep> Procedure = new List<SttpQueryProcedureStep>();
        public List<SttpQueryOutputColumns> Outputs = new List<SttpQueryOutputColumns>();
        public List<int> GroupByVariables = new List<int>();
        public int? WhereBooleanVariable;
        public List<SttpQueryProcedureStep> HavingProcedure = new List<SttpQueryProcedureStep>();
        public int? HavingBooleanVariable;
        public int? Limit;

        public SttpQueryStatement()
        {

        }

        public SttpQueryStatement(XmlReader reader)
        {
            if (reader.NodeType != XmlNodeType.Element)
                throw new Exception("Must seek to the SttpQuery element.");
            if (reader.Name != "SttpQuery")
                throw new Exception("Must seek to the SttpQuery element.");

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.None:
                    case XmlNodeType.Element:
                    case XmlNodeType.Attribute:
                    case XmlNodeType.Text:
                    case XmlNodeType.CDATA:
                    case XmlNodeType.EntityReference:
                    case XmlNodeType.Entity:
                    case XmlNodeType.ProcessingInstruction:
                    case XmlNodeType.Comment:
                    case XmlNodeType.Document:
                    case XmlNodeType.DocumentType:
                    case XmlNodeType.DocumentFragment:
                    case XmlNodeType.Notation:
                    case XmlNodeType.Whitespace:
                    case XmlNodeType.SignificantWhitespace:
                    case XmlNodeType.EndElement:
                    case XmlNodeType.EndEntity:
                    case XmlNodeType.XmlDeclaration:
                        break;
                }
            }

        }

        /// <summary>
        /// This will remap all table and variable indexes to a sequential range starting with 0.
        /// As a nature of the remap, this will also validate the query's indexing.
        /// </summary>
        public void ValidateAndRemapAllIndexes(out int variableIndexCount, out int tableIndexCount)
        {
            Dictionary<int, int> tableIndexMap = new Dictionary<int, int>();
            Dictionary<int, int> variableIndexMap = new Dictionary<int, int>();

            //Fill the table indexes
            tableIndexMap.Add(0, 0); //The direct table gets this mapping.
            foreach (var table in JoinedTables)
            {
                if (!tableIndexMap.ContainsKey(table.ExistingTableIndex))
                {
                    throw new Exception("Column Index Invalid");
                }
                tableIndexMap.Add(table.ForeignTableIndex, tableIndexMap.Count);
            }

            //Fill the variable indexes
            foreach (var v in Literals)
            {
                variableIndexMap.Add(v.Variable, variableIndexMap.Count);
            }
            foreach (var v in ColumnInputs)
            {
                if (!tableIndexMap.ContainsKey(v.TableIndex))
                {
                    throw new Exception("Column Index Invalid");
                }
                variableIndexMap.Add(v.Variable, variableIndexMap.Count);
            }

            foreach (var v in Procedure)
            {
                foreach (var vIn in v.InputVariables)
                {
                    if (!variableIndexMap.ContainsKey(vIn))
                        throw new Exception("Function with undefined input variable");
                }
                if (!variableIndexMap.ContainsKey(v.OutputVariable))
                {
                    variableIndexMap.Add(v.OutputVariable, variableIndexMap.Count);
                }
            }

            foreach (var v in Outputs)
            {
                if (!variableIndexMap.ContainsKey(v.Variable))
                {
                    throw new Exception("Output Column with undefined variable");
                }
            }

            if (WhereBooleanVariable.HasValue && !variableIndexMap.ContainsKey(WhereBooleanVariable.Value))
            {
                throw new Exception("WhereBooleanVariable with undefined variable");
            }

            //ToDo: Still need Having, Group By defined.

            //Now: Remap since the mapping is complete.
            foreach (var table in JoinedTables)
            {
                table.ForeignTableIndex = tableIndexMap[table.ForeignTableIndex];
                table.ExistingTableIndex = tableIndexMap[table.ExistingTableIndex];
            }

            //Fill the variable indexes
            foreach (var v in Literals)
            {
                v.Variable = variableIndexMap[v.Variable];
            }

            foreach (var v in ColumnInputs)
            {
                v.TableIndex = tableIndexMap[v.TableIndex];
                v.Variable = variableIndexMap[v.Variable];
            }

            foreach (var v in Procedure)
            {
                for (var x = 0; x < v.InputVariables.Count; x++)
                {
                    v.InputVariables[x] = variableIndexMap[v.InputVariables[x]];
                }
                v.OutputVariable = variableIndexMap[v.OutputVariable];
            }

            foreach (var v in Outputs)
            {
                v.Variable = variableIndexMap[v.Variable];
            }

            if (WhereBooleanVariable.HasValue)
            {
                WhereBooleanVariable = variableIndexMap[WhereBooleanVariable.Value];
            }

            tableIndexCount = tableIndexMap.Count;
            variableIndexCount = variableIndexMap.Count;
        }

        public void Save(XmlWriter writer)
        {

        }

        public void GetFullOutputString(string linePrefix, StringBuilder builder)
        {
            builder.Append(linePrefix); builder.AppendLine("(" + nameof(SttpQueryStatement) + ")");
            builder.Append(linePrefix); builder.AppendLine($"DirectTable: {DirectTable} ");
            builder.Append(linePrefix); builder.AppendLine($"WhereBooleanVariable: {WhereBooleanVariable} ");
            builder.Append(linePrefix); builder.AppendLine($"HavingBooleanVariable: {HavingBooleanVariable} ");
            builder.Append(linePrefix); builder.AppendLine($"Limit: {Limit} ");

            builder.Append(linePrefix); builder.AppendLine($"JoinedTables Count {JoinedTables.Count} ");
            foreach (var table in JoinedTables)
            {
                table.GetFullOutputString(linePrefix + " ", builder);
            }
            builder.Append(linePrefix); builder.AppendLine($"Literals Count {Literals.Count} ");
            foreach (var table in Literals)
            {
                table.GetFullOutputString(linePrefix + " ", builder);
            }
            builder.Append(linePrefix); builder.AppendLine($"ColumnInputs Count {ColumnInputs.Count} ");
            foreach (var table in ColumnInputs)
            {
                table.GetFullOutputString(linePrefix + " ", builder);
            }
            builder.Append(linePrefix); builder.AppendLine($"Procedure Count {Procedure.Count} ");
            foreach (var table in Procedure)
            {
                table.GetFullOutputString(linePrefix + " ", builder);
            }
            builder.Append(linePrefix); builder.AppendLine($"Outputs Count {Outputs.Count} ");
            foreach (var table in Outputs)
            {
                table.GetFullOutputString(linePrefix + " ", builder);
            }
            builder.Append(linePrefix); builder.AppendLine($"GroupByVariables Count {GroupByVariables.Count} ");
            builder.Append(linePrefix); builder.AppendLine($"Group By: ({string.Join(",", GroupByVariables)}) ");
            builder.Append(linePrefix); builder.AppendLine($"HavingProcedure Count {HavingProcedure.Count} ");
            foreach (var table in HavingProcedure)
            {
                table.GetFullOutputString(linePrefix + " ", builder);
            }
        }

        public SttpMarkup ToSttpMarkup()
        {
            var writer = new SttpMarkupWriter();
            using (writer.StartElement("SttpQuery"))
            {
                writer.WriteValue("DirectTable", DirectTable);
                writer.WriteValue("WhereBoolVariable", WhereBooleanVariable.ToString());
                writer.WriteValue("Limit", Limit.ToString());
                writer.WriteValue("HavingBooleanVariable", HavingBooleanVariable.ToString());

                using (writer.StartElement("JoinedTables"))
                {
                    foreach (var item in JoinedTables)
                    {
                        item.Save(writer);
                    }
                }

                using (writer.StartElement("Literals"))
                {
                    foreach (var item in Literals)
                    {
                        item.Save(writer);
                    }
                }


                using (writer.StartElement("ColumnInputs"))
                {
                    foreach (var item in ColumnInputs)
                    {
                        item.Save(writer);
                    }
                }

                using (writer.StartElement("Procedure"))
                {
                    foreach (var item in Procedure)
                    {
                        item.Save(writer);
                    }
                }

                using (writer.StartElement("Outputs"))
                {
                    foreach (var item in Outputs)
                    {
                        item.Save(writer);
                    }
                }

                using (writer.StartElement("GroupByVariables"))
                {
                    foreach (var item in GroupByVariables)
                    {
                        writer.WriteValue("Item", item);
                    }
                }

                using (writer.StartElement("HavingProcedure"))
                {
                    foreach (var item in HavingProcedure)
                    {
                        item.Save(writer);
                    }
                }
            }
            return writer.ToSttpMarkup();
        }
    }
}
