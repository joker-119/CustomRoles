using System.Collections.Generic;
using Exiled.API.Features;
using MEC;

namespace CustomRoles.Abilities
{
    using Exiled.API.Enums;
    using Exiled.API.Features.Items;
    using Exiled.CustomRoles.API.Features;
    using InventorySystem.Items.Firearms;
    using InventorySystem.Items.Firearms.BasicMessages;
    using Firearm = Exiled.API.Features.Items.Firearm;

    public class BlackoutAbility : ActiveAbility
    {
        public override string Name { get; set; } = "Blackout";

        public override string Description { get; set; } =
            "Causes all rooms in the facility to lose lighting. Damages players caught in the dark.";

        public override float Duration { get; set; } = 120f;

        public override float Cooldown { get; set; } = 90f;
        public float KeterDamage { get; set; } = 5;
        public string KeterHint { get; set; } = "You have been damaged by SCP-575!";
        private List<CoroutineHandle> Coroutines = new List<CoroutineHandle>();

        protected override void AbilityUsed(Player player)
        {
            DoBlackout(player);
        }

        protected override void AbilityRemoved(Player player)
        {
            foreach (CoroutineHandle handle in Coroutines)
                Timing.KillCoroutines(handle);
        }

        private void DoBlackout(Player player)
        {
            Cassie.Message("pitch_0.15 .g7");
            foreach (Room room in Map.Rooms)
            {
                if (room.Zone != ZoneType.Surface)
                    room.TurnOffLights(Duration);
            }

            Coroutines.Add(Timing.RunCoroutine(Keter(player, Duration)));
        }

        private IEnumerator<float> Keter(Player ply, float dur)
        {
            do
            {
                foreach (Player player in Player.List)
                    if (player.CurrentRoom.LightsOff && !HasLightSource(player) && player.IsHuman)
                    {
                        player.Hurt(KeterDamage, DamageTypes.Bleeding, ply.Nickname, ply.Id);
                        player.ShowHint(KeterHint,5f);
                    }

                yield return Timing.WaitForSeconds(5f);
            } while ((dur -= 5f) > 5f);
        }
        private static bool HasLightSource(Player player)
        {
            return (player.CurrentItem is Flashlight flashlight && flashlight.Active) || (player.CurrentItem is Firearm firearm && firearm.Base.Status.Flags.HasFlagFast(FirearmStatusFlags.FlashlightEnabled));
        }
    }
}