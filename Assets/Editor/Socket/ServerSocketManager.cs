using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace SocketTool
{
    public class ServerSocketManager
    {
        private static Socket _socket;
        public static Socket socket
        {
            get
            {
                if (_socket == null)
                {
                    _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                }
                return _socket;
            }
        }

        private static List<Socket> _clientSocket = new List<Socket>();

        /// <summary>
        /// 开启socket
        /// </summary>
        /// <returns></returns>
        public static void Open(string ipUrl, int port)
        {
            IPEndPoint ip = new IPEndPoint(IPAddress.Parse(ipUrl), port);
            socket.Bind(ip);

            // 设置最大连接数
            socket.Listen(10);

            Thread thread = new Thread(_Listen);
            thread.IsBackground = true;
            thread.Start(ip);
        }

        //监听客户端链接
        private static void _Listen(object ip)
        {
            Debug.Log(((IPEndPoint)ip).ToString() + " 开始监听");
            while (true)
            {
                //开始监听
                Socket acceptSocket = socket.Accept();
                Debug.Log(acceptSocket.RemoteEndPoint?.ToString() + " 客户端链接\n");
                //保存客户端
                _clientSocket.Add(acceptSocket);
                //开始接收消息
                Thread thread = new Thread(_ReceiveMessage);
                thread.IsBackground = true;
                thread.Start(acceptSocket);
            }
        }

        //监听客户端信息
        private static void _ReceiveMessage(object obj)
        {
            Socket socket = (Socket)obj;
            while (true)
            {
                try
                {
                    byte[] bs = new byte[1024];
                    int length = socket.Receive(bs);
                    //如果Receive返回值为0，则可以默认客户端已断开链接
                    if (length > 0)
                    {
                        //转发给其他连入的客户端
                        _Relay(bs);

                        Debug.Log(socket.RemoteEndPoint?.ToString() + ":" + new UTF8Encoding().GetString(bs) + "\n");

                        //Console.WriteLine(socket.RemoteEndPoint.ToString() + ":" + BitConverter.ToUInt32(bs) + "\n");
                    }
                    else
                    {
                        _ClientBreakOff(socket);

                        break;
                    }
                }
                catch
                {
                    _ClientBreakOff(socket);

                    break;
                }
            }
        }

        private static void _ClientBreakOff(Socket socket)
        {
            _clientSocket.Remove(socket);
            Debug.Log(socket.RemoteEndPoint?.ToString() + "客户端断开连接\n");
        }

        private static void _Relay(byte[] bs)
        {
            _clientSocket.ForEach(socket =>
            {
                socket.Send(bs);
            });
        }

        private static void Log(string str)
        {
            Debug.Log(str);
        }
    }
}
