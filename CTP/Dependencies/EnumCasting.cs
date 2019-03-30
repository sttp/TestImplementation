using System;
using System.Linq.Expressions;

namespace GSF.Reflection
{
    public static class EnumCasting<TEnum>
            where TEnum : struct
    {
        private static readonly Func<int, TEnum> s_fromInt;
        private static readonly Func<TEnum, int> s_toInt;
        private static readonly Exception s_exception;

        static EnumCasting()
        {
            try
            {
                Type enumType = typeof(TEnum);
                if (!enumType.IsEnum)
                    throw new ArgumentException("Declaring type must be an enum", nameof(enumType));
                if (enumType.GetEnumUnderlyingType() != typeof(int))
                    throw new ArgumentException("Enum must be of type int.", nameof(enumType));

                s_toInt = MakeToInt();
                s_fromInt = MakeFromInt();
            }
            catch (Exception e)
            {
                s_exception = e;
                s_toInt = null;
                s_fromInt = null;
            }
        }

        private static Func<int, TEnum> MakeFromInt()
        {
            //Creates method
            //int GetValue(TEnum value)
            //{
            //   return (int)value;
            //}

            ParameterExpression parameters = Expression.Parameter(typeof(TEnum));
            var cast = Expression.Convert(parameters, typeof(int));
            return Expression.Lambda<Func<int, TEnum>>(cast, parameters).Compile();
        }

        private static Func<TEnum, int> MakeToInt()
        {
            //Creates method
            //TEnum GetValue(int value)
            //{
            //   return (TEnum)value;
            //}
            ParameterExpression parameters = Expression.Parameter(typeof(int));
            var cast = Expression.Convert(parameters, typeof(TEnum));
            return Expression.Lambda<Func<TEnum, int>>(cast, parameters).Compile();
        }

        public static Func<TEnum, int> ToInt
        {
            get
            {
                if (s_exception != null)
                    throw s_exception;
                return s_toInt;
            }
        }

        public static Func<int, TEnum> FromInt
        {
            get
            {
                if (s_exception != null)
                    throw s_exception;
                return s_fromInt;
            }
        }

    }
}


