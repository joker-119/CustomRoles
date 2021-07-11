using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using Grenades;
using Mirror;
using UnityEngine;

namespace CustomRoles
{
    public class Methods
    {
        private readonly Plugin plugin;
        public Methods(Plugin plugin) => this.plugin = plugin;

        public bool CheckForCustomItems()
        {
            foreach (IPlugin<IConfig> plugin in Exiled.Loader.Loader.Plugins)
                if (plugin.Name == "CustomItems")
                    return true;
            return false;
        }
    }
}