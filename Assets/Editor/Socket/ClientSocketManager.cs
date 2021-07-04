using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using System.Text;
using System.Threading;
using UnityEngine.Events;
using System.Collections.Generic;

namespace SocketTool
{
    class ClientSocketManager
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

        private static UnityEvent _socketEngine = new UnityEvent();
        public static void SocketEngineInvoke()
        {
            _socketEngine.Invoke();
            _socketEngine.RemoveAllListeners();
        }

        public class LinkInfo { public string ip; public int port; public UnityAction onSuccess, onFail; }

        /// <summary>
        /// 连接socket
        /// </summary>
        /// <param name="onSuccess">连接成功回调</param>
        /// <param name="onFail">连接失败回调</param>
        public static void Link(LinkInfo info)
        {
            _Link(info);
        }

        private static Thread _linkThread;
        private static void _Link(LinkInfo linkInfo)
        {
            if (_linkThread == null)
            {
                _linkThread = new Thread(_OnLink);
                _linkThread.IsBackground = true;
                _linkThread.Start(linkInfo);
            }
            else
            {
                Debug.Log("一个连接正在进行，请稍后");
            }
        }

        private static void _OnLink(object obj)
        {
            LinkInfo info = (LinkInfo)obj;

            IPEndPoint ip = new IPEndPoint(IPAddress.Parse(info.ip), info.port);

            Debug.Log("开始链接服务器:" + info.ip + ":" + info.port);

            try
            {
                socket.Connect(ip);

                _socketEngine.AddListener(() => { info.onSuccess(); });

                Debug.Log("链接成功");
            }
            catch (Exception e)
            {
                _socketEngine.AddListener(() => { info.onFail(); });
                Debug.Log(e);
            }

            _linkThread = null;
        }

        //发送字符串UTF8
        public static void SendString(string str)
        {
            socket.Send(new UTF8Encoding().GetBytes(str));
        }

        private static Thread _receiveThread;

        /// <summary>
        /// 监听socket传输的数据
        /// </summary>
        /// <param name="action">收到数据的回调</param>
        public static void Receive(UnityAction<byte[]> action)
        {
            _receiveActionList.Add(action);
            //开始接收消息
            if (_receiveThread == null)
            {
                _receiveThread = new Thread(_ReceiveMessage);
                _receiveThread.IsBackground = true;
                _receiveThread.Start();
            }
        }

        private static List<UnityAction<byte[]>> _receiveActionList = new List<UnityAction<byte[]>>();
        private static void _ReceiveMessage()
        {
            while (true)
            {
                try
                {
                    byte[] bs = new byte[1024];
                    int length = socket.Receive(bs);
                    //如果Receive返回值为0，则可以默认客户端已断开链接
                    if (length > 0)
                    {
                        _socketEngine.AddListener(() =>
                        {
                            _receiveActionList.ForEach(action => { action(bs); });
                        });
                    }
                    else
                    {
                        break;
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                    break;
                }
            }
        }
    }
}
