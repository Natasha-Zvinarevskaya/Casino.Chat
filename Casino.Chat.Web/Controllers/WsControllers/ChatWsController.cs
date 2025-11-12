using Casino.Chat.Services.Interfaces;
using Casino.Chat.Services.Models.BaseResponse;
using Casino.Chat.Services.Models.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

//using WebSockets;
using WS.Extension.Client;
using WS.Extension.Client.Models;
//using WebSocketManager = WebSockets.WebSocketManager;

namespace Casino.Chat.Web.Controllers.WsControllers
{
    public class ChatWsController : WebSockets.WsController
    {
        private IChatService _chatService;
        private WebSockets.WebSocketManager _webSocketManager;
        IOptions<WsClientOptions> _options;

        public ChatWsController(IChatService chatService, WebSockets.WebSocketManager webSocketManager, IOptions<WsClientOptions> options)
        {
            _chatService = chatService;
            _webSocketManager = webSocketManager;
            _options = options;
        }

        public BaseResponse AddMessage(AddMessageRequest request)
        {

            _chatService.AddMessage(request, User.UserId);
            if (request.GameId.HasValue)
            {
                var wsClient = new WsClient(_options);
                MessageRequest message = new MessageRequest { Controller = "GameWsController", Method = "GetUsersIds", Value = request.GameId };
                wsClient.SendMessage(message);
            }
            else
            {
                _webSocketManager.SendMessageToUsers(User.UserId, nameof(ChatWsController), nameof(AddMessage), request);
            }
            return new BaseResponse();
        }
        public BaseResponse SendMessages(AddMessageRequest request, List<int> usersIds)
        {
            if (request.GameId != null)
            {
                foreach (var userId in usersIds)
                {
                    request.IdRecipient = userId;
                    _chatService.AddMessage(request, User.UserId);
                    _webSocketManager.SendMessageToUsers(User.UserId, nameof(ChatWsController), nameof(SendMessages), request);
                }
            }


            return new BaseResponse();
        }

    }
}
