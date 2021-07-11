using System;
using CommandSystem;
using CustomRoles.Abilities;
using CustomRoles.Roles;
using Exiled.API.Features;

namespace CustomRoles.Commands.Abilities
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class ActiveCamoCommand : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            try
            {
                Player player = Player.Get(((CommandSender) sender).SenderId);
                if (player == null)
                {
                    response = "Player is null.";
                    return false;
                }

                Phantom phantom = player.GameObject.GetComponent<Phantom>();

                if (phantom != null)
                {
                    Log.Debug(phantom.CanUseAbility(out DateTime _));
                    if (!phantom.CanUseAbility(out DateTime usableTime))
                    {
                        response = "This ability is still on cooldown.";
                        player.ShowHint($"Your class ability is still on cooldown for {(usableTime - DateTime.Now).TotalSeconds} seconds.", 5f);
                        return false;
                    }

                    phantom.UsedAbility = DateTime.Now;
                    player.GameObject.AddComponent<ActiveCamo>();
                    response = "Ability used.";
                    return true;
                }

                response = "Unable to use that ability.";
                return false;
            }
            catch (Exception e)
            {
                Log.Error($"{e}\n{e.StackTrace}");
                response = "Internal server error.";
                return false;
            }
        }

        public string Command { get; } = "activecamo";
        public string[] Aliases { get; } = new[] { "acamo" };
        public string Description { get; } = "Actives Active Camo.";
    }
}