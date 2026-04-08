using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;

public class ProtobufPacker
{
	private int sendMsgIndex = 0;
	public MemoryBuffer SerializeTo(long opcode, byte[] data)
	{
		sendMsgIndex += 1;
        MemoryBuffer ms = new MemoryBuffer(data.Length + 4);

        ms.Position = 0;
        BinaryWriter writer = new BinaryWriter(ms);

        writer.Write(BitConverter.GetBytes((short)(data.Length + 2)), 0, 2);
        ////发送的包索引
        //writer.Write(BitConverter.GetBytes(Converter.GetBigEndian((ushort)sendMsgIndex)), 0, 2);
        //发送的协议ID
        writer.Write(BitConverter.GetBytes((short)opcode), 0, 2);
        //writer
        //writer.Write(BitConverter.GetBytes(Converter.GetBigEndian((ushort)0)), 0, 2);
        //协议数据
        writer.Write(data, 0, data.Length);

        writer.Flush();
        ////复位以免后面用到
        ms.Position = 0;
        return ms;
	}

	public (int id, byte[] data) DeserializeFrom(MemoryBuffer memStream)
	{
        int total = memStream.GetBuffer().Length;
        BinaryReader reader = new BinaryReader(memStream);
        //byte[] totalLengthBytes = reader.ReadBytes(2);
        //int totalLength = BitConverter.ToInt16(totalLengthBytes, 0);
        //byte[] msgidexBytes = reader.ReadBytes(4);
        //int msgidex = Converter.GetBigEndian(BitConverter.ToInt32(msgidexBytes, 0));
        byte[] packetIdBytes = reader.ReadBytes(2);
        int packetId = BitConverter.ToInt16(packetIdBytes, 0);
        //byte[] aaBytes = reader.ReadBytes(4);
        //int aa = Converter.GetBigEndian(BitConverter.ToInt32(aaBytes, 0));
        byte[] packetArray = reader.ReadBytes(total - 2);

        return (packetId, packetArray);
	}
}