using System;
using CTP.Serialization;

namespace CTP
{
    public abstract class DocumentObject
    {
        internal DocumentObject()
        {

        }

        public abstract CtpDocument ToDocument();

        public static implicit operator CtpDocument(DocumentObject obj)
        {
            return obj.ToDocument();
        }

        public override string ToString()
        {
            return ToDocument().ToYAML();
        }
    }

    public abstract class DocumentObject<T>
        : DocumentObject
        where T : DocumentObject<T>
    {
        private static readonly string CommandName;
        private static Exception LoadError;
        private static readonly TypeSerializationMethodBase<T> Serialization;

        static DocumentObject()
        {
            LoadError = DocumentSerializationHelper<T>.LoadError;
            if (LoadError == null)
            {
                Serialization = DocumentSerializationHelper<T>.Serialization;
                CommandName = DocumentSerializationHelper<T>.CommandAttribute?.DocumentName ?? nameof(T);
            }
        }

        protected DocumentObject()
        {
            if (LoadError != null)
                throw LoadError;
            if (typeof(T) != GetType())
                throw new ArgumentException("The supplied type must exactly match the generic type parameter");
        }

        public override CtpDocument ToDocument()
        {
            return Save((T)this);
        }

        public static T FromDocument(CtpDocument obj)
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
