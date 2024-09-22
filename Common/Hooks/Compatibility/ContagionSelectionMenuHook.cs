using MonoMod.RuntimeDetour;
using System.Reflection;
using Terraria.ModLoader;
using Avalon.Hooks;

namespace TheGreatSidegrade.Common.Hooks.Compatibility;

[Autoload(Side = ModSide.Client)]
[ExtendsFromMod("Avalon")]
public class ContagionSelectionMenuHook { // this is so cursed
    private static ILHook applyHook = null;

    public static void Apply() {
        MethodInfo applyInfo = typeof(ContagionSelectionMenu).GetMethod("Apply", BindingFlags.NonPublic | BindingFlags.Instance);
        
        if (applyInfo != null ) {
            applyHook = new(applyInfo, Utils.CancelIL);
            applyHook.Apply();
        }
    }

    public static void Unapply() {
        applyHook?.Undo();
    }

    public static void SetContagion(bool c) {
        typeof(ContagionSelectionMenu).GetProperty("SelectedWorldEvil", BindingFlags.Public | BindingFlags.Instance)
            .SetValue(ModContent.GetInstance<ContagionSelectionMenu>(), c ?
                ContagionSelectionMenu.WorldEvilSelection.Contagion :
                ContagionSelectionMenu.WorldEvilSelection.Corruption);
    }
}
