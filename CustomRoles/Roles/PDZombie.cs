namespace CustomRoles.Roles;

using System.ComponentModel;
using CustomPlayerEffects;

using CustomRoles.API;

using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Loader;

using PlayerRoles;
using PlayerStatsSystem;
using Player = Exiled.Events.Handlers.Player;

[CustomRole(RoleTypeId.Scp0492)]
public class PDZombie : CustomRole, ICustomRole
{
    public int Chance { get; set; } = 15;

    public StartTeam StartTeam { get; set; } = StartTeam.Scp | StartTeam.Revived;

    public override uint Id { get; set; } = 9;

    public override RoleTypeId Role { get; set; } = RoleTypeId.Scp0492;

    public override int MaxHealth { get; set; } = 500;

    public override string Name { get; set; } = "Pocket Dimension Zombie";

    public override string Description { get; set; } =
        "A zombie with ballistic damage resistance, but is instantly killed by flash grenades. Has a 25% chance when hitting someone to teleport them to the Pocket Dimension";

    public override string CustomInfo { get; set; } = "Pocket Dimension Zombie";

    public override SpawnProperties SpawnProperties { get; set; } = new()
    {
        Limit = 1,
    };

    [Description("The chance the zombie has on each melee hit to teleport the target to the pocket dimension.")]
    public int TeleportChance { get; set; } = 25;

    protected override void SubscribeEvents()
    {
        Log.Debug($"{Name}:{nameof(SubscribeEvents)}: loading events.");
        Player.Hurting += OnHurt;
        Player.ReceivingEffect += OnReceivingEffect;
        base.SubscribeEvents();
    }

    protected override void UnsubscribeEvents()
    {
        Log.Debug($"{Name}:{nameof(UnsubscribeEvents)}: unloading events.");
        Player.Hurting -= OnHurt;
        Player.ReceivingEffect -= OnReceivingEffect;
        base.UnsubscribeEvents();
    }

    private void OnHurt(HurtingEventArgs ev)
    {
        if (Check(ev.Attacker))
        {
            int chance = Loader.Random.Next(100);

            if (chance <= TeleportChance)
                ev.Player.EnableEffect(EffectType.Corroding);
        }

        if (Check(ev.Player) && ev.Attacker is { CurrentItem: { }, IsHuman: true } && ev.Attacker.CurrentItem.Type.IsWeapon(false))
            ev.Amount *= 0.20f;
    }

    private void OnReceivingEffect(ReceivingEffectEventArgs ev)
    {
        if (Check(ev.Player) && ev.Effect is Flashed)
            ev.Player.Kill(DeathTranslations.Explosion.LogLabel);
    }
}