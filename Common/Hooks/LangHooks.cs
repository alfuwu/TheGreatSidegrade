using Terraria.Localization;
using Terraria;
using Terraria.ModLoader;
using System.Reflection;
using System.Collections.Generic;

namespace TheGreatSidegrade.Common.Hooks;

public class LangHooks {
    public static string OnGetDryadWorldStatusDialog(On_Lang.orig_GetDryadWorldStatusDialog orig, out bool worldIsEntirelyPure) {
        orig(out _);
        string unlocalizedText = "DryadSpecialText.WorldStatus";
        worldIsEntirelyPure = false;
        int tGood = WorldGen.tGood;
        int tEvil = WorldGen.tEvil;
        int tBlood = WorldGen.tBlood;
        int tSick = TheGreatSidegrade.HasAvalon ? TheGreatSidegrade.Avalon.TryFind("AvalonWorld", out ModSystem world) ? (byte) world.GetType().GetField("tSick", BindingFlags.Public | BindingFlags.Static).GetValue(null) : 0 : 0;
        //TheGreatSidegrade.Mod.Logger.Info("Curetn TSICK: " + tSick); // is zero :c boowomp
        int tCandy = TheGreatSidegrade.HasConfection ? TheGreatSidegrade.Confection.TryFind("ConfectionWorldGeneration", out ModSystem world2) ? (byte) world2.GetType().GetField("tCandy", BindingFlags.Public | BindingFlags.Static).GetValue(null) : 0 : 0;
        int tFract = GreatlySidegradedWorld.tFract;
        int tVoid = ModContent.GetInstance<SidegradeConfig>().DryadKnowsNothingStatus ? GreatlySidegradedWorld.tVoid : 0;
        int tRot = GreatlySidegradedWorld.tRot;
        int tTwisted = GreatlySidegradedWorld.tTwisted;
        int tStarved = GreatlySidegradedWorld.tStarved;
        int allEvil = tEvil + tBlood + tSick + tFract + tVoid + tRot + tTwisted + tStarved;
        int allGood = tGood + tCandy;
        if (allGood <= 0 &&  allEvil <= 0) {
            worldIsEntirelyPure = true;
            return Language.GetTextValue("DryadSpecialText.WorldStatusPure", Main.worldName);
        }
        //bool onlyVanilla = allEvil == tEvil + tBlood && tCandy <= 0;
        if (allEvil == tEvil + tBlood + tSick && tCandy <= 0 && tSick > 0)
            unlocalizedText = "Mods.Avalon." + unlocalizedText;
        else if (allEvil == tEvil + tBlood && tCandy > 0)
            unlocalizedText = "Mods.TheConfectionRebirth." + unlocalizedText;
        else if (tSick > 0 || tCandy > 0 || tFract > 0 || tVoid > 0 || tRot > 0 || tTwisted > 0 || tStarved > 0)
            unlocalizedText = $"{TheGreatSidegrade.Localization}.{unlocalizedText}";
        // what a goddamn mess
        // redcode go brrrrrrrr
        // sorry i tried fixing it but its still messy as hell
        List<object> args = [Main.worldName];
        if (tGood > 0) {
            unlocalizedText += "Hallow";
            args.Add(tGood);
        }
        if (tCandy > 0) {
            unlocalizedText += "Candy";
            args.Add(tCandy);
        }
        if (tEvil > 0) {
            unlocalizedText += "Corrupt";
            args.Add(tEvil);
        }
        if (tBlood > 0) {
            unlocalizedText += "Crimson";
            args.Add(tBlood);
        }
        if (tSick > 0) {
            unlocalizedText += "Sick";
            args.Add(tSick);
        }
        if (tFract > 0) {
            unlocalizedText += "Fractured";
            args.Add(tFract);
        }
        if (tVoid > 0) {
            unlocalizedText += "Nothing";
            args.Add(tVoid);
        }
        if (tRot > 0) {
            unlocalizedText += "Rotten";
            args.Add(tRot);
        }
        if (tTwisted > 0) {
            unlocalizedText += "Spiral";
            args.Add(tTwisted);
        }
        if (tStarved > 0) {
            unlocalizedText += "Starved";
            args.Add(tStarved);
        }
        bool insult = tGood > 0 && tEvil > 0 && tBlood > 0 && tFract > 0 && (!ModContent.GetInstance<SidegradeConfig>().DryadKnowsNothingStatus || tVoid > 0) && tRot > 0 && tTwisted > 0 && tStarved > 0 && (!TheGreatSidegrade.HasAvalon || tSick > 0) && (!TheGreatSidegrade.HasConfection || tCandy > 0);
        if (insult && tSick > 0 && tCandy > 0)
            unlocalizedText = $"{TheGreatSidegrade.Localization}.DryadSpecialText.WorldStatusAll";
        else if (allEvil == tEvil + tBlood + tSick && tEvil > 0 && tBlood > 0 && tSick > 0 && tCandy <= 0)
            unlocalizedText = "Mods.Avalon.DryadSpecialText.WorldStatusAll";
        else if (allEvil == tEvil + tBlood && tEvil > 0 && tBlood > 0 && tGood > 0 && tCandy > 0)
            unlocalizedText = "Mods.TheConfectionRebirth.DryadSpecialText.WorldStatusAll";
        else if (allEvil == tEvil + tBlood && tEvil > 0 && tBlood > 0 && tGood > 0 && tCandy <= 0)
            unlocalizedText = "DryadSpecialText.WorldStatusAll";
        //TheGreatSidegrade.Mod.Logger.Info(unlocalizedText);
        //TheGreatSidegrade.Mod.Logger.Info($"{Language.GetTextValue(unlocalizedText, [.. args])} " +
        //    (insult ? Language.GetTextValue("Mods.TheGreatSidegrade.DryadSpecialText.WhatTheHell") : ""));
        string arg = allGood * 1.2 >= allEvil && allGood * 0.8 <= allEvil ? Language.GetTextValue("DryadSpecialText.WorldDescriptionBalanced") :
            allGood >= allEvil ? Language.GetTextValue("DryadSpecialText.WorldDescriptionFairyTale") :
            allEvil > allGood + 20 ? Language.GetTextValue("DryadSpecialText.WorldDescriptionGrim") :
            allEvil <= 5 ? Language.GetTextValue("DryadSpecialText.WorldDescriptionClose") :
            Language.GetTextValue("DryadSpecialText.WorldDescriptionWork");
        return $"{Language.GetTextValue(unlocalizedText, [.. args])} {arg}" +
            (insult ? Language.GetTextValue($"{TheGreatSidegrade.Localization}.DryadSpecialText.WhatTheHell") : "");
    }
}
