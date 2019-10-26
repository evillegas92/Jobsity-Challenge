using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StocksChat.Persistence.Entities
{
    [Table( "Messages" )]
    public class MessageEntity
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Text { get; set; }

        public DateTime When { get; set; } = DateTime.UtcNow;

        public virtual AppUserEntity User { get; set; }
    }
}