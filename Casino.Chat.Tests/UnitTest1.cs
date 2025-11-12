
using Casino.Chat.DataContext;
using Casino.Chat.Services;
using Casino.Chat.Services.Models.Request;
using Microsoft.EntityFrameworkCore;

namespace Casino.Chat.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void AddMessage_SaveDb()
        {
            //Arange
            var contextOptions = GetContextOptions();
            var service = new ChatService(contextOptions);
            var idSender = 2;
            var request = new AddMessageRequest
            {
                GameId = 2,
                IdRecipient = 2,
                Message = "Test",
                TimeSending = DateTime.Now
            };
            //Act 
            var result = service.AddMessage(request, idSender);

            //Assert
            Assert.Equal(result.IsSucces, true);


        }

        private DbContextOptions<ChatDbContext> GetContextOptions()
        {
            var options = new DbContextOptionsBuilder<ChatDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var context = new ChatDbContext(options);
            context.Letters.Add(new Letter
            {
                GameId = 1,
                Id = 1,
                IdRecipient = 1,
                IdSender = 1,
                Message = "",
                TimeSending = DateTime.Now
            });
            context.SaveChanges();
            return options;
        }
    }
}
