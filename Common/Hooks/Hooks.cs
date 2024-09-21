using Terraria;
using Terraria.GameContent.UI.Elements;
using TheGreatSidegrade.Common.Hooks.Compatibility;

namespace TheGreatSidegrade.Common.Hooks;

public class Hooks {
    public static void RegisterHooks() {
        // il
        IL_UIGenProgressBar.DrawSelf += WorldUIHooks.DrawSelf;
        IL_WorldGen.GenerateWorld_RunTasksAndFinish += WorldUIHooks.GenerateWorld_RunTasksAndFinish;
        // on
        On_Main.UpdateTime_StartNight += MainHooks.OnBecomeNight;
        On_Main.UpdateTime_StartDay += MainHooks.OnBecomeDay;
        On_WorldGen.TileRunner += WorldGenHooks.TileRunner;
        //On_AWorldListItem.GetIcon += WorldUI.OnGetIcon;

        // mod hooks
        if (TheGreatSidegrade.HasAvalon)
            ContagionSelectionMenuHook.Apply();
    }

    public static void UnregisterHooks() {
        IL_UIGenProgressBar.DrawSelf -= WorldUIHooks.DrawSelf;
        IL_WorldGen.GenerateWorld_RunTasksAndFinish -= WorldUIHooks.GenerateWorld_RunTasksAndFinish;
        On_Main.UpdateTime_StartNight -= MainHooks.OnBecomeNight;
        On_Main.UpdateTime_StartDay -= MainHooks.OnBecomeDay;
        On_WorldGen.TileRunner -= WorldGenHooks.TileRunner;
        //On_AWorldListItem.GetIcon -= WorldUI.OnGetIcon;

        if (TheGreatSidegrade.HasAvalon)
            ContagionSelectionMenuHook.Unapply();
    }
}
