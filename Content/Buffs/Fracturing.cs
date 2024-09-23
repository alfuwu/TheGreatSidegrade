using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheGreatSidegrade.Content.Buffs;

public class Fracturing { // the Fractured main debuff
    public class FracturingStage1 : ModBuff {
        public override void SetStaticDefaults() {
            Main.debuff[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = true;
        }

        public override bool ReApply(NPC npc, int time, int buffIndex) {
            if (npc.HasBuff<FracturingStage1>())
                npc.buffType[buffIndex] = ModContent.BuffType<FracturingStage2>();
            else if (npc.HasBuff<FracturingStage2>())
                npc.buffType[buffIndex] = ModContent.BuffType<FracturingStage3>();
            else if (npc.HasBuff<FracturingStage3>() && !npc.buffImmune[Type])
                npc.SimpleStrikeNPC(99999, 0, damageType: DamageClass.Magic, luck: npc.GetSource_Buff(buffIndex) is EntitySource_OnHit onHit ? onHit.Attacker is Player plr ? plr.luck : 0 : 0);
            return false;
        }
    }

    public class FracturingStage2 : ModBuff {
        public override void SetStaticDefaults() {
            Main.debuff[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }
    }

    public class FracturingStage3 : ModBuff {
        public override void SetStaticDefaults() {
            Main.debuff[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }
    }
}
