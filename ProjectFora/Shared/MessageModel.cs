using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectFora.Shared
{
    public class MessageModel
    {
        [Key]
        public int Id { get; set; }
        public string Message { get; set; } = String.Empty;
        public DateTime MessageCreated { get; set; }
        public string CreatedBy { get; set; } = String.Empty;

        // Relations
        [ForeignKey(nameof(Thread))]
        public int ThreadId { get; set; }
        public ThreadModel Thread { get; set; }

        [ForeignKey(nameof(User))]
        public int? UserId { get; set; }
        public UserModel? User { get; set; }
    }
}
