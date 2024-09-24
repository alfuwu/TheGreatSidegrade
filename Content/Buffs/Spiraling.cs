using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheGreatSidegrade.Content.Buffs;

public class Spiraling : ModBuff { // the Spiral main debuff
    public override void SetStaticDefaults() {
        Main.debuff[Type] = true;
        BuffID.Sets.LongerExpertDebuff[Type] = true;
    }

    public override void Update(NPC npc, ref int buffIndex) {
        if (Main.rand.NextBool(2))
            npc.velocity.X = -npc.velocity.X;
    }
}
