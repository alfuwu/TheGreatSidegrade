using Microsoft.Xna.Framework;
using System.IO;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using TheGreatSidegrade.Content.NPCs;
using TheGreatSidegrade.Content.BossBars;
using System;
using TheGreatSidegrade.Content.WorldGeneration;
using System.Collections.Generic;
using Terraria.Graphics.Shaders;
using Microsoft.Xna.Framework.Graphics;
using TheGreatSidegrade.Assets;

namespace TheGreatSidegrade.Content.Bosses;

[AutoloadBossHead]
public class ChronovoreHead : WormHead {
    public class DoubleTap(Player target, Vector2 struckPos) {
        public Player target = target;
        public Vector2 struckPos = struckPos;
        public int time = 30; // half a second
    }

    private static int secondStageHeadSlot = -1;

    public bool SecondStage {
        get => NPC.ai[2] == 1;
        set => NPC.ai[2] = value ? 1 : 0;
    }

    public int Age {
        get => (int) NPC.ai[3];
        set => NPC.ai[3] = value;
    }

    //public bool CanTimeReverse {
    //    get => NPC.ai[3] <= 0;
    //}

    private readonly List<DoubleTap> doubleTapPos = [];
    private readonly List<DoubleTap> newTaps = [];

    public override int BodyType => ModContent.NPCType<ChronovoreBody>();
    public override int TailType => ModContent.NPCType<ChronovoreTail>();

    public bool IsTimeCoreOperational => FollowerNPC.ModNPC is ChronovoreBody chronovore && chronovore.BodySegmentType == ChronovoreBody.BodyType.TimeCore && !chronovore.Destroyed;

    public override string Texture => $"{nameof(TheGreatSidegrade)}/Content/Bosses/Chronovore_Head";
    public override string BossHeadTexture => $"{nameof(TheGreatSidegrade)}/Content/Bosses/Chronovore_BossHead";
    public override int MaxDistanceForUsingTileCollision => 10000;

    public static bool IsWithinAngle(Vector2 origin, Vector2 target, float maxAngleDegrees = 40f) {
        // Calculate the angle in radians between the origin and target
        float angleRadians = origin.AngleTo(target);

        // Convert the angle to degrees
        float angleDegrees = MathHelper.ToDegrees(angleRadians);

        // Normalize the angle to the range -180 to 180 degrees
        if (angleDegrees > 180)
            angleDegrees -= 360;
        else if (angleDegrees < -180)
            angleDegrees += 360;

        // Check if the angle is within the max angle range
        return Math.Abs(angleDegrees) <= maxAngleDegrees;
    }

    public override void Load() {
        // We want to give it a second boss head icon, so we register one
        string texture = BossHeadTexture + "_SecondStage"; // Our texture is called "ClassName_Head_Boss_SecondStage"
        secondStageHeadSlot = Mod.AddBossHeadTexture(texture, -1); // -1 because we already have one registered via the [AutoloadBossHead] attribute, it would overwrite it otherwise
    }

    public override void BossHeadSlot(ref int index) {
        int slot = secondStageHeadSlot;
        if (SecondStage && slot != -1) // If the boss is in its second stage, display the other head icon instead
            index = slot;
    }

    public override void SetStaticDefaults() {
        var drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers() { // Influences how the NPC looks in the Bestiary
            CustomTexturePath = $"{nameof(TheGreatSidegrade)}/Content/Bosses/Chronovore_Bestiary", // If the NPC is multiple parts like a worm, a custom texture for the Bestiary is encouraged.
            Position = new(40f, 24f),
            PortraitPositionXOverride = 0f,
            PortraitPositionYOverride = 12f
        };
        NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifier);
    }

    public override void SetDefaults() {
        // Head is 10 defense, body 20, tail 30.
        NPC.CloneDefaults(NPCID.DiggerHead);
        /* Copied from DiggerHead
			width = 22;
			height = 22;
			netAlways = true;
			damage = 45;
			defense = 10;
			lifeMax = 200;
			HitSound = SoundID.NPCHit1;
			DeathSound = SoundID.NPCDeath1;
			noGravity = true;
			noTileCollide = true;
			knockBackResist = 0f;
			behindTiles = true;
			scale = 0.9f;
			value = 300f;
        */
        NPC.aiStyle = -1;
        NPC.npcSlots = 5f;
        NPC.width = 46; // boss size is kinda funky
        NPC.height = 38;
        NPC.damage = 22;
        NPC.defense = 4;
        NPC.lifeMax = 30000;
        NPC.value = 800f;
        NPC.scale = 1.2f;
        NPC.SpawnWithHigherTime(30);
        NPC.boss = true;
        NPC.BossBar = ModContent.GetInstance<ChronovoreBossBar>(); // i dont think this is even required
        
        //if (!Main.dedServ)
        //    Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/Ropocalypse2");
    }

    public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
        // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
        bestiaryEntry.Info.AddRange([
				SpawnConditions.TheSpiral,

				new FlavorTextBestiaryInfoElement($"{TheGreatSidegrade.Localization}.Bestiary.Chronovore")
            ]);
    }

    public override void Init() {
        // Set the segment variance
        // If you want the segment length to be constant, set these two properties to the same value
        MinSegmentLength = 60;
        MaxSegmentLength = 60;

        CommonWormInit(this);
    }

    // This method is invoked from ChronovoreHead, ChronovoreBody and ChronovoreTail
    internal static void CommonWormInit(Worm worm) {
        // These two properties handle the movement of the worm
        worm.MoveSpeed = 10f;
        worm.Acceleration = 0.18f;
    }

    public int attackCounter;
    public override void SendExtraAI(BinaryWriter writer) {
        writer.Write(attackCounter);
        foreach (DoubleTap dt in newTaps) {
            writer.Write(dt.target.whoAmI);
            writer.WriteVector2(dt.struckPos);
        }
        newTaps.Clear();
    }

    public override void ReceiveExtraAI(BinaryReader reader) {
        attackCounter = reader.ReadInt32();
        try { // do i actually need to sync this
            while (true) {
                int plr = reader.ReadInt32();
                Vector2 pos = reader.ReadVector2();
                doubleTapPos.Add(new(Main.player[plr], pos));
            }
        } catch (EndOfStreamException) { }
    }

    public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers) {
        if (attackCounter > 95)
            modifiers.ModifyHurtInfo += (ref Player.HurtInfo hurtInfo) => {
                hurtInfo.Damage *= 2;
            };
    }

    public override void AI() {
        if (Main.netMode != NetmodeID.MultiplayerClient) {
            if (attackCounter > 0)
                attackCounter--; // tick down the attack counter.
            Age++;

            // get time reversed bitch
            // youre still getting time reversed but its more structured now bitch
            for (int i = 0; i < doubleTapPos.Count; i++) {
                DoubleTap dt = doubleTapPos[i];
                dt.time--;
                if (dt.time <= 0) {
                    dt.target.Teleport(dt.struckPos, 1); // needs shader effects and a ticking sound
                    doubleTapPos.RemoveAt(i);
                    i--;
                }
            }
        }
    }

    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
        Texture2D e = GameAssets.GetTexture($"Content/Bosses/Chronovore_Head{(SecondStage ? "_SecondStage" : "")}");
        Main.EntitySpriteDraw(e, NPC.Center - Main.screenPosition, null, NPC.GetAlpha(Lighting.GetColor((int)NPC.Center.X / 16, (int)NPC.Center.Y / 16)), NPC.rotation, e.Size() * 0.5f, NPC.scale, SpriteEffects.None, 0);
        return false;
    }

    public void AddDoubleTap(Player target, Vector2 pos) {
        DoubleTap dt = new(target, pos);
        doubleTapPos.Add(dt);
        newTaps.Add(dt);
    }

    public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo) {
        //if (IsTimeCoreOperational)
            AddDoubleTap(target, target.position);
    }
}

public class ChronovoreBody : WormBody {
    public enum BodyType {
        Body1,
        Body2,
        Body3,
        TimeCore
    }

    public BodyType BodySegmentType { get; set; }

    public bool Destroyed {
        get => NPC.ai[3] > (BodySegmentType == BodyType.TimeCore ? 1000 : BodySegmentType == BodyType.Body2 ? 300 : 150) * (Main.masterMode ? 2f : Main.expertMode ? 1.4f : 1f);
    }

    public override string Texture => $"{nameof(TheGreatSidegrade)}/Content/Bosses/Chronovore_Body1";
    public override string Name => $"Chronovore{Enum.GetName(BodySegmentType)}";

    public override void SetStaticDefaults() {
        Main.npcFrameCount[NPC.type] = 8;
        NPCID.Sets.NPCBestiaryDrawModifiers value = new() {
            Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
        };
        NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
    }

    public override void SetDefaults() {
        NPC.CloneDefaults(NPCID.DiggerBody);
        /* Copied from DiggerBody
			width = 22;
			height = 22;
			aiStyle = 6;
			netAlways = true;
			damage = 28;
			defense = 20;
			lifeMax = 200;
			HitSound = SoundID.NPCHit1;
			DeathSound = SoundID.NPCDeath1;
			noGravity = true;
			noTileCollide = true;
			knockBackResist = 0f;
			behindTiles = true;
			scale = 0.9f;
			value = 300f;
			dontCountMe = true;
        */
        NPC.aiStyle = -1;
        NPC.width = 46;
        NPC.height = 38;
        NPC.damage = 13;
        NPC.defense = 7;
        NPC.lifeMax = 30000;
        NPC.value = 800f;
        NPC.scale = 1.2f;
    }

    public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers) {
        if (HeadSegment.ModNPC is ChronovoreHead chronovore && chronovore.attackCounter > 495)
            modifiers.ModifyHurtInfo += (ref Player.HurtInfo hurtInfo) => {
                hurtInfo.Damage *= 2;
            };
    }

    public override void Init() {
        ChronovoreHead.CommonWormInit(this);

        if (FollowingNPC.ModNPC is ChronovoreHead)
            BodySegmentType = BodyType.TimeCore;
        else
            BodySegmentType = (BodyType)Main.rand.Next(3);
        NPC.netUpdate = true;
        NPC.netUpdate2 = true; // idfk
        // i mean it works
    }

    public override void SendExtraAI(BinaryWriter writer) {
        writer.Write((byte) BodySegmentType);
    }

    public override void ReceiveExtraAI(BinaryReader reader) {
        BodySegmentType = (BodyType) reader.ReadByte();
    }

    public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone) {
        NPC.ai[3] += damageDone;
    }

    public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone) {
        NPC.ai[3] += damageDone;
    }

    public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo) {
        if (HeadSegment.ModNPC is ChronovoreHead chronovore )//&& chronovore.IsTimeCoreOperational)
            chronovore.AddDoubleTap(target, target.position);
    }

    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
        Texture2D e = GameAssets.GetTexture($"Content/Bosses/Chronovore_{BodySegmentType}{(Destroyed ? "Destroyed" : "")}");
        Main.EntitySpriteDraw(e, NPC.Center - Main.screenPosition, null, NPC.GetAlpha(Lighting.GetColor((int)NPC.Center.X / 16, (int)NPC.Center.Y / 16)), NPC.rotation, e.Size() * 0.5f, NPC.scale, SpriteEffects.None, 0);
        return false;
    }
}

public class ChronovoreTail : WormTail {
    public bool Destroyed {
        get => NPC.ai[2] == 1;
        set => NPC.ai[2] = value ? 1 : 0;
    }

    public override string Texture => $"{nameof(TheGreatSidegrade)}/Content/Bosses/Chronovore_Tail";

    public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers) {
        if (HeadSegment.ModNPC is ChronovoreHead chronovore && chronovore.attackCounter > 495)
            modifiers.ModifyHurtInfo += (ref Player.HurtInfo hurtInfo) => {
                hurtInfo.Damage *= 2;
            };
    }

    public override void SetStaticDefaults() {
        NPCID.Sets.NPCBestiaryDrawModifiers value = new() {
            Hide = true
        };
        NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
    }

    public override void SetDefaults() {
        NPC.CloneDefaults(NPCID.DiggerTail);
        /* Copied from DiggerTail
			width = 22;
			height = 22;
			aiStyle = 6;
			netAlways = true;
			damage = 26;
			defense = 30;
			lifeMax = 200;
			HitSound = SoundID.NPCHit1;
			DeathSound = SoundID.NPCDeath1;
			noGravity = true;
			noTileCollide = true;
			knockBackResist = 0f;
			behindTiles = true;
			scale = 0.9f;
			value = 300f;
			dontCountMe = true;
        */
        NPC.aiStyle = -1;
        NPC.width = 46;
        NPC.height = 38;
        NPC.damage = 11;
        NPC.defense = 13;
        NPC.lifeMax = 15000;
        NPC.value = 800f;
        NPC.scale = 1.2f;
    }

    public override void Init() {
        ChronovoreHead.CommonWormInit(this);
    }

    public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo) {
        if (HeadSegment.ModNPC is ChronovoreHead chronovore)// && chronovore.IsTimeCoreOperational)
            chronovore.AddDoubleTap(target, target.position);
    }

    public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone) {
        NPC.ai[3] += damageDone;
    }

    public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone) {
        NPC.ai[3] += damageDone;
    }

    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
        Texture2D e = GameAssets.GetTexture($"Content/Bosses/Chronovore_Tail{(Destroyed ? "Destroyed" : "")}");
        Main.EntitySpriteDraw(e, NPC.Center - Main.screenPosition, null, NPC.GetAlpha(Lighting.GetColor((int)NPC.Center.X / 16, (int)NPC.Center.Y / 16)), NPC.rotation, e.Size() * 0.5f, NPC.scale, SpriteEffects.None, 0);
        return false;
    }
}