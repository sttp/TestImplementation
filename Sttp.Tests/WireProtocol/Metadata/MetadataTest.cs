using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sttp.Data;
using Sttp.WireProtocol;

namespace Sttp.Tests
{
    [TestClass]
    public class MetadataTests
    {
        //private WireDecoder m_decoder;
        private MetadataDatabaseDestination m_testDestinationSet;
        private WireDecoder m_wireDecoder;

        [TestMethod]
        public void TestUpdateDataSet()
        {
            DataSet testSet = new DataSet();

            var testTable = new DataTable("table1");
            testTable.Columns.Add("rowid", typeof(int));
            testTable.Columns.Add("key", typeof(string));
            testTable.Rows.Add(0,"rowkey0");
            testSet.Tables.Add(testTable);

            MetadataDatabaseSource testSourceSet = new MetadataDatabaseSource();
            m_wireDecoder = new WireDecoder();

            m_testDestinationSet = new MetadataDatabaseDestination();
            WireEncoder encoder = new WireEncoder();
            encoder.NewPacket += Encoder_NewPacket;

            UpdateDataSet(testSet, testSourceSet);

            for (short x = 0; x < testSet.Tables.Count; x++)
            {
                testSourceSet.RequestTableData(encoder.GetMetadataResponse(), x);
            }
            encoder.Flush();

            VerifyDataset(testSet, m_testDestinationSet);
        }


        private void Encoder_NewPacket(byte[] data, int position, int length)
        {
            m_wireDecoder.WriteData(data, position, length);

            CommandDecoder decoder;
            while ((decoder = m_wireDecoder.NextCommand()) != null)
            {
                switch (decoder.CommandCode)
                {
                    case CommandCode.NegotiateSession:
                    case CommandCode.Subscribe:
                    case CommandCode.SecureDataChannel:
                    case CommandCode.RegisterDataPoint:
                    case CommandCode.SendDataPoints:
                    case CommandCode.NoOp:
                    case CommandCode.Invalid:
                    case CommandCode.BulkTransport:
                    case CommandCode.BeginFragment:
                    case CommandCode.NextFragment:
                    case CommandCode.CompressedPacket:
                    case CommandCode.GetMetadataSchema:
                    case CommandCode.GetMetadataSchemaResponse:
                    case CommandCode.GetMetadata:
                        throw new NotSupportedException();
                    case CommandCode.GetMetadataResponse:
                        m_testDestinationSet.Process(decoder.GetMetadataResponse);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }


        public void UpdateDataSet(DataSet dataSet, MetadataDatabaseSource set)
        {
            foreach (DataTable table in dataSet.Tables)
            {
                set.AddTable(table.TableName, TableFlags.None);

                foreach (DataColumn column in table.Columns)
                {
                    set[table.TableName].AddColumn(column.ColumnName, ValueTypeCodec.FromType(column.DataType));
                }

                for (var rowIndex = 0; rowIndex < table.Rows.Count; rowIndex++)
                {
                    DataRow row = table.Rows[rowIndex];
                    for (int colIx = 0; colIx < table.Columns.Count; colIx++)
                    {
                        set[table.TableName].AddOrUpdateValue(table.Columns[colIx].ColumnName, rowIndex, row[colIx]);
                    }
                }
            }
        }

        public void VerifyDataset(DataSet dataSet, MetadataDatabaseDestination set)
        {
            foreach (DataTable table in dataSet.Tables)
            {
                for (short columnIndex = 0; columnIndex < table.Columns.Count; columnIndex++)
                {
                    DataColumn column = table.Columns[columnIndex];
                    var col2 = set[table.TableName].Columns[columnIndex];

                    Assert.AreEqual(column.ColumnName, col2.Name, "Column Name");
                    Assert.AreEqual(col2.Type, ValueTypeCodec.FromType(column.DataType), "Column Data Type");
                }

                for (var rowIndex = 0; rowIndex < table.Rows.Count; rowIndex++)
                {
                    DataRow row = table.Rows[rowIndex];
                    for (short col = 0; col < table.Columns.Count; col++)
                    {
                        object value = row[col];
                        object value2 = set[table.TableName].GetValue(rowIndex, col);

                        if (value == DBNull.Value)
                            value = null;

                        Assert.AreEqual(value, value2, "Encoded Value");
                    }
                }
            }
        }
    }
}
