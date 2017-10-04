using System;

namespace Sttp.Subscriber
{
    // API layer exists above wire protocol layer whose details are invisible to API user.
    // Goal here is that API is the primary interface any STTP user will ever see and hence
    // needs to remain as simple as possible.
    public class API
    {
        public event EventHandler ReceivedMetaDataTables;
        public event EventHandler ReceivedMetaData;
        public event EventHandler ReceivedDataPoints;
        public event EventHandler MetadataChanged;
        private event EventHandler<EventArgs<object>> ReceivedResponse;

        private dynamic m_tcpSocket;
        private dynamic m_udpSocket;

        public void Connect(string connectionString)
        {
            // Create socket - note reverse connection needs to be allowed,
            // i.e., where subscriber will establish a listening socket where
            // a publisher (thinking here is only one...?) will connect.
            // Typical use case is that subscriber will connect to a publisher's
            // listening socket.

            // m_tcpSocket = new Socket(of proper kind);
        }

        public void Disconnect()
        {
            m_tcpSocket?.Disconnect();
            m_udpSocket?.Disconnect();
        }

        // Request names of metadata tables that are available
        public void RequestMetadataTables()
        {
        }

        // Request desired metadata (or all of it)
        public void RequestMetadata(string filterString = null, int cachedVersion = 0)
        {
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

        public void Unsubscribe()
        {
        }

        public void SecureDataChannel()
        {
            // if not TLS throw exception
            // if no UDP port defined throw exception
        }

        private void SendCommand(object command)
        {

        }
    }
}
