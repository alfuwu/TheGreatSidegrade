using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using System;
using System.Reflection;
using Terraria.ModLoader;

namespace TheGreatSidegrade.Common.Hooks.Compatibility;

[ExtendsFromMod("TheConfectionRebirth")]
public class ConfectionDryadTextDetourHook {
    private static ILHook applyHook = null;

    public static void Apply() {
        MethodInfo applyInfo = TheGreatSidegrade.Confection.GetType().GetMethod("On_Lang_GetDryadWorldStatusDialog", BindingFlags.NonPublic | BindingFlags.Instance);

        if (applyInfo != null) {
            applyHook = new(applyInfo, CancelIL);
            applyHook.Apply();
        }
    }

    public static void Unapply() {
        applyHook?.Undo();
    }

    public static void CancelIL(ILContext il) { // god
        try {
            ILCursor c = new(il);
            c.GotoNext(MoveType.After, i => i.MatchCallvirt(out _));
            c.RemoveRange(c.Instrs.Count - 1 - c.Index); // delete all instructions after calling the original function :)
            c.Emit(OpCodes.Ret); // return
        } catch {
            MonoModHooks.DumpIL(TheGreatSidegrade.Mod, il);
        }
    }
}
