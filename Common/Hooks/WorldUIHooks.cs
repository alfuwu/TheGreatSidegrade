using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using ReLogic.Content;
using System;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;
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
            c.GotoNext(i => i.MatchRet()); // goto return
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
            GreatlySidegradedWorld.WorldEvil.Fractured => 0x0u,
            GreatlySidegradedWorld.WorldEvil.Nothing => 0xFFFFFFu,
            GreatlySidegradedWorld.WorldEvil.Rotten => 0xFF7FF0u,
            GreatlySidegradedWorld.WorldEvil.Spiral => 0x4965FFu,
            GreatlySidegradedWorld.WorldEvil.Starved => 0xFFBD8Cu,
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
        } catch (Exception e) {
            MonoModHooks.DumpIL(TheGreatSidegrade.Mod, il);

            //throw new ILPatchFailureException(TheGreatSidegrade.Mod, il, e);
        }
    }

    public static Asset<Texture2D> OnGetIcon(On_AWorldListItem.orig_GetIcon orig, AWorldListItem self) {
        WorldFileData data = self.Data;
        var path = Path.ChangeExtension(data.Path, ".twld");

        if (!FileUtilities.Exists(path, data.IsCloudSave))
            return orig(self);

        //Stream stream = (data.IsCloudSave ? ((Stream)new MemoryStream(Terraria.Social.SocialAPI.Cloud.Read(path))) : ((Stream)new FileStream(path, FileMode.Open)));
        byte[] buf = FileUtilities.ReadAllBytes(path, data.IsCloudSave);
        if (buf[0] != 31 || buf[1] != 139)
            return orig(self);

        var stream = new MemoryStream(buf);
        var tag = TagIO.FromStream(stream);

        GreatlySidegradedWorld.WorldEvil? worldEvil = null;

        if (tag.ContainsKey("modData")) {
            foreach (TagCompound modDataTag in tag.GetList<TagCompound>("modData").Skip(2)) {
                if (modDataTag.Get<string>("mod") == TheGreatSidegrade.Mod.Name) {
                    worldEvil = (GreatlySidegradedWorld.WorldEvil)modDataTag.Get<TagCompound>("data").GetByte($"{nameof(TheGreatSidegrade)}:WorldEvil");
                    break;
                }
            }
        }

        if (worldEvil != null && !GreatlySidegradedWorld.IsVanillaEvil(worldEvil.Value)) {
            string iconPath = "Assets/Textures/UI/Icon";
            iconPath += data.IsHardMode ? "Hallow" : "";
            iconPath += Enum.GetName(worldEvil.Value);
            return GameAssets.GetAsset(iconPath);
        } else {
            return orig(self);
        }
    }
}

//* Call to check if the world uses vanilla evil */
//* 0x0000016F 03           */ IL_016F: ldarg.1
//* 0x00000170 28????????   */ IL_0170: call      bool[TheGreatSidegrade] TheGreatSidegrade.Common.GreatlySidegradedWorld::IsVanillaEvil()
//* 0x00000175 2D08         */ IL_0175: brtrue.s IL_017F

//* Load null for GetGenProgressBar func */
//* 0x00000177 14           */ IL_0177: ldnull
//* Call the custom texture loading method */
//* 0x00000178 28????????   */ IL_0178: call      class [FNA] Microsoft.Xna.Framework.Graphics.Texture2D[TheGreatSidegrade]TheGreatSidegrade.Common.Hooks.WorldList::GetGenProgressBarTexture(class [FNA] Microsoft.Xna.Framework.Graphics.Texture2D)
//* Jump to the end of the branch
//* 0x0000017D 2B1B         */ IL_017D: br.s IL_019A
//
//* 0x0000017F 06           */ IL_017F: ldloc.0
//* 0x00000180 2D0D         */ IL_0180: brtrue.s IL_018F
//
//* 0x00000182 02           */ IL_0182: ldarg.0
//* 0x00000183 7BC9560004   */ IL_0183: ldfld     class [ReLogic] ReLogic.Content.Asset`1 <class [FNA] Microsoft.Xna.Framework.Graphics.Texture2D > Terraria.GameContent.UI.Elements.UIGenProgressBar::_texOuterCorrupt
//* 0x00000188 6F????????   */ IL_0188: callvirt instance !0 class [ReLogic] ReLogic.Content.Asset`1 <class [FNA] Microsoft.Xna.Framework.Graphics.Texture2D >::get_Value()
//* 0x0000018D 2B0B         */ IL_018D: br.s IL_019A
