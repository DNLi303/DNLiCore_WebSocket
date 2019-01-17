--------------------------------------DNLiCore_WebSocket框架-----------------------------------------  
愿景:打造一款即装即用的快速开发框架，更少的耦合更多的功能更高效的开发效率  
--------------------------------------介绍说明---------------------------------------------  
DNLiCore_WebSocket框架一款超简单入手的websocket框架,几行代码搞定,小白也会用，性能方面除非有十万级别的数据，否则没压力。。。  
--------------------------------------使用说明---------------------------------------------   
1.通过Nuget安装DNLiCore_WebSocket类
2.在appsettings.json配置所需要的数据库的配置例如
{
  "WebSocketOption": {
			"RequestUrlKey": "/websocket", //websocket的页面路径
			"KeepAliveInterval": 60, //服务端保持ping客户端秒数，即心跳频率，是由服务端下发
			"ReceiveBufferSize": 4096 //接收数据的缓存大小，单位为字节
		}
}
3.自定义一个类，继承于 DNLiCore_WebSocket.WebSocketHelper  
  例如  
  public class MyWebSocket : DNLiCore_WebSocket.WebSocketHelper  
    {  
        public MyWebSocket()  
        {  
            //客户端上线事件  
            NewSessionConnectedEvent += MyWebSocket_NewSessionConnectedEvent;  
            //接收数据事件  
            NewMessageReceivedEvent += MyWebSocket_NewMessageReceivedEvent;  
            //客户端关闭事件  
            SessionClosedEvent += MyWebSocket_SessionClosedEvent;  
        }  
  
        private void MyWebSocket_SessionClosedEvent(DNLiCore_WebSocket.WebSocketModel webSocketModel)  
        {  
           //自定义客户端上线逻辑  
        }   
  
        private void MyWebSocket_NewMessageReceivedEvent(DNLiCore_WebSocket.WebSocketModel webSocketModel)  
        {  
            //自定义接收客户端消息事件  
        }  
  
        private void MyWebSocket_NewSessionConnectedEvent(DNLiCore_WebSocket.WebSocketModel webSocketModel)  
        {  
           //自定义客户端掉线事件   
        }  
    }  

4.在项目启动执行自定义的构造事件，可以在Startup.cs里面，也可以在HomeController里面
  例如在starup.cs里面
  new MyWebSocket();
  


