namespace CustomRoles.Abilities
{
    using System.Collections.Generic;
    using Exiled.CustomRoles.API.Features;
    using Exiled.Events.EventArgs;
    using Exiled.Events.Handlers;
    using MEC;
    using UnityEngine;
    using Player = Exiled.API.Features.Player;

    public class HealOnKill : PassiveAbility
    {
        public override string Name { get; set; } = "Heal on Kill";
        public override string Description { get; set; } = "Heals the player when they kill someone.";
        public float HealAmount { get; set; } = 25f;
        public bool HealOverMax { get; set; } = false;
        public bool HealOverTime { get; set; } = true;
        public float HealOverTimeDuration { get; set; } = 10f;
        public float HealOverTimeTickFrequency { get; set; } = 1.0f;
        public bool DamageInterruptsHot { get; set; } = true;

        private Dictionary<Player, CoroutineHandle> ActiveHoTs = new Dictionary<Player, CoroutineHandle>();
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
                    ActiveHoTs[ev.Killer] = Timing.RunCoroutine(DoHealOverTime(ev.Killer));
                ev.Killer.Heal(HealAmount, HealOverMax);
            }
        }

        private void OnHurting(HurtingEventArgs ev)
        {
            if (Check(ev.Target))
            {
                if (DamageInterruptsHot && ActiveHoTs.ContainsKey(ev.Target))
                    Timing.KillCoroutines(ActiveHoTs[ev.Target]);
            }
        }

        private IEnumerator<float> DoHealOverTime(Player player)
        {
            float tickAmount = HealAmount / HealOverTimeDuration;
            int tickCount = Mathf.FloorToInt(HealOverTimeDuration / HealOverTimeTickFrequency);

            for (int i = 0; i < tickCount; i++)
            {
                player.Heal(tickAmount, HealOverMax);
                yield return Timing.WaitForSeconds(HealOverTimeTickFrequency);
            }
        }
    }
}