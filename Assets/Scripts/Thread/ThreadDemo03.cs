using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace ns
{
	/// <summary>
	/// 跨线程访问UnityAPI
	/// </summary>
	public class ThreadDemo03 : MonoBehaviour 
	{
        private Text txtTimer;
        public int second = 120;
        private Thread timerThread;
        private void Start()
        {
            txtTimer = GetComponent<Text>(); 

            timerThread = new Thread(Timer);
            timerThread.Start();
        }

        private Action action;

        private void Timer()
        {
            while (second > 0)
            {
              
                //在辅助线程中，访问Unity组件属性，会异常。
                //txtTimer.text = string.Format("{0}:{1}", second / 60, second % 60);
                //创建委托对象(交给委托)
                //action = () => 
                //{
                //    txtTimer.text = string.Format("{0}:{1}", second / 60, second % 60);
                //};
                //通过线程交叉访问助手
                ThreadCrossHelper.Instance.ExecuteOnMainThread(() =>
                {
                    second--;
                    txtTimer.text = string.Format("{0}:{1}", second / 60, second % 60);
                },3);
                Thread.Sleep(1000);
            }
        }

        private void Update()
        {
            if (action != null)
            {
                action();
                action = null;
            } 
        }

        private void OnApplicationQuit()
        {
            timerThread.Abort();
        }
    }
}