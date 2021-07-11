using System;
using System.Collections.Generic;
using System.Linq;
using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;

namespace CustomRoles.Abilities
{
    public class ActiveCamo : CustomAbility
    {
        public override string Name { get; set; } = "Active Camo";

        protected override string Description { get; set; } =
            "Activates a camouflage effect that acts like SCP-268 but doesn't break on interacting with objects, only when shooting.";

        protected override float Duration { get; set; } = 30f;
        private List<CoroutineHandle> Coroutines = new List<CoroutineHandle>();

        protected override void LoadEvents()
        {
            try
            {
                Log.Info($"{Name} starting");
                Exiled.Events.Handlers.Player.Shooting += OnShooting;
                Exiled.Events.Handlers.Player.Interacted += OnInteracted;
                Exiled.Events.Handlers.Player.InteractingDoor += OnInteractingDoor;
                Exiled.Events.Handlers.Player.InteractingElevator += OnInteractingElevator;
                Exiled.Events.Handlers.Player.InteractingLocker += OnInteractingLocker;
                Log.Debug($"{Name} enabled for {Duration}");
                Player.EnableEffect<Scp268>(Duration, true);
                Coroutines.Add(Timing.CallDelayed(14.99f, () => Player.EnableEffect<Scp268>()));
                
                ShowMessage();

                Coroutines.Add(Timing.CallDelayed(Duration, () => Destroy(this)));
            }
            catch (Exception e)
            {
                Log.Error($"{e}\n{e.StackTrace}");
            }
        }

        protected override void UnloadEvents()
        {
            Log.Debug($"{Name} destroyed.");
            Exiled.Events.Handlers.Player.Shooting -= OnShooting;
            Exiled.Events.Handlers.Player.Interacted -= OnInteracted;
            Exiled.Events.Handlers.Player.InteractingDoor -= OnInteractingDoor;
            Exiled.Events.Handlers.Player.InteractingElevator -= OnInteractingElevator;
            Exiled.Events.Handlers.Player.InteractingLocker -= OnInteractingLocker;
            foreach (CoroutineHandle handle in Coroutines)
                Timing.KillCoroutines(handle);
            Player.DisableEffect(EffectType.Scp268);
        }

        private void OnShooting(ShootingEventArgs ev)
        {
            if (Player == ev.Shooter)
            {
                Destroy(this);
            }
        }
        
        private void OnInteractingLocker(InteractingLockerEventArgs ev)
        {
            if (ev.Player == Player)
                Timing.CallDelayed(0.05f, () => Player.EnableEffect<Scp268>(Duration, true));
        }

        private void OnInteractingElevator(InteractingElevatorEventArgs ev)
        {
            if (ev.Player == Player)
                Timing.CallDelayed(0.05f, () => Player.EnableEffect<Scp268>(Duration, true));
        }

        private void OnInteractingDoor(InteractingDoorEventArgs ev)
        {
            if (ev.Player == Player)
                Timing.CallDelayed(0.05f, () => Player.EnableEffect<Scp268>(Duration, true));
        }

        private void OnInteracted(InteractedEventArgs ev)
        {
            if (ev.Player == Player)
                Timing.CallDelayed(0.05f, () => Player.EnableEffect<Scp268>(Duration, true));
        }
    }
}