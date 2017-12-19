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

        public abstract void Save(SttpMarkupWriter writer);

        private static ConcurrentDictionary<string, Func<SttpMarkupElement, SttpQueryBase>> m_queries;

        static SttpQueryBase()
        {
            m_queries = new ConcurrentDictionary<string, Func<SttpMarkupElement, SttpQueryBase>>();
        }

        public static SttpQueryBase Create(SttpMarkupElement element)
        {
            if (!m_queries.TryGetValue(element.ElementName, out Func<SttpMarkupElement, SttpQueryBase> query))
                throw new Exception("Query type has not been registered. " + element.ElementName);
            return query(element);
        }

        public static void Register(string commandName, Func<SttpMarkupElement, SttpQueryBase> constructor)
        {
            m_queries[commandName] = constructor;
        }

    }
}