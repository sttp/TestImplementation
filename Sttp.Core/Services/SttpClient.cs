using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Sttp.Codec;
using Sttp.Codec.DataPoint;
using Sttp.Codec.Metadata;
using Sttp.Data;

namespace Sttp.Services
{
    public class SttpClient
    {
        //private class SttpDataPointResponse
        //{
        //    private SttpClient m_client;
        //    private bool m_isEos;
        //    private BasicDecoder m_decoder;
        //    private byte m_streamID;

        //    public SttpDataPointResponse(SttpClient client)
        //    {
        //        m_client = client;
        //        var cmd = m_client.m_decoder.NextCommand();
        //        if (cmd.CommandName != "DataPointResponse")
        //        {
        //            m_decoder = new BasicDecoder();
        //            m_streamID = cmd.DataPointResponse.RawCommandCode;
        //        }

        //        cmd = m_client.m_decoder.NextCommand();
        //        if (cmd.CommandName == "DataPointResponseCompleted")
        //        {
        //            m_isEos = true;
        //        }
        //        else if (cmd.CommandName == "SubscriptionStream")
        //        {
        //            if (cmd.Raw.RawCommandCode != m_streamID)
        //                throw new Exception("Wrong encoding method");
        //            m_decoder.Load(cmd.Raw.Payload);
        //        }
        //        else
        //        {
        //            throw new Exception("Unknown Command");
        //        }
        //    }

        //    public bool Read(SttpDataPoint dataPoint)
        //    {
        //        tryAgain:
        //        if (m_isEos)
        //            return false;
        //        if (m_decoder.Read(dataPoint))
        //            return true;

        //        var cmd = m_client.m_decoder.NextCommand();
        //        if (cmd.CommandName == "DataPointResponseCompleted")
        //        {
        //            m_isEos = true;
        //            return false;
        //        }
        //        else if (cmd.CommandName == "SubscriptionStream")
        //        {
        //            if (cmd.Raw.RawCommandCode != m_streamID)
        //                throw new Exception("Wrong encoding method");
        //            m_decoder.Load(cmd.Raw.Payload);
        //            goto tryAgain;
        //        }
        //        else
        //        {
        //            throw new Exception("Unknown Command");
        //        }
        //    }
        //}
        private Stream m_stream;
        private WireEncoder m_encoder;
        private WireDecoder m_decoder;
        private byte[] m_buffer = new byte[4096];
        //private SttpDataPointResponse m_dataPointResponse;

        public SttpClient(Stream networkStream)
        {
            m_stream = networkStream;
            m_encoder = new WireEncoder();
            m_decoder = new WireDecoder();
            m_encoder.NewPacket += M_encoder_NewPacket;
        }

        private void M_encoder_NewPacket(byte[] data, int offset, int length)
        {
            m_stream.Write(data, offset, length);
        }

        public List<string> GetMetaDataTableList()
        {
            m_encoder.GetMetadataSchema();
            var cmd = GetNextCommand();
            return cmd.MetadataSchema.Tables.Select(x => x.TableName).ToList();
        }

        public List<string> GetMetaDataFieldList(string tableName)
        {
            m_encoder.GetMetadataSchema();
            var cmd = GetNextCommand();
            return cmd.MetadataSchema.Tables.First(x => x.TableName == tableName).Columns.Select(x => x.Name).ToList();
        }

        //public DataTable Exec(string procedureName, SttpMarkup options)
        //{
        //    m_encoder.GetMetadataProcedure(procedureName, options);
        //    return ParseDT();
        //}

        private DataTable ParseDT()
        {
            MetadataQueryTable table = null;
            TryAgain:

            var cmd = GetNextCommand();
            if (cmd.CommandName != "Metadata")
                throw new Exception("Wrong command");

            MetadataSubCommandObjects subCmd;
            while ((subCmd = cmd.Metadata.NextCommand()) != null)
            {
                switch (subCmd.SubCommand)
                {
                    case MetadataSubCommand.DefineResponse:
                        table = new MetadataQueryTable(subCmd.DefineResponse);
                        break;
                    case MetadataSubCommand.DefineRow:
                        table.ProcessCommand(subCmd.DefineRow);
                        break;
                    case MetadataSubCommand.UndefineRow:
                        table.ProcessCommand(subCmd.UndefineRow);
                        break;
                    case MetadataSubCommand.Finished:
                        return table.ToTable();
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            goto TryAgain;
        }

        public DataTable GetMetadata(string query)
        {
            string[] parts = Regex.Split(query, @"^SELECT\s", RegexOptions.IgnoreCase);
            if (parts.Length != 2)
                throw new Exception("Not properly formatted select statement.");
            if (parts[0].Length > 0)
                throw new Exception("Not properly formatted select statement.");

            parts = Regex.Split(parts[1], " FROM ", RegexOptions.IgnoreCase);

            if (parts.Length != 2)
                throw new Exception("Not properly formatted select statement.");

            string[] columns = parts[0].Split(',').Select(x => x.Trim()).ToArray();
            string[] tables = parts[1].Split(',').Select(x => x.Trim()).ToArray();

            if (tables.Length != 1)
                throw new Exception("Not properly formatted select statement.");

            if (columns.Length == 0)
                throw new Exception("Not properly formatted select statement.");

            m_encoder.GetMetadataSimple(null, null, tables[0], columns);
            return ParseDT();
        }

        //public void DataPointRequest(string instanceName, SttpTime startTime, SttpTime stopTime, SttpValue[] dataPointIDs, double? samplesPerSecond)
        //{
        //    m_encoder.DataPointRequest(instanceName, startTime, stopTime, dataPointIDs, samplesPerSecond);
        //    m_dataPointResponse = new SttpDataPointResponse(this);
        //}

        //public bool Read(SttpDataPoint value)
        //{
        //    if (m_dataPointResponse == null)
        //        return false;
        //    if (m_dataPointResponse.Read(value))
        //        return true;
        //    m_dataPointResponse = null;
        //    return false;
        //}

        private CommandObjects GetNextCommand()
        {
            TryAgain:
            CommandObjects obj = m_decoder.NextCommand();
            if (obj != null)
                return obj;
            int length = 0;

            if ((length = m_stream.Read(m_buffer, 0, m_buffer.Length)) > 0)
            {
                m_decoder.FillBuffer(m_buffer, 0, length);
                goto TryAgain;
            }

            throw new EndOfStreamException();
        }

    }
}
