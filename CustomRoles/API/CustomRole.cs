using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using UnityEngine;

namespace CustomRoles.API
{
    using Exiled.API.Enums;

    public abstract class CustomRole : MonoBehaviour
    {
        public DateTime UsedAbility { get; set; }
        protected Player Player;
        public abstract RoleType Type { get; set; }
        public abstract int MaxHealth { get; set; }
        public abstract string Name { get; set; }
        protected abstract string Description { get; set; }
        public virtual int AbilityCooldown { get; set; }
        protected virtual List<string> Inventory { get; set; } = new List<string>();
        protected virtual Dictionary<Vector3, float> SpawnLocations { get; set; } = new Dictionary<Vector3, float>();
        protected virtual bool KeepInventoryOnChange { get; set; } = false;
        protected virtual bool LoseRoleOnChange { get; set; } = true;

        public virtual string UseAbility() => "You have no custom ability to use.";

        protected void Start()
        {
            Player = Exiled.API.Features.Player.Get(gameObject);
            if (Player == null)
            {
                Log.Error($"{Name} was given to a game object that isn't a player!");
                Destroy(this);
            }
            
            foreach (CustomRole role in Player.GetPlayerRoles())
                if (role.Name != Name)
                {
                    Log.Warn($"{Name}: Destroying {role.Name}");
                    Destroy(role);
                }

            Timing.CallDelayed(0.25f, () =>
            {
                foreach (CustomRole role in Player.GetPlayerRoles())
                    Log.Warn($"{Name} {Player.Nickname} has {role.Name}");

                if (Player.GetPlayerRoles().Any(r => r.Name != this.Name))
                {
                    Log.Warn("Has role not this, returning.");
                    return;
                }

                LoadEvents();
                Add();
                Exiled.Events.Handlers.Player.ChangingRole += OnChangingRole;
                Exiled.Events.Handlers.Player.Dying += OnDying;
            });
        }

        private void OnDying(DyingEventArgs ev)
        {
            if (ev.Target == Player)
                Remove();
        }

        protected void OnDestroy()
        {
            Player.CustomInfo = string.Empty;
            Exiled.Events.Handlers.Player.ChangingRole -= OnChangingRole;
            Exiled.Events.Handlers.Player.Dying -= OnDying;
            UnloadEvents();
            RoleRemoved();
        }

        protected void Remove() => Destroy(this);

        protected void TryAddItem(string itemName)
        {
            ItemType type;

            try
            {
                type = (ItemType) Enum.Parse(typeof(ItemType), itemName);
            }
            catch (Exception)
            {
                if (CustomItem.TryGet(itemName, out CustomItem item))
                    item.Give(Player, false);
                else
                    Log.Warn($"{Name} tried to give {Player.Nickname} a {itemName} but it was neither an ItemType nor a CustomItem.");

                return;
            }

            Player.AddItem(type);
        }

        public virtual bool CanUseAbility(out DateTime usableTime)
        {
            usableTime = UsedAbility + TimeSpan.FromSeconds(AbilityCooldown);
            Log.Debug($"{nameof(CanUseAbility)}: {UsedAbility}, {usableTime}", Plugin.Singleton.Config.Debug);

            return DateTime.Now > usableTime;
        }

        protected Vector3 GetSpawnPosition()
        {
            if (SpawnLocations.Count == 0)
                return Vector3.zero;

            foreach (KeyValuePair<Vector3, float> kvp in SpawnLocations)
                if (Plugin.Singleton.Rng.Next(100) <= kvp.Value)
                    return kvp.Key;
            return SpawnLocations.ElementAt(Plugin.Singleton.Rng.Next(SpawnLocations.Count)).Key;
        }

        protected virtual void Add()
        {
            Log.Debug($"Adding {Name} to {Player.Nickname}..");
            if (Type != RoleType.None)
            {
                Log.Debug($"{Name}: Setting player role.");
                Player.SetRole(Type, SpawnReason.ForceClass, true);
            }

            Timing.CallDelayed(1.5f, () =>
            {
                Vector3 pos = GetSpawnPosition();
                Log.Debug($"{Name}: Got position: {pos}");
                if (pos != Vector3.zero)
                    Player.Position = pos + Vector3.up * 1.5f;

                if (!KeepInventoryOnChange)
                {
                    Log.Debug($"{Name}: Clearing inventory");
                    Player.ClearInventory();
                }

                foreach (string itemName in Inventory)
                {
                    Log.Debug($"{Name}: Adding {itemName} to inventory.");
                    TryAddItem(itemName);
                }

                Player.Ammo[ItemType.Ammo9x19] = 100;
                Player.Ammo[ItemType.Ammo12gauge] = 100;
                Player.Ammo[ItemType.Ammo44cal] = 100;
                Player.Ammo[ItemType.Ammo556x45] = 100;
                Player.Ammo[ItemType.Ammo762x39] = 100;

                Log.Debug($"{Name}: Setting health values..");
                Player.Health = MaxHealth;
                Player.MaxHealth = MaxHealth;
            });

            Log.Debug($"{Name}: Setting info");
            Player.CustomInfo = $"{Name} (Custom Role)";
            ShowMessage();
            RoleAdded();
        }

        protected virtual void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (ev.Player == Player)
                Remove();
        }

        protected virtual void ShowMessage() => Player.ShowHint($"You have spawned as a {Name}\n{Description}", 30f);
        protected virtual void LoadEvents() {}
        protected virtual void UnloadEvents() {}
        protected virtual void RoleAdded() {}
        protected virtual void RoleRemoved() {}
    }
}