using MonoMod.RuntimeDetour;
using System.Reflection;
using Terraria.ModLoader;
using System;

namespace TheGreatSidegrade.Common.Hooks.Compatibility;

[ExtendsFromMod("Avalon")]
public class AvalonDryadTextDetourHook {
    private static ILHook applyHook = null;

    private static bool TryGetType(Module mod, string className, out Type type) {
        type = mod.GetType(className);
        if (type != null)
            return true;
        return false;
    }

    public static void Apply() {
        // why is DryadTextDetour internal ahhhhhhhhhhhhhhhhhhhhhhhhhhhhh
        // oh im stupid, avalon's content aint gonna be loaded when this func runs, so it wont find DryadTextDetour using TryFind
        // oop
        MethodInfo applyInfo = null;

        foreach (Module mod in TheGreatSidegrade.Avalon.GetType().Assembly.GetModules())
            if (TryGetType(mod, "Avalon.Hooks.DryadTextDetour", out Type type))
                applyInfo = type.GetMethod("Apply", BindingFlags.NonPublic | BindingFlags.Instance);

        if (applyInfo != null) {
            applyHook = new(applyInfo, Utils.CancelIL);
            applyHook.Apply();
        }
    }

    public static void Unapply() {
        applyHook?.Undo();
    }
}
