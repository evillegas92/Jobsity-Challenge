using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using StocksChat.Business.Models;

namespace StocksChat.Business.Interfaces.Services
{
    public interface IMessagesService
    {
        Task<IEnumerable<Message>> GetAllMessages();
    }
}