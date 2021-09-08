using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Com.ARPGDemo.Character//命名空间（一般格式 域名.项目名.模块）
{
    public class GameMain : MonoBehaviour
    {
        private void Start()
        {
            transform.FindChildByName("ButtonClientEnter").GetComponent<Button>().onClick.AddListener(OnEnterClientButtonClick);

            transform.FindChildByName("ButtonServerEnter").GetComponent<Button>().onClick.AddListener(OnEnterServerButtonClick);
        }

        //当单机进入客户端按钮时执行
        private void OnEnterClientButtonClick()
        {
            string serverIP = transform.FindChildByName("InputServerIPForClient").GetComponent<InputField>().text;
            string serverPort = transform.FindChildByName("InputServerPortForClient").GetComponent<InputField>().text;
            //数据验证……
            int port = int.Parse(serverPort);
            Network.ClientUdpNetworkService.Instance.Initialized(serverIP, port);
            SceneManager.LoadScene("Chat");
        }

        private void OnEnterServerButtonClick()
        {
            string serverIP = transform.FindChildByName("InputServerIPForServer").GetComponent<InputField>().text;
            string serverPort = transform.FindChildByName("InputServerPortForServer").GetComponent<InputField>().text;
            //数据验证……
            int port = int.Parse(serverPort);
            Network.ServerUdpNetworkService.Instance.Initialized(serverIP, port);
            SceneManager.LoadScene("Server");
        }
    }
}


