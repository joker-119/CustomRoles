using CustomRoles.Roles;
using Exiled.API.Features;
using Exiled.API.Interfaces;

namespace CustomRoles
{
    using Exiled.CustomRoles.API;
    using Exiled.CustomRoles.API.Features;

    public class Methods
    {
        private readonly Plugin plugin;
        public Methods(Plugin plugin) => this.plugin = plugin;

        public void SelectRandomZombieType(Player player)
        {
            int r = plugin.Rng.Next(plugin.Config.EnabledZombies.Count - 1);
            Log.Debug($"{nameof(SelectRandomZombieType)}: {plugin.Config.EnabledZombies.Count} -- {plugin.Config.EnabledZombies[r]} -- Ex: {nameof(BallisticZombie)} - {nameof(PlagueZombie)}");
            string name = plugin.Config.EnabledZombies[r];
            if (!CustomRole.TryGet(name, out CustomRole role))
            {
                Log.Warn($"{nameof(SelectRandomZombieType)}: {name} is not a valid custom role.");
                return;
            }
            role.AddRole(player);
        }

        public void RegisterRoles()
        {
            Plugin.Singleton.Config.RoleConfigs.BallisticZombies?.Register();
            Plugin.Singleton.Config.RoleConfigs.BerserkZombies?.Register();
            Plugin.Singleton.Config.RoleConfigs.ChargerZombies?.Register();
            Plugin.Singleton.Config.RoleConfigs.Demolitionists?.Register();
            Plugin.Singleton.Config.RoleConfigs.Dwarves?.Register();
            Plugin.Singleton.Config.RoleConfigs.DwarfZombies?.Register();
            Plugin.Singleton.Config.RoleConfigs.Medics?.Register();
            Plugin.Singleton.Config.RoleConfigs.MedicZombies?.Register();
            Plugin.Singleton.Config.RoleConfigs.PdZombies?.Register();
            Plugin.Singleton.Config.RoleConfigs.Phantoms?.Register();
            Plugin.Singleton.Config.RoleConfigs.PlagueZombies?.Register();
            Plugin.Singleton.Config.RoleConfigs.Scp575s?.Register();
            Plugin.Singleton.Config.RoleConfigs.TankZombies?.Register();
        }
        
        public void UnregisterRoles()
        {
            Plugin.Singleton.Config.RoleConfigs.BallisticZombies?.Unregister();
            Plugin.Singleton.Config.RoleConfigs.BerserkZombies?.Unregister();
            Plugin.Singleton.Config.RoleConfigs.ChargerZombies?.Unregister();
            Plugin.Singleton.Config.RoleConfigs.Demolitionists?.Unregister();
            Plugin.Singleton.Config.RoleConfigs.Dwarves?.Unregister();
            Plugin.Singleton.Config.RoleConfigs.DwarfZombies?.Unregister();
            Plugin.Singleton.Config.RoleConfigs.Medics?.Unregister();
            Plugin.Singleton.Config.RoleConfigs.MedicZombies?.Unregister();
            Plugin.Singleton.Config.RoleConfigs.PdZombies?.Unregister();
            Plugin.Singleton.Config.RoleConfigs.Phantoms?.Unregister();
            Plugin.Singleton.Config.RoleConfigs.PlagueZombies?.Unregister();
            Plugin.Singleton.Config.RoleConfigs.Scp575s?.Unregister();
            Plugin.Singleton.Config.RoleConfigs.TankZombies?.Unregister();
        }
    }
}