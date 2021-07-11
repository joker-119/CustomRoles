using Exiled.API.Features;
using Exiled.Events.EventArgs;
using UnityEngine;

namespace CustomRoles.Abilities
{
    public abstract class CustomAbility : MonoBehaviour
    {
        protected Player Player;
        public abstract string Name { get; set; }
        protected abstract string Description { get; set; }
        protected virtual float Duration { get; set; }

        private void Start()
        {
            Player = Player.Get(gameObject);

            if (Player == null)
            {
                Log.Error($"{Name} was added to a game object that isn't a player!");
                Destroy(this);
                return;
            }

            Exiled.Events.Handlers.Player.ChangingRole += OnChangingRole;
            LoadEvents();
        }

        protected virtual void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (ev.Player == Player)
                Destroy(this);
        }

        private void OnDestroy()
        {
            UnloadEvents();
        }
        
        protected virtual void LoadEvents(){}
        protected virtual void UnloadEvents(){}

        protected virtual void ShowMessage() =>
            Player.ShowHint($"Ability {Name} has been activated.\n{Description}", 10f);
    }
}