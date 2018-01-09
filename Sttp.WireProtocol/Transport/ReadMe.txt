Sttp requires a transport protocol that has:
* Ordered Data Transport
* Retransmission of lost packets
* Error Checking
* Flow Control (Required for historical queries or metadata transfers.)

Therefore, a new transport layer must be defined if any of these cannot be provided by the underlying protocol. (eg: UDP)

--Lossy Transport. Unidirectional. 
The primary use case of a lossy transport layer is for unidirectional/multicast communications. Typically when UDP is required, it is for security reasons. 
Therefore, a bidirectional lossy transport layer will not be described in this protocol. If required, a hybird TCP/UDP is permitted.

Note: This consists of 2 layers, the first layer is unencrypted. An additional layer will wrap this layer to encrypt the traffic.

* Wire Format
    * (Guid) Packet ID - A new ID for each distinct packet that is transmitted
	* -- This data needs to be moved to the wire protocol
	* (int16) Sequence - A sequence number indicating what packet of the sequence this is. 
	* (int16) Length - The total number of sequences of this packet.
	* 