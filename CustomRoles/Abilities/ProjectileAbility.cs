namespace CustomRoles.Abilities
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Exiled.API.Features;
    using Exiled.API.Features.Items;
    using Generics;
    using InventorySystem.Items.Firearms.Modules;
    using MEC;
    using Mirror;
    using Roles;
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
            var target = Vector3.zero;
            if (RunRaycast(player, out var hit))
            {
                if ((player.Position - hit.point).sqrMagnitude > 400)
                {
                    target = Vector3.MoveTowards(player.Position, hit.point, 20f);

                    if (Physics.Linecast(target, Vector3.down * 20f, out var lineHit,
                        player.ReferenceHub.playerMovementSync.CollidableSurfaces))
                        target = lineHit.point;
                }
                else
                {
                    target = hit.point;
                }

                var pickup = new Item(ItemType.SCP018).Spawn(player.CameraTransform.position);
                NetworkServer.UnSpawn(pickup.Base.gameObject);
                pickup.Base.gameObject.transform.localScale = Vector3.one * 2f;
                NetworkServer.Spawn(pickup.Base.gameObject);
                var body = pickup.Base.GetComponent<Rigidbody>();
                body.useGravity = false;
                body.isKinematic = false;
                Timing.RunCoroutine(Update(pickup, target));
            }
        }

        public bool RunRaycast(Player player, out RaycastHit hit)
        {
            return Physics.Raycast(player.Position + player.CameraTransform.forward, player.CameraTransform.forward,
                out hit, 200f, StandardHitregBase.HitregMask);
        }

        private IEnumerator<float> Update(Pickup pickup, Vector3 target)
        {
            var deleted = false;
            var startPosition = pickup.Position;
            var stepScale = Speed / Vector3.Distance(startPosition, target);
            var progress = 0f;
            for (;;)
            {
                if (deleted)
                    yield break;
                // Increment our progress from 0 at the start, to 1 when we arrive.
                progress = Mathf.Min(progress + Time.deltaTime * stepScale, 1.0f);

                // Turn this 0-1 value into a parabola that goes from 0 to 1, then back to 0.
                var parabola = 1.0f - 4.0f * (progress - 0.5f) * (progress - 0.5f);

                // Travel in a straight line from our start position to the target.        
                var nextPos = Vector3.Lerp(startPosition, target, progress);

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

        private void Arrived(Vector3 target)
        {
            PlagueZombie.Grenades.Add(new ExplosiveGrenade(ItemType.GrenadeHE).Spawn(target));
        }
    }
}