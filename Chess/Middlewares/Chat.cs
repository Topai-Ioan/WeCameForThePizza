using Microsoft.AspNetCore.SignalR;

namespace Chess.Middlewares
{
    public class Chat : Hub
    {
        public async Task Send(string name, string message)
        {
            await Clients.All.SendAsync("Send", name, message);
        }
    }
}
