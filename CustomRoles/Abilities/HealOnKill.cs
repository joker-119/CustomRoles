namespace CustomRoles.Abilities
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using CustomRoles.Abilities.Generics;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using MEC;
    using UnityEngine;

    public class HealOnKill : PassiveAbilityResolvable
    {
        private readonly Dictionary<Player, CoroutineHandle> _activeHoTs = new Dictionary<Player, CoroutineHandle>();
        public override string Name { get; set; } = "Heal on Kill";
        public override string Description { get; set; } = "Heals the player when they kill someone.";

        [Description("How much health to give the player.")]
        public float HealAmount { get; set; } = 25f;

        [Description("Whether or not this heal can exceed their max health.")]
        public bool HealOverMax { get; set; } = false;

        [Description("Whether or not this heal is applied gradually over time (true) or instantly (false)")]
        public bool HealOverTime { get; set; } = true;

        [Description("How long the heal over time effect lasts, if used.")]
        public float HealOverTimeDuration { get; set; } = 10f;

        [Description("How often (in seconds) the heal over time effect ticks, if used.")]
        public float HealOverTimeTickFrequency { get; set; } = 1.0f;

        [Description("Whether or not the heal over time effect is ended early if the player takes damage.")]
        public bool DamageInterruptsHot { get; set; } = true;

        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Dying += OnDying;
            if (HealOverTime && DamageInterruptsHot)
                Exiled.Events.Handlers.Player.Hurting += OnHurting;
            base.SubscribeEvents();
        }

        protected override void UnSubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Dying -= OnDying;
            if (HealOverTime && DamageInterruptsHot)
                Exiled.Events.Handlers.Player.Hurting -= OnHurting;
            base.UnSubscribeEvents();
        }

        private void OnDying(DyingEventArgs ev)
        {
            if (Check(ev.Killer))
            {
                if (HealOverTime)
                    _activeHoTs[ev.Killer] = Timing.RunCoroutine(DoHealOverTime(ev.Killer));
                ev.Killer.Heal(HealAmount, HealOverMax);
            }
        }

        private void OnHurting(HurtingEventArgs ev)
        {
            if (Check(ev.Target))
                if (DamageInterruptsHot && _activeHoTs.ContainsKey(ev.Target))
                    Timing.KillCoroutines(_activeHoTs[ev.Target]);
        }

        private IEnumerator<float> DoHealOverTime(Player player)
        {
            var tickAmount = HealAmount / HealOverTimeDuration;
            var tickCount = Mathf.FloorToInt(HealOverTimeDuration / HealOverTimeTickFrequency);

            for (var i = 0; i < tickCount; i++)
            {
                player.Heal(tickAmount, HealOverMax);
                yield return Timing.WaitForSeconds(HealOverTimeTickFrequency);
            }
        }
    }
}