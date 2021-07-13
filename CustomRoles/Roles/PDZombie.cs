using System;
using CustomPlayerEffects;
using CustomRoles.API;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs;

namespace CustomRoles.Roles
{
    public class PDZombie : CustomRole
    {
        public override RoleType Type { get; set; } = Plugin.Singleton.Config.RoleConfigs.PDZombieCfg.RoleType;
        public override int MaxHealth { get; set; } = Plugin.Singleton.Config.RoleConfigs.PDZombieCfg.MaxHealth;
        public override string Name { get; set; } = Plugin.Singleton.Config.RoleConfigs.PDZombieCfg.Name;
        protected override string Description { get; set; } =
            "A zombie with ballistic damage resistance, but is instantly killed by flash grenades. Has a 25% chance when hitting someone to teleport them to the Pocket Dimension";

        private Random _random;
        protected override void RoleAdded()
        {
            try
            {
                Log.Debug($"{Name} added to {Player.Nickname}");
            }
            catch (Exception e)
            {
                Log.Error($"{Name}: {e}\n{e.StackTrace}");
            }
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

        private void OnHurt(HurtingEventArgs ev)
        {
            if (ev.Attacker == Player && _random.Next(1, 100) < 25)
            {
                ev.Target.EnableEffect(EffectType.Corroding);
            }

            if (ev.Target == Player && ev.Attacker.IsHuman)
            {
                switch (Extensions.IsGun(ev.Attacker.CurrentItem.id))
                {
                    case true:
                        ev.Amount *= 0.20f;
                        break;
                }
            }
        }

        private void OnFlashed(ReceivingEffectEventArgs ev)
        {
            if (ev.Player == Player && ev.Effect == ev.Player.GetEffect(EffectType.Flashed))
            {
                Player.Kill(DamageTypes.RagdollLess);
            }
        }
    }
}