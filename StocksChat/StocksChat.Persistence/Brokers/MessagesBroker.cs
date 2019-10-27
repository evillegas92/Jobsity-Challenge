using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StocksChat.Persistence.Contexts;
using StocksChat.Persistence.Entities;
using StocksChat.Persistence.Interfaces.Brokers;

namespace StocksChat.Persistence.Brokers
{
    public class MessagesBroker : IMessagesBroker
    {
        private readonly StocksChatContext _dbContext;

        public MessagesBroker(StocksChatContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<MessageEntity>> GetAllMessages()
        {
            return await _dbContext.Messages.ToListAsync();
        }

        public async Task<MessageEntity> AddMessage(MessageEntity message)
        {
            _dbContext.Add(message);
            int result = await _dbContext.SaveChangesAsync();
            if (result > 0)
                return message;
            return null;
        }
    }
}