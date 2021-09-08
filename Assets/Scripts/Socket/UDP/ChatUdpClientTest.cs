  using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Common;
using UnityEngine.UI;
using System;
using ns;

/// <summary>
/// 如何通过UDP协议发送消息
/// </summary>
public class ChatUdpClientTest : MonoBehaviour {
    public string ip;
    public int port;
    public UdpClient udpService;
    private IPEndPoint serverEP;
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
        //基于UDP协议的网络服务
        //终结点（地址与端口）
        IPEndPoint localEP = new IPEndPoint(IPAddress.Parse(ip),port);
        udpService = new UdpClient(localEP);
        var serverTest = FindObjectOfType<ChatUdpServerTest>();
        serverEP = new IPEndPoint(IPAddress.Parse(serverTest.ip),serverTest.port);

    }
    //释放资源
    private void OnApplicationQuit()
    {
        udpService.Close();
    }
    //提供发送消息功能
    public void SendChatMessage(string msg)
    {
        //string =编码=> Btye[]
        //码表 
        byte[] dgram = Encoding.UTF8.GetBytes(msg);
        int count = udpService.Send(dgram,dgram.Length,serverEP);
        print(count);//发送的字节数
    }
    //注册按钮事件
    private void OnSendButtonClick()
    {
        string msg = transform.FindChildByName("MessageInput").GetComponent<InputField>().text;
        SendChatMessage(msg);

    }


}
