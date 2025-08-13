using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;
using Revelations.Content.Players;
using Revelations.Systems;

namespace Revelations.Content.Items.Accessories
{
    public class InsolatedGloves : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.accessory = true;
            Item.value = Item.sellPrice(gold: 2);
            Item.rare = ItemRarityID.Pink;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.buffImmune[BuffID.Chilled] = true;
            player.buffImmune[BuffID.Frozen] = true;
            player.buffImmune[BuffID.Electrified] = true;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<RubberGloves>()); 
            recipe.AddIngredient(ItemID.HandWarmer);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.Register();
        }
    }
}