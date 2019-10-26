using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace StocksChat.Persistence.Entities
{
    public class AppUserEntity : IdentityUser
    {
        public virtual ICollection<MessageEntity> Messages {get; set;}
    }
}