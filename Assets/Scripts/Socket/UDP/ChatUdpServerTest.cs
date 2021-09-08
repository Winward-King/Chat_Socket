using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using ns;
using System.Threading;
using System.Text;
using UIWidgetsSamples;
using System;
using Common;

namespace ns
{
    /// <summary>
    /// 如何通过UDP协议接受消息
    /// </summary>
    public class ChatUdpServerTest : MonoBehaviour
    {

        public string ip;
        public int port;
        private UdpClient udpService;
        private ChatView chatView;
        private Thread threadReceive;
        //创建Scoket对象
        private void Start()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), port);
            udpService = new UdpClient(ep);

            //通过线程调度接受消息方法
            threadReceive = new Thread(ReceiveChatMessage);
            threadReceive.Start();

            chatView = GetComponentInChildren<ChatView>();
        }
        //接受消息

        private void ReceiveChatMessage()
        {
            while(true)
            {
                //任意地址  任意端口 
                IPEndPoint remote = new IPEndPoint(IPAddress.Any, 0);//0表示任意端口
                Debug.Log("收到消息前:" + remote.Address + "----" + remote.Port);
                //receive接受消息
                //--收到消息前，参数表示需要监听的终结点
                //--收到消息后，参数表示实际接受的终结点
                //--收到消息前，阻塞线程
                //--收到消息后，继续执行
                //--返回值 就是收到的消息
                byte[] data = udpService.Receive(ref remote);
                Debug.Log("收到消息后" + remote.Address + "----" + remote.Port);
                //byte[]-->解码--string
                string msg = Encoding.UTF8.GetString(data);

                //通过跨线程访问助手调用
                ThreadCrossHelper.Instance.ExecuteOnMainThread(() =>
                {
                    DisplayChatMessage(msg);
                });
            }
        }
        //显示数据
        private void DisplayChatMessage(string msg)
        {
            ChatLine line = new ChatLine();
            chatView.DataSource.Add(new ChatLine()
            {
                UserName ="qtx",
                Message = msg,
                Time = DateTime.Now,
                Type = ChatLineType.User,
            });
            chatView.DataSource.Add(line);
        }
        //释放资源
        private void OnApplicationQuit()
        {
            threadReceive.Abort();
            udpService.Close();
        }
    }
}