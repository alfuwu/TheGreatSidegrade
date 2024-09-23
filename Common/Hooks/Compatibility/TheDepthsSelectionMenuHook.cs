using MonoMod.RuntimeDetour;
using System;
using System.Reflection;
using Terraria.ModLoader;

namespace TheGreatSidegrade.Common.Hooks.Compatibility;

[ExtendsFromMod("TheDepths")]
public class TheDepthsSelectionMenuHook {
    private static ILHook applyHook = null;

    public static void Apply() {
        MethodInfo applyInfo = null;

        foreach (Module mod in ModLoader.GetMod("TheDepths").GetType().Assembly.GetModules())
            if (Utils.TryGetType(mod, "TheDepths.Hooks.DepthsSelectionMenu", out Type type))
                applyInfo = type.GetMethod("OnSetupGamepadPoints", BindingFlags.Public | BindingFlags.Static);

        if (applyInfo != null) {
            applyHook = new(applyInfo, Utils.CancelOn);
            applyHook.Apply();
        }
    }

    public static void Unapply() {
        applyHook?.Undo();
    }
}