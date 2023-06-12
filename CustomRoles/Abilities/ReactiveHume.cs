namespace CustomRoles.Abilities;

using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using Player = Exiled.Events.Handlers.Player;

[CustomAbility]
public class ReactiveHume : PassiveAbility
{
    public override string Name { get; set; } = "Reactive Hume Shield";

    public override string Description { get; set; } =
        "A Hume shield that builds up in power as the player takes damage. Instead of negating damage, it instead reduced incoming damage based on how full the shield is.";

    protected override void SubscribeEvents()
    {
        Player.Hurting += OnHurting;
        base.SubscribeEvents();
    }

    protected override void UnsubscribeEvents()
    {
        Player.Hurting -= OnHurting;
        base.UnsubscribeEvents();
    }

    private void OnHurting(HurtingEventArgs ev)
    {
        if (Check(ev.Attacker))
        {
            ev.Amount *= 0.4f;
        }
        else if (Check(ev.Player))
        {
            ev.IsAllowed = false;
            float perc = ev.Player.ArtificialHealth / ev.Player.MaxArtificialHealth;
            float reduction = ev.Amount * (perc * .8f);
            Log.Debug(
                $"{Name}: AHP: {ev.Player.ArtificialHealth} -- Reducing incoming damage by: ({perc * 100}%) -- Base: {ev.Amount} New: {ev.Amount - reduction}");
            float amountToAdd = ev.Amount * 0.75f;
            if (ev.Player.ArtificialHealth + amountToAdd > ev.Player.MaxArtificialHealth)
                ev.Player.ArtificialHealth = (ushort)ev.Player.MaxArtificialHealth;
            else
                ev.Player.ArtificialHealth += (ushort)(ev.Amount * 0.75f);
            Log.Debug($"{Name}: Adding {ev.Amount * 0.75f} to AHP. New value: {ev.Player.ArtificialHealth}");

            ev.Amount -= reduction;
            Log.Debug($"Dealing {ev.Amount} damage.");
            if (ev.Player.Health - ev.Amount <= 0)
                ev.Player.Kill(ev.DamageHandler);
            else
                ev.Player.Health -= ev.Amount;
        }
    }
}