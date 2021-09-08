using Common;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace ns
{
	/// <summary>
	/// 基于TCP协议发送数据
	/// </summary>
	public class ChatTcpClientTest : MonoBehaviour 
	{
        public  string ip;
        public int port;
        private TcpClient tcpService;

        private void OnEnable()
        {
            transform.FindChildByName("Send").GetComponent<Button>().onClick.AddListener(OnSendButtonClick);
        }

        private void OnDisable()
        {
            transform.FindChildByName("Send").GetComponent<Button>().onClick.RemoveListener(OnSendButtonClick);
        }

        //创建Socket对象
        private void Start()
        {
            IPEndPoint localEP = new IPEndPoint(IPAddress.Parse(ip), port);
            //客户端绑定端口
            //tcpService = new TcpClient(localEP);
            //客户端使用随机端口
            tcpService = new TcpClient();

            var serverTest = GetComponent<ChatTcpServerTest>();
            IPEndPoint serverEP = new IPEndPoint(IPAddress.Parse(serverTest.ip), serverTest.port);
            //与服务器建立连接
            tcpService.Connect(serverEP);
        }

        //发送消息
        public void SendChatMessage(string msg)
        {
            byte[] buffer = Encoding.Unicode.GetBytes(msg); 
            //获取网络流
            NetworkStream stream = tcpService.GetStream();
            stream.Write(buffer, 0, buffer.Length);
        }

        //按钮事件 
        private void OnSendButtonClick()
        {
            string msg = transform.FindChildByName("MessageInput").GetComponent<InputField>().text;
            SendChatMessage(msg);
        }

        //释放资源
        private void OnApplicationQuit()
        {
            //问题：客户端绑定端口后，短时间(30秒--4分钟)内再次链接，提示端口占用。
            //解决：客户端每次更换端口
            //          让服务端先断开(先断开的一方，会延迟释放端口)。

            //下线通知，让服务端先断开。
            SendChatMessage("Quit");
            Thread.Sleep(500);
            tcpService.Close();
        }
    }
}