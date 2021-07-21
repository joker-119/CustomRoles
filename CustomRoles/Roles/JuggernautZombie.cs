using CustomRoles.API;
using Exiled.API.Extensions;
using Exiled.Events.EventArgs;

namespace CustomRoles.Roles
{
    public class JuggernautZombie : CustomRole
    {
        public override RoleType Type { get; set; } = RoleType.Scp0492;
        public override int MaxHealth { get; set; } = Plugin.Singleton.Config.RoleConfigs.JuggCfg.MaxHealth;
        public override string Name { get; set; } = Plugin.Singleton.Config.RoleConfigs.JuggCfg.Name;
        protected override string Description { get; set; } = Plugin.Singleton.Config.RoleConfigs.JuggCfg.Description;

        protected override void RoleAdded()
        {
            Player.ChangeRunningSpeed(Plugin.Singleton.Config.RoleConfigs.JuggCfg.MovementSpeed);
            Player.ChangeWalkingSpeed(Plugin.Singleton.Config.RoleConfigs.JuggCfg.MovementSpeed);
            Player.Scale = Plugin.Singleton.Config.RoleConfigs.JuggCfg.Size;
        }
    }
}