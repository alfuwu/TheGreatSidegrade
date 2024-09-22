using MonoMod.RuntimeDetour;
using System;
using System.Reflection;
using Terraria.ModLoader;
using Avalon.Hooks;
using MonoMod.Cil;
using Mono.Cecil.Cil;

namespace TheGreatSidegrade.Common.Hooks.Compatibility;

[Autoload(Side = ModSide.Client)]
[ExtendsFromMod("Avalon")]
public class ContagionSelectionMenuHook { // this is so cursed
    private static ILHook applyHook = null;

    public static void Apply() {
        MethodInfo applyInfo = typeof(ContagionSelectionMenu).GetMethod("Apply", BindingFlags.NonPublic | BindingFlags.Instance);
        
        if (applyInfo != null ) {
            applyHook = new ILHook(applyInfo, Utils.CancelIL);
            applyHook.Apply();
        }
    }

    public static void Unapply() {
        applyHook?.Undo();
    }
}
