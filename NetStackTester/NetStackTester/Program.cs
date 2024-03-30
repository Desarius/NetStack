


// Create a BitBuffer

using System.Numerics;
using NetStack.Quantization;
using NetStack.Serialization;

BitBuffer bitBuffer = new BitBuffer();

// Add data to the BitBuffer
bitBuffer.AddInt(playerId);

// Quantize the player's position
QuantizedVector3 quantizedPosition = BoundedRange.Quantize(playerPosition, boundedRange);

// Add the quantized position to the BitBuffer
bitBuffer.AddUInt(quantizedPosition.x);
bitBuffer.AddUInt(quantizedPosition.y);
bitBuffer.AddUInt(quantizedPosition.z);

// Get the data from the BitBuffer as a byte array
// This is the data that will be sent over the network
byte[] data = bitBuffer.ToArray();

// Send the data over the network
networkManager.Send(data);

// On the receiving end, create a new BitBuffer with the received data
BitBuffer receivedBuffer = new BitBuffer(data);

// Read the player ID and position from the BitBuffer
int receivedPlayerId = receivedBuffer.ReadInt();
float receivedPlayerPosX = receivedBuffer.ReadUInt();
float receivedPlayerPosY = receivedBuffer.ReadUInt();
float receivedPlayerPosZ = receivedBuffer.ReadUInt();

// Dequantize the received position
Vector3 receivedPosition = BoundedRange.Dequantize(receivedQuantizedPosition, boundedRange);

// Update the player's position
UpdatePlayerPosition(receivedPlayerId, receivedPosition);