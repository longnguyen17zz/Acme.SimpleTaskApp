using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.SimpleTaskApp.Customers.Dto
{
    public class CustomerDto
    {
        public string UserName { get; set; }

        public string EmailAddress { get; set; }

        public string Phone { get; set; }
        public string Password { get; set; }
    }
}
