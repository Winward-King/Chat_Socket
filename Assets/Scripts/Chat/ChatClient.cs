using Common;
using System.Collections;
using System.Collections.Generic;
using UIWidgetsSamples;
using UnityEngine;
using UnityEngine.UI;

namespace Network//命名空间（一般格式 域名.项目名.模块）
{
    public class ChatClient : MonoBehaviour
    {

        private InputField messageInput, nameInput;
        private ChatView chatView;
        private void Start()
        {
            transform.FindChildByName("Send").GetComponent<Button>().onClick.AddListener(OnSendMessageButtonClick);
            messageInput = transform.FindChildByName("MessageInput").GetComponent<InputField>();
            nameInput = transform.FindChildByName("NameInput").GetComponent<InputField>();
            //注册消息到达事件
            ClientUdpNetworkService.Instance.MessageArrived += DisplayMessage;

            chatView = GetComponentInChildren<ChatView>();
        }

        //显示数据
        private void DisplayMessage(object sender, MessageArrivedEventArgs e)
        {
            ChatLine line = new ChatLine()
            {
                Time = e.ArrivedTime,
                Message = e.Message.Content,
                UserName = e.Message.SenderName,
                Type = ChatLineType.User
            };
            chatView.DataSource.Add(line);
            //滑动条移动到末尾
            chatView.ScrollRect.verticalNormalizedPosition = 0;
        }

        //发送消息
        private void OnSendMessageButtonClick()
        {
            //数据验证…… 
            ChatMessage msg = new ChatMessage()
            {
                Type = MessageType.General,
                SenderName = nameInput.text,
                Content = messageInput.text
            };
            ClientUdpNetworkService.Instance.SendChatMessage(msg);
        }
    }
}

