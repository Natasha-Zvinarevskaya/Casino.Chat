using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Chat.Services.Models.Request
{
    public class SendMessagesRequest
    {
        public int? GameId { get; set; }

        /// <summary>
        /// Сообщение
        /// </summary>
        public string Message { get; set; }

    }
}
