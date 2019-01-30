//******************************************************************************************************
//  TsscWordEncoding.cs - Gbtc
//
//  Copyright © 2016, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  12/02/2016 - Steven E. Chisholm
//       Generated original version of source code.
//
//******************************************************************************************************


using System;
using System.Collections.Generic;

namespace Sttp.DataPointEncoding
{
    /// <summary>
    /// The metadata kept for each pointID.
    /// </summary>
    public class TsscTimestampEncoding
    {
        private int m_codesSinceLast = 0;
        private int m_codesPer = 0;

        private long m_mode1;
        private long m_mode01;
        private long m_mode001;

        private readonly ByteWriter m_writeBits;

        private readonly ByteReader m_readBit;

        private Dictionary<long, int> m_validValues = new Dictionary<long, int>();

        public TsscTimestampEncoding(ByteWriter writer, ByteReader reader)
        {
            m_codesPer = 0;
            m_codesSinceLast = 0;
            m_writeBits = writer;
            m_readBit = reader;
        }

        public void WriteCode(long value)
        {
            if (value == m_mode1)
            {
                m_writeBits.WriteBits1(1);
                if (value < 0)
                    m_writeBits.WriteBits1(1);
                else
                    m_writeBits.WriteBits1(0);
            }
            else if (value == m_mode01)
            {
                m_writeBits.WriteBits1(0);
                m_writeBits.WriteBits1(1);
                if (value < 0)
                    m_writeBits.WriteBits1(1);
                else
                    m_writeBits.WriteBits1(0);
            }
            else if (value == m_mode001)
            {
                m_writeBits.WriteBits1(0);
                m_writeBits.WriteBits1(0);
                m_writeBits.WriteBits1(1);
                if (value < 0)
                    m_writeBits.WriteBits1(1);
                else
                    m_writeBits.WriteBits1(0);
            }
            else
            {
                m_writeBits.WriteBits1(0);
                m_writeBits.WriteBits1(0);
                m_writeBits.WriteBits1(1);
                if (value < 0)
                {
                    m_writeBits.Write8BitSegments((ulong)~value);
                    m_writeBits.WriteBits1(1);
                }
                else
                {
                    m_writeBits.Write8BitSegments((ulong)value);
                    m_writeBits.WriteBits1(0);
                }
            }

            UpdatedCodeStatistics(value);
        }

        public long ReadCode()
        {
            long code;
            if (m_readBit.ReadBits1() == 1)
            {
                code = m_mode1;
            }
            else if (m_readBit.ReadBits1() == 1)
            {
                code = m_mode01;
            }
            else if (m_readBit.ReadBits1() == 1)
            {
                code = m_mode001;
            }
            else
            {
                code = m_readBit.ReadBits4();
            }

            if (m_readBit.ReadBits1() == 1)
            {
                code = ~code;
            }

            UpdatedCodeStatistics(code);
            return code;
        }

        private void UpdatedCodeStatistics(long value)
        {
            if (m_codesPer == 0)
            {
                m_codesPer = 5;
                return;
            }

            value = Math.Abs(value);

            if (m_validValues.TryGetValue(value, out var cnt))
            {
                m_validValues[value] = cnt + 1;
            }
            else
            {
                m_validValues[value] = 1;
            }

            m_codesSinceLast++;

            if (m_codesSinceLast >= m_codesPer)
            {
                AdaptCommands();
                if (m_codesPer < 20)
                {
                    m_codesPer += 5;
                }
                else if (m_codesPer < 100)
                {
                    m_codesPer += 20;
                }
                else
                {
                    m_codesSinceLast = 0;
                    m_validValues.Clear();
                }
            }
        }

        private void AdaptCommands()
        {
            long code1 = 0;
            int count1 = 0;

            long code2 = 0;
            int count2 = 0;

            long code3 = 0;
            int count3 = 0;

            foreach (var item in m_validValues)
            {
                int cnt = item.Value;
                long x = item.Key;

                if (cnt > count3)
                {
                    if (cnt > count1)
                    {
                        code3 = code2;
                        count3 = count2;

                        code2 = code1;
                        count2 = count1;

                        code1 = x;
                        count1 = cnt;
                    }
                    else if (cnt > count2)
                    {
                        code3 = code2;
                        count3 = count2;

                        code2 = x;
                        count2 = cnt;
                    }
                    else
                    {
                        code3 = x;
                        count3 = cnt;
                    }
                }
            }

            m_mode1 = code1;
            m_mode01 = code2;
            m_mode001 = code3;

        }
    }
}
