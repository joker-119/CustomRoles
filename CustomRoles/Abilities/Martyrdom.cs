namespace CustomRoles.Abilities
{
    using CustomRoles.Abilities.Generics;
    using Exiled.API.Features.Items;
    using Exiled.Events.EventArgs;
    using Exiled.Events.Handlers;
    using InventorySystem.Items.ThrowableProjectiles;
    using Item = Exiled.API.Features.Items.Item;

    public class Martyrdom : PassiveAbilityResolvable
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
            if (Check(ev.Target))
                ((ExplosiveGrenade)Item.Create(ItemType.GrenadeHE)).SpawnActive(ev.Target.Position, ev.Target);
        }
    }
}