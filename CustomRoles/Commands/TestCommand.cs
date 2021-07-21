using System;
using CommandSystem;
using CustomRoles.Roles;
using Exiled.API.Features;

namespace CustomRoles.Commands.Abilities
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class TestCommand : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            string[] args = arguments.Array;
            if (args == null)
            {
                response = "You need arguments.";
                return false;
            }
            
            Player player = Player.Get(((CommandSender)sender).SenderId);
            Log.Debug(args[1]);
            Player target = Player.Get(args[1]);

            if (target == null)
            {
                response = "Must define a target";
                return false;
            }

            target.Role = RoleType.Spectator;

            switch (args[2])
            {
                case "575":
                    target.GameObject.AddComponent<Scp575>();
                    break;
                case "power":
                    if (target.GameObject.GetComponent<Scp575>() == null)
                    {
                        response = "The target must be 575 first.";
                        return false;
                    }
                    target.GameObject.GetComponent<Scp575>().IncreasePower();
                    break;
                case "phantom":
                    target.GameObject.AddComponent<Phantom>();
                    break;
                case "dwarf":
                    target.GameObject.AddComponent<Dwarf>();
                    break;
                case "shotgunner":
                    target.GameObject.AddComponent<Shotgunner>();
                    break;
                case "medic":
                    target.GameObject.AddComponent<Medic>();
                    break;
                case "berserk":
                    target.GameObject.AddComponent<BerserkZombie>();
                    break;
                case "medicz":
                    target.GameObject.AddComponent<MedicZombie>();
                    break;
                case "plague":
                    target.GameObject.AddComponent<PlagueZombie>();
                    break;
                case "ballistic":
                    target.GameObject.AddComponent<BallisticZombie>();
                    break;
                case "dwarfz":
                    target.GameObject.AddComponent<DwarfZombie>();
                    break;
                case "pdz":
                    target.GameObject.AddComponent<PDZombie>();
                    break;
                case "tank":
                    target.GameObject.AddComponent<TankZombie>();
                    break;
            }

            response = "Done";
            return true;
        }

        public string Command { get; } = "roletest";
        public string[] Aliases { get; } = new[] { "rtest" };
        public string Description { get; } = "thing";
    }
}