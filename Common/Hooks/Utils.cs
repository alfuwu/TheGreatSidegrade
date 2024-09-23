using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Reflection;
using Terraria.ModLoader;

namespace TheGreatSidegrade.Common.Hooks;

public static class Utils {
    public static void CancelIL(ILContext il) {
        try {
            ILCursor c = new(il);
            c.RemoveRange(c.Instrs.Count - 1); // delete all instructions
            c.Emit(OpCodes.Ret); // return
        } catch {
            MonoModHooks.DumpIL(TheGreatSidegrade.Mod, il);
        }
    }

    public static void CancelOn(ILContext il) { // god
        try {
            ILCursor c = new(il);
            c.GotoNext(MoveType.After, i => i.MatchCallvirt(out _));
            c.RemoveRange(c.Instrs.Count - 1 - c.Index); // delete all instructions after calling the original function :)
            c.Emit(OpCodes.Ret); // return
        } catch {
            MonoModHooks.DumpIL(TheGreatSidegrade.Mod, il);
        }
    }

    public static bool TryGetType(Module mod, string className, out Type type) {
        type = mod.GetType(className);
        if (type != null)
            return true;
        return false;
    }
}
