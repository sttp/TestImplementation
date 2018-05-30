using System;
using System.Linq.Expressions;
using System.Reflection;

namespace GSF.Reflection
{
    public static class FieldInfoExtensions
    {
        private static class Nested
        {
            public static readonly SharedObjectCache<FieldInfo, Action<object, object>> Setter1 = new SharedObjectCache<FieldInfo, Action<object, object>>(CompileSetter);
            public static readonly SharedObjectCache<FieldInfo, Func<object, object>> Getter1 = new SharedObjectCache<FieldInfo, Func<object, object>>(CompileGetter);

            private static Action<object, object> CompileSetter(FieldInfo fieldInfo)
            {
                //Creates method if field is a struct since null cannot be assigned to a struct.
                //void SetValue(object obj, object value)
                //{
                //   if (value == null)
                //      (class)obj.Item = default(type);
                //   else
                //      (class)obj.Item = (type)value;
                //}

                //Creates method if field is a class
                //void SetValue(object obj, object value)
                //{
                //   (class)obj.Item = (type)value;
                //}


                if (!fieldInfo.DeclaringType.IsClass)
                    throw new ArgumentException("Declaring type must be a class to assign a field", "fieldInfo");
                if (fieldInfo.IsInitOnly)
                    throw new ArgumentException("Field cannot be marked as readonly", nameof(fieldInfo));

                //With 2 parameters of Object
                ParameterExpression paramTargetObject = Expression.Parameter(typeof(object));
                ParameterExpression paramValueObject = Expression.Parameter(typeof(object));

                UnaryExpression paramTarget = Expression.TypeAs(paramTargetObject, fieldInfo.DeclaringType);
                Expression paramValue = Expression.Convert(paramValueObject, fieldInfo.FieldType);

                MemberExpression fieldAssignment = Expression.Field(paramTarget, fieldInfo);
                BinaryExpression assignNotNull = Expression.Assign(fieldAssignment, paramValue);
                BinaryExpression assignNull = Expression.Assign(fieldAssignment, Expression.Default(fieldInfo.FieldType));

                Expression equalsNull = Expression.ReferenceEqual(paramValueObject, Expression.Constant(null));

                var assign = Expression.IfThenElse(equalsNull, assignNull, assignNotNull);

                if (fieldInfo.FieldType.IsValueType)
                {
                    return Expression.Lambda<Action<object, object>>(assign, paramTargetObject, paramValueObject).Compile();
                }
                else
                {
                    return Expression.Lambda<Action<object, object>>(assignNotNull, paramTargetObject, paramValueObject).Compile();
                }
            }

            /// <summary>
            /// Creates a compiled getter for a field of a class.
            /// </summary>
            /// <param name="field"></param>
            /// <returns></returns>
            private static Func<object, object> CompileGetter(FieldInfo field)
            {
                //Creates method
                //object GetValue(object obj)
                //{
                //   return (object)((class)obj).Item;
                //}
                ParameterExpression paramTargetObject = Expression.Parameter(typeof(object));
                UnaryExpression paramTarget = Expression.TypeAs(paramTargetObject, field.DeclaringType);

                MemberExpression fieldRead = Expression.Field(paramTarget, field);
                var castToObject = Expression.Convert(fieldRead, typeof(object));
                Func<object, object> getter = Expression.Lambda<Func<object, object>>(castToObject, paramTargetObject).Compile();
                return getter;
            }
        }

        private static class Nested<T>
        {
            public static readonly SharedObjectCache<FieldInfo, Action<object, T>> Setter1 = new SharedObjectCache<FieldInfo, Action<object, T>>(CompileSetter);
            public static readonly SharedObjectCache<FieldInfo, Func<object, T>> Getter1 = new SharedObjectCache<FieldInfo, Func<object, T>>(CompileGetter);

            /// <summary>
            /// Creates a setter method that will set a field of a class. Note, does not work on structs since they are passed by value.
            /// </summary>
            /// <param name="fieldInfo"></param>
            /// <returns></returns>
            private static Action<object, T> CompileSetter(FieldInfo fieldInfo)
            {
                if (!fieldInfo.DeclaringType.IsClass)
                    throw new ArgumentException("Declaring type must be a class to assign a field", nameof(fieldInfo));
                if (fieldInfo.IsInitOnly)
                    throw new ArgumentException("Field cannot be marked as readonly", nameof(fieldInfo));

                //Creates method
                //void SetValue(object obj, object value)
                //{
                //   (class)obj.Item = (type)value;
                //}

                ParameterExpression paramTargetObject = Expression.Parameter(typeof(object));
                ParameterExpression paramValue = Expression.Parameter(typeof(T));

                UnaryExpression paramTarget = Expression.TypeAs(paramTargetObject, fieldInfo.DeclaringType);
                MemberExpression fieldToAssign = Expression.Field(paramTarget, fieldInfo);
                BinaryExpression assignment = Expression.Assign(fieldToAssign, paramValue);

                Action<object, T> setter = Expression.Lambda<Action<object, T>>(assignment, paramTargetObject, paramValue).Compile();
                return setter;
            }

            /// <summary>
            /// Creates a compiled getter for a field of a class.
            /// </summary>
            /// <param name="field"></param>
            /// <returns></returns>
            public static Func<object, T> CompileGetter(FieldInfo field)
            {
                //Creates method
                //T GetValue(object obj)
                //{
                //   return (T)((class)obj).Item;
                //}

                ParameterExpression paramTargetObject = Expression.Parameter(typeof(object));
                UnaryExpression paramTarget = Expression.TypeAs(paramTargetObject, field.DeclaringType);
                MemberExpression fieldToGet = Expression.Field(paramTarget, field);
                var castToT = Expression.Convert(fieldToGet, typeof(T));

                Func<object, T> getter = Expression.Lambda<Func<object, T>>(castToT, paramTargetObject).Compile();
                return getter;
            }
        }

        public static Action<object, object> CompileSetter(this FieldInfo fieldInfo)
        {
            return Nested.Setter1.Get(fieldInfo);
        }
        /// <summary>
        /// Creates a compiled getter for a field of a class.
        /// </summary>
        /// <param name="fieldInfo"></param>
        /// <returns></returns>
        public static Func<object, object> CompileGetter(this FieldInfo fieldInfo)
        {
            return Nested.Getter1.Get(fieldInfo);
        }

        /// <summary>
        /// Creates a setter method that will set a field of a class. Note, does not work on structs since they are passed by value.
        /// </summary>
        /// <param name="fieldInfo"></param>
        /// <returns></returns>
        public static Action<object, T> CompileSetter<T>(this FieldInfo fieldInfo)
        {
            return Nested<T>.Setter1.Get(fieldInfo);
        }

        /// <summary>
        /// Creates a compiled getter for a field of a class.
        /// </summary>
        /// <param name="fieldInfo"></param>
        /// <returns></returns>
        public static Func<object, T> CompileGetter<T>(this FieldInfo fieldInfo)
        {
            return Nested<T>.Getter1.Get(fieldInfo);
        }

    }
}


