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
using System.Numerics;

namespace CTP.SRP
{
    public static class BigIntegerExtensions
    {
        public static byte[] Concat(this byte[] value, params byte[][] additionalItems)
        {
            int finalLength = value?.Length ?? 0;
            foreach (var item in additionalItems)
            {
                if (item != null)
                    finalLength += item.Length;
            }
            byte[] rv = new byte[finalLength];
            finalLength = 0;
            if (value != null)
            {
                value.CopyTo(rv, 0);
                finalLength += value.Length;
            }
            foreach (var item in additionalItems)
            {
                if (item != null)
                {
                    item.CopyTo(rv, finalLength);
                    finalLength += item.Length;
                }
            }

            return rv;
        }

        public static BigInteger ToUnsignedBigInteger(this byte[] value)
        {
            if (value.Length == 0)
                return BigInteger.Zero;
            value = (byte[])value.Clone();
            Array.Reverse(value);
            if (value[value.Length - 1] >= 128) //This ensures that the highest order bit is not set so the value is unsigned.
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

        public static byte[] ToUnsignedByteArray(this BigInteger value, int padLength)
        {
            if (value.Sign < 0)
                throw new Exception("Value is negative");
            byte[] data = value.ToByteArray();
            byte[] rv = new byte[padLength];
            for (int x = 0; x < data.Length; x++)
            {
                if (x < padLength)
                {
                    rv[padLength - x - 1] = data[x];
                }
            }
            return rv;
        }

        public static BigInteger ModPow(this BigInteger value, BigInteger exponent, BigInteger mod)
        {
            return BigInteger.ModPow(value, exponent, mod);
        }

        public static BigInteger Mod(this BigInteger value, BigInteger modulo)
        {
            BigInteger rv = value % modulo;
            if (value.Sign < 0)
                return rv + modulo;
            return rv;
        }

        public static BigInteger ModMul(this BigInteger value, BigInteger value2, BigInteger modulo)
        {
            return (value * value2).Mod(modulo);
        }

        public static BigInteger ModAdd(this BigInteger value, BigInteger value2, BigInteger modulo)
        {
            return (value + value2).Mod(modulo);
        }

        public static BigInteger ModSub(this BigInteger value, BigInteger value2, BigInteger modulo)
        {
            return (value - value2).Mod(modulo);
        }

    }
}


