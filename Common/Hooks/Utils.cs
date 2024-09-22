using Mono.Cecil.Cil;
using MonoMod.Cil;
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
}
