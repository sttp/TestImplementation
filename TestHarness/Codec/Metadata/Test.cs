using System;
using Sttp.Codec;
using Sttp.WireProtocol;

namespace Prototype.Codec.Metadata
{
    public static class Test
    {
        public static void ImTheServer()
        {
            //This code is here to demonstrate how to use the wireAPI, it's not intended to work.

            var send = new WireEncoder();
            var receive = new WireDecoder();

            CommandObjects nextCommand;
            while ((nextCommand = receive.NextCommand()) != null)
            {
                switch (nextCommand.CommandCode)
                {
                    case CommandCode.Invalid:
                        break;
                    case CommandCode.BeginFragment:
                        break;
                    case CommandCode.NextFragment:
                        break;
                    case CommandCode.GetMetadata:
                        break;
                    case CommandCode.Metadata:
                        break;
                    case CommandCode.Subscription:
                        break;
                    case CommandCode.MapRuntimeIDs:
                        break;
                    case CommandCode.NegotiateSession:
                        break;
                    case CommandCode.RequestFailed:
                        break;
                    case CommandCode.RequestSucceeded:
                        break;
                    case CommandCode.NoOp:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }


        }

        public static void ImTheClient()
        {
            //This code is here to demonstrate how to use the wireAPI, it's not intended to work.

            var send = new WireEncoder();
            var receive = new WireDecoder();

            CommandObjects command = receive.NextCommand();

        }



        //public static void Test1()
        //{
        //    var codec = new MetadataEncoder(null);
        //    codec.BeginCommand();
        //    //codec.AddTable(Guid.Empty, 0, "DataPoint", 0, true);
        //    //codec.AddColumn(0, 0, "ID", ValueType.Guid);
        //    //codec.AddColumn(0, 1, "Name", ValueType.String);
        //    //codec.AddValue(0, 0, 1, Guid.NewGuid().ToRfcBytes());
        //    //codec.AddValue(0, 1, 0, Encoding.UTF8.GetBytes("PMU1"));
        //    //codec.AddTable(Guid.Empty, 1, "Synchrophasors", 0, true);
        //    //codec.AddColumn(1, 0, "Latitude", ValueType.Double);
        //    //codec.AddColumn(1, 1, "Longitude", ValueType.Double);
        //    //codec.AddValue(1, 0, 1, BigEndian.GetBytes(35.123));
        //    //codec.AddValue(1, 1, 1, BigEndian.GetBytes(-97.123));

        //    byte[] dataToSend = codec.EndCommand();

        //    var dec = new MetadataDecoder();
        //    dec.BeginCommand(dataToSend, 0, dataToSend.Length);

        //    VerifyAddTable(dec, Guid.Empty, 0, "DataPoint", 0, true);
        //    VerifyAddColumn(dec, 0, 0, "ID", ValueType.Guid);
        //    VerifyAddColumn(dec, 0, 1, "Name", ValueType.String);
        //    VerifyAddValue(dec, 0, 0, 1, Guid.NewGuid().ToRfcBytes());
        //    VerifyAddValue(dec, 0, 1, 0, Encoding.UTF8.GetBytes("PMU1"));
        //    VerifyAddTable(dec, Guid.Empty, 1, "Synchrophasors", 0, true);
        //    VerifyAddColumn(dec, 1, 0, "Latitude", ValueType.Double);
        //    VerifyAddColumn(dec, 1, 1, "Longitude", ValueType.Double);
        //    VerifyAddValue(dec, 1, 0, 1, BigEndian.GetBytes(35.123));
        //    VerifyAddValue(dec, 1, 1, 1, BigEndian.GetBytes(-97.123));
        //}

        //private static void VerifyAddColumn(MetadataDecoder dec, int tableIndex, int columnIndex, string columnName, ValueType columnType)
        //{
        //    if (dec.NextCommand() != MetadataCommand.AddColumn) throw new Exception();
        //    dec.AddColumn(out int tableIndex2, out int columnIndex2, out string columnName2, out ValueType columnType2);
        //    if (tableIndex != tableIndex2) throw new Exception();
        //    if (columnIndex != columnIndex2) throw new Exception();
        //    if (columnName != columnName2) throw new Exception();
        //    if (columnType != columnType2) throw new Exception();
        //}

        //private static void VerifyAddTable(MetadataDecoder dec, Guid instanceID, long transactionID, string tableName, int tableIndex, bool isMappedToDataPoint)
        //{
        //    if (dec.NextCommand() != MetadataCommand.AddTable) throw new Exception();
        //    dec.AddTable(out Guid instanceID2, out long transactionID2, out string tableName2, out int tableIndex2, out bool isMappedToDataPoint2);
        //}

        //private static void VerifyAddValue(MetadataDecoder dec, int tableIndex, int columnIndex, int rowIndex, byte[] value)
        //{
        //    if (dec.NextCommand() != MetadataCommand.AddValue) throw new Exception();
        //    dec.AddValue(out int tableIndex2, out int columnIndex2, out int rowIndex2, out byte[] value2);
        //}
    }
}
