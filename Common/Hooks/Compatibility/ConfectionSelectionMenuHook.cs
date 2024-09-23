using MonoMod.RuntimeDetour;
using System;
using System.Reflection;
using Terraria.ModLoader;

namespace TheGreatSidegrade.Common.Hooks.Compatibility;

[ExtendsFromMod("TheConfectionRebirth")]
public class ConfectionSelectionMenuHook {
    private static ILHook applyHook = null;

    public static void Apply() {
        MethodInfo applyInfo = null;

        foreach (Module mod in TheGreatSidegrade.Confection.GetType().Assembly.GetModules())
            if (Utils.TryGetType(mod, "TheConfectionRebirth.Hooks.ConfectionSelectionMenu", out Type type))
                applyInfo = type.GetMethod("OnSetupGamepadPoints", BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase);

        if (applyInfo != null) {
            applyHook = new(applyInfo, Utils.CancelOn);
            applyHook.Apply();
        }
    }

    public static void Unapply() {
        applyHook?.Undo();
    }
}
