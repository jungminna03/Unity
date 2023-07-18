using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    internal class RecvBuffer
    {
        ArraySegment<byte> _buffer;
        int _readPos = 0;
        int _writePos = 0;

        public RecvBuffer(int bufferSize)
        {
            _buffer = new ArraySegment<byte>(new byte[bufferSize], 0, bufferSize);
        }

        public int DataSize { get { return _writePos - _readPos; } }
        public int FreeSize { get { return _buffer.Count - _writePos; } }

        public ArraySegment<byte> DataSegment 
        { 
            get { return new ArraySegment<byte>(_buffer.Array, _buffer.Offset + _readPos, DataSize); } 
        }
        public ArraySegment<byte> ReadSegment
        { 
            get { return new ArraySegment<byte>(_buffer.Array, _buffer.Offset + _writePos, FreeSize); } 
        }

        public void Clean()
        {
            if (_readPos == _writePos)
                _readPos = _writePos = 0;
            else
            {
                Array.Copy(_buffer.Array, _buffer.Offset + _readPos, _buffer.Array, _buffer.Offset, DataSize);
                _readPos = 0;
                _writePos = DataSize;
            }
                
        }

        public bool OnRead(int numOfSize)
        {
            if (numOfSize > DataSize)
                return false;

            _readPos += numOfSize;
            return true;
        }

        public bool OnWrite(int numOfSize)
        {
            if (numOfSize > FreeSize)
                return false;

            _writePos += numOfSize;
            return true;
        }
    }
}
