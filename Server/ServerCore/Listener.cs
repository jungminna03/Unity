using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    class Listener
    {
        Socket _socket;
        Func<Session> _sessionFactory;

        SocketAsyncEventArgs _acceptArgs;

        public void Init(IPEndPoint endPoint, Func<Session> sessionFactory)
        {
            _socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _sessionFactory = sessionFactory;

            _socket.Bind(endPoint);
            _socket.Listen(10);

            _acceptArgs = new SocketAsyncEventArgs();
            _acceptArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCompleted);

            RegisterAccept();
        }

        void RegisterAccept()
        {
            _acceptArgs.AcceptSocket = null;

            bool pending = _socket.AcceptAsync(_acceptArgs);
            if (!pending)
                OnAcceptCompleted(null, _acceptArgs);
        }

        void OnAcceptCompleted(object sender, SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.Success)
            {
                Session session = _sessionFactory.Invoke();
                session.Init(_socket);
                session.OnConnect(args.AcceptSocket.RemoteEndPoint);
            }
            else
            {
                Console.WriteLine(args.SocketError);
            }
        }
    }
}
