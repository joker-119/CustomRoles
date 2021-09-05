using System.Collections.Generic;
using Exiled.API.Features;
using MEC;
using UnityEngine;

namespace CustomRoles.Abilities
{
    using Exiled.CustomRoles.API.Features;

    public class HealingMist : ActiveAbility
    {
        public override string Name { get; set; } = "Healing Mist";
        public override string Description { get; set; } =
            "Activates a short-term spray of chemicals which will heal and protect allies for a short duration.";

        public override float Duration { get; set; } = 15f;
        public override float Cooldown { get; set; } = 180f;
        public float HealAmount { get; set; } = 6;

        public ushort ProtectionAmount { get; set; } = 45;

        private List<CoroutineHandle> Coroutines = new List<CoroutineHandle>();

        protected override void AbilityUsed(Player player)
        {
            ActivateMist(player);
        }

        protected override void UnSubscribeEvents()
        {
            foreach (CoroutineHandle handle in Coroutines)
                Timing.KillCoroutines(handle);
            base.UnSubscribeEvents();
        }

        private void ActivateMist(Player ply)
        {
            foreach (Player player in Player.List)
                if (player.Side == ply.Side && player != ply)
                    Coroutines.Add(Timing.RunCoroutine(DoMist(ply, player)));
        }

        private IEnumerator<float> DoMist(Player activator, Player player)
        {
            for (int i = 0; i < Duration; i++)
            {
                if (player.Health + HealAmount >= player.MaxHealth || Vector3.Distance(player.Position, activator.Position) > 12.01f)
                    continue;

                player.Health += HealAmount;

                yield return Timing.WaitForSeconds(0.75f);
            }

            if ((activator.Position - player.Position).sqrMagnitude < 144f)
                player.ArtificialHealth += ProtectionAmount;
        }
    }
}