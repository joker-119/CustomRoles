namespace CustomRoles.Abilities
{
    using System;
    using System.Collections.Generic;
    using CustomRoles.Roles;
    using Exiled.API.Enums;
    using Exiled.API.Extensions;
    using Exiled.API.Features;
    using Exiled.API.Features.Items;
    using InventorySystem.Items.Firearms.Modules;
    using MEC;
    using Mirror;
    using UnityEngine;

    public class ProjectileAbility : CustomAbility
    {
        public override string Name { get; set; } = "Projectile";
        protected override string Description { get; set; } = "Shoots an item in the direction you are facing.";
        private Pickup _pickup;
        private bool _deleted;
        
        protected override void LoadEvents()
        {
            if (RunRaycast(out RaycastHit hit))
            {
                if ((Player.Position - hit.point).sqrMagnitude > 400)
                {
                    target = Vector3.MoveTowards(Player.Position, hit.point, 20f);

                    if (Physics.Linecast(target, Vector3.down * 20f, out RaycastHit lineHit, Player.ReferenceHub.playerMovementSync.CollidableSurfaces))
                        target = lineHit.point;
                }
                else
                    target = hit.point;

                Pickup pickup = new Item(ItemType.SCP018).Spawn(Player.CameraTransform.position);
                NetworkServer.UnSpawn(pickup.Base.gameObject);
                pickup.Base.gameObject.transform.localScale = Vector3.one * 2f;
                NetworkServer.Spawn(pickup.Base.gameObject);
                _pickup = pickup;
                Rigidbody body = pickup.Base.GetComponent<Rigidbody>();
                body.useGravity = false;
                body.isKinematic = false;
                _startPosition = pickup.Position;
                _stepScale = speed / Vector3.Distance(_startPosition, target);
                _deleted = false;
            }
        }

        public bool RunRaycast(out RaycastHit hit) => Physics.Raycast(Player.Position + Player.CameraTransform.forward, Player.CameraTransform.forward, out hit, 200f, StandardHitregBase.HitregMask);

        public float speed = 8f;
        public Vector3 target;
        public float arcHeight = 3f;

        private Vector3 _startPosition;
        private float _stepScale;
        private float _progress;

        private void Update()
        {
            if (_deleted)
                return;
            // Increment our progress from 0 at the start, to 1 when we arrive.
            _progress = Mathf.Min(_progress + Time.deltaTime * _stepScale, 1.0f);

            // Turn this 0-1 value into a parabola that goes from 0 to 1, then back to 0.
            float parabola = 1.0f - 4.0f * (_progress - 0.5f) * (_progress - 0.5f);

            // Travel in a straight line from our start position to the target.        
            Vector3 nextPos = Vector3.Lerp(_startPosition, target, _progress);

            // Then add a vertical arc in excess of this.
            nextPos.y += parabola * arcHeight;

            // Continue as before.
            _pickup.Position = nextPos;

            // I presume you disable/destroy the arrow in Arrived so it doesn't keep arriving.
            if(_progress == 1.0f)
                Arrived();
        }

        private void Arrived()
        {
            PlagueZombie.Grenades.Add(new ExplosiveGrenade(ItemType.GrenadeHE).Spawn(target));
            _deleted = true;
            _pickup.Destroy();
            Destroy(this);
        }
    }
}