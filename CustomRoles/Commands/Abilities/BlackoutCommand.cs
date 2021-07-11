using System;
using CommandSystem;
using CustomRoles.Abilities;
using CustomRoles.Roles;
using Player = Exiled.API.Features.Player;

namespace CustomRoles.Commands.Abilities
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class BlackoutCommand : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(((CommandSender)sender).SenderId);
            Scp575 scp575 = player.GameObject.GetComponent<Scp575>();

            if (scp575 == null)
            {
                response = "Only SCP-575 can use this ability.";
                return false;
            }

            if (!scp575.canUseAbility)
            {
                response = "You must reach a higher power level before unlocking this ability.";
                player.ShowHint(
                    $"You must reach a higher power level to use this ability. Current: {scp575.powerLevel} Required: 10.",
                    7f);
                return false;
            }

            if (!scp575.CanUseAbility(out DateTime usableTime))
            {
                response = "This ability is still on cooldown.";
                player.ShowHint($"Your class ability is still on cooldown for {(usableTime - DateTime.Now).TotalSeconds} seconds.", 5f);
                return false;
            }

            player.GameObject.AddComponent<BlackoutAbility>();
            scp575.UsedAbility = DateTime.Now;
            response = "Ability used.";
            return true;
        }

        public string Command { get; } = "blackout";
        public string[] Aliases { get; } = new[] { "keter" };

        public string Description { get; } =
            "Causes all lights in the facility to go dark. Causes players in the dark to take damage.";
    }
}