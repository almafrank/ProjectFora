using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectFora.Shared
{
    public class UserInterestModel
    {
        public int UserId { get; set; }
        public AccountUserModel User { get; set; }
        public int InterestId { get; set; }
        public InterestModel Interest { get; set; }
    }
}
