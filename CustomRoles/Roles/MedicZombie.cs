using CustomRoles.Abilities;
using CustomRoles.API;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs;

namespace CustomRoles.Roles
{
    public class MedicZombie : CustomRole
    {
        public override RoleType Type { get; set; } = Plugin.Singleton.Config.RoleConfigs.MedicZombieCfg.RoleType;
        public override int MaxHealth { get; set; } = Plugin.Singleton.Config.RoleConfigs.MedicZombieCfg.MaxHealth;
        public override string Name { get; set; } = Plugin.Singleton.Config.RoleConfigs.MedicZombieCfg.Name;

        protected override int AbilityCooldown { get; set; } =
            Plugin.Singleton.Config.RoleConfigs.MedicZombieCfg.AbilityCooldown;

        protected override string Description { get; set; } =
            "A slightly slower and weaker zombie that heals nearby SCPs";

        public override string UseAbility()
        {
            Player.GameObject.AddComponent<ZombieMist>();
            return "Ability used.";
        }
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
        protected override void LoadEvents()
        {
            Log.Debug($"{Name} loading events.");
            Exiled.Events.Handlers.Player.Hurting += OnHurt;
        }
        protected override void UnloadEvents()
        {
            Log.Debug($"{Name} unloading events.");
            Exiled.Events.Handlers.Player.Hurting -= OnHurt;
        }

        public void OnHurt(HurtingEventArgs ev)
        {
            if (ev.Attacker == Player)
            {
                ev.Amount *= .75f;
            }
        }
    }
}