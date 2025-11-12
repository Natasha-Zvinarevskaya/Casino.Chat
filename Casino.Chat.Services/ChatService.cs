using Casino.Chat.DataContext;
using Casino.Chat.Services.Interfaces;
using Casino.Chat.Services.Models.BaseResponse;
using Casino.Chat.Services.Models.Request;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;

namespace Casino.Chat.Services
{
    public class ChatService : IChatService
    {
        private DbContextOptions<ChatDbContext> _options;
        public ChatService(DbContextOptions<ChatDbContext> options)
        {
            _options = options;
        }
        /// <summary>
        /// Добавляет сообщение в бд
        /// </summary>
        /// <param name="request">Ид,получателя, Ид игры, Время отправки и сообщение</param>
        /// <param name="idSender">Ид отправителя</param>
        /// <returns></returns>
        public BaseResponse AddMessage(AddMessageRequest request, int idSender)
        {
            var db = new ChatDbContext(_options);
            var letter = new Letter
            {
                GameId = request.GameId,
                IdRecipient = request.IdRecipient,
                IdSender = idSender,
                Message = request.Message,
                TimeSending = DateTime.UtcNow
            };
            db.Letters.Add(letter);
            db.SaveChanges();
            return new BaseResponse();
        }

        public BaseResponse SendMessages (SendMessagesRequest request, int idSender,List<int> usersId)
        {
            var db = new ChatDbContext(_options);
            foreach(var userId in usersId )
            {
                var letter = new Letter
                {
                    GameId = request.GameId,
                    IdRecipient = userId,
                    IdSender = idSender,
                    Message = request.Message,
                    TimeSending = DateTime.UtcNow
                };
                db.Letters.Add(letter);
            }
            db.SaveChanges();
            return new BaseResponse();
        }

        
    }
}
