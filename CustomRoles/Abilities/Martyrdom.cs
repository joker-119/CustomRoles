namespace CustomRoles.Abilities
{
    using CustomRoles.Abilities.Generics;
    using Exiled.API.Features.Items;
    using Exiled.Events.EventArgs;
    using Exiled.Events.Handlers;
    using InventorySystem.Items.ThrowableProjectiles;

    public class Martyrdom : PassiveAbilityResolvable
    {
        public override string Name { get; set; } = "Martyrdom";
        public override string Description { get; set; } = "Causes the player to explode upon death.";

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
            if (Check(ev.Target))
                ((EffectGrenade)new ExplosiveGrenade(ItemType.GrenadeHE).Spawn(ev.Target.Position).Base)
                    .ServerActivate();
        }
    }
}