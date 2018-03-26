using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sttp.Codec;
using Sttp.Core;

namespace Sttp.Services
{
    public class SttpBulkTransportServer : ISttpCommandHandler
    {
        private class BulkTransportRecord
        {
            public FileStream Stream;
            public SttpBulkTransport Metadata;

            public BulkTransportRecord(FileStream stream, SttpBulkTransport metadata)
            {
                Stream = stream;
                Metadata = metadata;
            }
        }

        private Dictionary<Guid, BulkTransportRecord> m_records;

        public SttpBulkTransportServer()
        {
            m_records = new Dictionary<Guid, BulkTransportRecord>();
        }

        public SttpBulkTransport RegisterBulkData(byte[] data)
        {
            var fs = new FileStream(Path.GetTempFileName(), FileMode.Open, FileAccess.ReadWrite, FileShare.None, 4, FileOptions.DeleteOnClose);
            fs.Write(data, 0, data.Length);
            var bulkData = new SttpBulkTransport(SttpValueTypeCode.SttpBuffer, Guid.NewGuid(), data.Length);
            m_records[bulkData.BulkTransportID] = new BulkTransportRecord(fs, bulkData);
            return bulkData;
        }

        public void ProcessCommand(CommandBulkTransportRequest command, WireEncoder encoder)
        {
            if (m_records.TryGetValue(command.ID, out BulkTransportRecord record))
            {
                if (command.Offset < 0 || command.Length < 0 || command.Offset + command.Length > record.Stream.Length)
                {
                    encoder.RequestFailed(command.CommandName, false, "Arguments are invalid or reads past the end of the stream.", command.ID.ToString());
                }

                byte[] rv = new byte[command.Length];
                record.Stream.Position = command.Offset;
                record.Stream.ReadAll(rv, 0, rv.Length);
                encoder.BulkTransportReply(command.ID, command.Offset, rv);
            }
            else
            {
                encoder.RequestFailed(command.CommandName, false, "Specified ID does not exist.", command.ID.ToString());
            }
        }

        public List<string> CommandsHandled()
        {
            var lst = new List<string>();
            lst.Add("BulkTransportRequest");
            return lst;
        }

        public void HandleCommand(CommandObjects command, WireEncoder encoder)
        {
            if (command.CommandName != "BulkTransportRequest")
                throw new Exception("This command is not supported");
            ProcessCommand(command.BulkTransportRequest, encoder);
        }
    }
}
