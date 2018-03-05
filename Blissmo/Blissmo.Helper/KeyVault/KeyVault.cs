using System;
using System.Collections.Generic;
using System.Text;

namespace Blissmo.Helper.KeyVault
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
            { "RABBITMQ_ENDPOINT", "52.170.16.118" },
             { "RABBITMQ_PORT", "5672" }
        };

        public static string GetValue(string key)
        {
            return _keyVault.TryGetValue(key, out string value) ? value : null;
        }
    }
}
