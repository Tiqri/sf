using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blissmo.Helper.MailProvider
{
    public class EmailProvider : IEmailProvider
    {
        public async Task SendEmailAsync(Email mail)
        {
            var apiKey = KeyVault.KeyVault.GetValue("SendGridApiKey");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("system@blissmo.com", "Blissmo Platform");
            var toList = new List<EmailAddress>();
            foreach (var item in mail.Recipients)
            {
                toList.Add(new EmailAddress(item));
            }

            var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, toList, mail.Subject, mail.Message, "");
            var response = await client.SendEmailAsync(msg);
        }
    }
}
