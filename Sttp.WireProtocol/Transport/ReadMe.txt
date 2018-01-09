Sttp requires a transport protocol that has:
* Ordered Data Transport
* Retransmission of lost packets
* Error Checking
* Flow Control (Required for historical queries or metadata transfers.)

Therefore, a new transport layer must be defined if any of these cannot be provided by the underlying protocol. (eg: UDP)

--Lossy Transport. Unidirectional. 
The primary use case of a lossy transport layer is for unidirectional/multicast communications. Typically when UDP is required, it is for security reasons. 
Therefore, a bidirectional lossy transport layer will not be described in this protocol. If required, a hybird TCP/UDP is permitted.

In order to support UDP unidirectional encryption. This data will need to be exchanged by both partners:
 * byte[4] Key Identifier - This data exists in each encrypted packet and is used to identify which key was used to encrypt the data. 
 * byte[16] IV
 * byte[32] AES Key
 * byte[32] MAC Key

This data can either be exchanged over a secure TCP connection, or the data can be periodically sent in a special UDP packet.

In order to secure and authenticate the UDP channel, the public key of both systems must be exchanged. 

The supported certificate methods exist:
 * Client-RSA, Server-RSA
 * Client-RSA, Server-ECDSA
 * Client-ECDH, Server-ECDSA

Mode: RSA-RSA
  * (byte) Packet Type = 1
  * (int32) Epic ID
  * (int64) Creation Time
  * (int64) Expire Time
  * (byte[16]) Nonce
  * (int16) Length of encrypted block.
  * (byte[256]) a RSA 2048 encrypted packet of the following data. (Using the Client's Public Key)
	* byte[16] A different Nonce
	* byte[16] IV
	* byte[32] AES Key
	* byte[32] MAC Key
  * (int16) Length the signature.
  * (byte[256]) RSA digital signature of packet. (Using the server's private key)

Mode: RSA-ECDSA
  * (byte) Packet Type = 2
  * (int32) Epic ID
  * (int64) Creation Time
  * (int64) Expire Time
  * (byte[16]) Nonce
  * (int16) Length of encrypted block.
  * (byte[256]) a RSA 2048 encrypted packet of the following data. (Using the Client's Public Key)
	* byte[16] A different Nonce
	* byte[16] IV
	* byte[32] AES Key
	* byte[32] MAC Key
  * (int16) Length the signature.
  * (byte[64]) ECDSA digital signature of packet. (Using the server's private key)

Mode: ECDH-ECDSA
  * (byte) Packet Type = 3
  * (int32) Epic ID
  * (int64) Creation Time
  * (int64) Expire Time
  * (byte[16]) Nonce
  * (int16) Length of encrypted block.
  * (byte[96]) AES256 encryption CBC using the Nonce above for the IV. The key is the derived key from the ECDH.
	* byte[16] A different Nonce
	* byte[16] IV
	* byte[32] AES Key
	* byte[32] MAC Key
  * (int16) Length the signature.
  * (byte[64]) ECDSA digital signature of packet. (Using the server's private key)


--Data Packet

Each data packet will consist of:
 * (byte) Packet Type = 0
 * (int32) EpicID (the encryption method)
 * (int16) The encrypted data
 * (byte[]) Data (Using AES-256, CBC, Padding: PKCS7)
 * (byte[16]) Truncated HMAC. HMAC-SHA384 (Of the entire packet)






