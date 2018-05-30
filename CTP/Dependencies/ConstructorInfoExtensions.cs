using System;
using System.Linq.Expressions;
using System.Reflection;

namespace GSF.Reflection
{
    public static class ConstructorInfoExtensions
    {
        private static class Nested<R>
        {
            public static readonly SharedObjectCache<ConstructorInfo, Func<R>> Constructor = new SharedObjectCache<ConstructorInfo, Func<R>>(Compile);
            private static Func<R> Compile(ConstructorInfo constructor)
            {
                //Creates the following method:
                //R MakeNew()
                //{
                //   Return new R();  
                //}

                var makeNew = Expression.New(constructor);
                return Expression.Lambda<Func<R>>(makeNew).Compile();
            }
        }

        private static class Nested<T1, R>
        {
            public static readonly SharedObjectCache<ConstructorInfo, Func<T1, R>> Constructor = new SharedObjectCache<ConstructorInfo, Func<T1, R>>(Compile);
            private static Func<T1, R> Compile(ConstructorInfo constructor)
            {
                //Creates the following method:
                //R MakeNew(T1 param1)
                //{
                //   Return new R(param1);  
                //}

                ParameterExpression param1 = Expression.Parameter(typeof(T1));

                var makeNew = Expression.New(constructor, param1);

                return Expression.Lambda<Func<T1, R>>(makeNew, param1).Compile();
            }
        }
        private static class Nested<T1, T2, R>
        {
            public static readonly SharedObjectCache<ConstructorInfo, Func<T1, T2, R>> Constructor = new SharedObjectCache<ConstructorInfo, Func<T1, T2, R>>(Compile);
            private static Func<T1, T2, R> Compile(ConstructorInfo constructor)
            {
                //Creates the following method:
                //R MakeNew(T1 param1, T2 param2)
                //{
                //   Return new R(param1, param2);  
                //}

                ParameterExpression param1 = Expression.Parameter(typeof(T1));
                ParameterExpression param2 = Expression.Parameter(typeof(T2));

                var makeNew = Expression.New(constructor, param1, param2);

                return Expression.Lambda<Func<T1, T2, R>>(makeNew, param1, param2).Compile();
            }
        }

        private static class Nested<T1, T2, T3, R>
        {
            public static readonly SharedObjectCache<ConstructorInfo, Func<T1, T2, T3, R>> Constructor = new SharedObjectCache<ConstructorInfo, Func<T1, T2, T3, R>>(Compile);
            private static Func<T1, T2, T3, R> Compile(ConstructorInfo constructor)
            {
                //Creates the following method:
                //R MakeNew(T1 param1, T2 param2, T3 param3)
                //{
                //   Return new R(param1, param2, param3);  
                //}

                ParameterExpression param1 = Expression.Parameter(typeof(T1));
                ParameterExpression param2 = Expression.Parameter(typeof(T2));
                ParameterExpression param3 = Expression.Parameter(typeof(T3));

                var makeNew = Expression.New(constructor, param1, param2, param3);

                return Expression.Lambda<Func<T1, T2, T3, R>>(makeNew, param1, param2, param3).Compile();
            }
        }

        public static Func<R> Compile<R>(this ConstructorInfo constructor)
        {
            return Nested<R>.Constructor.Get(constructor);
        }

        public static Func<T1, R> Compile<T1, R>(this ConstructorInfo constructor)
        {
            return Nested<T1, R>.Constructor.Get(constructor);
        }

        public static Func<T1, T2, R> Compile<T1, T2, R>(this ConstructorInfo constructor)
        {
            return Nested<T1, T2, R>.Constructor.Get(constructor);
        }

        public static Func<T1, T2, T3, R> Compile<T1, T2, T3, R>(this ConstructorInfo constructor)
        {
            return Nested<T1, T2, T3, R>.Constructor.Get(constructor);

        }
    }
}