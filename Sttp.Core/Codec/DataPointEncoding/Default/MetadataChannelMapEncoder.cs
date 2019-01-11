using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sttp.Codec.DataPoint
{
    public class MetadataChannelMapEncoder
    {
        private Dictionary<int, int> RuntimeToChannelMapping = new Dictionary<int, int>();
        private List<SttpDataPointMetadata> ChannelMapping = new List<SttpDataPointMetadata>();

        public int GetChannelID(SttpDataPointMetadata metadata, out bool isNew)
        {
            if (!RuntimeToChannelMapping.TryGetValue(metadata.RuntimeID, out var channelID))
            {
                channelID = ChannelMapping.Count;
                ChannelMapping.Add(metadata);
                RuntimeToChannelMapping.Add(metadata.RuntimeID, channelID);
                isNew = true;
                return channelID;
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
