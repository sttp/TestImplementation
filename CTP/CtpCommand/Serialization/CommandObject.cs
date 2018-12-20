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

        public abstract CtpCommand ToCommand();

        public static implicit operator CtpCommand(CommandObject obj)
        {
            return obj.ToCommand();
        }

        public override string ToString()
        {
            return ToCommand().ToYAML();
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

        public override CtpCommand ToCommand()
        {
            return Save((T)this);
        }

        public static T FromDocument(CtpCommand obj)
        {
            return Load(obj);
        }

        public static CtpCommand Save(T obj)
        {
            var raw = obj as CtpRaw;
            if (raw != null)
            {
                return new CtpCommand(raw.Payload, raw.Channel);
            }
            if (LoadError != null)
                throw LoadError;
            var wr = new CtpCommandWriter();
            wr.Initialize(CommandName);
            Serialization.Save(obj, wr, null);
            return wr.ToCtpCommand();
        }

        public static T Load(CtpCommand command)
        {
            if (typeof(T) == typeof(CtpRaw))
            {
                return (T)(object)command.ToCtpRaw();
            }
            if (LoadError != null)
                throw LoadError;
            if (CommandName.Value != command.RootElement)
                throw new Exception("Document Mismatch");
            var rdr = command.MakeReader();
            return Serialization.Load(rdr);
        }

    }

}
