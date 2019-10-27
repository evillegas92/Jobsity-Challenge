using System;
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
        private readonly IUsersBroker _usersBroker;
        private readonly IMapper _mapper;

        public MessagesService(IMessagesBroker messagesBroker, IMapper mapper, IUsersBroker usersBroker)
        {
            _messagesBroker = messagesBroker;
            _usersBroker = usersBroker;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Message>> GetAllMessages()
        {
            IEnumerable<MessageEntity> messageEntities = await _messagesBroker.GetAllMessages();
            return _mapper.Map<IEnumerable<Message>>(messageEntities);
        }

        public async Task<Message> SaveMessage(string username, string message)
        {
            if (message.StartsWith("/"))
            {
                //TODO: handle command
                return null;
            }

            Message newMessage = new Message
            {
                Text = message,
                When = DateTime.UtcNow
            };
            MessageEntity messageEntity = _mapper.Map<MessageEntity>(newMessage);
            messageEntity.User = await _usersBroker.GetUserByUsername(username);
            messageEntity = await _messagesBroker.AddMessage(messageEntity);
            return _mapper.Map<Message>(messageEntity);
        }
    }
}