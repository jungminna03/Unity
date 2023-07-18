using System;
using System.Collections.Generic;
using System.Text;
using System.Threading; 

namespace ServerCore
{
    public class SendBuffHelper
    {
        public static ThreadLocal<SendBuffer> CurrentBuff = new ThreadLocal<SendBuffer>(() => { return null; });

        public static int ChunkSize { get; set; } = 4096 * 100;

        public static ArraySegment<byte> Open(int reservSize)
        {
            if (CurrentBuff.Value == null)
                CurrentBuff.Value = new SendBuffer(ChunkSize);

            if (CurrentBuff.Value.FreeSize < reservSize)
                CurrentBuff.Value = new SendBuffer(ChunkSize);

            return CurrentBuff.Value.Open(reservSize);
        }

        public static ArraySegment<byte> Close(int usedSize)
        {
            return CurrentBuff.Value.Close(usedSize);
        }
    }

    public class SendBuffer
    {
        byte[] _buffer;
        int _usedSize = 0;

        public SendBuffer(int chunkSize)
        {
            _buffer = new byte[chunkSize];
        }

        public int FreeSize { get { return _buffer.Length - _usedSize; } }

        public ArraySegment<byte> Open(int reserveSize)
        {
            if (reserveSize > FreeSize)
                return null;

            return new ArraySegment<byte>(_buffer, _usedSize, reserveSize);
        }

        public ArraySegment<byte> Close(int usedSize)
        {
            ArraySegment<byte> segment = new ArraySegment<byte>(_buffer, _usedSize, usedSize);
            _usedSize += usedSize;
            return segment;
        }
    }
}
