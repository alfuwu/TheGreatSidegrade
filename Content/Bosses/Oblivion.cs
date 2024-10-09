using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using TheGreatSidegrade.Assets;
using TheGreatSidegrade.Content.BossBars;
using TheGreatSidegrade.Content.WorldGeneration;

namespace TheGreatSidegrade.Content.Bosses;

[AutoloadBossHead]
public class Oblivion : ModNPC {
    public override string Texture => $"{base.Texture}_Bestiary";
    public override string BossHeadTexture => $"{nameof(TheGreatSidegrade)}/Content/Bosses/Oblivion_BossHead";

    public override void SetStaticDefaults() {
        NPCID.Sets.MPAllowedEnemies[Type] = true;
        NPCID.Sets.BossBestiaryPriority.Add(Type);
        NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
        NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
        NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Burning] = true;
        NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Invisibility] = true;
        NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Ironskin] = true;
        NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Midas] = true;
        NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.CursedInferno] = true;
        NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Ichor] = true;
        NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Slow] = true;
        NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Chilled] = true;

        NPCID.Sets.NPCBestiaryDrawModifiers drawModifier = new() {
            CustomTexturePath = Texture,
            Position = new(40f, 24f),
            PortraitPositionXOverride = 0f,
            PortraitPositionYOverride = 12f
        };
        NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifier);
    }

    public override void SetDefaults() { // how tf are you gonna kill oblivion
        // its a pre-hardmode boss :sob:
        NPC.aiStyle = -1;
        NPC.npcSlots = 10f;
        NPC.width = 100;
        NPC.height = 100;
        NPC.damage = 150;
        NPC.HitSound = SoundID.NPCHit1; // change
        NPC.DeathSound = SoundID.NPCDeath1; // change
        NPC.knockBackResist = 0f;
        NPC.noGravity = true;
        NPC.noTileCollide = true;
        NPC.defense = 9999;
        NPC.lifeMax = (int.MaxValue / 4) - 1;
        NPC.value = Item.buyPrice(gold: 15);
        NPC.scale = 2f;
        NPC.SpawnWithHigherTime(30);
        //NPC.boss = true;
        NPC.BossBar = ModContent.GetInstance<OblivionBossBar>(); // funky infinity/infinity boss bar, very threatening and mostly accurate

        if (!Main.dedServ)
            Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/Oblivion");
    }

    public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment) {
        NPC.lifeMax = (int.MaxValue / 4) - 1;
    }

    public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
        bestiaryEntry.Info.AddRange([
            SpawnConditions.TheNothing,
            new FlavorTextBestiaryInfoElement($"{TheGreatSidegrade.Localization}.Bestiary.Oblivion")
        ]);
    }

    public override void ModifyHoverBoundingBox(ref Rectangle boundingBox) {
        boundingBox = new();
    }

    public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position) {
        return false;
    }

    public override void AI() {
        // whiet fiyr :)
        Lighting.AddLight(NPC.Center, new Vector3(1.4f));
    }

    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
        // oblivion is rendered with shaders, not a regular sprite, so prevent normal drawing
        spriteBatch.End();
        spriteBatch.Begin(SpriteSortMode.Immediate, null, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

        Texture2D noiseTexture = GameAssets.GetTexture("Content/Bosses/Oblivion");
        Vector2 drawPosition = NPC.Center - Main.screenPosition;
        Vector2 origin = noiseTexture.Size() * 0.5f;
        //GameShaders.Misc["TheGreatSidegrade/Oblivion"].UseShaderSpecificData(new(0f, 0f, 0f, 0f));
        GameShaders.Misc["TheGreatSidegrade/Oblivion"].UseImage1(GameAssets.GetAsset("Assets/Textures/Misc/Vignette"));
        GameShaders.Misc["TheGreatSidegrade/Oblivion"].Apply();

        Main.EntitySpriteDraw(noiseTexture, drawPosition, null, Color.White, 0f, origin, 0.3f, SpriteEffects.None, 0);
        spriteBatch.End();
        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
        return false;
    }
}