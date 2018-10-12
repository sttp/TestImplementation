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

        public static Func<R> Compile<R>(this ConstructorInfo constructor)
        {
            return Nested<R>.Constructor.Get(constructor);
        }
    }
}