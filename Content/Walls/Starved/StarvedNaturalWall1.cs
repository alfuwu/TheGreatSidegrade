using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheGreatSidegrade.Content.Walls.Starved {
    public class StarvedNaturalWall1 : ModWall {
        public override void SetStaticDefaults() {
            WallID.Sets.Conversion.Sandstone[Type] = true;
            AddMapEntry(new Color(57, 55, 12));
        }
    }
}
