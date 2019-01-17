using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DNLiCore_WebSocket_Test
{
    public class websocketHelp : DNLiCore_WebSocket.WebSocketHelper
    {
        public websocketHelp()
        {
            NewSessionConnectedEvent += WebsocketHelp_NewSessionConnectedEvent;
            NewMessageReceivedEvent += WebsocketHelp_NewMessageReceivedEvent;
            SessionClosedEvent += WebsocketHelp_SessionClosedEvent;
        }

        private void WebsocketHelp_SessionClosedEvent(DNLiCore_WebSocket.WebSocketModel webSocketModel)
        {

        }

        private void WebsocketHelp_NewMessageReceivedEvent(DNLiCore_WebSocket.WebSocketModel webSocketModel)
        {
            byte[] mybytes = webSocketModel.receiveBytes;

            SendMsgToAllOnlineClient(webSocketModel.ConnectionId, mybytes);
        }

        private void WebsocketHelp_NewSessionConnectedEvent(DNLiCore_WebSocket.WebSocketModel webSocketModel)
        {

        }
    }
}
