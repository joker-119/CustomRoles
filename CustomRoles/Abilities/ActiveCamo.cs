namespace CustomRoles.Abilities
{
    using System;
    using System.Linq;
    using CustomPlayerEffects;
    using Exiled.API.Features;
    using Exiled.CustomRoles.API.Features;
    using Exiled.Events.EventArgs;
    using MEC;

    public class ActiveCamo : ActiveAbility
    {
        public override string Name { get; set; } = "Active Camo";

        public override string Description { get; set; } =
            "Activates a camouflage effect that acts like SCP-268 but doesn't break on interacting with objects, only when shooting.";

        public override float Duration { get; set; } = 30f;
        public override float Cooldown { get; set; } = 120f;

        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Shooting += OnShooting;
            Exiled.Events.Handlers.Player.Interacted += OnInteracted;
            Exiled.Events.Handlers.Player.InteractingDoor += OnInteractingDoor;
            Exiled.Events.Handlers.Player.InteractingElevator += OnInteractingElevator;
            Exiled.Events.Handlers.Player.InteractingLocker += OnInteractingLocker;
            base.SubscribeEvents();
        }

        protected override void AbilityUsed(Player player)
        {
            try
            {
                ActivePlayers.Add(player);
                Log.Debug($"{Name} enabled for {Duration}");
                player.EnableEffect<Invisible>(Duration);
            }
            catch (Exception e)
            {
                Log.Error($"{e}\n{e.StackTrace}");
            }
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Shooting -= OnShooting;
            Exiled.Events.Handlers.Player.Interacted -= OnInteracted;
            Exiled.Events.Handlers.Player.InteractingDoor -= OnInteractingDoor;
            Exiled.Events.Handlers.Player.InteractingElevator -= OnInteractingElevator;
            Exiled.Events.Handlers.Player.InteractingLocker -= OnInteractingLocker;
            foreach (Player player in ActivePlayers.ToList())
                EndAbility(player);
            base.UnsubscribeEvents();
        }

        protected override void AbilityEnded(Player player)
        {
            player.DisableEffect<Invisible>();
        }

        private void OnShooting(ShootingEventArgs ev)
        {
            if (Check(ev.Shooter)) AbilityEnded(ev.Shooter);
        }

        private void OnInteractingLocker(InteractingLockerEventArgs ev)
        {
            if (Check(ev.Player))
                Timing.CallDelayed(0.05f, () => ev.Player.EnableEffect<Invisible>(Duration, true));
        }

        private void OnInteractingElevator(InteractingElevatorEventArgs ev)
        {
            if (Check(ev.Player))
                Timing.CallDelayed(0.05f, () => ev.Player.EnableEffect<Invisible>(Duration, true));
        }

        private void OnInteractingDoor(InteractingDoorEventArgs ev)
        {
            if (Check(ev.Player))
                Timing.CallDelayed(0.05f, () => ev.Player.EnableEffect<Invisible>(Duration, true));
        }

        private void OnInteracted(InteractedEventArgs ev)
        {
            if (Check(ev.Player))
                Timing.CallDelayed(0.05f, () => ev.Player.EnableEffect<Invisible>(Duration, true));
        }
    }
}