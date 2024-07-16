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
        MemoryBuffer ms = new MemoryBuffer(4 + data.Length);

        ms.Position = 0;
        BinaryWriter writer = new BinaryWriter(ms);
        //uint msglen = (uint)(data.Length + 6);

        //writer.Write(BitConverter.GetBytes(Converter.GetBigEndian((uint)(msglen))), 0, 4);
        ////发送的包索引
        //writer.Write(BitConverter.GetBytes(Converter.GetBigEndian((ushort)sendMsgIndex)), 0, 2);
        //发送的协议ID
        writer.Write(BitConverter.GetBytes((int)opcode), 0, 4);
        //writer
        //writer.Write(BitConverter.GetBytes(Converter.GetBigEndian((ushort)0)), 0, 2);
        //协议数据
        writer.Write(data, 0, data.Length);

        writer.Flush();
        //复位以免后面用到
        ms.Position = 0;
        return ms;
	}

	public (int id, byte[] data) DeserializeFrom(MemoryBuffer memStream)
	{
        int total = memStream.GetBuffer().Length;
        BinaryReader reader = new BinaryReader(memStream);
        //byte[] totalLengthBytes = reader.ReadBytes(4);
        //int totalLength = Converter.GetBigEndian(BitConverter.ToInt32(totalLengthBytes, 0));
        //byte[] msgidexBytes = reader.ReadBytes(4);
        //int msgidex = Converter.GetBigEndian(BitConverter.ToInt32(msgidexBytes, 0));
        byte[] packetIdBytes = reader.ReadBytes(4);
        int packetId = BitConverter.ToInt32(packetIdBytes, 0);
        //byte[] aaBytes = reader.ReadBytes(4);
        //int aa = Converter.GetBigEndian(BitConverter.ToInt32(aaBytes, 0));
        byte[] packetArray = reader.ReadBytes(total - 4);

        return (packetId, packetArray);
	}
}