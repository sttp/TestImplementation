using System;
using System.Reflection;

namespace CTP.Serialization
{
    internal abstract class TypeSerializationMethodBase
    {
        protected internal TypeSerializationMethodBase()
        {

        }

        public abstract void SaveObject(object obj, CtpDocumentWriter writer);
        public abstract object LoadObject(CtpDocumentElement reader);


        /// <summary>
        /// The type that does the serialization.
        /// </summary>
        public abstract Type ObjectType { get; }

        public abstract CompiledSaveLoad CompiledSaveLoad(PropertyInfo property, CtpSerializeFieldAttribute autoLoad);
        public abstract CompiledSaveLoad CompiledSaveLoad(FieldInfo field, CtpSerializeFieldAttribute autoLoad);

        /// <summary>
        /// Due to circular dependencies, this method may need to be called before assigning nested dependencies.
        /// </summary>
        public virtual void InitializeSerializationMethod()
        {

        }
    }

    internal abstract class TypeSerializationMethodBase<T>
        : TypeSerializationMethodBase
    {

        public sealed override void SaveObject(object obj, CtpDocumentWriter writer)
        {
            Save((T)obj, writer);
        }

        public sealed override object LoadObject(CtpDocumentElement reader)
        {
            return Load(reader);
        }

        public abstract bool IsArrayType { get; }
        public abstract bool IsValueType { get; }

        public override Type ObjectType => typeof(T);

        public abstract T Load(CtpDocumentElement reader);

        public abstract T Load(CtpObject reader);

        public abstract CtpObject Save(T obj);

        public abstract void Save(T obj, CtpDocumentWriter writer);

        public override CompiledSaveLoad CompiledSaveLoad(PropertyInfo property, CtpSerializeFieldAttribute autoLoad)
        {
            return new CompiledSaveLoad<T>(this, property, autoLoad);
        }
        public override CompiledSaveLoad CompiledSaveLoad(FieldInfo field, CtpSerializeFieldAttribute autoLoad)
        {
            return new CompiledSaveLoad<T>(this, field, autoLoad);
        }
    }

    internal abstract class TypeSerializationMethodBase2<T>
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

        public override CompiledSaveLoad CompiledSaveLoad(PropertyInfo property, CtpSerializeFieldAttribute autoLoad)
        {
            return new CompiledSaveLoad<T>(this, property, autoLoad);
        }
        public override CompiledSaveLoad CompiledSaveLoad(FieldInfo field, CtpSerializeFieldAttribute autoLoad)
        {
            return new CompiledSaveLoad<T>(this, field, autoLoad);
        }
    }


}
