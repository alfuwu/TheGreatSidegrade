using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent.UI.BigProgressBar;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using TheGreatSidegrade.Content.Bosses;
using System;
using TheGreatSidegrade.Content.NPCs;

namespace TheGreatSidegrade.Content.BossBars;

public class ChronovoreBossBar : ModBossBar {
    private int bossHeadIndex = -1;

    public override Asset<Texture2D> GetIconTexture(ref Rectangle? iconFrame) {
        // Display the previously assigned head index
        if (bossHeadIndex != -1)
            return TextureAssets.NpcHeadBoss[bossHeadIndex];
        return null;
    }

    public override bool? ModifyInfo(ref BigProgressBarInfo info, ref float life, ref float lifeMax, ref float shield, ref float shieldMax) {
        // Here the game wants to know if to draw the boss bar or not. Return false whenever the conditions don't apply.
        // If there is no possibility of returning false (or null) the bar will get drawn at times when it shouldn't, so write defensive code!

        NPC npc = Main.npc[info.npcIndexToAimAt];
        if (!npc.active)
            return false;

        // We assign bossHeadIndex here because we need to use it in GetIconTexture
        bossHeadIndex = npc.GetBossHeadTextureIndex();

        life = npc.life;
        lifeMax = npc.lifeMax;
        shield = 0;
        shieldMax = 0;

        NPC follower = npc.ModNPC is Worm worm ? worm.FollowerNPC : null;
        while (follower != null) {
            if (follower.ModNPC is ChronovoreBody body && !body.Destroyed && body.BodySegmentType == ChronovoreBody.BodyType.Body2) {
                shield += Math.Max(0, Math.Min(follower.ai[3], 150) - 150);
                shieldMax += 150;
            }
            follower = follower.ModNPC is ChronovoreBody e ? e.FollowerNPC : null;
        } // notworking
        // i dont think the BodySegmentType is syncing across the network properly

        return true;
    }
}