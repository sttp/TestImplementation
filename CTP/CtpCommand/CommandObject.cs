using System;
using System.Dynamic;
using System.Linq;
using CTP.Collection;
using CTP.IO;
using CTP.Serialization;
using GSF.Collections;

namespace CTP
{
    public interface ICommandObjectOptionalMethods
    {
        /// <summary>
        /// When creating a new object from <see cref="CtpCommand"/>, this method is called first to allow the coder
        /// to define default values.
        /// </summary>
        void BeforeLoad();

        /// <summary>
        /// This occurs after a new object is loaded. This allows the coder to validate or finish the loading process.
        /// </summary>
        void AfterLoad();

        /// <summary>
        /// Occurs during loading when a value is present in the <see cref="CtpCommand"/> but
        /// there is not a corresponding field.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        void MissingValue(string name, CtpObject value);

        /// <summary>
        /// Occurs during loading when a element is present in the <see cref="CtpCommand"/> but
        /// there is not a corresponding field.
        /// </summary>
        /// <param name="name"></param>
        void MissingElement(string name);
    }

    /// <summary>
    /// The base class for all Object that can be automatically serialized to/from a <see cref="CtpCommand"/>.
    /// </summary>
    public abstract class CommandObject
    {
        //Ensures that users cannot directly inherit from this class.
        internal CommandObject()
        {

        }

        /// <summary>
        /// Gets the Schema associated with this command;
        /// </summary>
        public abstract CtpCommandSchema Schema { get; }

        /// <summary>
        /// The name that is associated with this command record. If this is a nested object, this value will be ignored.
        /// </summary>
        public abstract string CommandName { get; }

        /// <summary>
        /// Converts this object into a <see cref="CtpCommand"/>
        /// </summary>
        /// <returns></returns>
        public abstract CtpCommand ToCommand();

        internal abstract PooledBuffer ToDataCommandPacket(int schemeRuntimeID);

        /// <summary>
        /// Implicitly converts into a <see cref="CtpCommand"/>.
        /// </summary>
        /// <param name="obj"></param>
        public static explicit operator CtpCommand(CommandObject obj)
        {
            return obj.ToCommand();
        }

        /// <summary>
        /// A default <see cref="ToString"/> implementation that shows the YAML representation of the command
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToCommand().ToYAML();
        }
    }

    /// <summary>
    /// When creating a new command object, the <see cref="T"/> parameter must be the type of the class itself.
    /// The base class then compiles the necessary serialization methods.
    /// Note: The defining type must have a parameterless constructor (it may be private)
    /// </summary>
    /// <typeparam name="T">Must be the value returned by <see cref="Object.GetType"/></typeparam>
    public abstract class CommandObject<T>
        : CommandObject
        where T : CommandObject<T>
    {
        protected CommandObject()
        {
            if (LoadError != null)
                throw LoadError;
            if (typeof(T) != GetType())
                throw new ArgumentException("The supplied type must exactly match the generic type parameter");
        }

        /// <summary>
        /// The name that is associated with this command record. If this is a nested object, this value will be ignored.
        /// </summary>
        public sealed override string CommandName => CmdName;

        public sealed override CtpCommandSchema Schema => WriteSchema;

        internal sealed override PooledBuffer ToDataCommandPacket(int schemeRuntimeID)
        {
            T obj = this as T;
            if (LoadError != null)
                throw LoadError;


            var wr = ThreadStaticItems.CommandObject_Writer ?? new CtpObjectWriter();
            ThreadStaticItems.CommandObject_Writer = null;
            wr.Clear();
            IOMethods.Save(obj, wr);
            var buffer = PacketMethods.CreatePacket(PacketContents.CommandData, schemeRuntimeID, wr);
            ThreadStaticItems.CommandObject_Writer = wr;
            return buffer;
        }

        /// <summary>
        /// Converts this object into a <see cref="CtpCommand"/>
        /// </summary>
        /// <returns></returns>
        public sealed override CtpCommand ToCommand()
        {
            T obj = this as T;
            if (LoadError != null)
                throw LoadError;

            var wr = ThreadStaticItems.CommandObject_Writer ?? new CtpObjectWriter();
            ThreadStaticItems.CommandObject_Writer = null;
            wr.Clear();
            IOMethods.Save(obj, wr);
            var cmd = new CtpCommand(WriteSchema, wr.ToArray());
            ThreadStaticItems.CommandObject_Writer = wr;
            return cmd;

        }

        /// <summary>
        /// A method that is used by the defining class to support explicit type casting.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static T FromCommand(CtpCommand command)
        {
            if (LoadError != null)
                throw LoadError;
            if (CmdName != command.CommandName)
                throw new Exception("Document Mismatch");
            var rdr = command.MakeReader();
            rdr.Read();
            return IOMethods.Load(rdr);
        }

        private static readonly string CmdName;
        private static readonly Exception LoadError;
        private static readonly TypeIOMethodBase<T> IOMethods;
        private static readonly CtpCommandSchema WriteSchema;

        static CommandObject()
        {
            try
            {
                var type = typeof(T);
                var attribute = type.GetCustomAttributes(false).OfType<CommandNameAttribute>().FirstOrDefault();
                CmdName = attribute?.CommandName ?? type.Name;
                IOMethods = TypeIO.Create<T>(CmdName);
                var writer = new CommandSchemaWriter();
                IOMethods.WriteSchema(writer);
                WriteSchema = writer.ToSchema();
            }
            catch (Exception e)
            {
                LoadError = e;
            }
        }
    }

}
