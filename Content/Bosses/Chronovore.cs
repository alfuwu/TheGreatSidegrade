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
using System.Linq;
using System.Collections.Generic;

namespace TheGreatSidegrade.Content.Bosses;

[AutoloadBossHead]
public class ChronovoreHead : WormHead {

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

    protected Vector2[] storedTargetPositions = CreateDefaultedArray(new Vector2(-1), 10);

    public override int BodyType => ModContent.NPCType<ChronovoreBody>();

    public override int TailType => ModContent.NPCType<ChronovoreTail>();

    public override string Texture => $"{nameof(TheGreatSidegrade)}/Content/Bosses/Chronovore_Head";
    public override string BossHeadTexture => $"{nameof(TheGreatSidegrade)}/Content/Bosses/Chronovore_BossHead";
    public override int MaxDistanceForUsingTileCollision => 10000;

    public static T[] CreateDefaultedArray<T>(T defaultValue, int length) where T: unmanaged {
        T[] a = new T[length];
        for (int _ = 0; _ < length; _++)
            a[_] = defaultValue;
        return a;
    }

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
        NPC.width = 38; // boss size is kinda funky
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

				new FlavorTextBestiaryInfoElement("The Chronovore is an ancient machine cast into ruin by the very power it controls. Now it lay, desolate, in the heart of the Spiral. Beware, for though it has lost much of its might, it is still a cataclysmic foe.")
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
        worm.MoveSpeed = 17f;
        worm.Acceleration = 0.08f;
    }

    public int attackCounter;
    public override void SendExtraAI(BinaryWriter writer) {
        writer.Write(attackCounter);
        foreach (Vector2 v in storedTargetPositions)
            writer.WriteVector2(v);
    }

    public override void ReceiveExtraAI(BinaryReader reader) {
        attackCounter = reader.ReadInt32();
        for (int i = 0; i < storedTargetPositions.Length; i++)
            storedTargetPositions[i] = reader.ReadVector2();
    }

    internal override void OnTargetChange(int newTarget) {
        for (int i = 0; i < storedTargetPositions.Length; i++)
            storedTargetPositions[i] = new(-1);
    }

    public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers) {
        if (attackCounter > 495)
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
            Player target = Main.player[NPC.target];
            if (attackCounter <= 0 && storedTargetPositions.Where(v => v.X != -1 || v.Y != -1).Any()) {
                Vector2? closest = null;
                foreach (Vector2 p in storedTargetPositions)
                    if (closest == null || p.Distance(NPC.position) < closest.Value.Distance(NPC.position))
                        closest = p;
                float length = target.position.Distance(NPC.position) - closest.Value.Distance(NPC.position);
                if (length < 200 || !IsWithinAngle(NPC.position, closest.Value, 4f) || length > 1000)
                    return; // dont waste a teleport if the distance gained is tiny, the chronovore is actively moving away from the position, or the chronovore is really far away from the position
                target.Teleport(closest.Value, 1);
                attackCounter = 500;
            }
            if (Age % 240 == 0) { // update positions every four seconds
                List<Vector2> closestPositions = [target.position];
                TheGreatSidegrade.Mod.Logger.Info(closestPositions);
                foreach (Vector2 p in storedTargetPositions) {
                    if (closestPositions.Count < storedTargetPositions.Length || closestPositions.Where(v => v.Distance(NPC.position) > p.Distance(NPC.position)).Any()) {
                        closestPositions.Add(p);
                        int furthest = -1;
                        if (closestPositions.Count >= storedTargetPositions.Length)
                            for (int i = 0; i < closestPositions.Count; i++)
                                if (furthest == -1 || closestPositions[i].Distance(NPC.position) > closestPositions[furthest].Distance(NPC.position))
                                    furthest = i;
                        if (furthest != -1)
                            closestPositions.RemoveAt(furthest);
                    }
                }
                storedTargetPositions = [.. closestPositions];
            }
        }
    }
}

public class ChronovoreBody : WormBody {
    public enum BodyType {
        Body1,
        Body2,
        Body3,
        TimeCore
    }

    public BodyType SegmentBodyType {
        get => (BodyType) NPC.ai[2];
        set => NPC.ai[2] = (float) value;
    }

    public bool Destroyed {
        get => NPC.ai[3] > 150;
    }

    public override string Texture => $"{nameof(TheGreatSidegrade)}/Content/Bosses/Chronovore_{Enum.GetName(SegmentBodyType)}{(Destroyed ? "Destroyed" : "")}";

    public override void SetStaticDefaults() {
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
        NPC.width = 38;
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
            SegmentBodyType = BodyType.TimeCore;
        else
            SegmentBodyType = (BodyType) Main.rand.Next(3);
    }

    public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone) {
        NPC.ai[3] += damageDone;
    }

    public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone) {
        NPC.ai[3] += damageDone;
    }
}

public class ChronovoreTail : WormTail {
    public bool Destroyed {
        get => NPC.ai[2] == 1;
        set => NPC.ai[2] = value ? 1 : 0;
    }

    public override string Texture => $"{nameof(TheGreatSidegrade)}/Content/Bosses/Chronovore_Tail{(Destroyed ? "Destroyed" : "")}";

    public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers) {
        if (HeadSegment.ModNPC is ChronovoreHead chronovore && chronovore.attackCounter > 495)
            modifiers.ModifyHurtInfo += (ref Player.HurtInfo hurtInfo) => {
                hurtInfo.Damage *= 2;
            };
    }

    public override void SetStaticDefaults() {
        NPCID.Sets.NPCBestiaryDrawModifiers value = new() {
            Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
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
        NPC.width = 38;
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
}