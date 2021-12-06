namespace CustomRoles.Abilities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Generics;
    using InventorySystem.Items.Firearms.Modules;
    using MEC;
    using PlayerStatsSystem;
    using UnityEngine;

    public class ChargeAbility : ActiveAbilityResolvable
    {
        public override string Name { get; set; } = "Charge";
        public override string Description { get; set; } = "Charges towards the target location.";
        public override float Duration { get; set; } = 0f;
        public override float Cooldown { get; set; } = 45f;

        [Description("The amount of damage inflicted when the player collides with something.")]
        public float ContactDamage { get; set; } = 15f;

        [Description("The bonus multiplier if the target player wasn't moving.")]
        public float AccuracyMultiplier { get; set; } = 2f;

        [Description("How long the ensnare effect lasts.")]
        public float EnsnareDuration { get; set; } = 5f;

        protected override void AbilityUsed(Player player)
        {
            if (RunRaycast(player, out var hit))
            {
                Log.Debug($"{player.Nickname} -- {player.Position} - {hit.point}");
                var line = Physics.Linecast(hit.point, hit.point + Vector3.down * 5f, out var lineHit,
                    player.ReferenceHub.playerMovementSync.CollidableSurfaces);
                if (!line)
                {
                    player.ShowHint(
                        "You cannot charge straight up walls, silly.\nYour cooldown has been lowered to 5sec.");
                    LastUsed[player] = DateTime.Now + TimeSpan.FromSeconds(5);

                    return;
                }

                Log.Debug($"{player.Nickname} -- {lineHit.point}");
                Timing.RunCoroutine(MovePlayer(player, hit));
            }
        }

        public bool RunRaycast(Player player, out RaycastHit hit)
        {
            return Physics.Raycast(player.Position + player.CameraTransform.forward, player.CameraTransform.forward,
                out hit, 200f, StandardHitregBase.HitregMask);
        }

        public IEnumerator<float> MovePlayer(Player player, RaycastHit hit)
        {
            while ((player.Position - hit.point).sqrMagnitude >= 2.5f)
            {
                player.Position = Vector3.MoveTowards(player.Position, hit.point, 0.5f);

                yield return Timing.WaitForSeconds(0.00025f);
            }

            Timing.CallDelayed(0.5f, () => player.EnableEffect(EffectType.Ensnared, EnsnareDuration));

            var target = Player.Get(hit.collider.GetComponentInParent<ReferenceHub>());
            if (target != null)
            {
                if ((target.Position - hit.point).sqrMagnitude >= 3f)
                {
                    target.Hurt(new ScpDamageHandler(player.ReferenceHub, ContactDamage * AccuracyMultiplier,
                        DeathTranslations.Zombie));
                    target.EnableEffect(EffectType.Ensnared, EnsnareDuration);
                }
                else
                {
                    player.Hurt(new UniversalDamageHandler(ContactDamage, DeathTranslations.Falldown));
                }
            }
            else
            {
                player.Hurt(new UniversalDamageHandler(ContactDamage, DeathTranslations.Falldown));
            }

            EndAbility(player);
        }
    }
}