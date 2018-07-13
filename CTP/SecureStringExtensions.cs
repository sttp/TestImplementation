using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

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
                Marshal.ZeroFreeCoTaskMemUnicode(valuePtr);
            }
        }

        public static unsafe byte[] ToUTF8(this SecureString str)
        {
            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToCoTaskMemUnicode(str);
                int length = Encoding.UTF8.GetByteCount((char*)valuePtr, str.Length);
                byte[] rv = new byte[length];
                fixed (byte* ptr = &rv[0])
                {
                    Encoding.UTF8.GetBytes((char*)valuePtr, str.Length, ptr, length);
                }
                return rv;
            }
            finally
            {
                Marshal.ZeroFreeCoTaskMemUnicode(valuePtr);
            }
        }
    }
}