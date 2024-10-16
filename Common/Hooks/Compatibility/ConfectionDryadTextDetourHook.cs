﻿using MonoMod.RuntimeDetour;
using System.Reflection;
using Terraria.ModLoader;

namespace TheGreatSidegrade.Common.Hooks.Compatibility;

[ExtendsFromMod("TheConfectionRebirth")]
public class ConfectionDryadTextDetourHook {
    private static ILHook applyHook = null;

    public static void Apply() {
        MethodInfo applyInfo = TheGreatSidegrade.Confection.GetType().GetMethod("On_Lang_GetDryadWorldStatusDialog", BindingFlags.NonPublic | BindingFlags.Instance);

        if (applyInfo != null) {
            applyHook = new(applyInfo, Utils.CancelOn);
            applyHook.Apply();
        }
    }

    public static void Unapply() {
        applyHook?.Undo();
    }
}
