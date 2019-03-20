using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CTP.Serialization
{
    internal class EnumSerialization
    {
        private static readonly MethodInfo Method2;

        static EnumSerialization()
        {
            Method2 = typeof(EnumSerialization).GetMethod("Generic2", BindingFlags.Static | BindingFlags.NonPublic);
        }

        public static TypeIOMethodBase<T> TryCreate<T>(string recordName)
        {
            var type = typeof(T);
            if (type.IsEnum)
            {
                var func = Method2.MakeGenericMethod(type);
                return (TypeIOMethodBase<T>)func.Invoke(null, new object[] { recordName });
            }
            return null;
        }

        // ReSharper disable once UnusedMember.Local
        private static object Generic2<T>(string recordName)
            where T : struct
        {
            return new EnumSerializationMethod<T>(recordName);
        }
    }
}
