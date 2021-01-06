using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using System.Text;
using System.Threading;
using UnityEngine.Events;
using System.Collections.Generic;

class SocketManager : Socket
{
    public SocketManager(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType) : base(addressFamily, socketType, protocolType) { }

    private static SocketManager _socketManager;

    public static SocketManager Instance()
    {
        if (_socketManager == null)
        {
            _socketManager = new SocketManager(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        return _socketManager;
    }

    private static UnityEvent _socketEngine = new UnityEvent();
    public static void SocketEngineInvoke()
    {
        _socketEngine.Invoke();
        _socketEngine.RemoveAllListeners();
    }

    private class LinkInformation { public UnityAction onSuccess, onFail; }

    /// <summary>
    /// 连接socket
    /// </summary>
    public void Link()
    {
        LinkInformation linkInformation = new LinkInformation();
        _Link(linkInformation);
    }

    /// <summary>
    /// 连接socket
    /// </summary>
    /// <param name="onSuccess">连接成功回调</param>
    public void Link(UnityAction onSuccess)
    {
        LinkInformation linkInformation = new LinkInformation();
        linkInformation.onSuccess = onSuccess;
        _Link(linkInformation);
    }

    /// <summary>
    /// 连接socket
    /// </summary>
    /// <param name="onSuccess">连接成功回调</param>
    /// <param name="onFail">连接失败回调</param>
    public void Link(UnityAction onSuccess, UnityAction onFail)
    {
        LinkInformation linkInformation = new LinkInformation();
        linkInformation.onSuccess = onSuccess;
        linkInformation.onFail = onFail;
        _Link(linkInformation);
    }

    private Thread _linkThread;
    private void _Link(LinkInformation linkInfo)
    {
        if(_linkThread == null)
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

    private void _OnLink(object obj)
    {
        string targetIP = "192.168.108.43";
        IPEndPoint ip = new IPEndPoint(IPAddress.Parse(targetIP), 5555);

        Debug.Log("开始链接服务器:" + targetIP + ":5555");

        LinkInformation action = (LinkInformation)obj;
        try
        {
            _socketManager.Connect(ip);

            _socketEngine.AddListener(() => { action.onSuccess(); });

            Debug.Log("链接成功");
        }
        catch(Exception e)
        {
            _socketEngine.AddListener(() => { action.onFail(); });
            Debug.Log(e);
        }

        _linkThread = null;
    }

    //发送字符串UTF8
    public void SendString(string str)
    {
        _socketManager.Send(new UTF8Encoding().GetBytes(str));
    }

    private Thread _receiveThread;

    /// <summary>
    /// 监听socket传输的数据
    /// </summary>
    /// <param name="action">收到数据的回调</param>
    public void Receive(UnityAction<byte[]> action)
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

    private List<UnityAction<byte[]>> _receiveActionList = new List<UnityAction<byte[]>>();
    private void _ReceiveMessage()
    {
        Socket socket = _socketManager;
        while (true)
        {
            try
            {
                byte[] bs = new byte[1024];
                int length = socket.Receive(bs);
                //如果Receive返回值为0，则可以默认客户端已断开链接
                if (length > 0)
                {
                    _socketEngine.AddListener(()=> {
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
