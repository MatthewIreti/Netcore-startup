using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

namespace NCELAP.WebAPI
{
    using System.Text.Json;
    using System.IO;

    class ConfigurationLoader
    {

        private dynamic configJsonData;
        public ConfigurationLoader Load(string configFilePath = "appsettings.json")
        {
            var appSettings = File.ReadAllText(configFilePath);
            this.configJsonData = JsonSerializer.Deserialize(appSettings, typeof(object));
            return this;
        }

        public dynamic GetProperty(string key)
        {
            string d = string.Empty;
            var properties = key.Split(".");
            dynamic property = this.configJsonData;
            foreach (var prop in properties)
            {
                property = property.GetProperty(prop);
                try
                {
                     //d = property.GetString();
                }
                catch (Exception ex)
                {
                }
                
            }

            return property;
        }
    }
}
