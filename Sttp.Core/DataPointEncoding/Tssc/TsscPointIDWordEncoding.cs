﻿//******************************************************************************************************
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

namespace Sttp.DataPointEncoding
{
    /// <summary>
    /// The encoding commands supported by TSSC. This class is used by 
    /// <see cref="TsscDecoder"/> and <see cref="TsscEncoder"/>.
    /// </summary>
    internal static class TsscCodeWordsPointID
    {
        /// <summary>
        /// This code word will be adaptive.
        /// </summary>
        public const byte EndOfStream = 0;
        public const byte PointNextIDSameAsPrevious1 = 1;
        public const byte PointNextIDSameAsPrevious2 = 2;
        public const byte PointIDXOR8 = 3;
        public const byte PointIDXOR12 = 4;
        public const byte PointIDXOR16 = 5;
        public const byte PointIDXOR24 = 6;
        public const byte PointID32 = 7;
    }

    /// <summary>
    /// The metadata kept for each pointID.
    /// </summary>
    internal class TsscPointIDWordEncoding
    {
        private readonly byte[] m_commandStats;
        private int m_commandsSentSinceLastChange = 0;

        //Bit codes for the 4 modes of encoding. 
        private byte m_mode;

        //(Mode 1 means no prefix.)
        private byte m_mode21;

        private byte m_mode31;
        private byte m_mode301;

        private byte m_mode41;
        private byte m_mode401;
        private byte m_mode4001;

        private int m_startupMode = 0;

        private readonly ByteWriter m_writeBits;

        private readonly ByteReader m_readBit;

        public TsscPointIDWordEncoding(ByteWriter writer, ByteReader reader)
        {
            m_commandsSentSinceLastChange = 0;
            m_commandStats = new byte[32];
            m_mode = 2;
            m_mode21 = TsscCodeWordsPointID.PointNextIDSameAsPrevious1;

            m_writeBits = writer;
            m_readBit = reader;
        }

        public void WriteCode(uint code)
        {
            switch (m_mode)
            {
                case 1:
                    m_writeBits.WriteBits3(code);
                    break;
                case 2:
                    if (code == m_mode21)
                    {
                        m_writeBits.WriteBits1(1);
                    }
                    else
                    {
                        m_writeBits.WriteBits4(code);
                    }
                    break;
                case 3:
                    if (code == m_mode31)
                    {
                        m_writeBits.WriteBits1(1);
                    }
                    else if (code == m_mode301)
                    {
                        m_writeBits.WriteBits2(1);
                    }
                    else
                    {
                        m_writeBits.WriteBits5(code);
                    }
                    break;
                case 4:
                    if (code == m_mode41)
                    {
                        m_writeBits.WriteBits1(1);
                    }
                    else if (code == m_mode401)
                    {
                        m_writeBits.WriteBits2(1);
                    }
                    else if (code == m_mode4001)
                    {
                        m_writeBits.WriteBits3(1);
                    }
                    else
                    {
                        m_writeBits.WriteBits6(code);
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
                    code = m_readBit.ReadBits3();
                    break;
                case 2:
                    if (m_readBit.ReadBits1() == 1)
                    {
                        code = m_mode21;
                    }
                    else
                    {
                        code = m_readBit.ReadBits3();
                    }
                    break;
                case 3:
                    if (m_readBit.ReadBits1() == 1)
                    {
                        code = m_mode31;
                    }
                    else if (m_readBit.ReadBits1() == 1)
                    {
                        code = m_mode301;
                    }
                    else
                    {
                        code = m_readBit.ReadBits3();
                    }
                    break;
                case 4:
                    if (m_readBit.ReadBits1() == 1)
                    {
                        code = m_mode41;
                    }
                    else if (m_readBit.ReadBits1() == 1)
                    {
                        code = m_mode401;
                    }
                    else if (m_readBit.ReadBits1() == 1)
                    {
                        code = m_mode4001;
                    }
                    else
                    {
                        code = m_readBit.ReadBits3();
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
            m_commandsSentSinceLastChange++;
            m_commandStats[code]++;

            if (m_startupMode == 0 && m_commandsSentSinceLastChange > 5)
            {
                m_startupMode++;
                AdaptCommands();
            }
            else if (m_startupMode == 1 && m_commandsSentSinceLastChange > 20)
            {
                m_startupMode++;
                AdaptCommands();
            }
            else if (m_startupMode == 2 && m_commandsSentSinceLastChange > 100)
            {
                AdaptCommands();
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

            int total = 0;

            for (int x = 0; x < m_commandStats.Length; x++)
            {
                int cnt = m_commandStats[x];
                m_commandStats[x] = 0;

                total += cnt;

                if (cnt > count3)
                {
                    if (cnt > count1)
                    {
                        code3 = code2;
                        count3 = count2;

                        code2 = code1;
                        count2 = count1;

                        code1 = (byte)x;
                        count1 = cnt;
                    }
                    else if (cnt > count2)
                    {
                        code3 = code2;
                        count3 = count2;

                        code2 = (byte)x;
                        count2 = cnt;
                    }
                    else
                    {
                        code3 = (byte)x;
                        count3 = cnt;
                    }
                }
            }

            int mode1Size = total * 3;
            int mode2Size = count1 * 1 + (total - count1) * 4;
            int mode3Size = count1 * 1 + count2 * 2 + (total - count1 - count2) * 5;
            int mode4Size = count1 * 1 + count2 * 2 + count3 * 3 + (total - count1 - count2 - count3) * 6;

            int minSize = int.MaxValue;
            minSize = Math.Min(minSize, mode1Size);
            minSize = Math.Min(minSize, mode2Size);
            minSize = Math.Min(minSize, mode3Size);
            minSize = Math.Min(minSize, mode4Size);

            if (minSize == mode1Size)
            {
                m_mode = 1;
            }
            else if (minSize == mode2Size)
            {
                m_mode = 2;
                m_mode21 = code1;
            }
            else if (minSize == mode3Size)
            {
                m_mode = 3;
                m_mode31 = code1;
                m_mode301 = code2;
            }
            else if (minSize == mode4Size)
            {
                m_mode = 4;
                m_mode41 = code1;
                m_mode401 = code2;
                m_mode4001 = code3;
            }
            else
            {
                throw new Exception("Coding Error");
            }

            m_commandsSentSinceLastChange = 0;
        }
    }
}
