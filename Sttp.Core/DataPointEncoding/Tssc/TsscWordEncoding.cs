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
using System.Linq;

namespace Sttp.DataPointEncoding
{
    /// <summary>
    /// The metadata kept for each pointID.
    /// </summary>
    public class TsscWordEncoding
    {
        private readonly byte[] m_commandStats;
        private int m_codesSinceLast = 0;
        private int m_codesPer = 0;

        //Bit codes for the 4 modes of encoding. 
        private byte m_mode;

        private byte m_mode1;
        private byte m_mode01;
        private byte m_mode001;
        private byte m_mode0001;

        private readonly ByteWriter m_writeBits;

        private readonly ByteReader m_readBit;

        public TsscWordEncoding(ByteWriter writer, ByteReader reader)
        {
            m_codesPer = 0;
            m_codesSinceLast = 0;
            m_commandStats = new byte[16];
            m_mode = 1;
            m_writeBits = writer;
            m_readBit = reader;
        }

        public void WriteCode(byte code)
        {
            switch (m_mode)
            {
                case 1:
                    m_writeBits.WriteBits4(code);
                    break;
                case 2:
                    if (code == m_mode1)
                    {
                        m_writeBits.WriteBits1(1);
                    }
                    else
                    {
                        m_writeBits.WriteBits1(0);
                        m_writeBits.WriteBits4(code);
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
                        m_writeBits.WriteBits4(code);
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
                        m_writeBits.WriteBits4(code);
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
                        m_writeBits.WriteBits4(code);
                    }
                    break;
                default:
                    throw new Exception("Coding Error");
            }

            UpdatedCodeStatistics(code);
        }

        public uint ReadCode()
        {
            uint code = 0;
            switch (m_mode)
            {
                case 1:
                    code = m_readBit.ReadBits4();
                    break;
                case 2:
                    if (m_readBit.ReadBits1() == 1)
                    {
                        code = m_mode1;
                    }
                    else
                    {
                        code = m_readBit.ReadBits4();
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
                        code = m_readBit.ReadBits4();
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
                        code = m_readBit.ReadBits4();
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
                        code = m_readBit.ReadBits4();
                    }
                    break;
                default:
                    throw new Exception("Unsupported compression mode");
            }

            UpdatedCodeStatistics(code);
            return code;
        }

        private void UpdatedCodeStatistics(uint code)
        {
            if (m_codesPer == 0)
            {
                //Ignore all exception codes.
                //Skip first real code.
                if (code == 0)
                    return;
                m_codesPer = 1;
                return;
            }
            if (m_codesPer == 1)
            {
                //Ignore all exception codes.
                if (code == 0)
                    return;
                m_codesPer = 2;
                m_codesSinceLast++;
                m_commandStats[code]++;
                AdaptCommands();
                return;
            }
            m_codesSinceLast++;
            m_commandStats[code]++;

            if (m_codesSinceLast >= m_codesPer)
            {
                AdaptCommands();
                if (m_codesPer < 7)
                {
                    m_codesPer++;
                }
                else if (m_codesPer < 20)
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
                    Array.Clear(m_commandStats, 0, m_commandStats.Length);
                }
             
            }
        }

        private void AdaptCommands()
        {
            byte code1 = 0;
            int count1 = 0;

            byte code2 = 1;
            int count2 = 0;

            byte code3 = 2;
            int count3 = 0;

            byte code4 = 3;
            int count4 = 0;

            int total = 0;

            for (int x = 0; x < m_commandStats.Length; x++)
            {
                int cnt = m_commandStats[x];

                total += cnt;

                if (cnt > count4)
                {
                    if (cnt > count1)
                    {
                        code4 = code3;
                        count4 = count3;

                        code3 = code2;
                        count3 = count2;

                        code2 = code1;
                        count2 = count1;

                        code1 = (byte)x;
                        count1 = cnt;
                    }
                    else if (cnt > count2)
                    {
                        code4 = code3;
                        count4 = count3;

                        code3 = code2;
                        count3 = count2;

                        code2 = (byte)x;
                        count2 = cnt;
                    }
                    else if (cnt > count3)
                    {
                        code4 = code3;
                        count4 = count3;

                        code3 = (byte)x;
                        count3 = cnt;
                    }
                    else
                    {
                        code4 = (byte)x;
                        count4 = cnt;
                    }
                }
            }


            int mode1Size = total * 4;
            int mode2Size = count1 * 1 + (total - count1) * 5;
            int mode3Size = count1 * 1 + count2 * 2 + (total - count1 - count2) * 6;
            int mode4Size = count1 * 1 + count2 * 2 + count3 * 3 + (total - count1 - count2 - count3) * 7;
            int mode5Size = count1 * 1 + count2 * 2 + count3 * 3 + count4 * 3 + (total - count1 - count2 - count3 - count4) * 8;

            int minSize = int.MaxValue;
            minSize = Math.Min(minSize, mode1Size);
            minSize = Math.Min(minSize, mode2Size);
            minSize = Math.Min(minSize, mode3Size);
            minSize = Math.Min(minSize, mode4Size);
            minSize = Math.Min(minSize, mode5Size);

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
            else
            {
                throw new Exception("Coding Error");
            }

        }
    }
}
