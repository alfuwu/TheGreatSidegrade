using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ModLoader;
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
            }  else if (WorldGen.WorldGenParam_Evil == 1) {
                WorldGen.crimson = true;
            } else if (WorldGen.WorldGenParam_Evil == -1) {
                if (WorldGen.genRand.NextBool(Enum.GetValues<WorldEvil>().Length - 2, Enum.GetValues<WorldEvil>().Length)) {
                    WorldGen.crimson = false;
                    worldEvil = PickRandomEvil();
                }
            }
            if (WorldGen.WorldGenParam_Evil > 2) {
                WorldGen.crimson = false;
                worldEvil = (WorldEvil) WorldGen.WorldGenParam_Evil + 1;
            }
            if (WorldGen.WorldGenParam_Evil == 3)
                worldEvil = WorldEvil.Fractured;
        }

        public static WorldEvil PickRandomEvil() {
            return (WorldEvil) Main.rand.Next(Enum.GetValues<WorldEvil>().Length - 2) + 2;
        }

        public override void ModifyHardmodeTasks(List<GenPass> list) {
            int index = list.FindIndex(genpass => genpass.Name.Equals("Hardmode Good"));
            int index2 = list.FindIndex(genpass => genpass.Name.Equals("Hardmode Evil"));
            if (worldEvil != WorldEvil.Corruption && worldEvil != WorldEvil.Crimson) {
                list.Insert(index + 1, new PassLegacy($"{nameof(TheGreatSidegrade)}: Hardmode {worldEvil}", GetWorldGenPass()));
                // TODO REPLACE EVIL GEN INSTEAD OF REMOVING
                list.RemoveAt(index);
                list.RemoveAt(index2);
            }
        }

        public static WorldGenLegacyMethod GetWorldGenPass() {
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
    }
}
