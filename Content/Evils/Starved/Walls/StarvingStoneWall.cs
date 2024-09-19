using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheGreatSidegrade.Content.Evils.Starved.Walls {
    public class StarvingStoneWall : ModWall {
        public override void SetStaticDefaults() {
            WallID.Sets.Conversion.Stone[Type] = true;
            AddMapEntry(new Color(34, 44, 25));
            //DustType = ModContent.DustType<StarvedDust>();
        }
    }
}
