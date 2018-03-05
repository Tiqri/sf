using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blissmo.Helper.MailProvider
{
    public interface IEmailProvider
    {
        Task SendEmailAsync(Email mail);
    }
}
