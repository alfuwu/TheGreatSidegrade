using MonoMod.RuntimeDetour;
using System;
using System.Reflection;
using Terraria.ModLoader;

namespace TheGreatSidegrade.Common.Hooks.Compatibility;

[ExtendsFromMod("TheDepths")]
public class TheDepthsSelectionMenuHook {
    private static ILHook applyHook = null;
    private static ILHook ilApplyHook = null;

    public static void Apply() {
        MethodInfo applyInfo = null;
        MethodInfo ilApplyInfo = null;

        // the depths adds controller support through an il method
        // so look for either in case it gets switched to a detour
        foreach (Module mod in ModLoader.GetMod("TheDepths").GetType().Assembly.GetModules()) {
            if (Utils.TryGetType(mod, "TheDepths.Hooks.DepthsSelectionMenu", out Type type)) {
                applyInfo = type.GetMethod("OnSetupGamepadPoints", BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase);
                ilApplyInfo = type.GetMethod("ILSetupGamepadPoints", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.IgnoreCase);
            }
        }

        if (applyInfo != null) {
            applyHook = new(applyInfo, Utils.CancelOn);
            applyHook.Apply();
        }
        if (ilApplyInfo != null) {
            ilApplyHook = new(ilApplyInfo, Utils.CancelIL);
            ilApplyHook.Apply();
        }
    }

    public static void Unapply() {
        applyHook?.Undo();
        ilApplyHook?.Undo();
    }
}