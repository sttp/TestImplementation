using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSF;

namespace CTP
{
    /// <summary>
    /// Effectively maps an Int32 runtime ID into another Int32 Runtime ID.
    /// This is much faster than Dictionary_Int_Int.
    /// </summary>
    public class RuntimeMapping
    {
        public int Count { get; private set; }
        private int[] m_records;
        private int m_mask;

        public RuntimeMapping()
        {
            SetCapacity(32);
        }
        private void SetCapacity(int size)
        {
            var oldRecords = m_records;

            m_records = new int[size * 2];
            m_mask = size - 1;
            Count = 0;
            if (oldRecords != null)
            {
                for (int x = 0; x < oldRecords.Length; x += 2)
                {
                    if (oldRecords[x] > 0)
                        Add(oldRecords[x], oldRecords[x + 1]);
                }
            }
        }

        private void Grow()
        {
            SetCapacity(m_records.Length * 2);
        }

        public void Add(int key, int value)
        {
            if (Count * 3 > m_records.Length)
                Grow();
            if (key == 0)
                throw new ArgumentException();

            int position = (key & m_mask) * 2;
            while (m_records[position] != 0)
            {
                if (m_records[position] == key)
                {
                    throw new Exception("Duplicate Record Found");
                }
                position += 2;
                if (position > m_records.Length)
                    position = 0;
            }
            m_records[position] = key;
            m_records[position + 1] = value;
            Count++;
        }

        public bool TryGetValue(int key, out int value)
        {
            if (key == 0)
                throw new ArgumentException();
            int position = (key & m_mask) * 2;
            while (m_records[position] != 0)
            {
                if (m_records[position] == key)
                {
                    value = m_records[position + 1];
                    return true;
                }
                position += 2;
                if (position > m_records.Length)
                    position = 0;
            }
            value = 0;
            return false;
        }

    }
}
