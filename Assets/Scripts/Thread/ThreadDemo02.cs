using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Com.ARPGDemo.Character;

/// <summary>
/// 线程同步
/// </summary>
public class ThreadDemo02 : MonoBehaviour {

    private void Start()
    {
        //Thread thread01 = new Thread(Fun1);
        //thread01.IsBackground = false;
        //thread01.Start();
        //ThreadPool.QueueUserWorkItem(Fun2);
        //ThreadPool.QueueUserWorkItem(Fun1);
        //ThreadPool.QueueUserWorkItem(obj =>
        //{
        //    Fun2();
        //});
        //同步（排队）调用
        //Bank.GetMoney(1);
        //异步（不排队）调用
        ThreadPool.QueueUserWorkItem(obj => 
        {
            Bank.GetMoney(1);
        });
    }
    private void Fun1(object obj)
    {
        Fun2();
    }

    private void Fun2()
    {

    }
}
