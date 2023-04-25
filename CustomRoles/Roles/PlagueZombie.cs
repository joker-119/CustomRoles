namespace CustomRoles.Roles;

using System.Collections.Generic;
using CustomPlayerEffects;
using CustomRoles.Abilities;
using CustomRoles.API;

using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Spawn;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using Exiled.Loader;

using PlayerRoles;
using PlayerStatsSystem;

using YamlDotNet.Serialization;

using Map = Exiled.Events.Handlers.Map;
using Player = Exiled.Events.Handlers.Player;

[CustomRole(RoleTypeId.Scp0492)]
public class PlagueZombie : CustomRole, ICustomRole
{
    [YamlIgnore]
    public static List<ushort> Grenades { get; } = new ();

    public int Chance { get; set; } = 40;

    public StartTeam StartTeam { get; set; } = StartTeam.Scp | StartTeam.Revived;

    public override uint Id { get; set; } = 11;

    public override RoleTypeId Role { get; set; } = RoleTypeId.Scp0492;

    public override int MaxHealth { get; set; } = 500;

    public override string Name { get; set; } = "Plague Zombie";

    public override string Description { get; set; } =
        "A slower and weaker zombie that is infectious with SCP-008. You can launch a projectile that will poison enemies near where it hits with the console command `.special`.\nIt is recommended you keybind this by running the console command `cmdbind g .special`.\nThis keybind applies to all roles with special abilities.";

    public override string CustomInfo { get; set; } = "Plague Zombie";

    public override SpawnProperties SpawnProperties { get; set; } = new()
    {
        Limit = 1,
    };

    public override List<CustomAbility> CustomAbilities { get; set; } = new()
    {
        new ProjectileAbility(),
        new MoveSpeedReduction(),
    };

    protected override void SubscribeEvents()
    {
        Log.Debug($"{Name} loading events.");
        Player.Hurting += OnHurt;
        Map.ExplodingGrenade += OnExplodingGrenade;
        base.SubscribeEvents();
    }

    protected override void UnsubscribeEvents()
    {
        Log.Debug($"{Name} unloading events.");
        Player.Hurting -= OnHurt;
        Map.ExplodingGrenade -= OnExplodingGrenade;
        base.UnsubscribeEvents();
    }

    private void OnExplodingGrenade(ExplodingGrenadeEventArgs ev)
    {
        Log.Debug($"{Name}: {nameof(OnExplodingGrenade)}: {Grenades.Contains(ev.Projectile.Serial)}");
        if (!Grenades.Contains(ev.Projectile.Serial))
            return;
        ev.IsAllowed = false;
        foreach (Exiled.API.Features.Player player in ev.TargetsToAffect)
        {
            if (player.Role.Team == Team.SCPs || (player.Position - ev.Projectile.Transform.position).sqrMagnitude > 200)
                continue;
            player.Hurt(new UniversalDamageHandler(30f, DeathTranslations.Poisoned));
            player.EnableEffect(EffectType.Poisoned, 25f);
        }
    }

    private void OnHurt(HurtingEventArgs ev)
    {
        if (ev.Player.IsHuman && ev.Player.Health - ev.Amount <= 0 && ev.Player.TryGetEffect(EffectType.Poisoned, out StatusEffectBase poisoned) && poisoned.Intensity > 0)
        {
            ev.IsAllowed = false;
            ev.Amount = 0;
            ev.Player.DropItems();
            ev.Player.Role.Set(RoleTypeId.Scp0492, SpawnReason.ForceClass, RoleSpawnFlags.None);
        }

        if (!Check(ev.Attacker))
            return;

        if (ev.Player.Role.Team == Team.SCPs)
        {
            ev.Amount = 0f;
            return;
        }

        if (ev.DamageHandler.Type == DamageType.Explosion)
        {
            ev.Amount = 0;
            ev.Player.Hurt(new UniversalDamageHandler(30f, DeathTranslations.Poisoned));
            ev.Player.EnableEffect(EffectType.Poisoned);
            return;
        }

        ev.Amount = 10f;

        if (Loader.Random.Next(100) < 60)
            ev.Player.EnableEffect(EffectType.Poisoned);
    }
}