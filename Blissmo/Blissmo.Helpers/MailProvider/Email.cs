using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blissmo.Helpers.MailProvider
{
    public class Email
    {
        public List<string> Recipients { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }
    }
}
