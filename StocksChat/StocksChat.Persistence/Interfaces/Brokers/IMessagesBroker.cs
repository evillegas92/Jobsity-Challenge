using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using StocksChat.Persistence.Entities;

namespace StocksChat.Persistence.Interfaces.Brokers
{
    public interface IMessagesBroker
    {
        Task<IEnumerable<MessageEntity>> GetAllMessages();
    }
}