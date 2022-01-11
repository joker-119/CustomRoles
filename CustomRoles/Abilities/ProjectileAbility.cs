namespace CustomRoles.Abilities
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using CustomRoles.Abilities.Generics;
    using CustomRoles.Roles;
    using Exiled.API.Features;
    using Exiled.API.Features.Items;
    using InventorySystem.Items.Firearms.Modules;
    using MEC;
    using Mirror;
    using UnityEngine;

    public class ProjectileAbility : ActiveAbilityResolvable
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
            Vector3 target = Vector3.zero;
            if (RunRaycast(player, out RaycastHit hit))
            {
                if ((player.Position - hit.point).sqrMagnitude > 400)
                {
                    target = Vector3.MoveTowards(player.Position, hit.point, 20f);

                    if (Physics.Linecast(target, Vector3.down * 20f, out RaycastHit lineHit,
                        player.ReferenceHub.playerMovementSync.CollidableSurfaces))
                        target = lineHit.point;
                }
                else
                {
                    target = hit.point;
                }

                Pickup pickup = Item.Create(ItemType.SCP018).Spawn(player.CameraTransform.position);
                NetworkServer.UnSpawn(pickup.Base.gameObject);
                GameObject gameObject = pickup.Base.gameObject;
                gameObject.transform.localScale = Vector3.one * 2f;
                NetworkServer.Spawn(gameObject);
                Rigidbody body = pickup.Base.GetComponent<Rigidbody>();
                body.useGravity = false;
                body.isKinematic = false;
                Timing.RunCoroutine(Update(pickup, target));
            }
        }

        public bool RunRaycast(Player player, out RaycastHit hit)
        {
            Vector3 forward = player.CameraTransform.forward;
            return Physics.Raycast(player.Position + forward, forward,
                out hit, 200f, StandardHitregBase.HitregMask);
        }

        private IEnumerator<float> Update(Pickup pickup, Vector3 target)
        {
            bool deleted = false;
            Vector3 startPosition = pickup.Position;
            float stepScale = Speed / Vector3.Distance(startPosition, target);
            float progress = 0f;
            for (;;)
            {
                if (deleted)
                    yield break;
                // Increment our progress from 0 at the start, to 1 when we arrive.
                progress = Mathf.Min(progress + Time.deltaTime * stepScale, 1.0f);

                // Turn this 0-1 value into a parabola that goes from 0 to 1, then back to 0.
                float parabola = 1.0f - 4.0f * (progress - 0.5f) * (progress - 0.5f);

                // Travel in a straight line from our start position to the target.        
                Vector3 nextPos = Vector3.Lerp(startPosition, target, progress);

                // Then add a vertical arc in excess of this.
                nextPos.y += parabola * ArcHeight;

                // Continue as before.
                pickup.Position = nextPos;

                // I presume you disable/destroy the arrow in Arrived so it doesn't keep arriving.
                if (progress == 1.0f)
                {
                    Arrived(target);
                    pickup.Destroy();
                    deleted = true;
                }
            }
        }

        private void Arrived(Vector3 target) => 
            PlagueZombie.Grenades.Add(Item.Create(ItemType.GrenadeHE).Spawn(target));
    }
}