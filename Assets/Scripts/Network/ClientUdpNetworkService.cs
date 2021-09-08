using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using Common;
using System;
using System.Threading;

namespace Network//命名空间（一般格式 域名.项目名.模块）
{
    /// <summary>
    /// 客户端网络服务类
    /// </summary>
    public class ClientUdpNetworkService : MonoSingleton<ClientUdpNetworkService>
    {
        public UdpClient udpService;

        public event EventHandler<MessageArrivedEventArgs> MessageArrived;

        private Thread threadReceive;
        //创建Socket对象（由登录窗口传递服务端地址、端口）
        public void Initialized(string serverIP, int serverPort)
        {
            DontDestroyOnLoad(gameObject);
            //随机分配可以使用的端口
            udpService = new UdpClient();
            //与服务端建立连接(没有三次握手，仅仅配置自身Socket)
            //创建服务端终结点
            IPEndPoint serverEP = new IPEndPoint(IPAddress.Parse(serverIP), serverPort);
            udpService.Connect(serverEP);

            threadReceive = new Thread(ReceiveChatMessage);
            threadReceive.Start();

            //发送上线消息
            NotifyServer(MessageType.OnLine);
        }
        //通知服务端
        public void NotifyServer(MessageType type)
        {
            SendChatMessage(new ChatMessage() { Type = type });
        }

        //发送数据
        public void SendChatMessage(ChatMessage msg)
        {
            byte[] buffer = msg.ObjectToBytes();
            //备注：发送时不能指定终端(创建Socket对象时，建立了连接)
            int count = udpService.Send(buffer, buffer.Length);
            Debug.Log(count);
        }

        //接受数据
        private void ReceiveChatMessage()
        {
            while (true)
            {
                IPEndPoint remote = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = udpService.Receive(ref remote);
                ChatMessage msg = ChatMessage.BytesToObject(data);
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
            NotifyServer(MessageType.OffLine);
            threadReceive.Abort();
            udpService.Close();
        }
    }
}

