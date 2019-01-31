using System.Collections.Generic;

namespace Sttp.DataPointEncoding
{
    public class MetadataChannelMapDecoder
    {
        private List<SttpDataPointMetadata> ChannelMapping = new List<SttpDataPointMetadata>();

        public int GetNextChannelID => ChannelMapping.Count;

        public SttpDataPointMetadata GetMetadata(int channelID)
        {
            if (channelID >= ChannelMapping.Count)
            {
                return null;
            }
            return ChannelMapping[channelID];
        }

        public void Assign(SttpDataPointMetadata metadata, int channelID)
        {
            while (ChannelMapping.Count <= channelID)
                ChannelMapping.Add(null);
            ChannelMapping[channelID] = metadata;
        }

        public void Clear()
        {
            ChannelMapping.Clear();
        }
    }
}