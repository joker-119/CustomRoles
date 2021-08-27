using System.Collections.Generic;
using Exiled.API.Features;
using MEC;

namespace CustomRoles.Abilities
{
    using Exiled.API.Enums;
    using Exiled.API.Features.Items;
    using InventorySystem.Items.Firearms;
    using InventorySystem.Items.Firearms.BasicMessages;
    using Firearm = Exiled.API.Features.Items.Firearm;

    public class BlackoutAbility : CustomAbility
    {
        public override string Name { get; set; } = "Blackout";

        protected override string Description { get; set; } =
            "Causes all rooms in the facility to lose lighting. Damages players caught in the dark.";

        protected override float Duration { get; set; } = Plugin.Singleton.Config.RoleConfigs.Scp575Cfg.AbilityDuration;
        private List<CoroutineHandle> Coroutines = new List<CoroutineHandle>();

        protected override void LoadEvents()
        {
            ShowMessage();
            DoBlackout();
        }

        protected override void UnloadEvents()
        {
            foreach (CoroutineHandle handle in Coroutines)
                Timing.KillCoroutines(handle);
        }

        private void DoBlackout()
        {
            Cassie.Message("pitch_0.15 .g7");
            foreach (Room room in Map.Rooms)
            {
                if (room.Zone != ZoneType.Surface)
                    room.TurnOffLights(Duration);
            }

            Coroutines.Add(Timing.RunCoroutine(Keter(Duration)));
        }

        private IEnumerator<float> Keter(float dur)
        {
            do
            {
                foreach (Player player in Player.List)
                    if (player.CurrentRoom.LightsOff && !HasLightSource(player) && player.IsHuman)
                    {
                        player.Hurt(Plugin.Singleton.Config.RoleConfigs.Scp575Cfg.KeterDamage, DamageTypes.Bleeding,
                            Player.Nickname, Player.Id);
                        player.ShowHint(Plugin.Singleton.Config.RoleConfigs.Scp575Cfg.KeterBroadcast,
                            Plugin.Singleton.Config.RoleConfigs.Scp575Cfg.KeterBroadcastDuration);
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