namespace CustomRoles.Abilities
{
    using System;
    using System.Collections.Generic;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.CustomRoles.API.Features;
    using InventorySystem.Items.Firearms.Modules;
    using MEC;
    using UnityEngine;

    public class ChargeAbility : ActiveAbility
    {
        public override string Name { get; set; } = "Charge";
        public override string Description { get; set; } = "Charges towards the target location.";
        public override float Duration { get; set; } = 0f;
        public override float Cooldown { get; set; } = 45f;

        protected override void AbilityUsed(Player player)
        {
            if (RunRaycast(player, out RaycastHit hit))
            {
                Log.Debug($"{player.Nickname} -- {player.Position} - {hit.point}");
                bool line = Physics.Linecast(hit.point, hit.point + (Vector3.down * 5f), out RaycastHit lineHit, player.ReferenceHub.playerMovementSync.CollidableSurfaces);
                if (!line)
                {
                    player.ShowHint("You cannot charge straight up walls, silly.\nYour cooldown has been lowered to 5sec.");
                    LastUsed[player] = DateTime.Now + TimeSpan.FromSeconds(5);
                    
                    return;
                }
                Log.Debug($"{player.Nickname} -- {lineHit.point}");
                Timing.RunCoroutine(MovePlayer(player, hit));
            }
        }

        public bool RunRaycast(Player player, out RaycastHit hit) => Physics.Raycast(player.Position + player.CameraTransform.forward, player.CameraTransform.forward, out hit, 200f, StandardHitregBase.HitregMask);

        public IEnumerator<float> MovePlayer(Player player, RaycastHit hit)
        {
            while ((player.Position - hit.point).sqrMagnitude >= 2.5f)
            {
                player.Position = Vector3.MoveTowards(player.Position, hit.point, 0.5f);

                yield return Timing.WaitForSeconds(0.00025f);
            }

            Timing.CallDelayed(0.5f, () => player.EnableEffect(EffectType.Ensnared, 5f));
            
            Player target = Player.Get(hit.collider.GetComponentInParent<ReferenceHub>());
            if (target != null)
            {
                if ((target.Position - hit.point).sqrMagnitude >= 3f)
                {
                    target.Hurt(35f, DamageTypes.Scp0492, player.Nickname, player.Id);
                    target.EnableEffect(EffectType.Ensnared, 5f);
                }
                else
                    player.Hurt(10f, DamageTypes.Falldown, player.Nickname, player.Id);
            }
            else
                player.Hurt(10f, DamageTypes.Falldown, player.Nickname, player.Id);

            EndAbility(player);
        }
    }
}