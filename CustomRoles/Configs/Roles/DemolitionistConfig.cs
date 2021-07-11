using System.Collections.Generic;

namespace CustomRoles.Configs.Roles
{
    public class DemolitionistConfig
    {
        public string Name { get; set; } = "Demolitionist";
        public List<string> Inventory { get; set; } = new List<string>
        {
            "GL-119",
            "C4-119",
            "C4-119",
            ItemType.GrenadeFrag.ToString(),
            ItemType.GrenadeFrag.ToString(),
            ItemType.GrenadeFrag.ToString(),
            ItemType.GrenadeFrag.ToString(),
            ItemType.GrenadeFrag.ToString()
        };
        public RoleType RoleType { get; set; } = RoleType.NtfLieutenant;
        public int MaxHealth { get; set; } = 120;
        public string Description { get; set; } = "An NTF Lieutenant who specializes in explosive ordinance. Spawns with a Grenade Launcher, a number of grenades and some C4.";
    }
}