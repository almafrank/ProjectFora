using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectFora.Shared
{
    public class AccountUserModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public bool IsDeleting { get; set; }
    }
}
