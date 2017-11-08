namespace Sttp.WireProtocol.SubscribeResponse
{
    /// <summary>
    /// All of the permitted commands for metadata.
    /// </summary>
    public enum SubCommand : byte
    {
        /// <summary>
        /// An invalid command to indicate that nothing is assigned.
        /// This cannot be sent over the wire.
        /// </summary>
        Invalid = 0x00,


        /// <summary>
        /// The specified request failed. 
        /// 
        /// Payload: 
        /// string Reason      - A user friendly message for the failure, can be null.
        /// string Details     - A not so friendly message more helpful for troubleshooters.
        /// </summary>
        RequestFailed,


        /// <summary>
        /// The specified request has succeeded. 
        /// 
        /// Payload: 
        /// string Reason      - A user friendly message for the success, can be null.
        /// string Details     - A not so friendly message more helpful for troubleshooters.
        /// </summary>
        RequestSuccess,

    }
}