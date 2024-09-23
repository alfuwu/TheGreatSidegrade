﻿using System;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ObjectData;
using TheGreatSidegrade.Content.Bosses;

namespace TheGreatSidegrade.Content.Tiles.Spiral;

public class TwistedEnergyCore : ModTile {
    public override void SetStaticDefaults() {
        Main.tileFrameImportant[Type] = true;
        Main.tileLighted[Type] = true;
        Main.tileHammer[Type] = true;
        //AnimationFrameHeight = 36;
        TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
        TileObjectData.newTile.Width = 2;
        TileObjectData.newTile.Height = 2;
        TileObjectData.newTile.CoordinateHeights = [16, 16];
        TileObjectData.newTile.LavaDeath = false;
        TileObjectData.addTile(Type);

        AddMapEntry(new Color(144, 160, 38), LanguageManager.Instance.GetText("TwistedEnergyCore"));
    }

    /*public override void AnimateTile(ref int frame, ref int frameCounter) {
        frameCounter++;
        if (frameCounter > 12) {
            frameCounter = 0;
            frame++;
            if (frame > 1)
                frame = 0;
        }
    }*/

    public override void KillMultiTile(int i, int j, int frameX, int frameY) {
        if (Main.netMode != NetmodeID.MultiplayerClient && !WorldGen.noTileActions) {
            if (NPC.downedBoss2)
                if (WorldGen.genRand.NextBool(2))
                    WorldGen.spawnMeteor = true;
            int num3 = WorldGen.shadowOrbSmashed ? Main.rand.Next(5) : 0;
            //if (num3 == 0) {
            //    Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ModContent.ItemType<Blunderblight>(), 1, false, -1, false);
            //    Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, 97, 100, false, 0, false);
            //} else if (num3 == 1)
            //    Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ModContent.ItemType<TetanusChakram>(), 1, false, -1, false);
            //else if (num3 == 2)
            //    Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ModContent.ItemType<NerveNumbNecklace>(), 1, false, -1, false);
            //else if (num3 == 3)
            //    Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ModContent.ItemType<Items.Spiral.TwistedEnergyCore>(), 1, false, -1, false);
            //else if (num3 == 4)
            //    Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ModContent.ItemType<Smogscreen>(), 1, false, -1, false);
            WorldGen.shadowOrbSmashed = true;
            WorldGen.shadowOrbCount++;
            if (WorldGen.shadowOrbCount >= 3) {
                WorldGen.shadowOrbCount = 0;
                float num5 = i * 16;
                float num6 = j * 16;
                float num7 = -1f;
                int plr = 0;
                for (int k = 0; k < 255; k++) {
                    float num8 = Math.Abs(Main.player[k].position.X - num5) + Math.Abs(Main.player[k].position.Y - num6);
                    if (num8 < num7 || num7 == -1f) {
                        plr = k;
                        num7 = num8;
                    }
                }
                if (!NPC.AnyNPCs(ModContent.NPCType<ChronovoreHead>()))
                    NPC.SpawnOnPlayer(plr, ModContent.NPCType<ChronovoreHead>());
            } else {
                string text = Lang.misc[WorldGen.shadowOrbCount == 2 ? 11 : 10].Value;
                if (Main.netMode == NetmodeID.SinglePlayer)
                    Main.NewText(text, 50, 255, 130);
                else if (Main.netMode == NetmodeID.Server)
                    ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(text), new(50, 255, 130));
            }
            SoundEngine.PlaySound(SoundID.Splash, new(i * 16, j * 16));
        }
    }

    public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b) {
        var brightness = Main.rand.Next(-5, 6) * 0.0025f;
        r = (144f / 255f + brightness) * 0.65f;
        g = (160f / 255f + brightness) * 0.65f;
        b = (38f / 255f + brightness) * 0.65f;
    }
}