namespace Sttp
{
    public enum SttpMarkupCompatiblity : byte
    {
        /// <summary>
        /// Indicates that this option must be interpreted. If the option is not recognized, the request must be denied. 
        /// This also requires that if the option is properly interpreted, it must be enforced, otherwise the request must be denied.
        /// </summary>
        KnownAndEnforced,
        /// <summary>
        /// Indicates that this option must be interpreted. If the option is not recognized, the request must be denied.
        /// However, the option can be ignored if desired, but only by an entity that recognizes the option.
        /// </summary>
        Known,
        /// <summary>
        /// Indicates that if the server does not recognized this item. It can be safely ignored.
        /// </summary>
        Unknown,
    }
}