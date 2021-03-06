﻿using System;
using System.Runtime.InteropServices;

namespace CTP
{

    public enum CtpObjectSymbols : byte
    {
        Null = 0,
        IntNeg2 = 1,
        IntNeg1,
        Int0,
        Int1,
        Int2,
        Int3,
        Int4,
        Int5,
        Int6,
        Int7,
        Int8,
        Int9,
        Int10,
        Int11,
        Int12,
        Int13,
        Int14,
        Int15,
        Int16,
        Int17,
        Int18,
        Int19,
        Int20,
        Int21,
        Int22,
        Int23,
        Int24,
        Int25,
        Int26,
        Int27,
        Int28,
        Int29,
        Int30,
        Int31,
        Int32,
        Int33,
        Int34,
        Int35,
        Int36,
        Int37,
        Int38,
        Int39,
        Int40,
        Int41,
        Int42,
        Int43,
        Int44,
        Int45,
        Int46,
        Int47,
        Int48,
        Int49,
        Int50,
        Int51,
        Int52,
        Int53,
        Int54,
        Int55,
        Int56,
        Int57,
        Int58,
        Int59,
        Int60,
        Int61,
        Int62,
        Int63,
        Int64,
        IntMaxRunLen = Int64,

        IntBits8Pos,
        IntBits16Pos,
        IntBits24Pos,
        IntBits32Pos,
        IntBits40Pos,
        IntBits48Pos,
        IntBits56Pos,

        IntBits8Neg,
        IntBits16Neg,
        IntBits24Neg,
        IntBits32Neg,
        IntBits40Neg,
        IntBits48Neg,
        IntBits56Neg,

        IntBits64,
        IntElse = IntBits64,

        SingleNeg1 = 83,
        Single0,
        Single1,
        Single56,
        Single57,
        Single58,
        Single59,
        Single60,
        Single61,
        Single62,
        Single63,
        Single64,
        Single65,
        Single66,
        Single67,
        Single68,
        Single69,
        Single70,
        Single71,
        Single72,
        Single73,
        Single74,
        Single75,
        Single76,
        Single77,
        Single78,
        Single79,
        Single184,
        Single185,
        Single186,
        Single187,
        Single188,
        Single189,
        Single190,
        Single191,
        Single192,
        Single193,
        Single194,
        Single195,
        Single196,
        Single197,
        Single198,
        Single199,
        Single200,
        Single201,
        Single202,
        Single203,
        Single204,
        Single205,
        Single206,
        Single207,
        SingleElse,

        DoubleNeg1 = 135,
        Double0,
        Double1,
        Double63,
        Double64,
        Double65,
        Double191,
        Double192,
        Double193,
        DoubleElse,

        NumericNone = 145,
        NumericLow,
        NumericMid,
        NumericHigh,
        NumericElse = NumericHigh,

        CtpTimeZero = 149,
        CtpTime14,
        CtpTime15,
        CtpTime16,
        CtpTime17,
        CtpTimeElse,

        BoolFalse = 155,
        BoolTrue,
        BoolElse = BoolTrue,

        GuidEmpty = 157,
        GuidElse,

        String0 = 159,
        String1,
        String2,
        String3,
        String4,
        String5,
        String6,
        String7,
        String8,
        String9,
        String10,
        String11,
        String12,
        String13,
        String14,
        String15,
        String16,
        String17,
        String18,
        String19,
        String20,
        String21,
        String22,
        String23,
        String24,
        String25,
        String26,
        String27,
        String28,
        String29,
        String30,
        String8Bit,
        String16Bit,
        String24Bit,
        String32Bit,
        StringElse = String32Bit,

        CtpBuffer0 = 194,
        CtpBuffer1,
        CtpBuffer2,
        CtpBuffer3,
        CtpBuffer4,
        CtpBuffer5,
        CtpBuffer6,
        CtpBuffer7,
        CtpBuffer8,
        CtpBuffer9,
        CtpBuffer10,
        CtpBuffer11,
        CtpBuffer12,
        CtpBuffer13,
        CtpBuffer14,
        CtpBuffer15,
        CtpBuffer16,
        CtpBuffer17,
        CtpBuffer18,
        CtpBuffer19,
        CtpBuffer20,
        CtpBuffer21,
        CtpBuffer22,
        CtpBuffer23,
        CtpBuffer24,
        CtpBuffer25,
        CtpBuffer26,
        CtpBuffer27,
        CtpBuffer28,
        CtpBuffer29,
        CtpBuffer30,
        CtpBuffer31,
        CtpBuffer32,
        CtpBuffer33,
        CtpBuffer34,
        CtpBuffer35,
        CtpBuffer36,
        CtpBuffer37,
        CtpBuffer38,
        CtpBuffer39,
        CtpBuffer40,
        CtpBuffer41,
        CtpBuffer42,
        CtpBuffer43,
        CtpBuffer44,
        CtpBuffer45,
        CtpBuffer46,
        CtpBuffer47,
        CtpBuffer48,
        CtpBuffer49,
        CtpBuffer50,

        CtpBuffer8Bit = 245,
        CtpBuffer16Bit,
        CtpBuffer24Bit,
        CtpBuffer32Bit,
        CtpBufferElse = CtpBuffer32Bit,

        CtpCommand8Bit = 249,
        CtpCommand16Bit,
        CtpCommand24Bit,
        CtpCommand32Bit,
        CtpCommandElse = CtpCommand32Bit,
    }

}
