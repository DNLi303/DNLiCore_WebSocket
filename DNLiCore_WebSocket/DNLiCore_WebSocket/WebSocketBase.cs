using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DNLiCore_WebSocket
{
    public class RequestWebSocketBaseMiddleware
    {
        private readonly RequestDelegate _next;
        private IConfiguration _configuration;
        public RequestWebSocketBaseMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path == _configuration.GetSection("WebSocketOption:RequestUrlKey").Value)
            {
                //判断是不是websocket
                if (context.WebSockets.IsWebSocketRequest)
                {
                    WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    //客户端第一次连接时
                    var webSocketModel = new WebSocketModel
                    {
                        ConnectionId = context.Connection.Id,
                        context = context,
                        webSocket = webSocket,
                        RemoteIpAddress = context.Connection.RemoteIpAddress,
                        RemotePort = context.Connection.RemotePort
                    };
                    WebSocketHelper.NewSessionConnected(webSocketModel);                   
                    await Echo(context, webSocket);
                }
            }
            else
            {
                await _next(context);
            }
        }

        private async Task Echo(HttpContext context, WebSocket webSocket)
        {
            int defaultReceiveSize = 4096;
            try
            {
                defaultReceiveSize = Convert.ToInt32(_configuration.GetSection("WebSocketOption:RequestUrlKey").Value);
            }
            catch (Exception) { }
            byte[] buffer = new byte[defaultReceiveSize];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {                                                
                byte[] desReceiveBytes = new byte[result.Count];
                Array.Copy(buffer, desReceiveBytes, result.Count);
                var webSocketModel = new WebSocketModel
                {
                    ConnectionId = context.Connection.Id,
                    context = context,
                    webSocket = webSocket,
                    receiveBytes = desReceiveBytes,
                    receiveResult = result
                };
                WebSocketHelper.NewMessageReceived(webSocketModel);
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            if (result.CloseStatus.HasValue)
            {
                //关闭事件                
                var webSocketModel = new WebSocketModel
                {
                    ConnectionId = context.Connection.Id,
                    context = context,
                    webSocket = webSocket
                };
                WebSocketHelper.SessionClosed(webSocketModel);
            }
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }
    }






    public static class RequestWebSocketBaseMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestWebSocketBase(
             this IApplicationBuilder builder, IConfiguration Configuration)
        {
            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(Convert.ToDouble(Configuration.GetSection("WebSocketOption:KeepAliveInterval").Value)), //心跳包120秒
                ReceiveBufferSize = Convert.ToInt32(Configuration.GetSection("WebSocketOption:ReceiveBufferSize").Value)  //接收数据缓存大小
            };
            builder.UseWebSockets();
            return builder.UseMiddleware<RequestWebSocketBaseMiddleware>();
        }
    }
}
