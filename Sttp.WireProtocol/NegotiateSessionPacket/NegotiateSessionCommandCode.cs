
namespace Sttp.WireProtocol
{
    public enum NegotiateSessionCommandCode
    {
        /// <summary>
        /// Changes the default instance name for subscribing from.
        /// Payload:
        /// string InstanceName
        /// </summary>
        ChangeInstance,

        /// <summary>
        /// Gets all of the named instances for this publisher.
        /// </summary>
        GetAllInstances,

        /// <summary>
        /// The name of each instance.
        /// 
        /// int NumberOfInstances
        /// {
        ///     String Name,
        ///     String Description
        /// }
        /// </summary>
        InstanceList,
        
    }
}