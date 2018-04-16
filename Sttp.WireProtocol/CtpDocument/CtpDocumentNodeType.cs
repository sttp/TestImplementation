namespace CTP
{
    /// <summary>
    /// The valid node types of <see cref="CtpDocument"/>
    /// </summary>
    public enum CtpDocumentNodeType
    {
        /// <summary>
        /// Indicates that the node type is an element.
        /// </summary>
        Element,
        /// <summary>
        /// Indicates that the node type if a value.
        /// </summary>
        Value,
        /// <summary>
        /// Specifies that the node type is the ending marker of a Element, Values don't have ending markers. Neither do root elements.
        /// </summary>
        EndElement,
        /// <summary>
        /// Specifies that the end of the document has occurred.
        /// </summary>
        EndOfDocument,
        /// <summary>
        /// Specifies that the current node is at the start of the document.
        /// </summary>
        StartOfDocument,
    }
}