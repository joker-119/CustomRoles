using CustomRoles.API;
using Exiled.API.Extensions;

namespace CustomRoles.Roles
{
    public class TankZombie : CustomRole
    {
        public override RoleType Type { get; set; } = Plugin.Singleton.Config.RoleConfigs.TankZombieCfg.RoleType;
        public override int MaxHealth { get; set; } = Plugin.Singleton.Config.RoleConfigs.TankZombieCfg.MaxHealth;
        public override string Name { get; set; } = Plugin.Singleton.Config.RoleConfigs.TankZombieCfg.Name;
        protected override string Description { get; set; } = 
            "A slightly slower zombie with double the regular health";

        protected override void RoleAdded()
        {
            Player.ChangeRunningSpeed(0.80f);
            Player.ChangeWalkingSpeed(0.80f);
        }

        protected override void RoleRemoved()
        {
            Player.ChangeRunningSpeed(1.8f);
            Player.ChangeWalkingSpeed(1.8f);
        }
    }
}