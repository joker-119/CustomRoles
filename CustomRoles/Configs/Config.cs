using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using CustomRoles.API;
using CustomRoles.Roles;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using Exiled.Loader;

namespace CustomRoles.Configs
{
    public class Config : IConfig
    {
        [Description("Whether or not this plugin is enabled.")]
        public bool IsEnabled { get; set; } = true;
        
        [Description("Whether or not debug messages shoudl be shown.")]
        public bool Debug { get; set; } = true;
        
        [Description("The folder path where role configs will be stored.")]
        public string CustomRolesFolderPath { get; set; } = Path.Combine(Paths.Configs, "CustomRoles");
        
        [Description("The file name to load role configs from.")]
        public string CustomRolesFilename { get; set; } = "global.yml";
        
        public RoleConfigs RoleConfigs;
        public readonly List<string> EnabledZombies = new List<string>();

        public void LoadConfigs()
        {
            if (!Directory.Exists(CustomRolesFolderPath))
                Directory.CreateDirectory(CustomRolesFolderPath);

            string filePath = Path.Combine(CustomRolesFolderPath, CustomRolesFilename);

            if (!File.Exists(filePath))
            {
                RoleConfigs = new RoleConfigs();
                File.WriteAllText(filePath, Loader.Serializer.Serialize(RoleConfigs));
            }
            else
            {
                RoleConfigs = Loader.Deserializer.Deserialize<RoleConfigs>(File.ReadAllText(filePath));
                File.WriteAllText(filePath, Loader.Serializer.Serialize(RoleConfigs));
            }
            
            if (RoleConfigs.BallisticCfg.Enabled)
                EnabledZombies.Add(nameof(BallisticZombie));
            if (RoleConfigs.PlagueCfg.Enabled)
                EnabledZombies.Add(nameof(PlagueZombie));
            if (RoleConfigs.BerserkZombieCfg.Enabled)
                EnabledZombies.Add(nameof(BerserkZombie));
            if (RoleConfigs.DwarfZombieCfg.Enabled)
                EnabledZombies.Add(nameof(DwarfZombie));
            if (RoleConfigs.MedicZombieCfg.Enabled)
                EnabledZombies.Add(nameof(MedicZombie));
            if (RoleConfigs.TankZombieCfg.Enabled)
                EnabledZombies.Add(nameof(TankZombie));
            if (RoleConfigs.PDZombieCfg.Enabled)
                EnabledZombies.Add(nameof(PDZombie));
        }
    }
}