using System;
using JoKenPo.Domain.Interfaces;

namespace JoKenPo.Configuration
{
    public class ConfigurationManager : IConfigurationManager
    {
        private const string BaseUrl = "http://localhost:7071/api";
        public string GetConfigKey(string key)
        {
            return key switch
            {
                nameof(BaseUrl) => BaseUrl,
                _ => throw new NotImplementedException("Requested key doesn't exist")
            };
        }
    }
}