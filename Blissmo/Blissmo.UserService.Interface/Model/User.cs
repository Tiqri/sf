using System;
using System.Collections.Generic;
using System.Text;

namespace Blissmo.UserService.Interface.Model
{
    public class User
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public DateTime DOB { get; set; }
    }
}
