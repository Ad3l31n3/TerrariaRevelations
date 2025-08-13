using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;
using Revelations.Content.Players;
using Revelations.Systems;

namespace Revelations.Content.Items.Accessories
{
    public class CryosplashNinjaGear : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.accessory = true;
            Item.value = Item.sellPrice(gold: 12);
            Item.rare = ItemRarityID.Yellow;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.iceSkate = true;
            player.ignoreWater = true;      // No movement slowdown underwater
            player.accFlipper = true;       // Swim upward in water
            player.gills = true;            // Optional: Breathe underwater

            if (player.wet && !player.lavaWet && !player.honeyWet) // Adds a Jellyfish Necklace like effect while underwater
            {
                // Brighter lavender light in water
                Lighting.AddLight(player.Center, 0.5f, 0.7f, 1.4f);
            }
            else
            {
                // Normal glow
                Lighting.AddLight(player.Center, 0.2f, 0.4f, 0.6f);
            }        

            // Master Ninja Gear

            player.blackBelt = true;
            player.dashType = 2;
            player.spikedBoots = 2;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.MasterNinjaGear); 
            recipe.AddIngredient(ItemID.ArcticDivingGear); 
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.Register();
        }
    }
}