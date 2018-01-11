Sttp requires a transport protocol that has:
* Ordered Data Transport
* Retransmission of lost packets
* Error Checking
* Flow Control (Required for historical queries or metadata transfers.)

Therefore, a new transport layer must be defined if any of these cannot be provided by the underlying protocol. (eg: UDP)

--Lossy Transport. Unidirectional. 
The primary use case of a lossy transport layer is for unidirectional/multicast communications. Typically when UDP is required, it is for security reasons. 
Therefore, a bidirectional lossy transport layer will not be described in this protocol. If required, a hybird TCP/UDP is permitted. UDP is more susceptible to 
a DoS attack, therefore, bidirectional UDP is avoided to make it easier to avert DoS attacks.

In order to support UDP unidirectional encryption. This data will need to be exchanged by both partners:
 * byte[4] Key Identifier - This data exists in each encrypted packet and is used to identify which key was used to encrypt the data. 
 * byte[16] IV
 * byte[32] AES Key
 * byte[32] MAC Key

This data can either be exchanged over a secure TCP connection, or the data can be periodically sent in a special UDP packet.

In order to secure and authenticate the UDP channel, the public key of both systems must be exchanged. 

This layer MUST provide deduplication of packets. 
Dropped packets are acceptable. 
Reordering are also acceptable.

--Key Exchange Packet.

Mode: RSA Credentials - RSA Signature
  * (byte) Packet Type = 1
  * (int32) Epic ID
  * (int64) Valid After
  * (int64) Valid Before
  * (byte[32]) Nonce - This probably is not necessary
  * (int16) Length of encrypted block.
  * (byte[256]) a RSA 2048 with OAEP Padding encrypted block of the following data. (Using the Client's Public Key) 
	* byte[32] Nonce - This might also not be necessary since OAEP padding is used.
	* byte[16] IV
	* byte[32] AES Key
	* byte[64] MAC Key
	* int8 HMAC Truncation Length {4, 8, 12, 16, 32 }
  * (int16) Length the signature.
  * (byte[256]) SHA512-RSA digital signature of entire packet minus the signature length. (Using the server's private key).

--Data Packet

Each data packet will consist of:
 * (byte) Packet Type = 0
 * (int32) EpicID (the encryption method)
 * (int64) Sequence Number (for packet deduplication and replay attacks).
 * (int16) The encrypted data Length
 * Encrypted Data consisting of:  (Using AES-256, CBC, Padding: None)
 *  (byte) Length of HMAC (Note: Usually, the HMAC would be truncated and then some padding algorithm will extend the block size. However, this has been combined to increase the HMAC size)
 *	(byte[16..31]) Truncated HMAC. HMAC-SHA256 (Of the entire packet (before encryption) with 0's as placeholders for this field). HMAC will always be at least 16 bytes, the remainder up to 15 bytes are for padding.
 *	(byte[]) Data 


Each data packet will consist of:
 * (int8) Packet Type = 2
 * (int16) EpicID - Corresponds to a cipher that is periodically sent. 
        The client can only have 1 active cipher for a given EpicID, So the server could technically alternate between 2 numbers, 
		but additional length is provided to ensure that collisions are avoided during process restarts concurrent streams.
        Example Encoding: {DayOfMonth (5 bits) | HourOfDay (5 bits) | (6 bits, reserved for local sequencing)            
 * (int32) Sequence Number (for packet deduplication and replay attacks).
 * (byte[]) Encrypted Data (Using AES-256, CTR, Padding: None) CTR = {EpicID || Sequence Number || (int32)Block Index}
 * (byte[N]) Truncated HMAC. Determined by the cipher details for the given EPIC. HMAC-SHA256 (Of the entire packet after encryption). 






