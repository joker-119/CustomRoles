using CustomRoles.API;

namespace CustomRoles.Roles
{
    public class PDZombie : CustomRole
    {
        public override RoleType Type { get; set; } = Plugin.Singleton.Config.RoleConfigs.PDZombieCfg.RoleType;
        public override int MaxHealth { get; set; } = Plugin.Singleton.Config.RoleConfigs.PDZombieCfg.MaxHealth;
        public override string Name { get; set; } = Plugin.Singleton.Config.RoleConfigs.PDZombieCfg.Name;
        protected override string Description { get; set; } =
            "A zombie with ballistic damage resistance, but is instantly killed by flash grenades. Has a 25% chance when hitting someone to teleport them to the Pocket Dimension";
    }
}