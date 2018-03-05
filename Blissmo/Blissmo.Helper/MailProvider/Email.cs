using System;
using System.Collections.Generic;
using System.Text;

namespace Blissmo.Helper.MailProvider
{
    public class Email
    {
        public List<string> Recipients { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }
    }
}
