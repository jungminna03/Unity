using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using ServerCore;

namespace Server
{
    abstract class Packet
    {
        public ushort size;
        public ushort packetId;

        public abstract ArraySegment<byte> Serialization();
        public abstract void Deserialization(ArraySegment<byte> buffer);

    }

    class PlayerInfoReq : Packet
    {
        public long playerId;
        public string name;
        public List<SkillInfo> skillList = new List<SkillInfo>();

        public struct SkillInfo
        {
            public int id;
            public short level;
            public long duration;

            public bool Serialize(Span<byte> s, ref ushort count)
            {
                bool success = true;

                success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), id);
                count += sizeof(int);
                success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), level);
                count += sizeof(short);
                success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), id);
                count += sizeof(long);

                return success;
            }

            public void Deserialize(ReadOnlySpan<byte> s, ref ushort count)
            {
                id = BitConverter.ToInt32(s.Slice(count, s.Length - count));
                count += sizeof(int);
                level = BitConverter.ToInt16(s.Slice(count, s.Length - count));
                count += sizeof(short);
                duration = BitConverter.ToInt32(s.Slice(count, s.Length - count));
                count += sizeof(long);
            }
        }

        public override void Deserialization(ArraySegment<byte> buffer)
        {
            ushort count = 0;

            ReadOnlySpan<byte> s = new ReadOnlySpan<byte>(buffer.Array, count, buffer.Count);

            //ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
            count += sizeof(ushort);
            //ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
            count += sizeof(ushort);

            playerId = BitConverter.ToInt32(s.Slice(count, s.Length - count));

            ushort nameLen = (ushort)BitConverter.ToInt16(s.Slice(count, s.Length - count));
            count += sizeof(ushort);
            name = Encoding.Unicode.GetString(s.Slice(count, nameLen));
            count += nameLen;

            ushort skillLen = (ushort)BitConverter.ToInt16(s.Slice(count, s.Length - count));
            count += sizeof(ushort);
            for (int i = 0; i < skillLen; ++i)
            {
                SkillInfo temp = new SkillInfo();
                temp.Deserialize(s, ref count);
                skillList.Add(temp);
            }
        }

        public override ArraySegment<byte> Serialization()
        {
            ArraySegment<byte> segment = SendBufferHelper.Open(4096);

            ushort count = 0;
            bool success = true;

            Span<byte> s = new Span<byte>(segment.Array, segment.Offset, segment.Count);

            packetId = (ushort)PacketID.PlayerInfoReq;

            count += sizeof(ushort);
            success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), packetId);
            count += sizeof(ushort);
            success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), playerId);

            ushort nameLen = (ushort)Encoding.Unicode.GetByteCount(name);
            success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), nameLen);
            count += sizeof(ushort);

            Array.Copy(Encoding.Unicode.GetBytes(name), 0, segment.Array, count, nameLen);
            count += nameLen;

            ushort skillLen = (ushort)skillList.Count;
            success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), skillLen);
            count += sizeof(ushort);
            foreach (SkillInfo skill in skillList)
            {
                skill.Serialize(s, ref count);
            }

            count += sizeof(long);
            success &= BitConverter.TryWriteBytes(s, count);

            if (success)
                return SendBufferHelper.Close(count);

            return null;
        }
    }

    public enum PacketID
    {
        PlayerInfoReq = 1,
        PlayerInfoOk = 2,
    }

    class ClientSession : PacketSession
	{
		public override void OnConnected(EndPoint endPoint)
		{
			Console.WriteLine($"OnConnected : {endPoint}");
			Thread.Sleep(5000);
			Disconnect();
		}

		public override void OnRecvPacket(ArraySegment<byte> buffer)
		{
			int pos = 0;

			ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset); 
			pos += 2;
			ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
			pos += 2;
	
				// TODO
			switch ((PacketID)id)
			{
				case PacketID.PlayerInfoReq:
					{
						PlayerInfoReq player = new PlayerInfoReq();
						player.Deserialization(buffer);
                        Console.WriteLine($"Player ID: {player.playerId}\nPlayer Name: {player.name}");

                        foreach(PlayerInfoReq.SkillInfo skill in player.skillList)
                        {
                            Console.WriteLine($"Skill: ({skill.id}), ({skill.level}), ({skill.duration})");
                        }
                        
                        pos += 8;
					}
					break;
				case PacketID.PlayerInfoOk:
					{
						int hp = BitConverter.ToInt32(buffer.Array, buffer.Offset + pos);
						pos += 4;
						int attack = BitConverter.ToInt32(buffer.Array, buffer.Offset + pos);
						pos += 4;
					}
					//Handle_PlayerInfoOk();
					break;
				default:
					break;
			}

			Console.WriteLine($"RecvPacketId: {id}, Size {size}");
		}

		// TEMP
		public void Handle_PlayerInfoOk(ArraySegment<byte> buffer)
		{

		}

		public override void OnDisconnected(EndPoint endPoint)
		{
			Console.WriteLine($"OnDisconnected : {endPoint}");
		}

		public override void OnSend(int numOfBytes)
		{
			Console.WriteLine($"Transferred bytes: {numOfBytes}");
		}
	}
}
