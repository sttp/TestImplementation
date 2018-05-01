//******************************************************************************************************
//  SrpConstants.cs - Gbtc
//
//  Copyright © 2014, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the Eclipse Public License -v 1.0 (the "License"); you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/eclipse-1.0.php
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  7/27/2014 - Steven E. Chisholm
//       Generated original version of source code. 
//       
//
//******************************************************************************************************

using System;
using System.Globalization;
using System.Numerics;
using System.Text;

namespace CTP.SRP
{
    public static class BigIntegerExtensions
    {
        public static BigInteger ToUnsignedBigInteger(this byte[] value)
        {
            if (value.Length == 0)
                return BigInteger.Zero;
            Array.Reverse(value);
            if (value[value.Length - 1] >= 128)
            {
                var value2 = new byte[value.Length + 1];
                value.CopyTo(value2, 0);
                return new BigInteger(value2);
            }
            return new BigInteger(value);
        }

        public static byte[] ToUnsignedByteArray(this BigInteger value)
        {
            if (value.Sign < 0)
                throw new Exception("Value is negative");
            byte[] data = value.ToByteArray();
            Array.Reverse(data);
            for (int x = 0; x < data.Length; x++)
            {
                if (data[x] != 0)
                {
                    if (x == 0)
                        return data;

                    var rv = new byte[data.Length - x];
                    Array.Copy(data, x, rv, 0, rv.Length);
                    return rv;
                }
            }
            return new byte[0];
        }

        public static BigInteger ToBigInteger(this string data)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var b in data)
            {
                sb.Append(GetChar(b >> 4));
                sb.Append(GetChar(b & 15));
            }
            return BigInteger.Parse(sb.ToString(), NumberStyles.AllowHexSpecifier);
        }
        private static char GetChar(int value)
        {
            if (value <= 9)
                return (char)('0' + value);
            return (char)('A' - 10 + value);
        }

        public static BigInteger ModPow(this BigInteger value, BigInteger exponent, BigInteger mod)
        {
            return BigInteger.ModPow(value, exponent, mod);
        }

    }
}


