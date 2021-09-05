namespace CustomRoles.Abilities
{
    using Exiled.CustomRoles.API.Features;
    using Exiled.Events.EventArgs;

    public class HealOnKill : PassiveAbility
    {
        public override string Name { get; set; } = "Heal on Kill";
        public override string Description { get; set; } = "Heals the player when they kill someone.";
        public float HealAmount { get; set; } = 25f;
        public bool HealOverMax { get; set; } = false;

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
                ev.Killer.Heal(HealAmount, HealOverMax);
            }
        }
    }
}