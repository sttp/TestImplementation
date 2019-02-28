using System;

namespace CTP
{
    internal static class ThreadStaticItems
    {
        [ThreadStatic]
        public static CtpObjectWriter CommandObject_Writer;
    }
}