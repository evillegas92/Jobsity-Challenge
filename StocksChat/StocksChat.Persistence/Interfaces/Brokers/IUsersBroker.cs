using System.Collections.Generic;
using System.Threading.Tasks;
using StocksChat.Persistence.Entities;

namespace StocksChat.Persistence.Interfaces.Brokers
{
    public interface IUsersBroker
    {
        Task<IEnumerable<AppUserEntity>> GetUsers();
    }
}