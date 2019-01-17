using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
namespace DNLiCore_WebSocket
{

    public class WebSocketHelper
    {
        protected delegate void HelpNewSessionConnected(WebSocketModel webSocketModel);
        /// <summary>
        /// 客户端上线事件
        /// </summary>
        protected static event HelpNewSessionConnected NewSessionConnectedEvent;

        protected delegate void HelpNewMessageReceived(WebSocketModel webSocketModel);
        /// <summary>
        /// 接收到客户端消息事件
        /// </summary>
        protected static event HelpNewMessageReceived NewMessageReceivedEvent;

        protected delegate void HelpSessionClosed(WebSocketModel webSocketModel);
        /// <summary>
        /// 客户端断开事件
        /// </summary>
        protected static event HelpSessionClosed SessionClosedEvent;

        /// <summary>
        /// 在线客户端集合
        /// </summary>
        private static Dictionary<string, WebSocketModel> keyValuePairs = new Dictionary<string, WebSocketModel>();

        /// <summary>
        /// 新客户端连接时
        /// </summary>
        /// <param name="webSocket"></param>
        public  static  void NewSessionConnected(WebSocketModel webSocketModel)
        {
            AddOnlineClient(webSocketModel);
            //DNLiCore_Utility_Log.FileTxtLogs.WriteLog("客户端连接了连接ID:" + webSocketModel.ConnectionId);             
            NewSessionConnectedEvent(webSocketModel);
        }



        /// <summary>
        /// 接收到新消息时
        /// </summary>
        /// <param name="webSocket"></param>
        public static void NewMessageReceived(WebSocketModel webSocketModel)
        {
            //自定义业务逻辑写在这里....
            NewMessageReceivedEvent(webSocketModel);
        }

        /// <summary>
        /// 客户端断开时
        /// </summary>
        /// <param name="webSocket"></param>
        public static void SessionClosed(WebSocketModel webSocketModel)
        {
            RemoveOnlineClient(webSocketModel);
            SessionClosedEvent(webSocketModel);
        }


        #region 增加在线的客户端
        /// <summary>
        /// 增加在线的客户端
        /// </summary>
        /// <param name="webSocketModel"></param>
        private static void AddOnlineClient(WebSocketModel webSocketModel)
        {
            keyValuePairs.Add(webSocketModel.ConnectionId, webSocketModel);
        }
        #endregion

        #region 获取所有在线的客户端
        /// <summary>
        /// 获取所有在线的客户端
        /// </summary>
        /// <param name="webSocketModel"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Dictionary<string, WebSocketModel> GetOnlineClient()
        {
            return keyValuePairs;
        }
        #endregion

        /// <summary>
        /// 获取指定的客户端
        /// </summary>
        /// <param name="connectId"></param>
        /// <returns></returns>
        public static WebSocketModel GetClientByConnectionId(string connectId)
        {
            WebSocketModel socketModel;
            if (keyValuePairs.TryGetValue(connectId, out socketModel))
            {
            }
            else
            {
                socketModel = null;
            }
            return socketModel;
        }

        #region 广播，给其他在线客户端发消息
        /// <summary>
        /// 广播，给其他在线客户端发消息
        /// </summary>
        /// <param name="msg"></param>
        public static void SendMsgToAllOnlineClient(string msg)
        {
            byte[] sendBytes = System.Text.Encoding.Default.GetBytes(msg);
            foreach (string item in keyValuePairs.Keys)
            {
                WebSocketModel myWebSocketModel = new WebSocketModel();
                if (keyValuePairs.TryGetValue(item, out myWebSocketModel))
                {
                    myWebSocketModel.webSocket.SendAsync(new ArraySegment<byte>(sendBytes), myWebSocketModel.receiveResult.MessageType, myWebSocketModel.receiveResult.EndOfMessage, CancellationToken.None);
                }
            }
        }
        #endregion

        #region 移除断开的客户端
        /// <summary>
        /// 移除断开的客户端
        /// </summary>
        /// <param name="webSocketModel"></param>
        private static void RemoveOnlineClient(WebSocketModel webSocketModel)
        {
            keyValuePairs.Remove(webSocketModel.ConnectionId);
        }
        #endregion

        #region 给指定在线客户端发消息
        /// <summary>
        /// 给指定在线客户端发消息
        /// </summary>
        /// <param name="connectId"></param>
        /// <param name="msg"></param>
        public static bool SendMsgByConnectId(string connectId, string msg)
        {
            byte[] sendBytes = System.Text.Encoding.Default.GetBytes(msg);
            return SendMsgByConnectId(connectId,sendBytes);
        }
        public static bool SendMsgByConnectId(string connectId, byte[] sendBytes)
        {
            WebSocketModel webSocketModel = new WebSocketModel();
            if (keyValuePairs.TryGetValue(connectId, out webSocketModel))
            {
                try
                {
                    webSocketModel.webSocket.SendAsync(new ArraySegment<byte>(sendBytes), WebSocketMessageType.Text, true, CancellationToken.None);
                    return true;
                }
                catch (Exception ex)
                {

                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        #endregion




    }
}
