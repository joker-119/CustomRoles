using CustomRoles.API;

namespace CustomRoles.Roles
{
    public class PlagueZombie : CustomRole
    {
        public override RoleType Type { get; set; } = Plugin.Singleton.Config.RoleConfigs.PlagueCfg.RoleType;
        public override int MaxHealth { get; set; } = Plugin.Singleton.Config.RoleConfigs.PlagueCfg.MaxHealth;
        public override string Name { get; set; } = Plugin.Singleton.Config.RoleConfigs.PlagueCfg.Name;
        protected override string Description { get; set; } = 
            "A slightly slower zombie than is infectious with SCP-008 while being much weaker than regular zombies.";
    }
}