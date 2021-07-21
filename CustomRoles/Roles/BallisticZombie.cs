using CustomRoles.API;

namespace CustomRoles.Roles
{
    public class BallisticZombie : CustomRole
    {
        public override RoleType Type { get; set; } = Plugin.Singleton.Config.RoleConfigs.BallisticCfg.RoleType;
        public override int MaxHealth { get; set; } = Plugin.Singleton.Config.RoleConfigs.BallisticCfg.MaxHealth;
        public override string Name { get; set; } = Plugin.Singleton.Config.RoleConfigs.BallisticCfg.Name;
        protected override string Description { get; set; } =
            "A regular zombie that'll explode when killed.";
    }
}