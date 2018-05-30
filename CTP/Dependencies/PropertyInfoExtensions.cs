using System;
using System.Linq.Expressions;
using System.Reflection;

namespace GSF.Reflection
{
    public static class PropertyInfoExtensions
    {
        private static class Nested
        {
            public static readonly SharedObjectCache<PropertyInfo, Action<object, object>> Setter1 = new SharedObjectCache<PropertyInfo, Action<object, object>>(CompileSetter);
            public static readonly SharedObjectCache<PropertyInfo, Func<object, object>> Getter1 = new SharedObjectCache<PropertyInfo, Func<object, object>>(CompileGetter);

            private static Action<object, object> CompileSetter(PropertyInfo field)
            {
                if (!field.DeclaringType.IsClass)
                    throw new ArgumentException("Declaring type must be a class to assign a field", "field");

                ParameterExpression targetObject = Expression.Parameter(typeof(object));
                UnaryExpression targetType = Expression.TypeAs(targetObject, field.DeclaringType);

                ParameterExpression valueObject = Expression.Parameter(typeof(object));
                UnaryExpression valueType = Expression.Convert(valueObject, field.PropertyType);

                Expression callSetterNotNull = Expression.Call(targetType, field.GetSetMethod(true), valueType);
                Expression callSetterNull = Expression.Call(targetType, field.GetSetMethod(true), Expression.Default(field.PropertyType));

                Expression checkValueIsNull = Expression.ReferenceEqual(valueObject, Expression.Constant(null));

                var assign = Expression.IfThenElse(checkValueIsNull, callSetterNull, callSetterNotNull);

                Action<object, object> setter = Expression.Lambda<Action<object, object>>(assign, targetObject, valueObject).Compile();
                return setter;

            }

            /// <summary>
            /// Creates a compiled getter for a field of a class.
            /// </summary>
            /// <param name="field"></param>
            /// <returns></returns>
            private static Func<object, object> CompileGetter(PropertyInfo field)
            {
                ParameterExpression targetObject = Expression.Parameter(typeof(object));
                UnaryExpression targetType = Expression.TypeAs(targetObject, field.DeclaringType);
                Expression callGetter = Expression.Call(targetType, field.GetGetMethod(true));

                Func<object, object> getter = Expression.Lambda<Func<object, object>>(Expression.Convert(callGetter, typeof(object)), targetObject).Compile();
                return getter;
            }
        }

        private static class Nested<T>
        {
            public static readonly SharedObjectCache<PropertyInfo, Action<object, T>> Setter1 = new SharedObjectCache<PropertyInfo, Action<object, T>>(CompileSetter);
            public static readonly SharedObjectCache<PropertyInfo, Func<object, T>> Getter1 = new SharedObjectCache<PropertyInfo, Func<object, T>>(CompileGetter);

            /// <summary>
            /// Creates a setter method that will set a field of a class. Note, does not work on structs since they are passed by value.
            /// </summary>
            /// <param name="fieldInfo"></param>
            /// <returns></returns>
            private static Action<object, T> CompileSetter(PropertyInfo field)
            {
                if (!field.DeclaringType.IsClass)
                    throw new ArgumentException("Declaring type must be a class to assign a field", "field");


                ParameterExpression targetObject = Expression.Parameter(typeof(object));
                UnaryExpression targetType = Expression.TypeAs(targetObject, field.DeclaringType);

                ParameterExpression valueObject = Expression.Parameter(typeof(T));

                Expression callSetterNotNull = Expression.Call(targetType, field.GetSetMethod(true), valueObject);

                Action<object, T> setter = Expression.Lambda<Action<object, T>>(callSetterNotNull, targetObject, valueObject).Compile();
                return setter;
            }

            /// <summary>
            /// Creates a compiled getter for a field of a class.
            /// </summary>
            /// <param name="field"></param>
            /// <returns></returns>
            public static Func<object, T> CompileGetter(PropertyInfo field)
            {
                ParameterExpression targetObject = Expression.Parameter(typeof(object));
                UnaryExpression targetType = Expression.TypeAs(targetObject, field.DeclaringType);
                Expression callGetter = Expression.Call(targetType, field.GetGetMethod(true));
                Func<object, T> getter = Expression.Lambda<Func<object, T>>(callGetter, targetObject).Compile();
                return getter;
            }
        }


        /// <summary>
        /// Creates a setter method that will set a field of a class. Note, does not work on structs since they are passed by value.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static Action<object, object> CompileSetter(this PropertyInfo field)
        {
            return Nested.Setter1.Get(field);
        }

        /// <summary>
        /// Creates a setter method that will set a field of a class. Note, does not work on structs since they are passed by value.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static Action<object, T> CompileSetter<T>(this PropertyInfo field)
        {
            return Nested<T>.Setter1.Get(field);

        }

        /// <summary>
        /// Creates a compiled getter for a field of a class or a struct.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static Func<object, object> CompileGetter(this PropertyInfo field)
        {
            return Nested.Getter1.Get(field);

        }

        /// <summary>
        /// Creates a compiled getter for a field of a class or a struct.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static Func<object, T> CompileGetter<T>(this PropertyInfo field)
        {
            return Nested<T>.Getter1.Get(field);

        }

    }
}