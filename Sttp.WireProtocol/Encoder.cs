using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Sttp.WireProtocol
{
    /// <summary>
    /// Responsible for encoding each command into bytes
    /// </summary>
    public class Encoder
    {
        /// <summary>
        /// Once this size has been reached, the protocol will automatically call 
        /// </summary>
        private int m_autoFlushPacketSize; 

        /// <summary>
        /// The bytes that need to be reliably sent. Note, this data is not valid until <see cref="Flush"/> has been called.
        /// </summary>
        public byte[] ReliableSendBuffer { get; private set; }
        /// <summary>
        /// The length of <see cref="ReliableSendBuffer"/>
        /// </summary>
        public int ReliableSendBufferLength { get; private set; }
        /// <summary>
        /// The bytes that can be sent unreliably. Note, this data is not valid until <see cref="Flush"/> has been called.
        /// </summary>
        public byte[] UnreliableSendBuffer { get; private set; }
        /// <summary>
        /// The length of <see cref="UnreliableSendBuffer"/>
        /// </summary>
        public int UnreliableSendBufferLength { get; private set; }

        /// <summary>
        /// Occurs when a new packet of data must be sent on the wire. This is called immediately
        /// after <see cref="Flush"/> or when the <see cref="m_autoFlushPacketSize"/> has been exceeded.
        /// </summary>
        public event EventHandler<EventArgs<bool, byte[], int>> NewPacket;

        public Encoder(int autoflushPacketSize)
        {
            m_autoFlushPacketSize = autoflushPacketSize;
        }

        public void NegotiateSessionStep1(ProtocolVersions protocolVersionNumber)
        {
            //Client connects to server and specifies it's protocol versions supported
        }

        public void NegotiateSessionStep1Reply(OperationalModes modes)
        {
            //The server replies with the supported operational modes
        }

        public void NegotiateSessionStep2(OperationalModes modes)
        {
            //The client picks the operational modes
        }

        public void CommandSuccess(CommandCode command, string response)
        {

        }

        public void CommandFailed(CommandCode command, string response)
        {

        }


        public void RequestMetadataTables()
        {
            
        }

        public void RequestMetadataTablesReply(MetadataTable[] tableDefinitions)
        {

        }

        public void RequestMetadata(string tableName, Guid cachedBaseVersionNumber, int versionNumber, string filterString)
        {
            //Requests a set of metadata to be populated.
            //if tablename is null, returns all tables
            //if cachedBaseVersionNumber is not the same as the version number, return all metadata
            //if versionNumber is different from the cached one, return all data in the version delta.

            //Notes from other command.
            // Note: Thinking on metadata is that it exists as a set of simple tabular data
            // where a base table "DataPoints" always exists with a minimal set of fields
            // such as "ID guid | TagName string | AltTagName string | Enabled bool"

            // Any other tables could be added that can extend the "DataPoints" table, i.e.,
            // if a "Synchrophasor" table is added - the first field would need to be the
            // "ID" reference back to "DataPoints", then filters on this table would reduce
            // available "DataPoints", for example, "Synchrophasor" table fields could be:
            // "ID guid | SignalReference string | SignalType string(4) | Phase string(1)"

            // It is expected that standalone metadata sets can also be support 

            // Null filter means all metadata is requested

            // "Tables" filter should support the reduction to desired tables:
            //      + "; tables=DataPoints,Synchrophasor
            // DataPoints should always be available, whether requested or not

            // Metadata filtering syntax should support SQL like expressions or RegEx (or other)
            // which will reduce metadata that comes through for "DataPoints" and associated tables
            //      + "; sqlFilter=FILTER DataPoints WHERE TagName LIKE '%-FQ'"
            //  or  + "; sqlFilter=FILTER Synchrophasor WHERE SignalType = 'FREQ'"
            //  or  + "; regexFilter=DataPoints.TagName /^.*-FQ$/"
            //  or  + "; regexFilter=Synchrophasor.SignalType /FREQ/"

            // Metadata returned will always include a version and/or timestamp, if available
            // this should always be provided to the metadata filter such that only changes
            // to the metadata can be reported:
            //      + "; cachedVersion=" + cachedVersion

            // Need to consider filtering options an impacts on server
        }

        public void RequestMetadataReply(Guid cachedBaseVersionNumber, int versionNumber, MetadataRow[] rows)
        {
            //Metadata will be chunked one row at a time for the reply. 
        }

        public void Subscribe(string subscriptionString, bool augment)
        {
            // Subscription needs to support direct point identification list as well as
            // a filtering syntax that works with metadata, e.g., SQL like / RegEx, etc.
            //      + "; ids=7F2D26ED-DFF8-4E5B-9871-00AB97F8AE61,52039879-8F8D-4C59-B7BA-03D6CCAC2D5B"
            //  or  + "; tagNames=TVA_SHELBY!IS:ST8,TVA_SHELBY-DELL:ABBIH,TVA_SHELBY!IS:ST25"
            //  or  + "; sqlFilter=FILTER DataPoints WHERE TagName LIKE '%-FQ'"
            //  or  + "; sqlFilter=FILTER Synchrophasor WHERE SignalType = 'FREQ'"
            //  or  + "; regexFilter=DataPoints.TagName /^.*-FQ$/"
            //  or  + "; regexFilter=Synchrophasor.SignalType /FREQ/"

            // Subscription info needs to optionally specify desired UDP port:
            //      + "; udpPort=9195"

            // Subscription info needs a flag to specify if this is a new subscription,
            // or an augmentation of an existing one:
            //      + "; augment=" + augment

            // Overloads should be created to support simplification of subscription process.

            // Need to consider filtering options an impacts on server
        }

        public void SendMetadata()
        {
            
        }

        public void Unsubscribe()
        {

        }

        public void SecureDataChannel(byte[] key, byte[] iv)
        {

        }

        public void NoOp()
        {

        }

        /// <summary>
        /// Begins sending a series of <see cref="DataPointKey"/>, one at a time. After
        /// sending each metadata segment, <see cref="ReliableSendBufferLength"/> or <see cref="UnreliableSendBufferLength"/>
        /// will indicate approximately how much data is pending to be sent. This will allow control of the packet size.
        /// </summary>
        public void RuntimeIDMapping(DataPointKeyWire requiredMetadata)
        {
            //Sends this to the client and builds compression state data for it.
        }

        /// <summary>
        /// Begins sending a series of data points, one at a time. After
        /// sending each point, <see cref="ReliableSendBufferLength"/> or <see cref="UnreliableSendBufferLength"/>
        /// will indicate approximately how much data is pending to be sent. This will allow control of the packet size.
        /// </summary>
        /// <param name="sendReliably"></param>
        /// <param name="point"></param>
        public void SendDataPoint(bool sendReliably, DataPointWire point)
        {

        }

        /// <summary>
        /// Flushes all pending data to the provided socket.
        /// </summary>
        public void Flush()
        {

        }
    }
}
