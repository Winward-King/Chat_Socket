using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


/// <summary>
/// 线程基本操作(创建 暂停 中止)
/// </summary>
public class ThreadDemo01 : MonoBehaviour
{
    public ManualResetEvent signal;
    private Thread thread03;
    //调度脚本生命周期的线程 称之为主线程
    private void Start()
    {
        //Fun1();

        //1.创建灯
        //信号灯
        signal = new ManualResetEvent(false);

        //public delegate void ParameterizedThreadStart(object obj);
        //public delegate void ThreadStart();
        //自行创建的线程，称之为“子线程/辅助线程”
        //1.通过方法创建线程
        Thread thread01 = new Thread(Fun1);
        //2.开启线程
        thread01.Start();

        Thread thread02 = new Thread(Fun2);
        //可以将方法传递参数
        //thread02.Start(2);

        thread03 = new Thread(Fun3);
        
        thread03.Start();

    }
    private void Fun1()
    {
        for (int i = 0; i < 5; i++)
        {
            //2.等一下
            signal.WaitOne();
            print(i);
            Thread.Sleep(1000);//线程睡眠1秒
        }
    }

    private void Fun2(object obj)
    {
        int n = (int)obj;
        for (int i = 0; i < 5; i++)
        {
            print(i);
            Thread.Sleep(1000);
        }
    }

    private void Fun3()
    {
        int i = 0;
        while(true)
        {
            i++;
            print(i);
            Thread.Sleep(1000);
        }
    }
    private void OnGUI()
    {
        if(GUILayout.Button("暂停"))
        {
            //3.红灯
            signal.Reset();//暂停的意思
        }
        if (GUILayout.Button("恢复"))
        {
            //4.绿灯
            signal.Set();
        }
    }

    private void OnApplicationQuit()
    {
        thread03.Abort();
    }
}
