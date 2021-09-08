using System;
using System.Collections;
using System.Collections.Generic;
using Network;
using UnityEngine;
using UnityEngine.UI;

namespace Network//命名空间（一般格式 域名.项目名.模块）
{
    public class ChatServer : MonoBehaviour
    {
        private void Start()
        {
            Network.ServerUdpNetworkService.Instance.MessageArrived += DisplayMessage;
        }

        private void DisplayMessage(object sender, MessageArrivedEventArgs e)
        {
            GetComponentInChildren<Text>().text += e.Message.Type + "\t";
        }
    }        
}

