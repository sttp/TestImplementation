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
using System.Linq;
using Sttp.Collection;

namespace Sttp.DataPointEncoding
{
    /// <summary>
    /// The metadata kept for each pointID.
    /// </summary>
    public class TsscPointIDEncoding
    {
        private int m_maxJumpCounter = 0;
        private int m_state = 0;
        private int m_bitsPerCode = 0;

        private IndexedArray<TsscPointMetadata> m_points;

        private readonly ByteWriter m_writeBits;

        private readonly ByteReader m_readBit;

        private Dictionary<int, int> m_validValues = new Dictionary<int, int>();

        internal TsscPointIDEncoding(IndexedArray<TsscPointMetadata> points, ByteWriter writer, ByteReader reader)
        {
            m_points = points;
            m_state = 0;
            m_maxJumpCounter = 0;
            m_writeBits = writer;
            m_readBit = reader;
        }

        public void WriteCode(int nextID)
        {
            var nextPoint = m_points[nextID];
            nextPoint.JumpedToCounter++;
            m_maxJumpCounter = Math.Max(m_maxJumpCounter, nextPoint.JumpedToCounter);
            if (nextPoint.JumpID > 0)
            {
                m_writeBits.WriteBits(m_bitsPerCode, (uint)nextID);
            }
            else
            {
                m_writeBits.WriteBits(m_bitsPerCode, 0);
                m_writeBits.Write8BitSegments((uint)nextID);
            }

            UpdatedCodeStatistics();
        }

        public int ReadCode()
        {
            return 0;
        }

        private void UpdatedCodeStatistics()
        {
            if (m_state == 0)
            {
                if (m_maxJumpCounter == 3)
                {
                    m_maxJumpCounter = 0;
                    AdaptCommands(2);
                }

                m_state = 1;
            }
            else if (m_state == 1)
            {
                if (m_maxJumpCounter == 10)
                {
                    m_maxJumpCounter = 0;
                    AdaptCommands(4);
                }

                m_state = 2;
            }
            else
            {
                if (m_maxJumpCounter == 30)
                {
                    m_maxJumpCounter = 0;
                    AdaptCommands(5);
                }

                m_state = 2;
            }
        }

        private void AdaptCommands(int threshold)
        {
            int indexedItems = 0;
            foreach (var item in m_points)
            {
                if (item != null)
                {
                    if (item.JumpedToCounter >= threshold)
                    {
                        indexedItems++;
                        item.JumpID = indexedItems;
                    }
                    else
                    {
                        item.JumpID = 0;
                    }
                    item.JumpedToCounter = 0;
                }
            }

            m_bitsPerCode = CompareUInt32.RequiredBits((uint)indexedItems);
        }
    }
}
