using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 跨线程访问 思想
/// </summary>
public class ThreadDemo03_01 : MonoBehaviour
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
            second--;
            //在辅助线程中，访问Unity组件属性，会异常
            //txtTimer.text = string.Format("{0}:{1}", second / 60, second % 60);
            action = () =>
            {
                txtTimer.text = string.Format("{0}:{1}", second / 60, second % 60);
            };
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
