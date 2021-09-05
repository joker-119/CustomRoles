namespace CustomRoles.Abilities
{
    using Exiled.API.Features;
    using Exiled.CustomRoles.API.Features;
    using Exiled.Events.EventArgs;

    public class ReactiveHume : PassiveAbility
    {
        public override string Name { get; set; } = "Reactive Hume Shield";

        public override string Description { get; set; } =
            "A Hume shield that builds up in power as the player takes damage. Instead of negating damage, it instead reduced incoming damage based on how full the shield is.";

        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Hurting += OnHurting;
            base.SubscribeEvents();
        }

        protected override void UnSubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Hurting -= OnHurting;
            base.UnSubscribeEvents();
        }

        private void OnHurting(HurtingEventArgs ev)
        {
            if (Check(ev.Attacker))
                ev.Amount *= 0.4f;
            else if (Check(ev.Target))
            {
                float perc = ev.Target.ArtificialHealth / ev.Target.MaxArtificialHealth;
                float reduction = ev.Amount * (perc * .8f);
                Log.Debug($"{Name}: AHP: {ev.Target.ArtificialHealth} -- Reducing incoming damage by: ({perc * 100}%) -- Base: {ev.Amount} New: {ev.Amount - reduction}", Plugin.Singleton.Config.Debug);
                float amountToAdd = ev.Amount * 0.75f;
                if (ev.Target.ArtificialHealth + amountToAdd > ev.Target.MaxArtificialHealth)
                    ev.Target.ArtificialHealth = (ushort)ev.Target.MaxArtificialHealth;
                else
                    ev.Target.ArtificialHealth += (ushort)(ev.Amount * 0.75f);
                Log.Debug($"{Name}: Adding {ev.Amount * 0.75f} to AHP. New value: {ev.Target.ArtificialHealth}", Plugin.Singleton.Config.Debug);
                
                ev.Amount -= reduction;
            }
        }
    }
}