using System.Collections.Generic;
using Exiled.API.Features;
using MEC;
using UnityEngine;

namespace CustomRoles.Abilities
{
    public class HealingMist : CustomAbility
    {
        public override string Name { get; set; } = "Healing Mist";
        protected override string Description { get; set; } =
            "Activates a short-term spray of chemicals which will heal and protect allies for a short duration.";

        protected override float Duration { get; set; } = Plugin.Singleton.Config.RoleConfigs.MediCfg.AbilityDuration;
        private float HealAmount { get; set; } = Plugin.Singleton.Config.RoleConfigs.MediCfg.AbilityHealAmount;

        private ushort ProtectionAmount { get; set; } = Plugin.Singleton.Config.RoleConfigs.MediCfg.AbilityFinaleProtection;

        private List<CoroutineHandle> Coroutines = new List<CoroutineHandle>();

        protected override void LoadEvents()
        {
            ActivateMist();
        }

        protected override void UnloadEvents()
        {
            foreach (CoroutineHandle handle in Coroutines)
                Timing.KillCoroutines(handle);
        }

        private void ActivateMist()
        {
            foreach (Player player in Exiled.API.Features.Player.List)
                if (player.Side == Player.Side && player != Player)
                    Coroutines.Add(Timing.RunCoroutine(DoMist(player)));
        }

        private IEnumerator<float> DoMist(Player player)
        {
            for (int i = 0; i < Duration; i++)
            {
                if (player.Health + HealAmount >= player.MaxHealth || Vector3.Distance(player.Position, Player.Position) > 12.01f)
                    continue;

                player.Health += HealAmount;

                yield return Timing.WaitForSeconds(0.75f);
            }

            if (Vector3.Distance(player.Position, Player.Position) < 12f)
                player.ArtificialHealth += ProtectionAmount;
        }
    }
}