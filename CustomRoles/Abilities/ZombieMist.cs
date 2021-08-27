using System.Collections.Generic;
using Exiled.API.Features;
using MEC;
using UnityEngine;

namespace CustomRoles.Abilities
{
    public class ZombieMist : CustomAbility
    {
        public override string Name { get; set; } = "Zombie Mist";
        protected override string Description { get; set; } =
            "Secretes a short-term mist of the cure which will heal and protect other SCPs for a short duration.";

        protected override float Duration { get; set; } = Plugin.Singleton.Config.RoleConfigs.MedicZombieCfg.AbilityDuration;
        private float HealAmount { get; set; } = Plugin.Singleton.Config.RoleConfigs.MedicZombieCfg.AbilityHealAmount;

        private ushort ProtectionAmount { get; set; } = Plugin.Singleton.Config.RoleConfigs.MedicZombieCfg.AbilityFinaleProtection;

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
            foreach (Player player in Player.List)
                if (player.Team == Team.SCP && player != Player)
                    Coroutines.Add(Timing.RunCoroutine(DoMist(player)));
        }

        private IEnumerator<float> DoMist(Player player)
        {
            for (int i = 0; i < Duration; i++)
            {
                if (player.Health + HealAmount >= player.MaxHealth || Vector3.Distance(player.Position, Player.Position) > 10f)
                    continue;

                player.Health += HealAmount;

                yield return Timing.WaitForSeconds(1f);
            }

            if (Vector3.Distance(player.Position, Player.Position) < 10.1f)
                player.ArtificialHealth += ProtectionAmount;
        }
    }
}