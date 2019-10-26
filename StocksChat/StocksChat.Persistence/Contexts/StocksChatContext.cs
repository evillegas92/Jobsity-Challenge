using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StocksChat.Persistence.Entities;

namespace StocksChat.Persistence.Contexts
{
    public class StocksChatContext : IdentityDbContext<AppUserEntity>
    {
        public StocksChatContext(DbContextOptions<StocksChatContext> options) : base(options)
        {
            
        }

        public DbSet<MessageEntity> Messages { get; set; }
    }
}