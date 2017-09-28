using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Sttp.WireProtocol.Data;

namespace Sttp.WireProtocol
{
    /// <summary>
    /// Responsible for encoding each command into bytes
    /// </summary>
    public class SubscriptionEncoder
    {
        public void SubscribeAdvance(string filter, bool augment)
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

        public void SubscribeFilter(string filter, bool augment)
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

        public void SubscribeList(int[] pointsList, bool augment)
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

        public void SubscribeAll()
        {
            
        }

        public void UnsubscribeAll()
        {

        }

        public void UnsubscribeList(int[] pointList)
        {
            
        }

        public void UnsubscribeFilter(string filter)
        {
            
        }

        public void UnsubscribeAdvance(string advanceFilterMethod)
        {
            
        }
    }
}
