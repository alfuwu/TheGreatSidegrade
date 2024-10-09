using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheGreatSidegrade.Content.Gores;

public class RotTreeLeaf : ModGore {
    public override void SetStaticDefaults() {
        ChildSafety.SafeGore[Type] = true;
        GoreID.Sets.SpecialAI[Type] = 3;
        GoreID.Sets.PaintedFallingLeaf[Type] = true;
    }
}