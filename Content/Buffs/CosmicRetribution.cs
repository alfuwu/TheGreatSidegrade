using Terraria;
using Terraria.ModLoader;

namespace TheGreatSidegrade.Content.Buffs;

public class CosmicRetribution : ModBuff {
    public override void SetStaticDefaults() {
        Main.debuff[Type] = true;
    }
}
