using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
namespace Network//命名空间（一般格式 域名.项目名.模块）
{
    /// <summary>
	/// 聊天消息
	/// </summary>
    public class ChatMessage 
    {
        public MessageType Type { get; set; }

        public string SenderName { get; set; }

        public string Content { get; set; }
        /// <summary>
        /// 对象转换 为 字节数组
        /// </summary>
        /// <returns></returns>
        public byte[] ObjectToBytes()
        {
            //sting/int/bool -二进制写入器BinaryWrite->内存流MemoryStream-->byte
            using (MemoryStream stream = new MemoryStream())
            {
                //属性-写入器-》内存流
                BinaryWriter writer = new BinaryWriter(stream);

                WriteString(writer, Type.ToString());
                WriteString(writer, SenderName);
                WriteString(writer, Content);
                //内存流  转--》byte[]  字节数组
                return stream.ToArray();
            }               
        }
        //将字符串写入到流中
        private void WriteString(BinaryWriter writer,string str)
        {
            //如果字符串为null,则赋值为""
            if (str == null) str = string.Empty;//"" 
            //编码
            byte[] typeBTS = Encoding.Unicode.GetBytes(str);
            //写入长度
            writer.Write(typeBTS.Length);
            //写入内容
            writer.Write(typeBTS);
        }
        /// <summary>
        /// 字节数组转换为对象
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static ChatMessage BytesToObject(byte[] bytes)
        {
            

            ChatMessage obj = new ChatMessage();

            //byte[] --内存流--二进制读取器 --> string/int/bool
            MemoryStream stream = new MemoryStream(bytes);
            using (BinaryReader reader = new BinaryReader(stream))
            { 
                string strType = ReadString(reader);
                obj.Type = (MessageType)Enum.Parse(typeof(MessageType), strType);
                obj.SenderName = ReadString(reader);
                obj.Content = ReadString(reader);
                return obj;
            }
        }

        private static string ReadString(BinaryReader reader)
        {
            int length = reader.ReadInt32();
            byte[] bts = reader.ReadBytes(length);
            return Encoding.Unicode.GetString(bts);           
       }
    }
}

