using Terraria;
using Terraria.ModLoader;
using TheGreatSidegrade.Content.Buffs;

namespace TheGreatSidegrade.Common;

public class GreatlySidegradedNPC : GlobalNPC {
    public override void PostAI(NPC npc) {
        if (npc.HasBuff<SpiralingDebuff>() && Main.rand.NextBool(2))
            npc.velocity.X = -npc.velocity.X;
    }
}
