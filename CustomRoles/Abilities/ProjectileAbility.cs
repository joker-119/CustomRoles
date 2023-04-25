namespace CustomRoles.Abilities;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using CustomRoles.Roles;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using Exiled.CustomRoles.API.Features;
using InventorySystem.Items.Firearms.Modules;
using MEC;
using Mirror;
using UnityEngine;

[CustomAbility]
public class ProjectileAbility : ActiveAbility
{
    public override string Name { get; set; } = "Projectile";

    public override string Description { get; set; } = "Shoots an item in the direction you are facing.";

    public override float Duration { get; set; } = 1f;

    public override float Cooldown { get; set; } = 35f;

    [Description("The speed of the projectile.")]
    public float Speed { get; set; } = 8f;

    [Description("How high the projectile arcs upwards when thrown.")]
    public float ArcHeight { get; set; } = 3f;

    protected override void AbilityUsed(Player player)
    {
        Log.Debug("Projectile used.");
        Vector3 target = Vector3.zero;
        if (RunRaycast(player, out RaycastHit hit))
        {
            Log.Debug("Raycast hits");
            if ((player.Position - hit.point).sqrMagnitude > 400)
            {
                Log.Debug("over max distance");
                target = Vector3.MoveTowards(player.Position, hit.point, 20f);

                if (Physics.Linecast(target, Vector3.down * 20f, out RaycastHit lineHit))
                {
                    Log.Debug("Max distance linecast");
                    target = lineHit.point;
                }
            }
            else
            {
                target = hit.point;
            }

            Log.Debug("Spawning pickup");
            Pickup pickup = Pickup.CreateAndSpawn(ItemType.SCP018, player.CameraTransform.position, default);
            pickup.Scale *= 2f;
            Rigidbody body = pickup.Base.GetComponent<Rigidbody>();
            body.useGravity = false;
            body.isKinematic = false;
            Log.Debug("Starting throw");
            Timing.RunCoroutine(DoLerpArc(pickup, target));
        }
    }

    public bool RunRaycast(Player player, out RaycastHit hit)
    {
        Vector3 forward = player.CameraTransform.forward;
        return Physics.Raycast(player.Position + forward, forward,
                               out hit, 200f, StandardHitregBase.HitregMask);
    }

    private IEnumerator<float> DoLerpArc(Pickup pickup, Vector3 target)
    {
        bool deleted = false;
        Vector3 startPosition = pickup.Position;
        float stepScale = Speed / Vector3.Distance(startPosition, target);
        float progress = 0f;
        for (;;)
        {
            Log.Debug("Moving object");
            if (deleted)
            {
                Log.Debug("Is deleted.");
                yield break;
            }

            progress = Mathf.Min(progress + (Time.deltaTime * stepScale), 1.0f);

            float parabola = 1.0f - (4.0f * (progress - 0.5f) * (progress - 0.5f));

            Vector3 nextPos = Vector3.Lerp(startPosition, target, progress);

            nextPos.y += parabola * ArcHeight;

            if (Physics.Linecast(pickup.Position, nextPos, out RaycastHit hit, StandardHitregBase.HitregMask))
            {
                progress = 1.0f;
                target = hit.point;
            }
            else
                pickup.Position = nextPos;

            if (progress >= 1.0f)
            {
                Log.Debug("Arrived");
                Arrived(target);
                pickup.Destroy();
                deleted = true;
            }

            yield return Timing.WaitForOneFrame;
        }
    }

    private void Arrived(Vector3 target)
    {
        ExplosiveGrenade grenade = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE);
        grenade.FuseTime = 0.5f;
        PlagueZombie.Grenades.Add(grenade.Serial);
        grenade.SpawnActive(target);
    }
}