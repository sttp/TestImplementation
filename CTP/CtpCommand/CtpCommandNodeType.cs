namespace CTP
{
    /// <summary>
    /// The valid node types of <see cref="CtpCommand"/>
    /// </summary>
    internal enum CtpCommandNodeType : byte
    {
        /// <summary>
        /// Indicates that the node type is an element.
        /// </summary>
        StartElement,
        /// <summary>
        /// Indicates that the node type if a value.
        /// </summary>
        Value,
        /// <summary>
        /// Specifies that the node type is the ending marker of a Element, Values don't have ending markers.
        /// </summary>
        EndElement,
        /// <summary>
        /// Specifies that the end of the document has occurred.
        /// </summary>
        EndOfCommand,
        /// <summary>
        /// Specifies that the current node is at the start of the document. This value will never be serialized 
        /// to the stream, but rather is a placeholder during the reading process.
        /// </summary>
        StartOfCommand, 
    }
}