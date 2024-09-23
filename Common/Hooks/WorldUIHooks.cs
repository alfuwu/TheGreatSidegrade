using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent.UI.States;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
using Terraria.UI.Gamepad;
using TheGreatSidegrade.Assets;
using TheGreatSidegrade.Common.UI;

namespace TheGreatSidegrade.Common.Hooks;

public class WorldUIHooks {
    public static void DrawSelf(ILContext il) {
        try {
            ILCursor c = new(il);
            // color swapping
            //c.GotoNext(i => i.MatchInitobj(out _)); // goto init color
            c.GotoNext(i => i.MatchCall(typeof(Color).GetMethod("set_PackedValue")));
            ILLabel colorEnd = c.DefineLabel();
            c.MarkLabel(colorEnd);
            c.GotoPrev(MoveType.After, i => i.MatchLdloca(out _)); // go back to init color
            ILLabel vanillaColor = c.DefineLabel();
            c.Emit(OpCodes.Call, typeof(GreatlySidegradedWorld).GetMethod(nameof(GreatlySidegradedWorld.IsVanillaEvil), []));
            c.Emit(OpCodes.Brtrue_S, vanillaColor);
            c.Emit(OpCodes.Call, typeof(WorldUIHooks).GetMethod(nameof(GetGenProgressBarColor))); // get uint
            c.Emit(OpCodes.Br_S, colorEnd);
            c.MarkLabel(vanillaColor);

            // texture swapping
            c.Index = c.Instrs.Count - 1; // goto return
            c.GotoPrev(i => i.MatchLdarg0()); // goto load UIGenProgress._texOuterLower
            c.GotoPrev(i => i.MatchLdloc(out _)); // goto end
            ILLabel end = c.DefineLabel(); // create a label to skip to when done with textures
            c.MarkLabel(end);
            c.GotoPrev(i => i.MatchLdarg0()); // goto load UIGenProgress._texOuterCrimson
            c.GotoPrev(i => i.MatchLdarg0()); // goto load UIGenProgress._texOuterCorruption
            c.GotoPrev(i => i.MatchLdloc0()); // goto vanilla behavior
            ILLabel vanilla = c.DefineLabel(); // why
            c.Emit(OpCodes.Call, typeof(GreatlySidegradedWorld).GetMethod(nameof(GreatlySidegradedWorld.IsVanillaEvil), []));
            c.Emit(OpCodes.Brtrue_S, vanilla); // skip to vanilla textures if world is using a vanilla evil
            c.Emit(OpCodes.Call, typeof(WorldUIHooks).GetMethod(nameof(GetGenProgressBarTexture))); // get Texture2D
            c.Emit(OpCodes.Br_S, end); // skip to end
            c.MarkLabel(vanilla);
            //MonoModHooks.DumpIL(TheGreatSidegrade.Mod, il);
        } catch (Exception e) {
            //MonoModHooks.DumpIL(TheGreatSidegrade.Mod, il);

            throw new ILPatchFailureException(TheGreatSidegrade.Mod, il, e);
        }
    }

    public static uint GetGenProgressBarColor() {
        return GreatlySidegradedWorld.worldEvil switch {
            GreatlySidegradedWorld.WorldEvil.Fractured => 0xFF000000u,
            GreatlySidegradedWorld.WorldEvil.Nothing => 0xFFFFFFFFu,
            GreatlySidegradedWorld.WorldEvil.Rotten => 0xFFF0CFFFu,
            GreatlySidegradedWorld.WorldEvil.Spiral => 0xFFFFA291u,
            GreatlySidegradedWorld.WorldEvil.Starved => 0xFF4C7DFFu,
            _ => 0u
        };
    }

    public static Texture2D GetGenProgressBarTexture() {
        return GreatlySidegradedWorld.worldEvil switch {
            GreatlySidegradedWorld.WorldEvil.Fractured => GameAssets.GetTexture("Assets/Textures/UI/WorldGen/Outer_Fractured"),
            GreatlySidegradedWorld.WorldEvil.Nothing => GameAssets.GetTexture("Assets/Textures/UI/WorldGen/Outer_Nothing"),
            GreatlySidegradedWorld.WorldEvil.Rotten => GameAssets.GetTexture("Assets/Textures/UI/WorldGen/Outer_Rotten"),
            GreatlySidegradedWorld.WorldEvil.Spiral => GameAssets.GetTexture("Assets/Textures/UI/WorldGen/Outer_Spiral"),
            GreatlySidegradedWorld.WorldEvil.Starved => GameAssets.GetTexture("Assets/Textures/UI/WorldGen/Outer_Starved"),
            _ => null
        };
    }

    // why did i do this
    // ocd is real
    public static void GenerateWorld_RunTasksAndFinish(ILContext il) {
        try {
            ILCursor c = new(il);
            c.GotoNext(MoveType.After, i => i.MatchLdstr("Crimson"));
            ILLabel end = c.DefineLabel();
            c.MarkLabel(end);
            c.GotoPrev(MoveType.Before, i => i.MatchLdsfld(typeof(WorldGen).GetField(nameof(WorldGen.crimson))));
            ILLabel vanilla = c.DefineLabel();
            c.Emit(OpCodes.Call, typeof(GreatlySidegradedWorld).GetMethod(nameof(GreatlySidegradedWorld.IsVanillaEvil), []));
            c.Emit(OpCodes.Brtrue_S, vanilla);
            c.Emit(OpCodes.Ldsfld, typeof(GreatlySidegradedWorld).GetField(nameof(GreatlySidegradedWorld.worldEvil)));
            c.Emit(OpCodes.Box, typeof(GreatlySidegradedWorld.WorldEvil));
            c.Emit(OpCodes.Call, typeof(object).GetMethod(nameof(GetType)));
            c.Emit(OpCodes.Ldsfld, typeof(GreatlySidegradedWorld).GetField(nameof(GreatlySidegradedWorld.worldEvil)));
            c.Emit(OpCodes.Box, typeof(GreatlySidegradedWorld.WorldEvil));
            c.Emit(OpCodes.Call, typeof(Enum).GetMethod(nameof(Enum.GetName), [typeof(Type), typeof(object)]));
            c.Emit(OpCodes.Br_S, end);
            c.MarkLabel(vanilla);
            //MonoModHooks.DumpIL(TheGreatSidegrade.Mod, il);
        } catch {
            MonoModHooks.DumpIL(TheGreatSidegrade.Mod, il);

            //throw new ILPatchFailureException(TheGreatSidegrade.Mod, il, e);
        }
    }

    public static Asset<Texture2D> OnGetIcon(On_AWorldListItem.orig_GetIcon orig, AWorldListItem self) {
        if (self.Data.TryGetHeaderData<GreatlySidegradedWorld>(out TagCompound header)) {
            GreatlySidegradedWorld.WorldEvil worldEvil = 0;

            if (header.TryGet("WorldEvil", out byte tmp))
                worldEvil = (GreatlySidegradedWorld.WorldEvil) tmp;

            if (!GreatlySidegradedWorld.IsVanillaEvil(worldEvil)) {
                string iconPath = $"{nameof(TheGreatSidegrade)}/Assets/Textures/UI/";
                iconPath += Enum.GetName(worldEvil) + "/Icon";
                iconPath += self.Data.IsHardMode ? "Hallow" : "";
                if (self.Data.ZenithWorld)
                    iconPath += "";//"Everything";
                else if (self.Data.DrunkWorld)
                    iconPath += "Corruption";
                else if (self.Data.ForTheWorthy)
                    iconPath += "FTW";
                else if (self.Data.NotTheBees)
                    iconPath += "NotTheBes";
                else if (self.Data.Anniversary)
                    iconPath += "Anniversary";
                else if (self.Data.DontStarve)
                    iconPath += "DontStarve";
                else if (self.Data.RemixWorld)
                    iconPath += "Remix";
                else if (self.Data.NoTrapsWorld)
                    iconPath += "Traps";

                return ModContent.Request<Texture2D>(iconPath, AssetRequestMode.ImmediateLoad);
            }
        }
        return orig(self);
    }

    private static readonly CustomGroupOptionButton<WorldEvilId>[] evilButtons = new CustomGroupOptionButton<WorldEvilId>[Enum.GetValues<WorldEvilId>().Length];
    public static WorldEvilId SelectedWorldEvil { get; set; }

    public enum WorldEvilId {
        Random,
        Corruption,
        Crimson,
        Contagion,
        Fractured,
        Nothing,
        Rotten,
        Spiral,
        Starved
    }

    public static void BuildPage(ILContext il) {
        ILCursor c = new(il);

        // Increase page size
        c.GotoNext(i => i.MatchStloc(0));
        c.Emit(OpCodes.Ldc_I4, 38);
        c.Emit(OpCodes.Add);
    }

    public static void MakeInfoMenu(ILContext il) {
        ILCursor c = new(il);

        c.GotoNext(i => i.MatchLdstr("evil"));

        c.Emit(OpCodes.Ldloc_1);
        c.Emit(OpCodes.Ldc_R4, 38f);
        c.Emit(OpCodes.Add);
        c.Emit(OpCodes.Stloc_1);
    }

    public static void OnAddWorldEvilOptions(On_UIWorldCreation.orig_AddWorldEvilOptions orig, UIWorldCreation self, UIElement container, float accumulatedHeight, UIElement.MouseEvent clickEvent, string tagGroup, float usableWidthPercent) {
        orig(self, new UIElement(), accumulatedHeight, clickEvent, tagGroup, usableWidthPercent);
        LocalizedText[] titles = [
            Lang.misc[103],
            Lang.misc[101],
            Lang.misc[102],
            Language.GetText("Mods.Avalon.World.EvilSelection.Contagion.Title"),
            Language.GetText($"Mods.{nameof(TheGreatSidegrade)}.World.Creation.EvilFractured.Title"),
            Language.GetText($"Mods.{nameof(TheGreatSidegrade)}.World.Creation.EvilNothing.Title"),
            Language.GetText($"Mods.{nameof(TheGreatSidegrade)}.World.Creation.EvilRotten.Title"),
            Language.GetText($"Mods.{nameof(TheGreatSidegrade)}.World.Creation.EvilSpiral.Title"),
            Language.GetText($"Mods.{nameof(TheGreatSidegrade)}.World.Creation.EvilStarved.Title")
        ];
        LocalizedText[] descriptions = [
            Language.GetText("UI.WorldDescriptionEvilRandom"),
            Language.GetText("UI.WorldDescriptionEvilCorrupt"),
            Language.GetText("UI.WorldDescriptionEvilCrimson"),
            Language.GetText("Mods.Avalon.World.EvilSelection.Contagion.Description"),
            Language.GetText($"Mods.{nameof(TheGreatSidegrade)}.World.Creation.EvilFractured.Description"),
            Language.GetText($"Mods.{nameof(TheGreatSidegrade)}.World.Creation.EvilNothing.Description"),
            Language.GetText($"Mods.{nameof(TheGreatSidegrade)}.World.Creation.EvilRotten.Description"),
            Language.GetText($"Mods.{nameof(TheGreatSidegrade)}.World.Creation.EvilSpiral.Description"),
            Language.GetText($"Mods.{nameof(TheGreatSidegrade)}.World.Creation.EvilStarved.Description")
        ];
        Color[] colors = [
            Color.White,
            Color.MediumPurple,
            Color.IndianRed,
            Color.Green,
            Color.Gray,
            Color.Black,
            new(132, 78, 70),
            Color.LightSkyBlue,
            Color.Yellow
        ];
        Asset<Texture2D>[] icons = [
            Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/IconEvilRandom"),
            Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/IconEvilCorruption"),
            Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/IconEvilCrimson"),
            TheGreatSidegrade.HasAvalon ? ModContent.Request<Texture2D>("Avalon/Assets/Textures/UI/WorldCreation/IconContagion",
                AssetRequestMode.ImmediateLoad) : null,
            ModContent.Request<Texture2D>($"{TheGreatSidegrade.AssetPath}/Textures/UI/WorldCreation/IconEvilFractured",
                AssetRequestMode.ImmediateLoad),
            ModContent.Request<Texture2D>($"{TheGreatSidegrade.AssetPath}/Textures/UI/WorldCreation/IconEvilNothing",
                AssetRequestMode.ImmediateLoad),
            ModContent.Request<Texture2D>($"{TheGreatSidegrade.AssetPath}/Textures/UI/WorldCreation/IconEvilRotten",
                AssetRequestMode.ImmediateLoad),
            ModContent.Request<Texture2D>($"{TheGreatSidegrade.AssetPath}/Textures/UI/WorldCreation/IconEvilSpiral",
                AssetRequestMode.ImmediateLoad),
            ModContent.Request<Texture2D>($"{TheGreatSidegrade.AssetPath}/Textures/UI/WorldCreation/IconEvilStarved",
                AssetRequestMode.ImmediateLoad),
        ];

        // this is fucking cursed
        int f = TheGreatSidegrade.HasAvalon ? 4 : 5;
        for (int i = 0; i < 2; i++) {
            int max = i == 0 ? 4 : evilButtons.Length - f;
            for (int j = f * i; j < (i == 0 ? f : evilButtons.Length); j++) {
                if (!TheGreatSidegrade.HasAvalon && Enum.GetValues<WorldEvilId>()[j] == WorldEvilId.Contagion) {
                    evilButtons[j] = null;
                    continue;
                }
                CustomGroupOptionButton<WorldEvilId> groupOptionButton = new(
                    Enum.GetValues<WorldEvilId>()[j],
                    titles[j],
                    descriptions[j],
                    colors[j],
                    icons[j],
                    1f,
                    1f,
                    16f) {
                    Width = StyleDimension.FromPixelsAndPercent(
                        -(max - 1),
                        1f / max * usableWidthPercent),
                    Left = StyleDimension.FromPercent(1f - usableWidthPercent),
                    HAlign = (j - (f * i) - (!TheGreatSidegrade.HasAvalon && Enum.GetValues<WorldEvilId>()[j] > WorldEvilId.Contagion && i == 0 ? 1 : 0)) / (float) (max - 1),
                };
                TheGreatSidegrade.Mod.Logger.Info(1f - usableWidthPercent);
                groupOptionButton.Top.Set(accumulatedHeight + (38 * i), 0f);
                groupOptionButton.OnLeftMouseDown += (evt, element) => ClickEvilOption(self, evt, element);
                groupOptionButton.OnMouseOver += self.ShowOptionDescription;
                groupOptionButton.OnMouseOut += self.ClearOptionDescription;
                groupOptionButton.SetSnapPoint(i == 0 ? tagGroup : "evil2", j);
                container.Append(groupOptionButton);
                evilButtons[j] = groupOptionButton;
            }
        }
    }

    public static void OnSetDefaultOptions(On_UIWorldCreation.orig_SetDefaultOptions orig, UIWorldCreation self) {
        orig(self);

        foreach (CustomGroupOptionButton<WorldEvilId> evilButton in evilButtons)
            evilButton?.SetCurrentOption(SelectedWorldEvil);
    }

    public static void ShowOptionDescription(ILContext il) {
        ILCursor c = new(il);

        // Navigate to before final break
        c.Index = c.Instrs.Count - 1;
        c.GotoPrev(i => i.MatchBrfalse(out _));

        // Add description handling logic
        c.Emit(OpCodes.Pop);
        c.Emit(OpCodes.Ldloc_0); // localizedText
        c.Emit(OpCodes.Ldarg_2); // listeningElement
        c.EmitDelegate((LocalizedText localizedText, UIElement listeningElement) => {
            if (listeningElement is CustomGroupOptionButton<WorldEvilId> evilButton)
                localizedText = evilButton.Description;

            return localizedText;
        });
        c.Emit(OpCodes.Stloc_0);
        c.Emit(OpCodes.Ldloc_0);
    }

    public static void ClickEvilOption(UIWorldCreation self, UIMouseEvent _, UIElement listeningElement) {
        CustomGroupOptionButton<WorldEvilId> groupOptionButton = (CustomGroupOptionButton<WorldEvilId>)listeningElement;

        SelectedWorldEvil = groupOptionButton.OptionValue;
        foreach (CustomGroupOptionButton<WorldEvilId> evilButton in evilButtons)
            evilButton?.SetCurrentOption(SelectedWorldEvil);

        typeof(UIWorldCreation).GetMethod("UpdatePreviewPlate", BindingFlags.Instance | BindingFlags.NonPublic)?.Invoke(self, []);
    }

    public static void UpdatePreviewPlate(ILContext il) {
        ILCursor c = new(il);

        c.GotoNext(i =>
                i.MatchLdfld(typeof(UIWorldCreation).GetField("_optionEvil",
                    BindingFlags.Instance | BindingFlags.NonPublic)!))
            .GotoNext(MoveType.After, i => i.MatchConvU1())
            .EmitDelegate((byte value) => (byte) SelectedWorldEvil);
    }

    public static void PreviewDrawSelf(ILContext il) {
        ILCursor c = new(il);

        c.GotoNext(MoveType.After, i =>
                i.MatchLdfld(typeof(UIWorldCreationPreview).GetField("_evil",
                    BindingFlags.Instance | BindingFlags.NonPublic)!))
            .Emit(OpCodes.Ldarg_1)
            .Emit(OpCodes.Ldloc_1)
            .Emit(OpCodes.Ldloc_2)
            .EmitDelegate((byte evil, SpriteBatch spriteBatch, Vector2 position, Color color) => {
                switch (evil) {
                    case 0:
                    case 1:
                    case 2:
                        break;
                    case 3:
                        spriteBatch.Draw(ModContent.Request<Texture2D>(
                            "Avalon/Assets/Textures/UI/WorldCreation/PreviewEvilContagion",
                            AssetRequestMode.ImmediateLoad).Value, position, color);
                        break;
                    default:
                        spriteBatch.Draw(ModContent.Request<Texture2D>(
                            $"{TheGreatSidegrade.AssetPath}/Textures/UI/WorldCreation/PreviewEvil{Enum.GetNames<WorldEvilId>()[evil]}",
                            AssetRequestMode.ImmediateLoad).Value, position, color);
                        break;
                }

                return evil;
            });
    }

    public static void OnSetupGamepadPoints(On_UIWorldCreation.orig_SetupGamepadPoints orig, UIWorldCreation self, SpriteBatch spriteBatch) {
        orig(self, spriteBatch);
        int num = 3006;
        List<SnapPoint> snapPoints = self.GetSnapPoints();
        List<SnapPoint> snapGroup = GetSnapGroup(self, snapPoints, "size");
        List<SnapPoint> snapGroup2 = GetSnapGroup(self, snapPoints, "difficulty");
        List<SnapPoint> snapGroup3 = GetSnapGroup(self, snapPoints, "evil");
        num += snapGroup.Count + snapGroup2.Count;
        List<SnapPoint> snapGroup4 = GetSnapGroup(self, snapPoints, "evil2");
        List<SnapPoint> snapGroup5 = GetSnapGroup(self, snapPoints, "hallow");

        UILinkPoint uILinkPoint;
        UILinkPoint uILinkPoint2 = UILinkPointNavigator.Points[3000];
        UILinkPoint uILinkPoint3 = UILinkPointNavigator.Points[3001];

        UILinkPoint[] array = new UILinkPoint[snapGroup3.Count];
        for (int l = 0; l < snapGroup4.Count; l++) {
            UILinkPointNavigator.SetPosition(num, snapGroup3[l].Position);
            uILinkPoint = UILinkPointNavigator.Points[num];
            array[l] = uILinkPoint;
            num++;
        }
        UILinkPoint[] array2 = new UILinkPoint[snapGroup4.Count];
        for (int l = 0; l < snapGroup4.Count; l++) {
            UILinkPointNavigator.SetPosition(num, snapGroup4[l].Position);
            uILinkPoint = UILinkPointNavigator.Points[num];
            uILinkPoint.Unlink();
            array2[l] = uILinkPoint;
            num++;
        }
        UILinkPoint[] array3 = snapGroup5.Count > 0 ? new UILinkPoint[snapGroup5.Count] : null;
        for (int l = 0; l < snapGroup5.Count; l++) {
            UILinkPointNavigator.SetPosition(num, snapGroup5[l].Position);
            uILinkPoint = UILinkPointNavigator.Points[num];
            uILinkPoint.Unlink();
            array3[l] = uILinkPoint;
            num++;
        }

        LoopHorizontalLineLinks(self, array2);
        EstablishUpDownRelationship(self, array, array2);
        if (array3 == null) {
            for (int n = 0; n < array2.Length; n++)
                array2[n].Down = uILinkPoint2.ID;

            array2[^1].Down = uILinkPoint3.ID;
            uILinkPoint3.Up = array2[^1].ID;
            uILinkPoint2.Up = array2[0].ID;
        } else {
            LoopHorizontalLineLinks(self, array3);
            EstablishUpDownRelationship(self, array2, array3);
            for (int n = 0; n < array3.Length; n++)
                array3[n].Down = uILinkPoint2.ID;

            array3[^1].Down = uILinkPoint3.ID;
            uILinkPoint3.Up = array3[^1].ID;
            uILinkPoint2.Up = array3[0].ID;
        }
    }

    // reflection go brrrr
    private static List<SnapPoint> GetSnapGroup(UIWorldCreation self, List<SnapPoint> snapPoints, string group) {
        return (List<SnapPoint>) typeof(UIWorldCreation).GetMethod("GetSnapGroup", BindingFlags.NonPublic | BindingFlags.Instance)?
            .Invoke(self, [snapPoints, group]);
    }

    private static void LoopHorizontalLineLinks(UIWorldCreation self, UILinkPoint[] pointsLine) {
        typeof(UIWorldCreation).GetMethod("LoopHorizontalLineLinks", BindingFlags.NonPublic | BindingFlags.Instance)?
            .Invoke(self, [pointsLine]);
    }

    private static void EstablishUpDownRelationship(UIWorldCreation self, UILinkPoint[] topSide, UILinkPoint[] bottomSide) {
        typeof(UIWorldCreation).GetMethod("EstablishUpDownRelationship", BindingFlags.NonPublic | BindingFlags.Instance)?
            .Invoke(self, [topSide, bottomSide]);
    }
}