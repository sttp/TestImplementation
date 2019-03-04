//using System;
//using System.Collections.Generic;
//using CTP;

//namespace Sttp
//{
//    /// <summary>
//    /// All of the attributes that are associated with a measurement key.
//    /// </summary>
//    public class AttributeList
//    {
//        private Dictionary<string, Table> m_tables;

//        public int Version { get; private set; } //a change counter
//        public void Set(string table, string attributeName, CtpTypeCode attributeTypeCode, object value)
//        {
//            //ToDO: Update and the sort

//            this[table][attributeName, attributeTypeCode].Value = value;
//            Version++;
//        }

//        private Table this[string tableName]
//        {
//            get
//            {
//                Table rv;
//                if (m_tables.TryGetValue(tableName, out rv))
//                {
//                    rv = new Table();
//                    m_tables[tableName] = rv;
//                }
//                return rv;
//            }
//        }

//        public IEnumerable<Tuple<string, string, CtpTypeCode>> GetSchema()
//        {
//            foreach (var table in m_tables)
//            {
//                foreach (var columns in table.Value.Columns)
//                {
//                    yield return Tuple.Create(table.Key, columns.Key, columns.Value.ColumnTypeCode);
//                }
//            }
//        }

//        public IEnumerable<Tuple<string, string, object>> GetValues()
//        {
//            foreach (var table in m_tables)
//            {
//                foreach (var columns in table.Value.Columns)
//                {
//                    yield return Tuple.Create(table.Key, columns.Key, columns.Value.Value);
//                }
//            }
//        }

//        private class Table
//        {
//            public Dictionary<string, Column> Columns;

//            public Column this[string columnName, CtpTypeCode columnTypeCode]
//            {
//                get
//                {
//                    Column rv;
//                    if (Columns.TryGetValue(columnName, out rv))
//                    {
//                        rv = new Column(columnTypeCode);
//                        Columns[columnName] = rv;
//                    }
//                    return rv;
//                }
//            }
//        }

//        private class Column
//        {
//            public CtpTypeCode ColumnTypeCode;

//            public object Value;

//            public Column(CtpTypeCode columnTypeCode)
//            {
//                ColumnTypeCode = columnTypeCode;
//            }
//        }
//    }
//}