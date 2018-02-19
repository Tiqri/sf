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
            { "SearchServiceKey", "A5EEFE72F7876E2DEF92259280423BB2" }
        };

        public static string GetValue(string key)
        {
            return _keyVault.TryGetValue(key, out string value) ? value : null;
        }
    }
}
