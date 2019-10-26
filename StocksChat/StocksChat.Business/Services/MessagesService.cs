using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using StocksChat.Business.Interfaces.Services;
using StocksChat.Business.Models;
using StocksChat.Persistence.Entities;
using StocksChat.Persistence.Interfaces.Brokers;

namespace StocksChat.Business.Services
{
    public class MessagesService : IMessagesService
    {
        private readonly IMessagesBroker _messagesBroker;
        private readonly IMapper _mapper;

        public MessagesService(IMessagesBroker messagesBroker, IMapper mapper)
        {
            _messagesBroker = messagesBroker;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Message>> GetAllMessages()
        {
            IEnumerable<MessageEntity> messageEntities = await _messagesBroker.GetAllMessages();
            return _mapper.Map<IEnumerable<Message>>(messageEntities);
        }
    }
}