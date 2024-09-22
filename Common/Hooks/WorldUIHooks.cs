using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using TheGreatSidegrade.Assets;

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
                return ModContent.Request<Texture2D>(iconPath, AssetRequestMode.ImmediateLoad);
            }
        }
        return orig(self);
    }
}