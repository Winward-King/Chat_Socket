using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

namespace Network
{
    /// <summary>
    /// 服务端网络服务类
    /// </summary>
    public class ServerUdpNetworkService : MonoSingleton<ServerUdpNetworkService>
    {
        private UdpClient udpService;

        public event EventHandler<MessageArrivedEventArgs> MessageArrived;

        private Thread threadReceive;

        private List<IPEndPoint> allClientEP;

        public override void Init()
        {
            base.Init();
            allClientEP = new List<IPEndPoint>();
        }

        //创建Socket对象（由登录窗口传递服务端地址、端口）
        public void Initialized(string serverIP, int serverPort)
        {
            DontDestroyOnLoad(gameObject);
            //创建服务端终结点
            IPEndPoint serverEP = new IPEndPoint(IPAddress.Parse(serverIP), serverPort);
            udpService = new UdpClient(serverEP);

            threadReceive = new Thread(ReceiveChatMessage);
            threadReceive.Start();
        }
        //发送数据
        public void SendChatMessage(ChatMessage msg, IPEndPoint remote)
        {
            byte[] buffer = msg.ObjectToBytes();
            udpService.Send(buffer, buffer.Length, remote);
        }
        //接受数据
        private void ReceiveChatMessage()
        {
            while (true)
            {
                IPEndPoint remote = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = udpService.Receive(ref remote);
                ChatMessage msg = ChatMessage.BytesToObject(data);
                //根据消息类型、执行相关逻辑……
                OnMessageArried(msg, remote);
                //引发事件
                if (MessageArrived == null) continue;
                MessageArrivedEventArgs args = new MessageArrivedEventArgs()
                {
                    ArrivedTime = DateTime.Now,
                    Message = msg
                };
                //在主线程中引发事件
                ThreadCrossHelper.Instance.ExecuteOnMainThread(() =>
                {
                    MessageArrived(this, args);
                });
            }
        }

        //关闭释放资源
        private void OnApplicationQuit()
        {
            threadReceive.Abort();
            udpService.Close();
        }

        private void OnMessageArried(ChatMessage msg, IPEndPoint remote)
        {
            switch (msg.Type)
            {
                case MessageType.OnLine:
                    //添加客户端
                    allClientEP.Add(remote);
                    break;
                case MessageType.OffLine:
                    //移除客户端
                    allClientEP.Remove(remote);
                    break;
                case MessageType.General:
                    //转发
                    //foreach (var item in allClientEP)
                    //    SendChatMessage(msg, item);
                    allClientEP.ForEach(item => SendChatMessage(msg, item));
                    break;
            }
        }
    }
}