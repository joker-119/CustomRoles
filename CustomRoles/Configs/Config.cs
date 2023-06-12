namespace CustomRoles.Configs;

using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using Exiled.Loader;

public class Config : IConfig
{
    public Roles RoleConfigs { get; private set; } = null!;

    [Description("Whether or not debug messages shoudl be shown.")]
    public bool Debug { get; set; } = true;

    [Description("The folder path where role configs will be stored.")]
    public string RolesFolder { get; set; } = Path.Combine(Paths.Configs, "CustomRoles");

    [Description("The file name to load role configs from.")]
    public string RolesFile { get; set; } = "global.yml";

    [Description("A list of zombie class names that are enabled on this server.")]
    public List<string> EnabledZombies { get; set; } = new ();

    [Description("Whether or not this plugin is enabled.")]
    public bool IsEnabled { get; set; } = true;

    public void LoadConfigs()
    {
        if (!Directory.Exists(RolesFolder))
            Directory.CreateDirectory(RolesFolder);

        string filePath = Path.Combine(RolesFolder, RolesFile);
        if (!File.Exists(filePath))
        {
            RoleConfigs = new Roles();
            File.WriteAllText(filePath, Loader.Serializer.Serialize(RoleConfigs));
        }
        else
        {
            RoleConfigs = Loader.Deserializer.Deserialize<Roles>(File.ReadAllText(filePath));
            File.WriteAllText(filePath, Loader.Serializer.Serialize(RoleConfigs));
        }
    }
}