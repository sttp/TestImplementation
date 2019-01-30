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
    internal class AdvancedTimestampEncoding
    {
        private int m_codesSinceLast = 0;
        private int m_codesPer = 0;

        private long m_time1;
        private long m_time2;
        private long m_time3;
        private long m_time4;
        private long m_time5;

        private readonly ByteWriter m_writeBits;

        private readonly ByteReader m_readBit;

        private Dictionary<long, int> m_validValues = new Dictionary<long, int>();

        public AdvancedTimestampEncoding(ByteWriter writer, ByteReader reader)
        {
            m_codesPer = 0;
            m_codesSinceLast = 0;
            m_writeBits = writer;
            m_readBit = reader;
        }

        public void WriteCode(AdvancedWordEncoding encoder, long value)
        {
            var valueCheck = Math.Abs(value);
            if (valueCheck == m_time1)
            {
                encoder.WriteCode(AdvancedCodeWords.Timestamp1);
                if (value < 0)
                    m_writeBits.WriteBits1(1);
                else
                    m_writeBits.WriteBits1(0);
            }
            else if (valueCheck == m_time2)
            {
                encoder.WriteCode(AdvancedCodeWords.Timestamp2);
                if (value < 0)
                    m_writeBits.WriteBits1(1);
                else
                    m_writeBits.WriteBits1(0);
            }
            else if (valueCheck == m_time3)
            {
                encoder.WriteCode(AdvancedCodeWords.Timestamp3);
                if (value < 0)
                    m_writeBits.WriteBits1(1);
                else
                    m_writeBits.WriteBits1(0);
            }
            else if (valueCheck == m_time4)
            {
                encoder.WriteCode(AdvancedCodeWords.Timestamp4);
                if (value < 0)
                    m_writeBits.WriteBits1(1);
                else
                    m_writeBits.WriteBits1(0);
            }
            else if (valueCheck == m_time5)
            {
                encoder.WriteCode(AdvancedCodeWords.Timestamp5);
                if (value < 0)
                    m_writeBits.WriteBits1(1);
                else
                    m_writeBits.WriteBits1(0);
            }
            else
            {
                encoder.WriteCode(AdvancedCodeWords.TimestampElse);
                if (value < 0)
                {
                    m_writeBits.WriteBits1(1);
                    m_writeBits.Write8BitSegments((ulong)~value);
                }
                else
                {
                    m_writeBits.WriteBits1(0);
                    m_writeBits.Write8BitSegments((ulong)value);
                }
            }

            UpdatedCodeStatistics(value);
        }

        public long ReadCode(AdvancedCodeWords encoding)
        {
            long code;

            switch (encoding)
            {
                case AdvancedCodeWords.Timestamp1:
                    if (m_readBit.ReadBits1() == 0)
                    {
                        code = m_time1;
                    }
                    else
                    {
                        code = -m_time1;
                    }
                    break;
                case AdvancedCodeWords.Timestamp2:
                    if (m_readBit.ReadBits1() == 0)
                    {
                        code = m_time2;
                    }
                    else
                    {
                        code = -m_time2;
                    }
                    break;
                case AdvancedCodeWords.Timestamp3:
                    if (m_readBit.ReadBits1() == 0)
                    {
                        code = m_time3;
                    }
                    else
                    {
                        code = -m_time3;
                    }
                    break;
                case AdvancedCodeWords.Timestamp4:
                    if (m_readBit.ReadBits1() == 0)
                    {
                        code = m_time4;
                    }
                    else
                    {
                        code = -m_time4;
                    }
                    break;
                case AdvancedCodeWords.Timestamp5:
                    if (m_readBit.ReadBits1() == 0)
                    {
                        code = m_time5;
                    }
                    else
                    {
                        code = -m_time5;
                    }
                    break;
                case AdvancedCodeWords.TimestampElse:
                    if (m_readBit.ReadBits1() == 0)
                    {
                        code = (long)m_readBit.Read8BitSegments();
                    }
                    else
                    {
                        code = -(long)m_readBit.Read8BitSegments();
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(encoding), encoding, null);
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

            long code4 = 0;
            int count4 = 0;

            long code5 = 0;
            int count5 = 0;

            foreach (var item in m_validValues)
            {
                int cnt = item.Value;
                long x = item.Key;

                if (cnt > count5)
                {
                    if (cnt > count1)
                    {
                        code5 = code4;
                        count5 = count4;

                        code4 = code3;
                        count4 = count3;

                        code3 = code2;
                        count3 = count2;

                        code2 = code1;
                        count2 = count1;

                        code1 = x;
                        count1 = cnt;
                    }
                    else if (cnt > count2)
                    {
                        code5 = code4;
                        count5 = count4;

                        code4 = code3;
                        count4 = count3;

                        code3 = code2;
                        count3 = count2;

                        code2 = x;
                        count2 = cnt;
                    }
                    else if (cnt > count3)
                    {
                        code5 = code4;
                        count5 = count4;

                        code4 = code3;
                        count4 = count3;

                        code3 = x;
                        count3 = cnt;
                    }
                    else if (cnt > count4)
                    {
                        code5 = code4;
                        count5 = count4;

                        code4 = x;
                        count4 = cnt;
                    }
                    else
                    {
                        code5 = x;
                        count5 = cnt;
                    }
                }
            }

            m_time1 = code1;
            m_time2 = code2;
            m_time3 = code3;
            m_time4 = code4;
            m_time5 = code5;

        }
    }
}
