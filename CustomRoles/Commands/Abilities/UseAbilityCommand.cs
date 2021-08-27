using System;
using CommandSystem;
using CustomRoles.Abilities;
using CustomRoles.API;
using CustomRoles.Roles;
using Exiled.API.Features;

namespace CustomRoles.Commands
{
    using UnityEngine;

    [CommandHandler(typeof(ClientCommandHandler))]
    public class UseAbilityCommand : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(((CommandSender) sender).SenderId);

            CustomRole customRole = null;
            string reply = string.Empty;
            foreach (CustomRole role in player.GetPlayerRoles())
            {
                Log.Debug($"{nameof(UseAbilityCommand)}: Checking {player.Nickname} for usability of {role.Name}");
                if (!role.CanUseAbility(out DateTime usableTime))
                {
                    response = $"You cannot use the ability for {role.Name} for another {Math.Round((usableTime - DateTime.Now).TotalSeconds, 2)} seconds.\n";
                    player.ShowHint(response);
                    return false;
                }
                else
                {
                    customRole = role;
                    
                    break;
                }
            }

            if (customRole == null)
            {
                response = "You are not a role capable of using any custom abilities.";
                player.ShowHint(response);
                return false;
            }

            customRole.UsedAbility = DateTime.Now;
            response = customRole.UseAbility();
            return true;
        }

        public string Command { get; } = "useability";
        public string[] Aliases { get; } = new[] { "special" };
        public string Description { get; } = "Use your classes special ability, if available.";
    }
}