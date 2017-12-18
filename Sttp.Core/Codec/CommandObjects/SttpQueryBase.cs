using System;
using System.Collections.Concurrent;

namespace Sttp.Codec
{
    public abstract class SttpQueryBase
    {
        public readonly string CommandName;

        protected SttpQueryBase(string name)
        {
            CommandName = name;
        }

        public abstract SttpQueryBase Load(SttpMarkupElement reader);
        public abstract void Save(SttpMarkupWriter writer);

        private static ConcurrentDictionary<string, SttpQueryBase> m_queries;

        static SttpQueryBase()
        {
            m_queries = new ConcurrentDictionary<string, SttpQueryBase>();
        }

        public static SttpQueryBase Create(SttpMarkupElement element)
        {
            if (!m_queries.TryGetValue(element.ElementName, out SttpQueryBase query))
                throw new Exception("Query type has not been registered. " + element.ElementName);
            return query.Load(element);
        }

    }
}