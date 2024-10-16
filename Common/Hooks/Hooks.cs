﻿using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent.UI.States;
using Terraria.ModLoader;
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
        On_WorldGen.AddUpAlignmentCounts += WorldGenHooks.AddUpAlignmentCounts;
        On_WorldGen.CountTiles += WorldGenHooks.CountTiles;
        On_Lang.GetDryadWorldStatusDialog += LangHooks.OnGetDryadWorldStatusDialog;
        On_AWorldListItem.GetIcon += WorldUIHooks.OnGetIcon;
        On_Player.UpdateLifeRegen += PlayerHooks.OnUpdateLifeRegen;
        On_Player.UpdateManaRegen += PlayerHooks.OnUpdateManaRegen;
        
        // world creation ui
        IL_UIWorldCreation.BuildPage += WorldUIHooks.BuildPage;
        IL_UIWorldCreation.MakeInfoMenu += WorldUIHooks.MakeInfoMenu;
        IL_UIWorldCreation.ShowOptionDescription += WorldUIHooks.ShowOptionDescription;
        IL_UIWorldCreation.UpdatePreviewPlate += WorldUIHooks.UpdatePreviewPlate;
        IL_UIWorldCreationPreview.DrawSelf += WorldUIHooks.PreviewDrawSelf;
        On_UIWorldCreation.SetDefaultOptions += WorldUIHooks.OnSetDefaultOptions;
        On_UIWorldCreation.AddWorldEvilOptions += WorldUIHooks.OnAddWorldEvilOptions;
        On_UIWorldCreation.SetupGamepadPoints += WorldUIHooks.OnSetupGamepadPoints;

        // mod hooks
        if (TheGreatSidegrade.HasAvalon) {
            ContagionSelectionMenuHook.Apply();
            AvalonDryadTextDetourHook.Apply();
        }
        if (TheGreatSidegrade.HasConfection) {
            ConfectionSelectionMenuHook.Apply();
            ConfectionDryadTextDetourHook.Apply();
        }
        if (ModLoader.HasMod("TheDepths"))
            TheDepthsSelectionMenuHook.Apply();
    }

    public static void UnregisterHooks() {
        IL_UIGenProgressBar.DrawSelf -= WorldUIHooks.DrawSelf;
        IL_WorldGen.GenerateWorld_RunTasksAndFinish -= WorldUIHooks.GenerateWorld_RunTasksAndFinish;
        On_Main.UpdateTime_StartNight -= MainHooks.OnBecomeNight;
        On_Main.UpdateTime_StartDay -= MainHooks.OnBecomeDay;
        On_WorldGen.TileRunner -= WorldGenHooks.TileRunner;
        On_Lang.GetDryadWorldStatusDialog -= LangHooks.OnGetDryadWorldStatusDialog;
        On_AWorldListItem.GetIcon -= WorldUIHooks.OnGetIcon;
        On_Player.UpdateLifeRegen -= PlayerHooks.OnUpdateLifeRegen;
        On_Player.UpdateManaRegen -= PlayerHooks.OnUpdateManaRegen;
        
        IL_UIWorldCreation.BuildPage -= WorldUIHooks.BuildPage;
        IL_UIWorldCreation.MakeInfoMenu -= WorldUIHooks.MakeInfoMenu;
        IL_UIWorldCreation.ShowOptionDescription -= WorldUIHooks.ShowOptionDescription;
        IL_UIWorldCreation.UpdatePreviewPlate -= WorldUIHooks.UpdatePreviewPlate;
        IL_UIWorldCreationPreview.DrawSelf -= WorldUIHooks.PreviewDrawSelf;
        On_UIWorldCreation.SetDefaultOptions -= WorldUIHooks.OnSetDefaultOptions;
        On_UIWorldCreation.AddWorldEvilOptions -= WorldUIHooks.OnAddWorldEvilOptions;
        On_UIWorldCreation.SetupGamepadPoints -= WorldUIHooks.OnSetupGamepadPoints;

        if (TheGreatSidegrade.HasAvalon) {
            ContagionSelectionMenuHook.Unapply();
            AvalonDryadTextDetourHook.Unapply();
        }
        if (TheGreatSidegrade.HasConfection) {
            ConfectionSelectionMenuHook.Unapply();
            ConfectionDryadTextDetourHook.Unapply();
        }
        if (ModLoader.HasMod("TheDepths"))
            TheDepthsSelectionMenuHook.Unapply();
    }
}
