using System;
using System.Collections.Generic;
using System.Linq;
using CustomRoles.Roles;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using Mirror;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CustomRoles
{
    using MEC;
    using RemoteAdmin;

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

        public void SelectRandomZombieType(Player player)
        {
            int r = plugin.Rng.Next(plugin.Config.EnabledZombies.Count - 1);
            Log.Debug($"{nameof(SelectRandomZombieType)}: {plugin.Config.EnabledZombies.Count} -- {plugin.Config.EnabledZombies[r]} -- Ex: {nameof(BallisticZombie)} - {nameof(PlagueZombie)}");
            string name = plugin.Config.EnabledZombies[r];

            switch (name)
            {
                case nameof(BallisticZombie):
                    player.GameObject.AddComponent<BallisticZombie>();
                    break;
                case nameof(BerserkZombie):
                    player.GameObject.AddComponent<BerserkZombie>();
                    break;
                case nameof(DwarfZombie):
                    player.GameObject.AddComponent<DwarfZombie>();
                    break;
                case nameof(MedicZombie):
                    player.GameObject.AddComponent<MedicZombie>();
                    break;
                case nameof(PDZombie):
                    player.GameObject.AddComponent<PDZombie>();
                    break;
                case nameof(PlagueZombie):
                    player.GameObject.AddComponent<PlagueZombie>();
                    break;
                case nameof(TankZombie):
                    player.GameObject.AddComponent<TankZombie>();
                    break;
            }
        }
    }
}