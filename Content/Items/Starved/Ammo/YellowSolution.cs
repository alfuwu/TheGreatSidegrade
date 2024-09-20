using System;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using TheGreatSidegrade.Content.Tiles.Starved;
using TheGreatSidegrade.Content.Walls.Starved;

namespace TheGreatSidegrade.Content.Items.Starved.Ammo;

public class YellowSolutionItem : ModItem {
    public override string Texture => TheGreatSidegrade.AssetPath + "Textures/Items/YellowSolution";

    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 99;
    }

    public override void SetDefaults() {
        Item.DefaultToSolution(ModContent.ProjectileType<YellowSolutionProjectile>());
        Item.value = Item.buyPrice(0, 0, 25);
        Item.rare = ItemRarityID.Orange;
    }

    public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup) {
        itemGroup = ContentSamples.CreativeHelper.ItemGroup.Solutions;
    }
}

public class YellowSolutionProjectile : ModProjectile {
    public override string Texture => TheGreatSidegrade.AssetPath + "Textures/Projectiles/YellowSolution";

    public ref float Progress => ref Projectile.ai[0];

    public override void SetDefaults() {
        // This method quickly sets the projectile properties to match other sprays.
        Projectile.DefaultToSpray();
        Projectile.aiStyle = 0; // Here we set aiStyle back to 0 because we have custom AI code
    }

    public override void AI() {
        // Set the dust type to ExampleSolution
        int dustType = ModContent.DustType<Dusts.Starved.YellowSolution>();

        if (Projectile.owner == Main.myPlayer)
            Convert((int)(Projectile.position.X + Projectile.width * 0.5f) / 16, (int)(Projectile.position.Y + Projectile.height * 0.5f) / 16, 2);

        if (Projectile.timeLeft > 133)
            Projectile.timeLeft = 133;

        if (Progress > 7f) {
            float dustScale = 1f;

            if (Progress == 8f)
                dustScale = 0.2f;
            else if (Progress == 9f)
                dustScale = 0.4f;
            else if (Progress == 10f)
                dustScale = 0.6f;
            else if (Progress == 11f)
                dustScale = 0.8f;

            Progress += 1f;


            var dust = Dust.NewDustDirect(new(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, dustType, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100);

            dust.noGravity = true;
            dust.scale *= 1.75f;
            dust.velocity.X *= 2f;
            dust.velocity.Y *= 2f;
            dust.scale *= dustScale;
        } else {
            Progress += 1f;
        }

        Projectile.rotation += 0.3f * Projectile.direction;
    }

    private static void Convert(int i, int j, int size = 4) {
        for (int k = i - size; k <= i + size; k++) {
            for (int l = j - size; l <= j + size; l++) {
                if (WorldGen.InWorld(k, l, 1) && Math.Abs(k - i) + Math.Abs(l - j) < Math.Sqrt(size * size + size * size)) {
                    int type = Main.tile[k, l].TileType;
                    int wall = Main.tile[k, l].WallType;

                    if (wall == WallID.Stone) {
                        Main.tile[k, l].WallType = (ushort)ModContent.WallType<StarvingStoneWall>();
                        WorldGen.SquareWallFrame(k, l);
                        NetMessage.SendTileSquare(-1, k, l, 1);
                    }

                    // If the tile is stone, convert to Starving Stone
                    if (TileID.Sets.Conversion.Stone[type]) {
                        Main.tile[k, l].TileType = (ushort)ModContent.TileType<StarvingStone>();
                        WorldGen.SquareTileFrame(k, l);
                        NetMessage.SendTileSquare(-1, k, l, 1);
                    }
                    // If the tile is sand, convert to Starved Sand
                    else if (TileID.Sets.Conversion.Sand[type]) {
                        Main.tile[k, l].TileType = (ushort)ModContent.TileType<StarvedSand>();
                        WorldGen.SquareTileFrame(k, l);
                        NetMessage.SendTileSquare(-1, k, l, 1);
                    }
                    // If the tile is hardened sand, convert to Hardened Starved Sand
                    else if (TileID.Sets.Conversion.HardenedSand[type]) {
                        Main.tile[k, l].TileType = (ushort)ModContent.TileType<HardenedStarvedSand>();
                        WorldGen.SquareTileFrame(k, l);
                        NetMessage.SendTileSquare(-1, k, l, 1);
                    }
                    // If the tile is sandstone, convert to Starved Sandstone
                    else if (TileID.Sets.Conversion.Sandstone[type]) {
                        Main.tile[k, l].TileType = (ushort)ModContent.TileType<StarvedSandstone>();
                        WorldGen.SquareTileFrame(k, l);
                        NetMessage.SendTileSquare(-1, k, l, 1);
                    }
                    // If the tile is ice, convert to Starved Ice
                    else if (TileID.Sets.Conversion.Ice[type]) {
                        Main.tile[k, l].TileType = (ushort)ModContent.TileType<StarvedIce>();
                        WorldGen.SquareTileFrame(k, l);
                        NetMessage.SendTileSquare(-1, k, l, 1);
                    }
                    // If the tile is grass, convert it to Starved Grass
                    else if (TileID.Sets.Conversion.Sand[type]) {
                        Main.tile[k, l].TileType = (ushort)ModContent.TileType<StarvedGrass>();
                        WorldGen.SquareTileFrame(k, l);
                        NetMessage.SendTileSquare(-1, k, l, 1);
                    }
                }
            }
        }
    }
}
