using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using TheGreatSidegrade.Common.Hooks;

namespace TheGreatSidegrade;

public class TheGreatSidegrade : Mod {
    public static Mod Avalon { get; private set; }
    public static Mod Confection { get; private set; }
    public static TheGreatSidegrade Mod { get; private set; }
    public static bool NightJustStarted { get; set; }
    public static bool DayJustStarted { get; set; }
    public const string AssetPath = $"{nameof(TheGreatSidegrade)}/Assets/";

    public static bool HasAvalon { get => Avalon != null; }
    public static bool HasConfection { get => Confection != null; }

    public static bool IsContagion {
        get => HasAvalon && Avalon.TryFind("AvalonWorld", out ModSystem world) && (int) world.GetType().GetField("WorldEvil").GetValue(world) == 2;
        set {
            if (HasAvalon && Avalon.TryFind("AvalonWorld", out ModSystem world))
                world.GetType().GetField("WorldEvil").SetValue(world, value ? 2 : 0);
        }
    }

    public override void Load() {
        Mod = this;
        if (ModLoader.TryGetMod("Avalon", out Mod tmp))
            Avalon = tmp;
        if (ModLoader.TryGetMod("TheConfectionRebirth", out Mod tmp2))
            Confection = tmp2;

        Hooks.RegisterHooks();
    }

    public override void Unload() {
        Hooks.UnregisterHooks();
    }
}