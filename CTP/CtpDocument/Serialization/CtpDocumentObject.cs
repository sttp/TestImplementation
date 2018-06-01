using System;
using CTP.Serialization;

namespace CTP
{
    public abstract class CtpDocumentObject
    {
        internal CtpDocumentObject()
        {

        }

        protected abstract CtpDocument ToDocument();

        public static implicit operator CtpDocument(CtpDocumentObject obj)
        {
            return obj.ToDocument();
        }

        public override string ToString()
        {
            return ToDocument().ToYAML();
        }
    }

    public abstract class CtpDocumentObject<T>
        : CtpDocumentObject
        where T : CtpDocumentObject<T>
    {
        private static readonly string CommandName;
        private static Exception LoadError;
        private static readonly TypeSerializationMethodBase<T> Serialization;

        static CtpDocumentObject()
        {
            Serialization = DocumentSerializationHelper<T>.Serialization;
            LoadError = DocumentSerializationHelper<T>.LoadError;
        }

        protected CtpDocumentObject()
        {
            if (typeof(T) != GetType())
            {
                throw new ArgumentException("The supplied type must exactly match the generic type parameter");
            }
        }

        protected override CtpDocument ToDocument()
        {
            return Save((T)this);
        }

        public static T ConvertFromDocument(CtpDocument obj)
        {
            return Load(obj);
        }

        public static CtpDocument Save(T obj)
        {
            if (LoadError != null)
                throw LoadError;
            var wr = new CtpDocumentWriter(CommandName);
            Serialization.Save(obj, wr);
            return wr.ToCtpDocument();
        }

        public static T Load(CtpDocument document)
        {
            if (LoadError != null)
                throw LoadError;
            if (CommandName != document.RootElement)
                throw new Exception("Document Mismatch");

            return Serialization.Load(document.MakeReader().ReadEntireElement());
        }

    }
   
}
