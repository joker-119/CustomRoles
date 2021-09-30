namespace CustomRoles.Abilities
{
    using System.ComponentModel;
    using CustomPlayerEffects;
    using Exiled.API.Enums;
    using Exiled.CustomRoles.API.Features;
    using Exiled.Events.EventArgs;

    public class SpeedOnKill : PassiveAbility
    {
        public override string Name { get; set; } = "Speed on Kill";
        public override string Description { get; set; } = "Gives the user speed when they kill another player.";
        public float Duration { get; set; } = 5f;
        
        [Description("The highest intensity level of SCP-207 speed this ability can give.")]
        public byte IntensityLimit { get; set; } = 2;

        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Dying += OnDying;
            base.SubscribeEvents();
        }

        protected override void UnSubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Dying -= OnDying;
            base.UnSubscribeEvents();
        }

        private void OnDying(DyingEventArgs ev)
        {
            if (Check(ev.Killer))
            {
                byte curIntensity = ev.Killer.GetEffectIntensity<Scp207>();
                if (curIntensity < IntensityLimit)
                {
                    ev.Killer.ChangeEffectIntensity<Scp207>((byte)(curIntensity + 1));
                    ev.Killer.GetEffect(EffectType.Scp207).Duration = Duration;
                }
            }
        }
    }
}