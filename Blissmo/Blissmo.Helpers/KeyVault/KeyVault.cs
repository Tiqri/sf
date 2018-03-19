using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Fabric;
using System.Fabric.Description;

namespace Blissmo.Helpers.KeyVault
{
    public static class KeyVault
    {
        private static readonly KeyedCollection<string, ConfigurationProperty> _keyedCollection =
            FabricRuntime.GetActivationContext()
             .GetConfigurationPackageObject("Config")
             .Settings
             .Sections["ConfigurationSection"]
             .Parameters;

        private static Dictionary<string, string> _keyVault = new Dictionary<string, string>()
        {
            { "SEARCH_SERVICE_NAME", GetConfigurationValue("SearchServiceName") },
            { "SEARCH_SERVICE_KEY", GetConfigurationValue("SearchServiceKey") },
            { "SENDGRID_API_KEY", GetConfigurationValue("SendGridApiKey") },
            { "AZURE_SERVICE_BUS_ENDPOINT", GetConfigurationValue("AzureServiceBusEndpoint") },
            { "RESERVATION_QUEUE_NAME", GetConfigurationValue("ReservationQueueName") },
            { "RESERVATION_RESPONSE_QUEUENAME", GetConfigurationValue("ReservationResponseQueueName") },
            { "RABBITMQ_ENDPOINT", GetConfigurationValue("RabbitMQEndpoint") },
            { "RABBITMQ_USERNAME", GetConfigurationValue("RabbitMQUserName") },
            { "RABBITMQ_PASSWORD", GetConfigurationValue("RabbitMQPassword") },
            { "RABBITMQ_PORT", GetConfigurationValue("RabbitMQPort") }
        };

        private static string GetConfigurationValue(string key) =>
            _keyedCollection.Contains(key) ? _keyedCollection[key].Value : "";

        public static string GetValue(string key)
        {
            return _keyVault.TryGetValue(key, out string value) ? value : null;
        }
    }
}
