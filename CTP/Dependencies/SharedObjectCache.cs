using System;
using System.Collections.Concurrent;

namespace GSF.Reflection
{
    internal class SharedObjectCache<T1, T2>
        where T1 : class
        where T2 : class
    {
        private object m_syncRoot = new object();
        private ConcurrentDictionary<T1, Tuple<string, T2>> m_cachedObjects = new ConcurrentDictionary<T1, Tuple<string, T2>>();
        private Func<T1, T2> m_constructor;
        public SharedObjectCache(Func<T1, T2> constructor)
        {
            m_constructor = constructor;
        }

        public T2 Get(T1 item)
        {
            if (!m_cachedObjects.TryGetValue(item, out var rv))
            {
                lock (m_syncRoot)
                {
                    if (!m_cachedObjects.TryGetValue(item, out rv))
                    {
                        try
                        {
                            rv = Tuple.Create((string)null, m_constructor(item));
                            m_cachedObjects.TryAdd(item, rv);

                        }
                        catch (Exception e)
                        {
                            m_cachedObjects.TryAdd(item, Tuple.Create(e.ToString(), (T2)null));
                            throw;
                        }
                    }
                }
            }
            if (rv.Item1 != null)
                throw new InvalidOperationException("Previous call resulted in a compile error", new Exception(rv.Item1));
            return rv.Item2;
        }
    }
}