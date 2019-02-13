//******************************************************************************************************
//  AdvancedSymbolEncoding.cs - Gbtc
//
//  Copyright © 2019, Grid Protection Alliance.  All Rights Reserved.
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
//  01/30/2019 - Steven E. Chisholm
//       Generated original version of source code.
//
//******************************************************************************************************


using System;
using System.Text;
using CTP;

namespace Sttp.DataPointEncoding
{
    /// <summary>
    /// Symbols are adaptively encoded on the wire. This designates the specific encoding of the symbols.
    /// </summary>
    public class AdvancedSymbolEncoding
    {
        private readonly byte[] m_commandStats;
        private int m_codesSinceLast = 0;
        private int m_codesPer = 0;

        private byte m_mode;
        private AdvancedSymbols m_mode1;
        private AdvancedSymbols m_mode01;
        private AdvancedSymbols m_mode001;
        private AdvancedSymbols m_mode0001;
        private AdvancedSymbols m_mode00001;

        private readonly ByteWriter m_writeBits;
        private readonly ByteReader m_readBit;

        public AdvancedSymbolEncoding(ByteWriter writer, ByteReader reader)
        {
            m_codesPer = 0;
            m_codesSinceLast = 0;
            m_commandStats = new byte[32];
            m_mode = 1;
            m_writeBits = writer;
            m_readBit = reader;
        }

        public void WriteCode(AdvancedSymbols code)
        {
            switch (m_mode)
            {
                case 1:
                    m_writeBits.WriteBits5((byte)code);
                    break;
                case 2:
                    if (code == m_mode1)
                    {
                        m_writeBits.WriteBits1(1);
                    }
                    else
                    {
                        m_writeBits.WriteBits1(0);
                        m_writeBits.WriteBits5((byte)code);
                    }
                    break;
                case 3:
                    if (code == m_mode1)
                    {
                        m_writeBits.WriteBits1(1);
                    }
                    else if (code == m_mode01)
                    {
                        m_writeBits.WriteBits2(1);
                    }
                    else
                    {
                        m_writeBits.WriteBits2(0);
                        m_writeBits.WriteBits5((byte)code);
                    }
                    break;
                case 4:
                    if (code == m_mode1)
                    {
                        m_writeBits.WriteBits1(1);
                    }
                    else if (code == m_mode01)
                    {
                        m_writeBits.WriteBits2(1);
                    }
                    else if (code == m_mode001)
                    {
                        m_writeBits.WriteBits3(1);
                    }
                    else
                    {
                        m_writeBits.WriteBits3(0);
                        m_writeBits.WriteBits5((byte)code);
                    }
                    break;
                case 5:
                    if (code == m_mode1)
                    {
                        m_writeBits.WriteBits1(1);
                    }
                    else if (code == m_mode01)
                    {
                        m_writeBits.WriteBits2(1);
                    }
                    else if (code == m_mode001)
                    {
                        m_writeBits.WriteBits3(1);
                    }
                    else if (code == m_mode0001)
                    {
                        m_writeBits.WriteBits4(1);
                    }
                    else
                    {
                        m_writeBits.WriteBits4(0);
                        m_writeBits.WriteBits5((byte)code);
                    }
                    break;
                case 6:
                    if (code == m_mode1)
                    {
                        m_writeBits.WriteBits1(1);
                    }
                    else if (code == m_mode01)
                    {
                        m_writeBits.WriteBits2(1);
                    }
                    else if (code == m_mode001)
                    {
                        m_writeBits.WriteBits3(1);
                    }
                    else if (code == m_mode0001)
                    {
                        m_writeBits.WriteBits4(1);
                    }
                    else if (code == m_mode00001)
                    {
                        m_writeBits.WriteBits5(1);
                    }
                    else
                    {
                        m_writeBits.WriteBits5(0);
                        m_writeBits.WriteBits5((byte)code);
                    }
                    break;
                default:
                    throw new Exception("Coding Error");
            }

            UpdatedCodeStatistics(code);
        }

        public AdvancedSymbols ReadCode()
        {
            AdvancedSymbols code = 0;
            switch (m_mode)
            {
                case 1:
                    code = (AdvancedSymbols)m_readBit.ReadBits5();
                    break;
                case 2:
                    if (m_readBit.ReadBits1() == 1)
                    {
                        code = m_mode1;
                    }
                    else
                    {
                        code = (AdvancedSymbols)m_readBit.ReadBits5();
                    }
                    break;
                case 3:
                    if (m_readBit.ReadBits1() == 1)
                    {
                        code = m_mode1;
                    }
                    else if (m_readBit.ReadBits1() == 1)
                    {
                        code = m_mode01;
                    }
                    else
                    {
                        code = (AdvancedSymbols)m_readBit.ReadBits5();
                    }
                    break;
                case 4:
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
                        code = (AdvancedSymbols)m_readBit.ReadBits5();
                    }
                    break;
                case 5:
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
                    else if (m_readBit.ReadBits1() == 1)
                    {
                        code = m_mode0001;
                    }
                    else
                    {
                        code = (AdvancedSymbols)m_readBit.ReadBits5();
                    }
                    break;
                case 6:
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
                    else if (m_readBit.ReadBits1() == 1)
                    {
                        code = m_mode0001;
                    }
                    else if (m_readBit.ReadBits1() == 1)
                    {
                        code = m_mode00001;
                    }
                    else
                    {
                        code = (AdvancedSymbols)m_readBit.ReadBits5();
                    }
                    break;
                default:
                    throw new Exception("Unsupported compression mode");
            }

            UpdatedCodeStatistics(code);
            return code;
        }

        private void UpdatedCodeStatistics(AdvancedSymbols code)
        {
            if (code >= AdvancedSymbols.ValueDefault)
                m_codesSinceLast++;
            m_commandStats[(byte)code]++;

            if (m_codesPer < 5)
            {
                m_codesPer++;
                AdaptCommands();
            }
            else if (m_codesPer < 10)
            {
                m_codesPer += 2;
                AdaptCommands();
            }
            else if (m_codesPer < 50)
            {
                m_codesPer += 10;
                AdaptCommands();
            }
            else
            {
                if (m_codesSinceLast >= 100)
                {
                    AdaptCommands();
                    m_codesSinceLast = 0;
                    Array.Clear(m_commandStats, 0, m_commandStats.Length);
                }
            }

        }

        private void AdaptCommands()
        {
            AdvancedSymbols code1 = 0;
            int count1 = 0;

            AdvancedSymbols code2 = 0;
            int count2 = 0;

            AdvancedSymbols code3 = 0;
            int count3 = 0;

            AdvancedSymbols code4 = 0;
            int count4 = 0;

            AdvancedSymbols code5 = 0;
            int count5 = 0;

            int total = 0;

            for (int x = 0; x < m_commandStats.Length; x++)
            {
                int cnt = m_commandStats[x];
                total += cnt;
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

                        code1 = (AdvancedSymbols)x;
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

                        code2 = (AdvancedSymbols)x;
                        count2 = cnt;
                    }
                    else if (cnt > count3)
                    {
                        code5 = code4;
                        count5 = count4;

                        code4 = code3;
                        count4 = count3;

                        code3 = (AdvancedSymbols)x;
                        count3 = cnt;
                    }
                    else if (cnt > count4)
                    {
                        code5 = code4;
                        count5 = count4;

                        code4 = (AdvancedSymbols)x;
                        count4 = cnt;
                    }
                    else
                    {
                        code5 = (AdvancedSymbols)x;
                        count5 = cnt;
                    }
                }
            }

            int mode1Size = total * 5;
            int mode2Size = count1 * 1 + (total - count1) * 6;
            int mode3Size = count1 * 1 + count2 * 2 + (total - count1 - count2) * 7;
            int mode4Size = count1 * 1 + count2 * 2 + count3 * 3 + (total - count1 - count2 - count3) * 8;
            int mode5Size = count1 * 1 + count2 * 2 + count3 * 3 + count4 * 4 + (total - count1 - count2 - count3 - count4) * 9;
            int mode6Size = count1 * 1 + count2 * 2 + count3 * 3 + count4 * 4 + count5 * 5 + (total - count1 - count2 - count3 - count4 - count5) * 10;

            int minSize = int.MaxValue;
            minSize = Math.Min(minSize, mode1Size);
            minSize = Math.Min(minSize, mode2Size);
            minSize = Math.Min(minSize, mode3Size);
            minSize = Math.Min(minSize, mode4Size);
            minSize = Math.Min(minSize, mode5Size);
            minSize = Math.Min(minSize, mode6Size);

            if (minSize == mode1Size)
            {
                m_mode = 1;
            }
            else if (minSize == mode2Size)
            {
                m_mode = 2;
                m_mode1 = code1;
            }
            else if (minSize == mode3Size)
            {
                m_mode = 3;
                m_mode1 = code1;
                m_mode01 = code2;
            }
            else if (minSize == mode4Size)
            {
                m_mode = 4;
                m_mode1 = code1;
                m_mode01 = code2;
                m_mode001 = code3;
            }
            else if (minSize == mode5Size)
            {
                m_mode = 5;
                m_mode1 = code1;
                m_mode01 = code2;
                m_mode001 = code3;
                m_mode0001 = code4;
            }
            else if (minSize == mode6Size)
            {
                m_mode = 6;
                m_mode1 = code1;
                m_mode01 = code2;
                m_mode001 = code3;
                m_mode0001 = code4;
                m_mode00001 = code5;
            }
            else
            {
                throw new Exception("Coding Error");
            }

        }
    }
}
