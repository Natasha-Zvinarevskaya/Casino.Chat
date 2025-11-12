using Casino.Chat.Services.Models.BaseResponse;
using Casino.Chat.Services.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Chat.Services.Interfaces
{
   public interface IChatService
    {
        BaseResponse AddMessage(AddMessageRequest request,int idSender);
    }
}
