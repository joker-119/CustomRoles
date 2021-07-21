using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using Grenades;
using Mirror;
using UnityEngine;
using Object = UnityEngine.Object;

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
        
        public Grenade Spawn(Vector3 position, Vector3 velocity, float fuseTime = 3f, ItemType grenadeType = ItemType.GrenadeFrag, Player player = null)
        {
            if (player == null)
                player = Server.Host;

            GrenadeManager grenadeManager = player.GrenadeManager;
            GrenadeSettings settings =
                grenadeManager.availableGrenades.FirstOrDefault(g => g.inventoryID == grenadeType);

            if (settings == null)
                return null;

            Grenade grenade = Object.Instantiate(settings.grenadeInstance).GetComponent<Grenade>();

            grenade.FullInitData(grenadeManager, position, Quaternion.Euler(grenade.throwStartAngle), velocity, grenade.throwAngularVelocity, player == Server.Host ? Team.RIP : player.Team);
            grenade.NetworkfuseTime = NetworkTime.time + fuseTime;

            NetworkServer.Spawn(grenade.gameObject);

            return grenade;
        }
    }
}