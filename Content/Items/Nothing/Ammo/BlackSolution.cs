﻿using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace TheGreatSidegrade.Content.Items.Nothing.Ammo;

public class BlackSolution : ModItem {
    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 99;
    }

    public override void SetDefaults() {
        Item.DefaultToSolution(ModContent.ProjectileType<Projectiles.BlackSolution>());
        Item.value = Item.buyPrice(0, 0, 25);
        Item.rare = ItemRarityID.Orange;
    }

    public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup) {
        itemGroup = ContentSamples.CreativeHelper.ItemGroup.Solutions;
    }
}
