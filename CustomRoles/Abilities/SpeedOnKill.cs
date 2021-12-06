namespace CustomRoles.Abilities
{
    using System.ComponentModel;
    using CustomPlayerEffects;
    using Exiled.API.Enums;
    using Exiled.Events.EventArgs;
    using Exiled.Events.Handlers;
    using Generics;

    public class SpeedOnKill : PassiveAbilityResolvable
    {
        public override string Name { get; set; } = "Speed on Kill";
        public override string Description { get; set; } = "Gives the user speed when they kill another player.";
        public float Duration { get; set; } = 5f;

        [Description("The highest intensity level of SCP-207 speed this ability can give.")]
        public byte IntensityLimit { get; set; } = 2;

        protected override void SubscribeEvents()
        {
            Player.Dying += OnDying;
            base.SubscribeEvents();
        }

        protected override void UnSubscribeEvents()
        {
            Player.Dying -= OnDying;
            base.UnSubscribeEvents();
        }

        private void OnDying(DyingEventArgs ev)
        {
            if (Check(ev.Killer))
            {
                var curIntensity = ev.Killer.GetEffectIntensity<Scp207>();
                if (curIntensity < IntensityLimit)
                {
                    ev.Killer.ChangeEffectIntensity<Scp207>((byte)(curIntensity + 1));
                    ev.Killer.GetEffect(EffectType.Scp207).Duration = Duration;
                }
            }
        }
    }
}