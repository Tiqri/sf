using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blissmo.Helpers.MailProvider
{
    public interface IEmailProvider
    {
        Task SendEmailAsync(Email mail);
    }
}
