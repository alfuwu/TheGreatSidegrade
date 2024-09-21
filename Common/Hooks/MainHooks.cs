using Terraria;

namespace TheGreatSidegrade.Common.Hooks;

public class MainHooks {
    public static void OnBecomeNight(On_Main.orig_UpdateTime_StartNight orig, ref bool stopEvents) {
        orig(ref stopEvents);
        TheGreatSidegrade.NightJustStarted = true;
    }

    public static void OnBecomeDay(On_Main.orig_UpdateTime_StartDay orig, ref bool stopEvents) {
        orig(ref stopEvents);
        TheGreatSidegrade.DayJustStarted = true;
    }
}
