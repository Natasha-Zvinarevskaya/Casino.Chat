using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Chat.Services.Models.Request
{
   public class AddMessageRequest
    {
        /// <summary>
        /// Ид получателя
        /// </summary>
        public int IdRecipient { get; set; }
        /// <summary>
        /// Ид игры
        /// </summary>
        public int? GameId { get; set; }

        /// <summary>
        /// Сообщение
        /// </summary>
        public string Message { get; set; }
    }
}
