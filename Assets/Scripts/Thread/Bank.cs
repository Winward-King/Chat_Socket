using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
namespace Com.ARPGDemo.Character//命名空间（一般格式 域名.项目名.模块）
{
    public class Bank 
    {

        private static int totalMoney = 1;
        private static object locker = new object();
        //Start 0 1 2
        public static void GetMoney(int val)
        {  
            //线程锁 锁定程序执行流程

            //判断该对象locker 的同步块索引是否为-1
            lock (locker)
            {
                if (totalMoney >= val)
                {
                    //如果时-1 进入程序，在改变索引。如果不是-1则等待……
                    Thread.Sleep(500); //用于模拟取钱的过程比较慢
                    //0 -1 -2
                    totalMoney -= val;
                    Debug.Log("取钱成功" + totalMoney);
                }
                else
                {
                    Debug.Log("取钱失败" + totalMoney);
                }
            }
        }
    }
}

