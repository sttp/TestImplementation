
namespace CTP
{


    /// <summary>
    /// The lowest level command codes supported by the protocol. Nearly all commands exposed to higher level APIs will be in the form of a MarkupCommand.
    /// </summary>
    public enum CommandCode : byte
    {
        /// <summary>
        /// An invalid command to indicate that nothing is assigned.
        /// This cannot be sent over the wire.
        /// </summary>
        Invalid = 0x00,

        /// <summary>
        /// While most commands should be markup commands, it's necessary for some commands to be encoded in a raw binary format. 
        /// One example is a measurement stream. 
        /// </summary>
        Raw,

        /// <summary>
        /// All other commands fall under the classification of Document commands. These use the CtpDocument Object
        /// that has been optimized to exchange data in a binary format rather than strings. It also follows a more strict
        /// format than XML, YAML, or JSON. 
        /// 
        /// The benefit of Document Commands over raw binary streams is that all commands can be properly serialized, even if the command is not
        /// recognized. It also greatly simplifies the wire protocol level and keeps the lowest level from changing when additional commands are added in the future.
        /// 
        /// </summary>
        DocumentCommand,

    }
}