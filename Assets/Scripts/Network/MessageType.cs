using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Network//命名空间（一般格式 域名.项目名.模块）
{
    /// <summary>
    /// 消息类型
    /// </summary>
    public enum MessageType 
    {
        /// <summary>
        /// 上线
        /// </summary>
        OnLine,
        /// <summary>
        /// 下线
        /// </summary>
        OffLine,
        /// <summary>
        /// 常规
        /// </summary>
        General
    }
}

