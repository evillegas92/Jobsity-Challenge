using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using StocksChat.Business.Interfaces.Services;

namespace StocksChat.Web.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IMessagesService _messagesService;

        public ChatHub(IMessagesService messagesService)
        {
            _messagesService = messagesService;
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
            await _messagesService.SaveMessage(user, message);
        }
    }
}