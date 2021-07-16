using CustomRoles.API;
using Exiled.API.Enums;
using MEC;

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
            Timing.CallDelayed(2.5f, () =>
            {
                Player.EnableEffect(EffectType.SinkHole);
            });
        }

        protected override void RoleRemoved()
        {
            Player.DisableEffect(EffectType.SinkHole);
        }
    }
}