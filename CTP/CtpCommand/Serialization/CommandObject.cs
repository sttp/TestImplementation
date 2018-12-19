using System;
using CTP.Serialization;
using GSF.Collections;

namespace CTP
{
    public abstract class CommandObject
    {
        internal CommandObject()
        {

        }

        public abstract CtpCommand ToDocument();

        public static implicit operator CtpCommand(CommandObject obj)
        {
            return obj.ToDocument();
        }

        public override string ToString()
        {
            return ToDocument().ToYAML();
        }

        public virtual void BeforeLoad()
        {

        }

        public virtual void AfterLoad()
        {

        }

        internal virtual void MissingValue(string name, CtpObject value)
        {
            throw new NotSupportedException();
        }

        internal virtual void MissingElement(string name)
        {
            throw new NotSupportedException();
        }
    }

    public abstract class CommandObject<T>
        : CommandObject
        where T : CommandObject<T>
    {
        private static readonly CtpCommandKeyword CommandName;
        private static readonly Exception LoadError;
        private static readonly TypeSerializationMethodBase<T> Serialization;

        static CommandObject()
        {
            LoadError = TypeSerialization<T>.LoadError;
            if (LoadError == null)
            {
                Serialization = TypeSerialization<T>.Serialization;
                CommandName = CtpCommandKeyword.Create(TypeSerialization<T>.CommandAttribute?.DocumentName ?? nameof(T));
            }
        }

        protected CommandObject()
        {
            if (LoadError != null)
                throw LoadError;
            if (typeof(T) != GetType())
                throw new ArgumentException("The supplied type must exactly match the generic type parameter");
        }

        public override CtpCommand ToDocument()
        {
            return Save((T)this);
        }

        public static T FromDocument(CtpCommand obj)
        {
            return Load(obj);
        }

        public static CtpCommand Save(T obj)
        {
            if (LoadError != null)
                throw LoadError;
            var wr = new CtpCommandWriter();
            wr.Initialize(CommandName);
            Serialization.Save(obj, wr, null);
            return wr.ToCtpDocument();
        }

        public static T Load(CtpCommand command)
        {
            if (LoadError != null)
                throw LoadError;
            if (CommandName.Value != command.RootElement)
                throw new Exception("Document Mismatch");
            var rdr = command.MakeReader();
            return Serialization.Load(rdr);
        }

    }

}
