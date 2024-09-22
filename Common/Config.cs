using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace TheGreatSidegrade.Common;

public class SidegradeConfig : ModConfig {
    public override ConfigScope Mode => ConfigScope.ServerSide;

    [Header("NewWorldEvils")]

    [DefaultValue(true)]
    [ReloadRequired]
    public bool EnableRandomFracturedWorld;
    [DefaultValue(true)]
    [ReloadRequired]
    public bool EnableRandomNothingWorld;
    [DefaultValue(true)]
    [ReloadRequired]
    public bool EnableRandomRottenWorld;
    [DefaultValue(true)]
    [ReloadRequired]
    public bool EnableRandomSpiralWorld;
    [DefaultValue(true)]
    [ReloadRequired]
    public bool EnableRandomStarvedWorld;

    [DefaultValue(true)]
    [ReloadRequired]
    public bool EnableMorePrefixes;

    [DefaultValue(false)]
    public bool DryadKnowsNothingStatus;
}

public class SidegradeClient : ModConfig {
    public override ConfigScope Mode => ConfigScope.ClientSide;
}
