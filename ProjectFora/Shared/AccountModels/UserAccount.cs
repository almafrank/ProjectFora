using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectFora.Shared.AccountModels
{
    public class UserAccount
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Token { get; set; } = String.Empty;
        public bool IsDeleting { get; set; }
    }
}
