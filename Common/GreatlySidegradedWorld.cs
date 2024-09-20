using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;
using TheGreatSidegrade.Content.WorldGeneration.Passes;

namespace TheGreatSidegrade.Common {
    public class GreatlySidegradedWorld : ModSystem {
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
            WorldGen.crimson = WorldGen.genRand.NextBool(2);
            if (WorldGen.WorldGenParam_Evil == 0) {
                WorldGen.crimson = false;
                worldEvil = WorldEvil.Corruption;
            } else if (WorldGen.WorldGenParam_Evil == 1) {
                WorldGen.crimson = true;
                worldEvil = WorldEvil.Crimson;
            } else if (WorldGen.WorldGenParam_Evil == -1) {
                Mod.Logger.Info("RANDOM WORLD EVIL");
                if (WorldGen.genRand.NextBool(Enum.GetValues<WorldEvil>().Length - 2, Enum.GetValues<WorldEvil>().Length)) {
                    WorldGen.crimson = false;
                    worldEvil = PickRandomEvil();
                } else if (WorldGen.crimson) {
                    worldEvil = WorldEvil.Crimson;
                } else {
                    worldEvil = WorldEvil.Corruption;
                }
                Mod.Logger.Info("CRIMSN: " + WorldGen.crimson);
                Mod.Logger.Info("WORLDE EVL: " + Enum.GetName(worldEvil));
            }
            if (WorldGen.WorldGenParam_Evil > 2) { // 2 is taken by Exxo Avalon Origins for the Contagion
                WorldGen.crimson = false;
                worldEvil = (WorldEvil) WorldGen.WorldGenParam_Evil + 1;
            }
        }

        public static WorldEvil PickRandomEvil() {
            return (WorldEvil) Main.rand.Next(Enum.GetValues<WorldEvil>().Length - 2) + 2;
        }

        public override void ModifyWorldGenTasks(List<GenPass> list, ref double totalWeight) {
            int index = list.FindIndex(genpass => genpass.Name.Equals("Corruption"));
            if (!IsVanillaEvil()) {
                list.RemoveAt(index);
                list.Insert(index, new PassLegacy(/*$"{nameof(TheGreatSidegrade)}: {Enum.GetName(worldEvil)}"*/ "Corruption", GetWorldGenPass()));
            }
        }

        public override void ModifyHardmodeTasks(List<GenPass> list) {
            int index = list.FindIndex(genpass => genpass.Name.Equals("Hardmode Good"));
            int index2 = list.FindIndex(genpass => genpass.Name.Equals("Hardmode Evil"));
            if (!IsVanillaEvil()) {
                list.RemoveAt(index2);
                list.Insert(index2, new PassLegacy(/*$"{nameof(TheGreatSidegrade)}: Hardmode {Enum.GetName(worldEvil)}"*/ "Hardmode Evil", GetHardModeWorldGenPass()));
                //list.RemoveAt(index);
                //list.RemoveAt(index2);
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
            BitsByte evilType = new();
            evilType[0] = true;
            switch (worldEvil) {
                case WorldEvil.Fractured:
                    evilType[0] = true;
                    break;
                case WorldEvil.Nothing:
                    evilType[1] = true;
                    break;
                case WorldEvil.Rotten:
                    evilType[2] = true;
                    break;
                case WorldEvil.Spiral:
                    evilType[3] = true;
                    break;
                case WorldEvil.Starved:
                    evilType[4] = true;
                    break;
            }
            bw.Write(evilType);
        }

        public override void NetReceive(BinaryReader br) {
            BitsByte evilType = br.ReadByte();
            Mod.Logger.Info("GOT DATA");
            switch (evilType) {
                case 1:
                    Mod.Logger.Info("CAN READ EVILTYPE");
                    break;
            }
        }

        public override void SaveWorldData(TagCompound tag) {
            tag["WorldEvil"] = (byte) worldEvil;
        }

        public override void LoadWorldData(TagCompound tag) {
            if (tag.ContainsKey("WorldEvil"))
                worldEvil = (WorldEvil) tag.GetByte("WorldEvil");
        }

        // check for contagion?
        public static bool IsVanillaEvil() => IsVanillaEvil(worldEvil, TheGreatSidegrade.ExxoAvalonOrigins == null);

        public static bool IsVanillaEvil(WorldEvil evil, bool contagion) => !contagion && (evil == WorldEvil.Corruption || worldEvil == WorldEvil.Crimson);
    }
}
