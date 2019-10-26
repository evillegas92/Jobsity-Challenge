using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using StocksChat.Persistence.Contexts;
using StocksChat.Persistence.Entities;

namespace StocksChat.Persistence
{
    public class Seeder
    {
        private readonly StocksChatContext _context;
        private readonly UserManager<AppUserEntity> _userManager;

        private const string userOneUserName = "user1@stockschat.com", userTwoUserName = "user2@stockschat.com", password ="P@ssw0rd!";

        public Seeder(StocksChatContext context, UserManager<AppUserEntity> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task Seed()
        {
            _context.Database.EnsureCreated();

            AppUserEntity userOne = await _userManager.FindByEmailAsync(userOneUserName);
            if (userOne == null)
            {
                userOne = new AppUserEntity
                {
                    Email = userOneUserName,
                    UserName = userOneUserName
                };
                var result = await _userManager.CreateAsync(userOne, password);
                if (result != IdentityResult.Success)
                    throw new InvalidOperationException($"Error seeding user {userOneUserName}.");
            }

            AppUserEntity userTwo = await _userManager.FindByEmailAsync(userTwoUserName);
            if (userTwo == null)
            {
                userTwo = new AppUserEntity
                {
                    Email = userTwoUserName,
                    UserName = userTwoUserName
                };
                var result = await _userManager.CreateAsync(userTwo, password);
                if (result != IdentityResult.Success)
                    throw new InvalidOperationException($"Error seeding user {userTwoUserName}.");
            }
        }
    }
}