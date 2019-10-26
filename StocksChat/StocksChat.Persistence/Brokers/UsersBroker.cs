using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StocksChat.Persistence.Entities;
using StocksChat.Persistence.Interfaces.Brokers;

namespace StocksChat.Persistence.Brokers
{
    public class UsersBroker : IUsersBroker
    {
        private readonly UserManager<AppUserEntity> _userManager;

        public UsersBroker(UserManager<AppUserEntity> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<AppUserEntity>> GetUsers()
        {
            List<AppUserEntity> users = await _userManager.Users.ToListAsync();
            return users;
        }
    }
}