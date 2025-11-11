using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models
{
    public class LoginUsers
    {
        public string UserName
        {
            get;
            set;
        }
        public Guid Id
        {
            get;
            set;
        }
        public string EmailId
        {
            get;
            set;
        }
        public string Password
        {
            get;
            set;
        }
    }
}

