using Terraria.Localization;
using Terraria;
using Terraria.ModLoader;
using System.Reflection;

namespace TheGreatSidegrade.Common.Hooks;

public class LangHooks {
    private string OnGetDryadWorldStatusDialog(On_Lang.orig_GetDryadWorldStatusDialog orig, out bool worldIsEntirelyPure) {
        orig(out worldIsEntirelyPure);
        string text = "";
        worldIsEntirelyPure = false;
        int tGood = WorldGen.tGood;
        int tEvil = WorldGen.tEvil;
        int tBlood = WorldGen.tBlood;
        int tSick = TheGreatSidegrade.HasAvalon ? TheGreatSidegrade.Avalon.TryFind("AvalonWorld", out ModSystem world) ? (int) world.GetType().GetField("tSick", BindingFlags.Public | BindingFlags.Static).GetValue(null) : 0 : 0;
        int tFract = GreatlySidegradedWorld.tFract;
        int tVoid = GreatlySidegradedWorld.tVoid;
        int tRot = GreatlySidegradedWorld.tRot;
        int tTwisted = GreatlySidegradedWorld.tTwisted;
        int tStarved = GreatlySidegradedWorld.tStarved;
        if (tGood > 0 && tEvil > 0 && tBlood > 0 && tSick > 0)
            text = Language.GetTextValue("Mods.Avalon.DryadSpecialText.WorldStatusAll", Main.worldName, tGood, tEvil, tBlood, tSick); //We use a combination of vanilla and our own localization to put less on translators
        else if (tGood > 0 && tSick > 0 && tEvil > 0)
            text = Language.GetTextValue("Mods.Avalon.DryadSpecialText.WorldStatusHallowCorruptSick", Main.worldName, tGood, tEvil, tSick);
        else if (tGood > 0 && tSick > 0 && tBlood > 0)
            text = Language.GetTextValue("Mods.Avalon.DryadSpecialText.WorldStatusHallowCrimsonSick", Main.worldName, tGood, tBlood, tSick);
        else if (tSick > 0 && tEvil > 0 && tBlood > 0)
            text = Language.GetTextValue("Mods.Avalon.DryadSpecialText.WorldStatusCorruptCrimsonSick", Main.worldName, tEvil, tBlood, tSick);
        else if (tGood > 0 && tEvil > 0 && tBlood > 0)
            text = Language.GetTextValue("DryadSpecialText.WorldStatusAll", Main.worldName, tGood, tEvil, tBlood);
        else if (tGood > 0 && tEvil > 0)
            text = Language.GetTextValue("DryadSpecialText.WorldStatusHallowCorrupt", Main.worldName, tGood, tEvil);
        else if (tGood > 0 && tBlood > 0)
            text = Language.GetTextValue("DryadSpecialText.WorldStatusHallowCrimson", Main.worldName, tGood, tBlood);
        else if (tSick > 0 && tEvil > 0)
            text = Language.GetTextValue("Mods.Avalon.DryadSpecialText.WorldStatusCorruptSick", Main.worldName, tSick, tEvil, tSick);
        else if (tSick > 0 && tBlood > 0)
            text = Language.GetTextValue("Mods.Avalon.DryadSpecialText.WorldStatusCrimsonSick", Main.worldName, tSick, tBlood, tSick);
        else if (tEvil > 0 && tBlood > 0)
            text = Language.GetTextValue("DryadSpecialText.WorldStatusCorruptCrimson", Main.worldName, tEvil, tBlood);
        else if (tGood > 0 && tSick > 0)
            text = Language.GetTextValue("Mods.Avalon.DryadSpecialText.WorldStatusHallowSick", Main.worldName, tGood, tSick);
        else if (tSick > 0)
            text = Language.GetTextValue("Mods.Avalon.DryadSpecialText.WorldStatusSick", Main.worldName, tSick);
        else if (tEvil > 0)
            text = Language.GetTextValue("DryadSpecialText.WorldStatusCorrupt", Main.worldName, tEvil);
        else if (tBlood > 0)
            text = Language.GetTextValue("DryadSpecialText.WorldStatusCrimson", Main.worldName, tBlood);
        else {
            if (tGood <= 0) {
                text = Language.GetTextValue("DryadSpecialText.WorldStatusPure", Main.worldName);
                worldIsEntirelyPure = true;
                return text;
            }
            text = Language.GetTextValue("DryadSpecialText.WorldStatusHallow", Main.worldName, tGood);
        }
        int all = tEvil + tBlood + tSick + tFract + tVoid + tRot + tTwisted + tStarved;
        string arg = tGood * 1.2 >= all && tGood * 0.8 <= all ?Language.GetTextValue("DryadSpecialText.WorldDescriptionBalanced") :
            tGood >= all ? Language.GetTextValue("DryadSpecialText.WorldDescriptionFairyTale") :
            all > tGood + 20 ? Language.GetTextValue("DryadSpecialText.WorldDescriptionGrim") :
            all <= 5 ? Language.GetTextValue("DryadSpecialText.WorldDescriptionClose") :
            Language.GetTextValue("DryadSpecialText.WorldDescriptionWork");
        return $"{text} {arg}";
    }
}
