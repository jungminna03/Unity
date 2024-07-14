using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServerCore
{
    internal abstract class Session
    {
        Socket _socket;

        RecvBuffer _recvBuff = new RecvBuffer(1024);

        object _lock = new object();
        Queue<ArraySegment<byte>> _sendQueue = new Queue<ArraySegment<byte>>();
        List<ArraySegment<byte>> _pendingList = new List<ArraySegment<byte>>();
        SocketAsyncEventArgs _recvArgs = new SocketAsyncEventArgs();
        SocketAsyncEventArgs _sendArgs = new SocketAsyncEventArgs();

        abstract public void OnConnect(EndPoint endPoint);
        abstract public int  OnReceive(ArraySegment<byte> buffer);
        abstract public void OnSend(int numOfByte);
        abstract public void OnDisconnect(EndPoint endPoint);

        public void Init(Socket socket)
        {
            _socket = socket;

            _recvArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnRecvCompleted);
            _sendArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnSendCompleted);

            RegisterRecv();
        }

        public void Send(ArraySegment<byte> sendBuff)
        {
            lock (_lock)
            {
                _sendQueue.Enqueue(sendBuff);
                if (_pendingList.Count == 0)
                    RegisterSend();
            }
        }

        public void Disconnect()
        {
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
        }

        void RegisterRecv()
        {
            _recvBuff.Clean();
            ArraySegment<byte> segment = _recvBuff.DataSegment;
            _recvArgs.SetBuffer(segment);

            bool pending = _socket.ReceiveAsync(_recvArgs);
            if (pending == false)
                OnRecvCompleted(null, _recvArgs);
        }

        void OnRecvCompleted(object sender, SocketAsyncEventArgs args)
        {
            if (args.BytesTransferred > 0 && args.SocketError == SocketError.Success)
            {
                if (_recvBuff.OnWrite(args.BytesTransferred) == false)
                {
                    Disconnect();
                    return;
                }

                int processLen = OnReceive(_recvBuff.DataSegment);
                if (processLen < 0 || _recvBuff.DataSize < processLen)
                {
                    Disconnect();
                    return;
                }

                if (_recvBuff.OnRead(processLen) == false)
                {
                    Disconnect();
                    return;
                }

                RegisterRecv();
            }
            else
            {
                Console.WriteLine(args.SocketError);
                Disconnect();
            }
        }

        void RegisterSend()
        {
            while (_sendQueue.Count > 0)
            {
                _pendingList.Add(_sendQueue.Dequeue());
            }
            _sendArgs.BufferList = _pendingList;

            bool pending = _socket.SendAsync(_sendArgs);
            if (!pending)
                OnSendCompleted(null, _sendArgs);
        }

        void OnSendCompleted(object sender, SocketAsyncEventArgs args)
        {
            lock (_lock)
            {
                if (args.BytesTransferred > 0 && args.SocketError == SocketError.Success)
                {
                    _pendingList.Clear();
                    _sendArgs.BufferList = null;

                    OnSend(args.BytesTransferred);

                    if (_sendQueue.Count > 0)
                        RegisterSend();
                }
                else
                {
                    Console.WriteLine(args.SocketError);
                }
            }
        }
    }
}
