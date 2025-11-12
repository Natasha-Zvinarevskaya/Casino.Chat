using Azure.Core;
using Casino.Chat.DataContext;
using Casino.Chat.Services;
using Casino.Chat.Services.Interfaces;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Sockets;
using System.Net.WebSockets;
using WebSockets;
using WebSockets.WsServer;
using WS.Extension.Client.Models;
//using WS.Extension.Client;
//using WS.Extension.Listener.Models;



namespace Casino.Chat.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<ChatDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString(nameof(ChatDbContext))));
            builder.Services.AddScoped<IChatService, ChatService>();

            builder.Services.Configure<WsClientOptions>(builder.Configuration.GetSection(nameof(WsClientOptions)));

            builder.Services.AddSingleton<WebSockets.WebSocketManager>();

            var app = builder.Build();




            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseWebSockets();  //Подключение вебСокета
            app.UseCors(builder => builder.AllowAnyOrigin());

            //Подключение сервисов,чтобы передать значение переменной
            var serviceProvider = builder.Services.BuildServiceProvider();
            app.MapGet("/", () => "Casino.Chat");
            // Для вебСокета
            app.Map("/ws", async context =>
            {
                if (!context.WebSockets.IsWebSocketRequest)
                {
                    context.Response.StatusCode = 400;
                    return;
                }

                //var wsConnection = serviceProvider.GetService<WsConnection>();
                //await wsConnection.Connection(new ConnectRequest { Context = context, ServiceProvider = serviceProvider });
                //var wsServer = serviceProvider.GetService<WsServer>();


                var webSocketManager = serviceProvider.GetService<WebSockets.WebSocketManager>();

                using var socket = await context.WebSockets.AcceptWebSocketAsync();

                var ct = CancellationToken.None;
                var wsUser = new WsUser { UserId = 1 };
                webSocketManager.AddSocket(socket, wsUser);

                var token = context.Request.Query.FirstOrDefault(x => x.Key == "token");

                //Цикл (пока открыт сокет) слушает сообщение от фронта
                while (socket.State == WebSocketState.Open)
                {
                    var messageJson = await WebSocketsHelper.ReceiveStringAsync(socket, ct);
                    if (messageJson == null) break;

                    //webSocketManager.SendMessageToUsers();
                    await WebSocketsHelper.DispatchToControllerAsync(serviceProvider, context, socket, messageJson, ct, token.Value);
                }

                //Нужно создать событие , отслеживающие закрытие сокета
                webSocketManager.RemoveSocket(socket);

            });


            app.Run();
        }
    }
}
