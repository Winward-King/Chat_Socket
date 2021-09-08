using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Network//命名空间（一般格式 域名.项目名.模块）
{
    /// <summary>
    /// 消息到达事件参数类
    /// </summary>
    public class MessageArrivedEventArgs:EventArgs
    {
        public ChatMessage Message { get; set; }

        public DateTime ArrivedTime { get; set; }
       
    }
}

