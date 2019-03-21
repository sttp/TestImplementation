using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTP
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CommandEventAttribute
    : Attribute
    {
        public readonly CommandEvents Events;

        public CommandEventAttribute(CommandEvents events)
        {
            Events = events;
        }
    }

    public enum CommandEvents
    {
        /// <summary>
        /// This is invoked before a load starts.
        /// void BeforeLoad();
        /// </summary>
        BeforeLoad,
        /// <summary>
        /// This is invoked after a load finishes.
        /// void AfterLoad();
        /// </summary>
        AfterLoad,
        /// <summary>
        /// This is invoked before a save starts.
        /// void BeforeSave();
        /// </summary>
        BeforeSave,
        /// <summary>
        /// This is invoked after a save starts.
        /// void AfterSave();
        /// </summary>
        AfterSave,
        /// <summary>
        /// This is invoked when there is a value missing in the command.
        /// void MissingValue(string name, CtpObject value);
        /// </summary>
        MissingValue,
        /// <summary>
        /// This is invoked when there is an element in the serialization but missing in the class.
        /// void MissingElement(string name);
        /// </summary>
        MissingElement,
    }
}
