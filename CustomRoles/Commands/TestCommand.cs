namespace CustomRoles.Commands.Abilities;

using System;
using CommandSystem;
using CustomRoles.Roles;
using Exiled.API.Features;
using Exiled.CustomRoles.API;
using Exiled.CustomRoles.API.Features;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class TestCommand : ICommand
{
    public string Command { get; } = "roletest";

    public string[] Aliases { get; } = { "rtest" };

    public string Description { get; } = "thing";

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

        switch (args[2])
        {
            case "roles":
                response = string.Empty;
                foreach (CustomRole role in target.GetCustomRoles())
                    response += $"{role.Name}\n";
                return true;
            case "phantom":
                CustomRole.Get(typeof(Phantom)).AddRole(target);
                break;
            case "dwarf":
                CustomRole.Get(typeof(Dwarf)).AddRole(target);
                break;
            case "medic":
                CustomRole.Get(typeof(Medic)).AddRole(target);
                break;
            case "berserk":
                CustomRole.Get(typeof(BerserkZombie)).AddRole(target);
                break;
            case "charger":
                CustomRole.Get(typeof(ChargerZombie)).AddRole(target);
                break;
            case "medicz":
                CustomRole.Get(typeof(MedicZombie)).AddRole(target);
                break;
            case "plague":
                CustomRole.Get(typeof(PlagueZombie)).AddRole(target);
                break;
            case "ballistic":
                CustomRole.Get(typeof(BallisticZombie)).AddRole(target);
                break;
            case "dwarfz":
                CustomRole.Get(typeof(DwarfZombie)).AddRole(target);
                break;
            case "pdz":
                CustomRole.Get(typeof(PDZombie)).AddRole(target);
                break;
            case "tank":
                CustomRole.Get(typeof(TankZombie)).AddRole(target);
                break;
        }

        response = "Done";
        return true;
    }
}