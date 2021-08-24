using CustomRoles.API;
using Exiled.API.Enums;
using MEC;

namespace CustomRoles.Roles
{
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using ServerOutput;

    public class TankZombie : CustomRole
    {
        public override RoleType Type { get; set; } = Plugin.Singleton.Config.RoleConfigs.TankZombieCfg.RoleType;
        public override int MaxHealth { get; set; } = Plugin.Singleton.Config.RoleConfigs.TankZombieCfg.MaxHealth;
        public override string Name { get; set; } = Plugin.Singleton.Config.RoleConfigs.TankZombieCfg.Name;
        protected override string Description { get; set; } = 
            "A slightly slower zombie with double the regular health. As you take damage your AHP meter will fill. The higher it's value, the less damage you take.";

        protected override void RoleAdded()
        {
            Log.Debug($"{Name}: Setting Max AHP and Decay", Plugin.Singleton.Config.Debug);
            Player.MaxArtificialHealth = 500;
            Player.ArtificialHealthDecay = 1.5f;
            Timing.CallDelayed(2.5f, () =>
            {
                Player.EnableEffect(EffectType.SinkHole);
            });
        }

        protected override void LoadEvents()
        {
            Exiled.Events.Handlers.Player.Hurting += OnHurting;
            Exiled.Events.Handlers.Player.Dying += OnDying;
        }

        protected override void UnloadEvents()
        {
            Exiled.Events.Handlers.Player.Hurting -= OnHurting;
            Exiled.Events.Handlers.Player.Dying -= OnDying;
        }

        private void OnDying(DyingEventArgs ev)
        {
            if (ev.Target == Player)
                Destroy(this);
        }

        private void OnHurting(HurtingEventArgs ev)
        {
            if (ev.Attacker == Player)
                ev.Amount *= 0.4f;
            else if (ev.Target == Player)
            {
                float perc = Player.ArtificialHealth / Player.MaxArtificialHealth;
                float reduction = ev.Amount * (perc * .8f);
                Log.Debug($"{Name}: AHP: {Player.ArtificialHealth} -- Reducing incoming damage by: ({perc * 100}%) -- Base: {ev.Amount} New: {ev.Amount - reduction}", Plugin.Singleton.Config.Debug);
                float amountToAdd = ev.Amount * 0.75f;
                if (Player.ArtificialHealth + amountToAdd > Player.MaxArtificialHealth)
                    Player.ArtificialHealth = (ushort)Player.MaxArtificialHealth;
                else
                    Player.ArtificialHealth += (ushort)(ev.Amount * 0.75f);
                Log.Debug($"{Name}: Adding {ev.Amount * 0.75f} to AHP. New value: {Player.ArtificialHealth}", Plugin.Singleton.Config.Debug);
                
                ev.Amount -= reduction;
            }
        }

        protected override void RoleRemoved()
        {
            Log.Debug($"{Name}: Resetting AHP values.", Plugin.Singleton.Config.Debug);
            Player.MaxArtificialHealth = 75;
            Player.ArtificialHealth = 0;
            Player.ArtificialHealthDecay = 0.75f;
            Player.DisableEffect(EffectType.SinkHole);
            base.RoleRemoved();
        }
    }
}