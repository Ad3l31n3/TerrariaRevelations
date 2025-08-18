using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;
using Revelations.Content.Players;
using Revelations.Systems;

namespace Revelations.Content.Items.Accessories
{
    public class DauntlessBoots : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.accessory = true;
            Item.value = Item.sellPrice(gold: 16);
            Item.rare = ItemRarityID.Lime;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.iceSkate = true;

            player.buffImmune[BuffID.OnFire] = true; // Immune to On Fire! debuff
            player.lavaImmune = true; //Makes player completely immune to lava      

            // Terraspark Boots
            if (!RevelationsUtil.HasAccessoryEquipped(player, [ItemID.TerrasparkBoots, ItemID.FrostsparkBoots, ItemID.LightningBoots]))
            {
                player.moveSpeed += 0.08f;
                player.accRunSpeed = Math.Max(player.accRunSpeed, 6.75f); //This makes it so if you have an item that gives higher accRunSpeed it will overwrite this one
            }
            player.rocketBoots = 3;
            player.waterWalk = true;
            player.fireWalk = true;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.TerrasparkBoots);
            recipe.AddIngredient(ItemID.ObsidianShield);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.Register();
        }
    }
}