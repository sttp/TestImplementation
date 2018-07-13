using System;
using System.Collections.Generic;
using System.Numerics;
using CTP.Authentication;
using CTP.SRP;
using GSF;

namespace CTP.Net
{
    public class MasterSecretDatabase
    {
        private object m_syncRoot = new object();
        private Dictionary<uint, MasterSecrets> m_keys = new Dictionary<uint, MasterSecrets>();
        private MasterSecrets m_currentLongKey;
        private MasterSecrets m_currentShortKey;

        private const int LongKeyDuration = 24 * 60;
        private const int ShortKeyDuration = 60;

        public MasterSecretDatabase()
        {
            m_currentLongKey = CreateKey(LongKeyDuration);
            m_currentShortKey = CreateKey(ShortKeyDuration);
        }

        private MasterSecrets CreateKey(int expireMinutes)
        {
            lock (m_syncRoot)
            {
                //Remove expired keys;
                List<uint> expiredKeys = new List<uint>();
                foreach (var key in m_keys.Values)
                {
                    if (key.Expired)
                    {
                        expiredKeys.Add(key.ID);
                    }
                }
                foreach (var key in expiredKeys)
                {
                    m_keys.Remove(key);
                }

                //Create a new key.
                uint id = BigEndian.ToUInt32(Security.CreateSalt(4), 0);
                while (m_keys.ContainsKey(id))
                    id = BigEndian.ToUInt32(Security.CreateSalt(4), 0);

                var s = new MasterSecrets(id, expireMinutes);
                m_keys[s.ID] = s;
                return s;
            }
        }

        public MasterSecrets GetShortKey()
        {
            lock (m_syncRoot)
            {
                if (m_currentShortKey.NearExpiration)
                {
                    m_currentShortKey = CreateKey(ShortKeyDuration);
                }

                return m_currentShortKey;
            }
        }

        public MasterSecrets GetLongKey()
        {
            lock (m_syncRoot)
            {
                if (m_currentLongKey.NearExpiration)
                {
                    m_currentLongKey = CreateKey(ShortKeyDuration);
                }
                return m_currentLongKey;
            }
        }

        public MasterSecrets TryGetKey(uint id)
        {
            lock (m_syncRoot)
            {
                if (m_keys.TryGetValue(id, out MasterSecrets s))
                {
                    if (!s.Expired)
                    {
                        return s;
                    }
                    m_keys.Remove(id);
                }
                return null;
            }
        }

    }
}