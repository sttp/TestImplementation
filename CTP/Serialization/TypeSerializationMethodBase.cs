using System;
using System.Reflection;

namespace CTP.Serialization
{
    public abstract class TypeSerializationMethodBase
    {
        protected internal TypeSerializationMethodBase()
        {

        }

        /// <summary>
        /// The type that does the serialization.
        /// </summary>
        public abstract Type ObjectType { get; }

        public abstract CompiledSaveLoad CompiledSaveLoad(PropertyInfo property, CtpSerializeField autoLoad);
        public abstract CompiledSaveLoad CompiledSaveLoad(FieldInfo field, CtpSerializeField autoLoad);

        /// <summary>
        /// Due to circular dependencies, this method may need to be called before assigning nested dependencies.
        /// </summary>
        public virtual void InitializeSerializationMethod()
        {

        }
    }

    public abstract class TypeSerializationMethodBase<T>
        : TypeSerializationMethodBase
    {
        public abstract bool IsArrayType { get; }
        public abstract bool IsValueType { get; }

        public override Type ObjectType => typeof(T);

        public abstract T Load(CtpDocumentElement reader);

        public abstract T Load(CtpObject reader);

        public abstract CtpObject Save(T obj);

        public abstract void Save(T obj, CtpDocumentWriter writer);

        public override CompiledSaveLoad CompiledSaveLoad(PropertyInfo property, CtpSerializeField autoLoad)
        {
            return new CompiledSaveLoad<T>(this, property, autoLoad);
        }
        public override CompiledSaveLoad CompiledSaveLoad(FieldInfo field, CtpSerializeField autoLoad)
        {
            return new CompiledSaveLoad<T>(this, field, autoLoad);
        }
    }
    public abstract class TypeSerializationMethodBase2<T>
        : TypeSerializationMethodBase<T>
    {
        public sealed override bool IsValueType => true;
        public sealed override Type ObjectType => typeof(T);
        public sealed override bool IsArrayType => false;

        public sealed override T Load(CtpDocumentElement reader)
        {
            throw new NotSupportedException();
        }
    
        public sealed override void Save(T obj, CtpDocumentWriter writer)
        {

        }

        public override CompiledSaveLoad CompiledSaveLoad(PropertyInfo property, CtpSerializeField autoLoad)
        {
            return new CompiledSaveLoad<T>(this, property, autoLoad);
        }
        public override CompiledSaveLoad CompiledSaveLoad(FieldInfo field, CtpSerializeField autoLoad)
        {
            return new CompiledSaveLoad<T>(this, field, autoLoad);
        }
    }


}
