using System;
using System.Collections.Generic;
using System.Text;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Net;

namespace DNLiCore_WebSocket
{
    public class WebSocketModel
    {

        /// <summary>
        /// 连接ID
        /// </summary>
        public string ConnectionId { get; set; }
        /// <summary>
        /// webSocket对象
        /// </summary>
        public WebSocket webSocket { get; set; }
        /// <summary>
        /// 当前Httpcontext对象
        /// </summary>
        public HttpContext context { get; set; }
        /// <summary>
        /// 接收消息的Result对象
        /// </summary>
        public WebSocketReceiveResult receiveResult { get; set; }
        /// <summary>
        /// 接收到的消息数据的byte[]
        /// </summary>
        public byte[] receiveBytes { get; set; }

        /// <summary>
        /// 远程客户端IP地址
        /// </summary>
        public IPAddress RemoteIpAddress { get; set; }
        /// <summary>
        /// 远程客户端断开
        /// </summary>
        public int RemotePort { get; set; }
    }
}
