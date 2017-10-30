using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sttp.Data;
using Sttp.WireProtocol;
using Sttp.WireProtocol.Data;
using Sttp.WireProtocol.MetadataPacket;

namespace Sttp.Tests
{
    [TestClass]
    public class MetadataEncoderTests
    {
        private PacketReader m_sr;
        private MetadataDecoder m_decoder;
        private MetadataEncoder m_encoder;


        public static void CompareCollection<T>(IList<T> a, IList<T> b)
        {
            if (a == null && b == null)
            {
                Assert.IsNull(a);
                Assert.IsNull(b);
                return;
            }

            Assert.AreEqual(a.Count, b.Count);


            for (int i = 0; i < a.Count; i++)
            {
                Assert.AreEqual(a[i], b[i]);
            }
        }

        [TestInitialize]
        public void Init()
        {
            m_sr = new PacketReader(new SessionDetails());
            m_decoder = new MetadataDecoder(new SessionDetails());

            void ReceivePacket(byte[] data, int position, int length)
            {
                m_sr.Fill(CommandCode.Metadata, data, position, length);
                m_decoder.Fill(m_sr);
                Assert.AreEqual(CommandCode.Metadata, m_sr.Read<CommandCode>());
                Assert.AreEqual(m_sr.Length, m_sr.ReadUInt16());
            }

            m_encoder = new MetadataEncoder(ReceivePacket, new SessionDetails());
        }

        [DataTestMethod]
        [DataRow(1, "table", TableFlags.None)]
        public void TestAddTable(int tableIndex, string tableName, TableFlags tableFlags)
        {
            m_encoder.BeginCommand();
            m_encoder.AddTable(tableIndex, tableName, tableFlags);
            m_encoder.EndCommand();

            var decoded = m_decoder.NextCommand() as MetadataAddTableParams;
            Assert.IsNotNull(decoded);
            Assert.AreEqual(tableIndex, decoded.TableIndex);
            Assert.AreEqual(tableName, decoded.TableName);
            Assert.AreEqual(tableFlags, decoded.TableFlags);
        }

        [DataTestMethod]
        [DataRow(1, 0, "col1", Sttp.WireProtocol.ValueType.Int32)]
        public void TestAddColumn(int tableIndex, int columnIndex, string columnName, Sttp.WireProtocol.ValueType columnType)
        {
            m_encoder.BeginCommand();
            m_encoder.AddColumn(tableIndex, columnIndex, columnName, columnType);
            m_encoder.EndCommand();

            var decoded = m_decoder.NextCommand() as MetadataAddColumnParams;
            Assert.IsNotNull(decoded);
            Assert.AreEqual(tableIndex, decoded.TableIndex);
            Assert.AreEqual(columnIndex, decoded.ColumnIndex);
            Assert.AreEqual(columnName, decoded.ColumnName);
            Assert.AreEqual(columnType, decoded.ColumnType);
        }

        [DataTestMethod]
        [DataRow(1, 2)]
        public void TestAddRow(int tableIndex, int rowIndex)
        {
            m_encoder.BeginCommand();
            m_encoder.AddRow(tableIndex, rowIndex);
            m_encoder.EndCommand();

            var decoded = m_decoder.NextCommand() as MetadataAddRowParams;
            Assert.IsNotNull(decoded);
            Assert.AreEqual(tableIndex, decoded.TableIndex);
            Assert.AreEqual(rowIndex, decoded.RowIndex);
        }

        [DataTestMethod]
        [DataRow(1, 10, 10, new byte[] { 1, 2, 3 })]
        public void TestAddValue(int tableIndex, int columnIndex, int rowIndex, byte[] value)
        {
            m_encoder.BeginCommand();
            m_encoder.AddValue(tableIndex, columnIndex, rowIndex, value);
            m_encoder.EndCommand();

            var decoded = m_decoder.NextCommand() as MetadataAddValueParams;
            Assert.IsNotNull(decoded);
            Assert.AreEqual(tableIndex, decoded.TableIndex);
            Assert.AreEqual(columnIndex, decoded.ColumnIndex);
            Assert.AreEqual(rowIndex, decoded.RowIndex);
            Assert.AreEqual(value.Length, decoded.Value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                Assert.AreEqual(value[i], decoded.Value[i]);
            }

        }

        [DataTestMethod]
        [DataRow(1, 2)]
        public void TestDeleteRow(int tableIndex, int rowIndex)
        {
            m_encoder.BeginCommand();
            m_encoder.DeleteRow(tableIndex, rowIndex);
            m_encoder.EndCommand();

            var decoded = m_decoder.NextCommand() as MetadataDeleteRowParams;
            Assert.IsNotNull(decoded);
            Assert.AreEqual(tableIndex, decoded.TableIndex);
            Assert.AreEqual(rowIndex, decoded.RowIndex);
        }

        [DataTestMethod]
        [DataRow("8587912A-0B31-4AAD-ADC3-D66B6FAE0A31", 2)]
        public void TestDatabaseVersion(string majorVersionStr, long minorVersion)
        {
            var majorVersion = new Guid(majorVersionStr);
            m_encoder.BeginCommand();
            m_encoder.DatabaseVersion(majorVersion, minorVersion);
            m_encoder.EndCommand();

            var decoded = m_decoder.NextCommand() as MetadataDatabaseVersionParams;
            Assert.IsNotNull(decoded);
            Assert.AreEqual(majorVersion, decoded.MajorVersion);
            Assert.AreEqual(minorVersion, decoded.MinorVersion);
        }

        [DataTestMethod]
        [DataRow(1, new[] { 1, 2, 3 })]
        public void TestGetTable(int tableIndex, int[] columnList)
        {
            var filterExpression = new List<Tuple<int, string>> { new Tuple<int, string>(1, "one"), new Tuple<int, string>(2, "two") };

            m_encoder.BeginCommand();
            m_encoder.GetTable(tableIndex, columnList, filterExpression);
            m_encoder.EndCommand();

            var decoded = m_decoder.NextCommand() as MetadataGetTableParams;
            Assert.IsNotNull(decoded);
            Assert.AreEqual(tableIndex, decoded.TableIndex);
            CompareCollection(columnList, decoded.ColumnList);
            CompareCollection(filterExpression, decoded.FilterExpression);
        }

        [TestMethod]
        public void TestGetQuery()
        {
            var columnList = new List<Tuple<int, int>> { new Tuple<int, int>(1, 1), new Tuple<int, int>(2, 2) };
            var joinFields = new List<Tuple<int, int, int>> { new Tuple<int, int, int>(1, 1, 1), new Tuple<int, int, int>(2, 2, 2) };
            var filterExpression = new List<Tuple<int, int, string>> { new Tuple<int, int, string>(1, 1, "one"), new Tuple<int, int, string>(2, 2, "one") };

            m_encoder.BeginCommand();
            m_encoder.GetQuery(columnList, joinFields, filterExpression);
            m_encoder.EndCommand();

            var decoded = m_decoder.NextCommand() as MetadataGetQueryParams;
            Assert.IsNotNull(decoded);
            CompareCollection(columnList, decoded.ColumnList);
            CompareCollection(joinFields, decoded.JoinFields);
            CompareCollection(filterExpression, decoded.FilterExpression);
        }

        [DataTestMethod]
        [DataRow("8587912A-0B31-4AAD-ADC3-D66B6FAE0A31", 2)]
        public void TestSyncDatabase(string majorVersionStr, long minorVersion)
        {
            var columnList = new List<Tuple<int, int>> { new Tuple<int, int>(1, 1), new Tuple<int, int>(2, 2) };
            var majorVersion = new Guid(majorVersionStr);

            m_encoder.BeginCommand();
            m_encoder.SyncDatabase(majorVersion, minorVersion, columnList);
            m_encoder.EndCommand();

            var decoded = m_decoder.NextCommand() as MetadataSyncDatabaseParams;
            Assert.IsNotNull(decoded);
            Assert.AreEqual(majorVersion, decoded.MajorVersion);
            Assert.AreEqual(minorVersion, decoded.MinorVersion);
            CompareCollection(columnList, decoded.ColumnList);
        }

        [DataTestMethod]
        [DataRow("8587912A-0B31-4AAD-ADC3-D66B6FAE0A31", 2)]
        public void TestSyncTableOrQuery(string majorVersionStr, long minorVersion)
        {
            var columnList = new List<Tuple<int, int>> { new Tuple<int, int>(1, 1), new Tuple<int, int>(2, 2) };
            var criticalColumnList = new List<Tuple<int, int>> { new Tuple<int, int>(1, 1), new Tuple<int, int>(2, 2) };
            var majorVersion = new Guid(majorVersionStr);

            m_encoder.BeginCommand();
            m_encoder.SyncTableOrQuery(majorVersion, minorVersion, columnList, criticalColumnList);
            m_encoder.EndCommand();

            var decoded = m_decoder.NextCommand() as MetadataSyncTableOrQueryParams;
            Assert.IsNotNull(decoded);
            Assert.AreEqual(majorVersion, decoded.MajorVersion);
            Assert.AreEqual(minorVersion, decoded.MinorVersion);
            CompareCollection(columnList, decoded.ColumnList);
            CompareCollection(criticalColumnList, decoded.CriticalColumnList);
        }

        [TestMethod]
        public void TestGetDatabaseSchema()
        {
            m_encoder.BeginCommand();
            m_encoder.GetDatabaseSchema();
            m_encoder.EndCommand();

            var decoded = m_decoder.NextCommand() as MetadataGetDatabaseSchemaParams;
            Assert.IsNotNull(decoded);
        }

        [TestMethod]
        public void TestGetDatabaseVersion()
        {
            m_encoder.BeginCommand();
            m_encoder.GetDatabaseVersion();
            m_encoder.EndCommand();

            var decoded = m_decoder.NextCommand() as MetadataGetDatabaseVersionParams;
            Assert.IsNotNull(decoded);
        }



    }
}
