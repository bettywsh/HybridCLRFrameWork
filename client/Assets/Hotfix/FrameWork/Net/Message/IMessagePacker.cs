using System;
using System.IO;


public interface IMessagePacker
{
	byte[] SerializeTo(int opcode, byte[] data);
    void DeserializeFrom(MemoryStream stream, int packetLength);

}