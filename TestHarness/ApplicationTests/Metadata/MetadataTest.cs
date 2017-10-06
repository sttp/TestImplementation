using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Sttp.Data;
using Sttp.WireProtocol;
using Sttp.WireProtocol.Data;
using Sttp.WireProtocol.MetadataPacket;

namespace TestHarness.ApplicationTests.Metadata
{
    public class MetadataTest
    {
        private WireDecoder m_decoder;
        private MetadataSetDestination m_testDestinationSet;

        public void Test()
        {
            DataSet testSet = new DataSet();
            MetadataSetSource testSourceSet = new MetadataSetSource();
            m_testDestinationSet = new MetadataSetDestination();
            WireEncoder encoder = new WireEncoder(1500);
            m_decoder = new WireDecoder();
            encoder.NewPacket += Encoder_NewPacket;

            UpdateDataSet(testSet, testSourceSet);

            for (int x = 0; x < testSet.Tables.Count; x++)
            {
                testSourceSet.RequestTableData(encoder.BeginMetadataPacket(), x);
            }
            encoder.Flush();

            VerifyDataset(testSet, m_testDestinationSet);
        }

        private void Encoder_NewPacket(byte[] data, int position, int length)
        {
            m_decoder.WriteData(data, position, length);
            IPacketDecoder decoder;
            while ((decoder = m_decoder.NextPacket()) != null)
            {
                switch (decoder.CommandCode)
                {
                    case CommandCode.NegotiateSession:
                    case CommandCode.Subscribe:
                    case CommandCode.SecureDataChannel:
                    case CommandCode.RuntimeIDMapping:
                    case CommandCode.DataPointPacket:
                    case CommandCode.Fragment:
                    case CommandCode.NoOp:
                    case CommandCode.Invalid:
                        throw new NotSupportedException();
                    case CommandCode.Metadata:
                        m_testDestinationSet.Process(decoder as IMetadataDecoder);
                        break;
                }
            }
        }


        public void UpdateDataSet(DataSet dataSet, MetadataSetSource set)
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
                    for (int x = 0; x < table.Columns.Count; x++)
                    {
                        set[table.TableName].AddOrUpdateValue(table.Columns[x].ColumnName, rowIndex, row[rowIndex]);
                    }
                }
            }
        }

        public void VerifyDataset(DataSet dataSet, MetadataSetDestination set)
        {
            foreach (DataTable table in dataSet.Tables)
            {
                for (var columnIndex = 0; columnIndex < table.Columns.Count; columnIndex++)
                {
                    DataColumn column = table.Columns[columnIndex];
                    var col2 = set[table.TableName].Columns[columnIndex];

                    if (column.ColumnName != col2.Name)
                        throw new Exception();
                    if (col2.Type != ValueTypeCodec.FromType(column.DataType))
                        throw new Exception();
                }

                for (var rowIndex = 0; rowIndex < table.Rows.Count; rowIndex++)
                {
                    DataRow row = table.Rows[rowIndex];
                    for (int x = 0; x < table.Columns.Count; x++)
                    {
                        object value = row[rowIndex];
                        object value2 = set[table.TableName].GetValue(rowIndex, x);

                        if (value == DBNull.Value)
                            value = null;
                        if (value == null && value2 == null)
                        {
                            //Are Equal
                        }
                        else if (value == null || value2 == null)
                        {
                            throw new Exception();
                        }
                        else
                        {
                            if (!value.Equals(value2))
                            {
                                throw new Exception();
                            }
                        }
                    }
                }
            }

        }

    }
}
