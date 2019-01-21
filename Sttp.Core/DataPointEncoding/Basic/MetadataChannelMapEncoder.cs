using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTP;

namespace Sttp.DataPointEncoding
{
    public class MetadataChannelMapEncoder
    {
        private Dictionary<int, int> RuntimeToChannelMapping = new Dictionary<int, int>();
        private Dictionary<CtpObject, int> RuntimeToChannelMapping2 = new Dictionary<CtpObject, int>();
        private List<SttpDataPointMetadata> ChannelMapping = new List<SttpDataPointMetadata>();

        public int GetChannelID(SttpDataPointMetadata metadata, out bool isNew)
        {
            int channelID;
            if (metadata.RuntimeID.HasValue)
            {
                if (!RuntimeToChannelMapping.TryGetValue(metadata.RuntimeID.Value, out channelID))
                {
                    channelID = ChannelMapping.Count;
                    ChannelMapping.Add(metadata);
                    RuntimeToChannelMapping.Add(metadata.RuntimeID.Value, channelID);
                    isNew = true;
                    return channelID;
                }
            }
            else
            {
                if (!RuntimeToChannelMapping2.TryGetValue(metadata.DataPointID, out channelID))
                {
                    channelID = ChannelMapping.Count;
                    ChannelMapping.Add(metadata);
                    RuntimeToChannelMapping2.Add(metadata.DataPointID.Clone(), channelID);
                    isNew = true;
                    return channelID;
                }
            }
            isNew = false;
            return channelID;
        }

        public void Clear()
        {
            RuntimeToChannelMapping.Clear();
            ChannelMapping.Clear();
        }
    }
}
