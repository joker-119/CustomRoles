namespace CustomRoles.Configs
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using Exiled.API.Features;
    using Exiled.API.Interfaces;
    using Exiled.CustomRoles.API.Features;
    using Exiled.Loader.Features.Configs;
    using Exiled.Loader.Features.Configs.CustomConverters;
    using YamlDotNet.Serialization;
    using YamlDotNet.Serialization.NamingConventions;
    using YamlDotNet.Serialization.NodeDeserializers;

    public class Config : IConfig
    {
        private static readonly ISerializer Serializer = new SerializerBuilder()
            .WithTypeConverter(new VectorsConverter())
            .WithTypeInspector(i => new CommentGatheringTypeInspector(i))
            .WithEmissionPhaseObjectGraphVisitor(a => new CommentsObjectGraphVisitor(a.InnerVisitor))
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .IgnoreFields()
            .Build();

        private static readonly IDeserializer Deserializer = new DeserializerBuilder()
            .WithTypeConverter(new VectorsConverter())
            .WithNodeDeserializer(
                inner => new AbstractNodeNodeTypeResolverWithValidation(inner,
                    new AggregateExpectationTypeResolver<CustomAbility>(UnderscoredNamingConvention.Instance)),
                s => s.InsteadOf<ObjectNodeDeserializer>())
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .IgnoreFields()
            .IgnoreUnmatchedProperties()
            .Build();

        public Roles RoleConfigs;

        [Description("Whether or not debug messages shoudl be shown.")]
        public bool Debug { get; set; } = true;

        [Description("The folder path where role configs will be stored.")]
        public string RolesFolder { get; set; } = Path.Combine(Paths.Configs, "CustomRoles");

        [Description("The file name to load role configs from.")]
        public string RolesFile { get; set; } = "global.yml";

        [Description("A list of zombie class names that are enabled on this server.")]
        public List<string> EnabledZombies { get; set; } = new List<string>();

        [Description("Whether or not this plugin is enabled.")]
        public bool IsEnabled { get; set; } = true;

        public void LoadConfigs()
        {
            if (!Directory.Exists(RolesFolder))
                Directory.CreateDirectory(RolesFolder);

            var filePath = Path.Combine(RolesFolder, RolesFile);
            if (!File.Exists(filePath))
            {
                RoleConfigs = new Roles();
                File.WriteAllText(filePath, Serializer.Serialize(RoleConfigs));
            }
            else
            {
                RoleConfigs = Deserializer.Deserialize<Roles>(File.ReadAllText(filePath));
                File.WriteAllText(filePath, Serializer.Serialize(RoleConfigs));
            }
        }
    }
}