namespace CustomRoles.Abilities
{
    using Exiled.API.Features.Items;
    using Exiled.CustomRoles.API.Features;
    using Exiled.Events.EventArgs;
    using Exiled.Events.EventArgs.Player;
    using Exiled.Events.Handlers;
    using Item = Exiled.API.Features.Items.Item;

    public class Martyrdom : PassiveAbility
    {
        public override string Name { get; set; } = "Martyrdom";
        public override string Description { get; set; } = "Causes the player to explode upon death.";

        protected override void SubscribeEvents()
        {
            Player.Dying += OnDying;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Player.Dying -= OnDying;
            base.UnsubscribeEvents();
        }

        private void OnDying(DyingEventArgs ev)
        {
            if (Check(ev.Player))
                ((ExplosiveGrenade)Item.Create(ItemType.GrenadeHE)).SpawnActive(ev.Player.Position, ev.Player);
        }
    }
}