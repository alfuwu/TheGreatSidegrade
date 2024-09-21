using Terraria;
using Terraria.GameContent.UI.Elements;

namespace TheGreatSidegrade.Common.Hooks;

public class Hooks {
    public static void RegisterHooks() {
        // il
        IL_UIGenProgressBar.DrawSelf += WorldUI.DrawSelf;
        IL_WorldGen.GenerateWorld_RunTasksAndFinish += WorldUI.GenerateWorld_RunTasksAndFinish;
        // on
        On_Main.UpdateTime_StartNight += Main.OnBecomeNight;
        On_Main.UpdateTime_StartDay += Main.OnBecomeDay;
        //On_AWorldListItem.GetIcon += WorldUI.OnGetIcon;
    }

    public static void UnregisterHooks() {
        IL_UIGenProgressBar.DrawSelf -= WorldUI.DrawSelf;
        IL_WorldGen.GenerateWorld_RunTasksAndFinish -= WorldUI.GenerateWorld_RunTasksAndFinish;
        //On_AWorldListItem.GetIcon -= WorldUI.OnGetIcon;
    }
}
