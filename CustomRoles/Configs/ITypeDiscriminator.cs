namespace CustomRoles.Configs
{
    using System;

    public interface ITypeDiscriminator
    {
        Type BaseType { get; }

        bool TryResolve(ParsingEventBuffer buffer, out Type suggestedType);
    }
}