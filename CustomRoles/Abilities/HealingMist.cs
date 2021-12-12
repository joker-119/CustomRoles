namespace CustomRoles.Abilities
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Exiled.API.Features;
    using Generics;
    using MEC;

    public class HealingMist : ActiveAbilityResolvable
    {
        private readonly List<CoroutineHandle> _coroutines = new();
        public override string Name { get; set; } = "Healing Mist";

        public override string Description { get; set; } =
            "Activates a short-term spray of chemicals which will heal and protect allies for a short duration.";

        public override float Duration { get; set; } = 15f;
        public override float Cooldown { get; set; } = 180f;

        [Description("The amount healed every second the ability is active.")]
        public float HealAmount { get; set; } = 6;

        [Description("The amount of AHP given when the ability ends.")]
        public ushort ProtectionAmount { get; set; } = 45;

        protected override void AbilityUsed(Player player)
        {
            ActivateMist(player);
        }

        protected override void UnSubscribeEvents()
        {
            foreach (var handle in _coroutines)
                Timing.KillCoroutines(handle);
            base.UnSubscribeEvents();
        }

        private void ActivateMist(Player ply)
        {
            foreach (var player in Player.List)
                if (player.Side == ply.Side && player != ply)
                    _coroutines.Add(Timing.RunCoroutine(DoMist(ply, player)));
        }

        private IEnumerator<float> DoMist(Player activator, Player player)
        {
            for (var i = 0; i < Duration; i++)
            {
                if (player.Health + HealAmount >= player.MaxHealth ||
                    (player.Position - activator.Position).sqrMagnitude > 144f)
                    continue;

                player.Health += HealAmount;

                yield return Timing.WaitForSeconds(0.75f);
            }

            if ((activator.Position - player.Position).sqrMagnitude < 144f)
                player.ArtificialHealth += ProtectionAmount;
        }
    }
}