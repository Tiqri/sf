using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blissmo.Helpers.KeyVault
{
    public static class KeyVault
    {
        private static Dictionary<string, string> _keyVault = new Dictionary<string, string>()
        {
            { "SearchServiceName", "blissmo" },
            { "SearchServiceKey", "A5EEFE72F7876E2DEF92259280423BB2" },
            { "SendGridApiKey", "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" },
            { "AZURE_SERVICE_BUS_ENDPOINT", "Endpoint=sb://blissmo.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=BT1A0IXPd6goWbJ4PSidrtMMIQXcKNh4TYJAJcVf364=" },
            { "RESERVATION_QUEUE_NAME", "reservationqueue" },
            { "RESERVATION_RESPONSE_QUEUE_NAME", "reservationresponse" },
            { "RABBITMQ_ENDPOINT", "52.179.96.170" },
             { "RABBITMQ_PORT", "5672" }
        };

        public static string GetValue(string key)
        {
            return _keyVault.TryGetValue(key, out string value) ? value : null;
        }
    }
}
