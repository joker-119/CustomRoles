namespace CustomRoles
{
    using CustomRoles.Roles;
    using Exiled.API.Features;
    using Exiled.CustomRoles.API.Features;

    public class Methods
    {
        private readonly Plugin plugin;

        public Methods(Plugin plugin)
        {
            this.plugin = plugin;
        }

        public void SelectRandomZombieType(Player player)
        {
            int r = plugin.Rng.Next(plugin.Config.EnabledZombies.Count - 1);
            Log.Debug(
                $"{nameof(SelectRandomZombieType)}: {plugin.Config.EnabledZombies.Count} -- {plugin.Config.EnabledZombies[r]} -- Ex: {nameof(BallisticZombie)} - {nameof(PlagueZombie)}");
            string name = plugin.Config.EnabledZombies[r];
            if (!CustomRole.TryGet(name, out CustomRole role))
            {
                Log.Warn($"{nameof(SelectRandomZombieType)}: {name} is not a valid custom role.");
                return;
            }

            role.AddRole(player);
        }
    }
}