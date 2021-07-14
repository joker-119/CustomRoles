using CustomRoles.API;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;

namespace CustomRoles.Roles
{
    public class BerserkZombie : CustomRole
    {
        public override RoleType Type { get; set; } = Plugin.Singleton.Config.RoleConfigs.BallisticCfg.RoleType;
        public override int MaxHealth { get; set; } = Plugin.Singleton.Config.RoleConfigs.BallisticCfg.MaxHealth;
        public override string Name { get; set; } = Plugin.Singleton.Config.RoleConfigs.BallisticCfg.Name;

        protected override string Description { get; set; } =
            "A regular zombie that'll explode when killed.";

        protected override void LoadEvents()
        {
            Log.Debug($"{Name} loading events.");
            Exiled.Events.Handlers.Player.Hurting += OnHurt;
            Exiled.Events.Handlers.Player.Died += OnDied;
        }

        protected override void UnloadEvents()
        {
            Log.Debug($"{Name} unloading events.");
            Exiled.Events.Handlers.Player.Hurting -= OnHurt;
            Exiled.Events.Handlers.Player.Died -= OnDied;
        }

        public void OnHurt(HurtingEventArgs ev)
        {
            if (ev.Attacker == Player)
            {
                ev.Amount *= 2f;
            }
        }

        public void OnDied(DiedEventArgs ev)
        {
            if (ev.Killer == Player)
            {
                Player.ChangeRunningSpeed(1.3f);
                Player.ChangeWalkingSpeed(1.3f);
                Timing.CallDelayed(10f, () =>
                {
                    Player.ChangeRunningSpeed(0.3f);
                    Player.ChangeWalkingSpeed(0.3f);
                });
            }
        }
    }
}