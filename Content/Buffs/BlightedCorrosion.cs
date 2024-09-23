using Terraria;
using Terraria.ModLoader;

namespace TheGreatSidegrade.Content.Buffs;

public class BlightedCorrosion : ModBuff { // the Rotten main debuff
    public override void SetStaticDefaults() {
        Main.debuff[Type] = true;
    }
}
