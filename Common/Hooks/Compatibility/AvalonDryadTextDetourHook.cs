using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using System.Reflection;
using System;
using Terraria.ModLoader;
using Mono.Cecil.Cil;
using Avalon.Common;

namespace TheGreatSidegrade.Common.Hooks.Compatibility;

[ExtendsFromMod("Avalon")]
public class AvalonDryadTextDetourHook {
    private static ILHook applyHook = null;

    public static void Apply() {
        // why is DryadTextDetour internal ahhhhhhhhhhhhhhhhhhhhhhhhhhhhh
        MethodInfo applyInfo = TheGreatSidegrade.Avalon.TryFind("DryadTextDetour", out ModHook dryadTextDetour) ? dryadTextDetour.GetType().GetMethod("Apply", BindingFlags.NonPublic | BindingFlags.Instance) : null;

        if (applyInfo != null) {
            applyHook = new ILHook(applyInfo, Utils.CancelIL);
            applyHook.Apply();
        }
    }

    public static void Unapply() {
        applyHook?.Undo();
    }
}
