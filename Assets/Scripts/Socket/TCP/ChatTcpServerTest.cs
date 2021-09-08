using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UIWidgetsSamples;
using UnityEngine;

namespace ns
{
    /// <summary>
    /// 基于TCP协议接收数据
    /// </summary>
    public class ChatTcpServerTest : MonoBehaviour 
	{
        public string ip;
        public int port;
        private TcpListener listener;
        private ChatView chatView;
        //创建监听器(Socket对象)
        private void Start()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), port);
            listener = new TcpListener(ep);
            listener.Start();
            Thread receiveThread = new Thread(ReceiveChatMessage);
            receiveThread.Start();


            chatView = GetComponentInChildren<ChatView>();
        }

        //接收消息
        private void ReceiveChatMessage()
        {
            //接受客户端 AcceptTcpClient 
            //--如果没有监听到客户端连接，则阻塞线程。
            //--如果监听到客户端连接，则继续执行。
            TcpClient client = listener.AcceptTcpClient();
            using (NetworkStream stream = client.GetStream())
            {
                byte[] buffer = new byte[512];
                int count;
                //如果读取到数据，则解析/显示。
                while ((count = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    //读取数据 Read 
                    //--如果没有读取到数据，则阻塞线程。
                    //  如果读取到数据，则继续执行。
                    //-- 返回值表示实际读取到的字节数,如果为零表示客户端下线。
                    //-- 如果需要监听多个客户端连接，则需要开启线程。 
                    string msg = Encoding.Unicode.GetString(buffer, 0, count);
                    if (msg == "Quit") break;
                    ThreadCrossHelper.Instance.ExecuteOnMainThread(() =>
                    {
                        DisplayChatMessage(msg);
                    });
                }// stream.Dispose(); 
            }
        }

        //显示数据
        //显示数据
        private void DisplayChatMessage(string msg)
        {
            ChatLine line = new ChatLine()
            {
                UserName = "QTX",
                Message = msg,
                Time = DateTime.Now,
                Type = ChatLineType.User,
            };
            chatView.DataSource.Add(line);
        }

        //关闭连接
        private void OnApplicationQuit()
        {
            //停止监听
            listener.Stop();
        }
    }
}