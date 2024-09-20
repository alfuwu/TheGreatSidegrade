using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;
using TheGreatSidegrade.Content.WorldGeneration.Passes;

namespace TheGreatSidegrade.Common;

public class GreatlySidegradedWorld : ModSystem {
    public static Color FracturedBiomeColor = new(111, 110, 110);
    public static Color NothingBiomeColor = new(10, 10, 10);
    public static Color RottenBiomeColor = new(142, 76, 59);
    public static Color SpiralBiomeColor = new(56, 76, 59);
    public static Color StarvedBiomeColor = new(199, 173, 90);

    public static WorldEvil worldEvil;

    public enum WorldEvil {
        Corruption,
        Crimson,
        Fractured,
        Nothing,
        Rotten,
        Spiral,
        Starved
    }

    public override void PreWorldGen() {
        if (WorldGen.WorldGenParam_Evil == 0 || TheGreatSidegrade.IsContagion) {
            worldEvil = WorldEvil.Corruption;
        } else if (WorldGen.WorldGenParam_Evil == 1) {
            worldEvil = WorldEvil.Crimson;
        } else if (WorldGen.WorldGenParam_Evil == -1) {
            Mod.Logger.Info("RANDOM WORLD EVIL");
            if (WorldGen.genRand.NextBool(Enum.GetValues<WorldEvil>().Length - 2, Enum.GetValues<WorldEvil>().Length)) {
                worldEvil = PickRandomEvil();
            } else if (WorldGen.genRand.NextBool(2)) {
                worldEvil = WorldEvil.Crimson;
            } else {
                worldEvil = WorldEvil.Corruption;
            }
            Mod.Logger.Info("CRIMSN: " + WorldGen.crimson);
            Mod.Logger.Info("WORLDE EVL: " + Enum.GetName(worldEvil));
        }
        if (WorldGen.WorldGenParam_Evil > 2) { // 2 is taken by Exxo Avalon Origins for the Contagion
            worldEvil = (WorldEvil)WorldGen.WorldGenParam_Evil + 1;
        }
        Mod.Logger.Info(TheGreatSidegrade.IsContagion);
        WorldGen.crimson = worldEvil == WorldEvil.Crimson;
    }

    public static WorldEvil PickRandomEvil() {
        return (WorldEvil)Main.rand.Next(Enum.GetValues<WorldEvil>().Length - 2) + 2;
    }

    public override void ModifyWorldGenTasks(List<GenPass> list, ref double totalWeight) {
        int index = list.FindIndex(genpass => genpass.Name.Equals("Corruption"));
        if (!IsVanillaEvil() && index > -1) {
            list.RemoveAt(index);
            list.Insert(index, new PassLegacy(/*$"{nameof(TheGreatSidegrade)}: {Enum.GetName(worldEvil)}"*/ "Corruption", GetWorldGenPass()));
        }
    }

    public override void ModifyHardmodeTasks(List<GenPass> list) {
        //int index2 = list.FindIndex(genpass => genpass.Name.Equals("Hardmode Good"));
        int index = list.FindIndex(genpass => genpass.Name.Equals("Hardmode Evil"));
        if (!IsVanillaEvil() && index > -1) {
            list.RemoveAt(index);
            list.Insert(index, new PassLegacy(/*$"{nameof(TheGreatSidegrade)}: Hardmode {Enum.GetName(worldEvil)}"*/ "Hardmode Evil", GetHardModeWorldGenPass()));
        }
    }

    public static WorldGenLegacyMethod GetWorldGenPass() {
        return worldEvil switch {
            WorldEvil.Fractured => new WorldGenLegacyMethod(Starved.Method),
            WorldEvil.Nothing => new WorldGenLegacyMethod(Starved.Method),
            WorldEvil.Rotten => new WorldGenLegacyMethod(Starved.Method),
            WorldEvil.Spiral => new WorldGenLegacyMethod(Starved.Method),
            WorldEvil.Starved => new WorldGenLegacyMethod(Starved.Method),
            _ => null
        };
    }

    public static WorldGenLegacyMethod GetHardModeWorldGenPass() {
        return worldEvil switch {
            WorldEvil.Fractured => new WorldGenLegacyMethod(StarvedHardMode.Method),
            WorldEvil.Nothing => new WorldGenLegacyMethod(StarvedHardMode.Method),
            WorldEvil.Rotten => new WorldGenLegacyMethod(StarvedHardMode.Method),
            WorldEvil.Spiral => new WorldGenLegacyMethod(StarvedHardMode.Method),
            WorldEvil.Starved => new WorldGenLegacyMethod(StarvedHardMode.Method),
            _ => null
        };
    }

    public override void NetSend(BinaryWriter bw) {
        bw.Write((byte)worldEvil);
    }

    public override void NetReceive(BinaryReader br) {
        worldEvil = (WorldEvil)br.ReadByte();
    }

    public override void SaveWorldData(TagCompound tag) {
        tag["WorldEvil"] = (byte)worldEvil;
    }

    public override void LoadWorldData(TagCompound tag) {
        if (tag.ContainsKey("WorldEvil"))
            worldEvil = (WorldEvil)tag.GetByte("WorldEvil");
    }

    public static bool IsVanillaEvil() => IsVanillaEvil(worldEvil);

    public static bool IsVanillaEvil(WorldEvil evil) => evil == WorldEvil.Corruption || worldEvil == WorldEvil.Crimson;
}
