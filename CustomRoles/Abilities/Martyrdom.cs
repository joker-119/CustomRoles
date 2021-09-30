namespace CustomRoles.Abilities
{
    using Exiled.API.Features.Items;
    using Exiled.CustomRoles.API.Features;
    using Exiled.Events.EventArgs;
    using InventorySystem.Items.ThrowableProjectiles;

    public class Martyrdom : PassiveAbility
    {
        public override string Name { get; set; } = "Martyrdom";
        public override string Description { get; set; } = "Causes the player to explode upon death.";

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
            if (Check(ev.Target))
                ((EffectGrenade)new ExplosiveGrenade(ItemType.GrenadeHE).Spawn(ev.Target.Position).Base).ServerActivate();
        }
    }
}