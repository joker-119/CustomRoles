namespace CustomRoles.Abilities
{
    using System;
    using System.Collections.Generic;
    using CustomRoles.Roles;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using InventorySystem.Items.Firearms.Modules;
    using MEC;
    using PlayableScps.Messages;
    using UnityEngine;

    public class ChargeAbility : CustomAbility
    {
        public override string Name { get; set; } = "Charge";
        protected override string Description { get; set; } = "Charges towards the target location.";

        protected override void LoadEvents()
        {
            if (RunRaycast(out RaycastHit hit))
            {
                Log.Debug($"{Player.Nickname} -- {Player.Position} - {hit.point}");
                bool line = Physics.Linecast(hit.point, hit.point + (Vector3.down * 5f), out RaycastHit lineHit, Player.ReferenceHub.playerMovementSync.CollidableSurfaces);
                if (!line)
                {
                    Player.ShowHint("You cannot charge straight up walls, silly.\nYour cooldown has been lowered to 5sec.");
                    ChargerZombie component = Player.GameObject.GetComponent<ChargerZombie>();
                    if (component != null)
                    {
                        component.UsedAbility = DateTime.Now - TimeSpan.FromSeconds(component.AbilityCooldown - 5);
                    }
                    return;
                }
                Log.Debug($"{Player.Nickname} -- {lineHit.point}");
                Timing.RunCoroutine(MovePlayer(hit));
            }
        }

        public bool RunRaycast(out RaycastHit hit) => Physics.Raycast(Player.Position + Player.CameraTransform.forward, Player.CameraTransform.forward, out hit, 200f, StandardHitregBase.HitregMask);

        public IEnumerator<float> MovePlayer(RaycastHit hit)
        {
            while ((Player.Position - hit.point).sqrMagnitude >= 2.5f)
            {
                Player.Position = Vector3.MoveTowards(Player.Position, hit.point, 0.5f);

                yield return Timing.WaitForSeconds(0.00025f);
            }

            Timing.CallDelayed(0.5f, () => Player.EnableEffect(EffectType.Ensnared, 5f));
            
            Player target = Player.Get(hit.collider.GetComponentInParent<ReferenceHub>());
            if (target != null)
            {
                if ((target.Position - hit.point).sqrMagnitude >= 3f)
                {
                    target.Hurt(35f, DamageTypes.Scp0492, Player.Nickname, Player.Id);
                    target.EnableEffect(EffectType.Ensnared, 5f);
                }
                else
                    Player.Hurt(10f, DamageTypes.Falldown, Player.Nickname, Player.Id);
            }
            else
                Player.Hurt(10f, DamageTypes.Falldown, Player.Nickname, Player.Id);

            Destroy(this);
        }
    }
}