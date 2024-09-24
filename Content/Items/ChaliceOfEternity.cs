using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using TheGreatSidegrade.Content.Buffs;

namespace TheGreatSidegrade.Content.Items;

public class ChaliceOfEternity : ModItem {
    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 1;

        ItemID.Sets.DrinkParticleColors[Type] = [
                new(240, 240, 240),
                new(200, 200, 200),
                new(140, 140, 140)
            ];
    }

    public override void SetDefaults() {
        Item.width = 28;
        Item.height = 26;
        Item.useStyle = ItemUseStyleID.HoldUp;
        Item.useAnimation = 15;
        Item.useTime = 15;
        Item.useTurn = true;
        Item.UseSound = SoundID.Item3; // SoundID.Item29
        Item.maxStack = Item.CommonMaxStack;
        Item.consumable = true;
        Item.rare = ItemRarityID.Yellow;
        Item.value = Item.buyPrice(gold: 30);
        Item.buffType = ModContent.BuffType<Eternalized>();
        Item.buffTime = 600; // 10 sec
        Item.maxStack = 1;
    }

    public override bool CanUseItem(Player player) => !player.HasBuff<CosmicRetribution>() && !player.HasBuff<Eternalized>();
}