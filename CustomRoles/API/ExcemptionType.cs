namespace CustomRoles.API;

using System;

[Flags]
public enum ExemptionType
{
    RoundStart,
    Respawn,
    Revive,
}