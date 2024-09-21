using Terraria;
using Terraria.GameContent.UI.Elements;

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
    }

    public static void UnregisterHooks() {
        IL_UIGenProgressBar.DrawSelf -= WorldUIHooks.DrawSelf;
        IL_WorldGen.GenerateWorld_RunTasksAndFinish -= WorldUIHooks.GenerateWorld_RunTasksAndFinish;
        On_Main.UpdateTime_StartNight -= MainHooks.OnBecomeNight;
        On_Main.UpdateTime_StartDay -= MainHooks.OnBecomeDay;
        On_WorldGen.TileRunner -= WorldGenHooks.TileRunner;
        //On_AWorldListItem.GetIcon -= WorldUI.OnGetIcon;
    }
}
