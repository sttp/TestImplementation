using System;
using System.Runtime.InteropServices;
using System.Security;

namespace CTP.SRP
{
    internal static class SecureStringExtensions
    {
        public static char[] ToCharArray(this SecureString str)
        {
            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToCoTaskMemUnicode(str);
                char[] items = new char[str.Length];
                Marshal.Copy(valuePtr, items, 0, str.Length);
                return items;
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }
    }
}