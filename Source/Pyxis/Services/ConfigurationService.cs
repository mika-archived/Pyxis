using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Storage;

using Pyxis.Services.Interfaces;

namespace Pyxis.Services
{
    class ConfigurationService : IConfigurationService
    {
        private readonly ApplicationDataContainer _dataContainer;

        public ConfigurationService()
        {
            this._dataContainer = ApplicationData.Current.RoamingSettings;
        }

        public object this[string key]
        {
            get { return this._dataContainer.Values[key]; }
            set { this._dataContainer.Values[key] = value; }
        }
    }
}
