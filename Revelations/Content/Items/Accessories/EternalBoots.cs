using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;
using Revelations.Content.Players;
using Revelations.Content.Items.Accessories;
using Revelations.Systems;

namespace Revelations.Content.Items.Accessories
{
    public class EternalBoots : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.accessory = true;
            Item.value = Item.sellPrice(gold: 25);
            Item.rare = ItemRarityID.Red;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.iceSkate = true;
            // Check if player is standing on ice
            Tile tileBelow = Framing.GetTileSafely(player.Center.ToTileCoordinates().X, player.Bottom.ToTileCoordinates().Y + 1);
            
            //Makes it so the player runs faster on ice
            if (tileBelow.TileType == TileID.BreakableIce)
            {
                player.runAcceleration *= 1.5f; // Accelerate faster
                player.maxRunSpeed += 1.5f;     // Increase max speed
            }

            player.lavaImmune = true; //Makes player completely immune to lava 

            player.ignoreWater = true;      // No movement slowdown underwater
            player.accFlipper = true;       // Swim upward in water
            player.gills = true;            // Optional: Breathe underwater

            if (player.wet && !player.lavaWet && !player.honeyWet) // Adds a Jellyfish Necklace like effect while underwater
            {
                // Brighter lavender light in water
                Lighting.AddLight(player.Center, 1.6f, 0.7f, 1.8f);
            }
            else
            {
                // Normal glow
                Lighting.AddLight(player.Center, 1.0f, 0.4f, 1.2f);
            }        

            // Terraspark Boots
            if (!RevelationsUtil.HasAccessoryEquipped(player, [ItemID.TerrasparkBoots, ItemID.FrostsparkBoots, ItemID.LightningBoots]))
            {
                player.moveSpeed += 0.08f;
                player.accRunSpeed = Math.Max(player.accRunSpeed, 6.75f); //This makes it so if you have an item that gives higher accRunSpeed it will overwrite this one
            }
            player.rocketBoots = 3;
            player.waterWalk = true;
            player.fireWalk = true;


            player.GetModPlayer<RevelationsPlayer>().AccessoryDefenseBoost += 4;
            player.buffImmune[BuffID.OnFire] = true; // Immune to On Fire! debuff
            player.buffImmune[BuffID.Bleeding] = true;
            player.buffImmune[BuffID.Poisoned] = true;
            player.buffImmune[BuffID.BrokenArmor] = true;
            player.buffImmune[BuffID.Slow] = true;
            player.buffImmune[BuffID.Confused] = true;
            player.buffImmune[BuffID.Cursed] = true;
            player.buffImmune[BuffID.Darkness] = true;
            player.buffImmune[BuffID.Silenced] = true;
            player.buffImmune[BuffID.CursedInferno] = true;
            player.buffImmune[BuffID.Frostburn] = true;
            player.buffImmune[BuffID.Stoned] = true;
            player.buffImmune[BuffID.Chilled] = true;
            player.buffImmune[BuffID.Frozen] = true;
            player.buffImmune[BuffID.Electrified] = true;
            player.noKnockback = true;


            // Master Ninja Gear

            player.blackBelt = true;
            player.dashType = 2;
            player.spikedBoots = 2;

            

            // Magiluminescence
            if (!RevelationsUtil.HasAccessoryEquipped(player, [ItemID.Magiluminescence]))
            {
                player.moveSpeed += 0.2f;
                player.accRunSpeed += 1.5f;
                player.runAcceleration *= 1.1f; // Better acceleration
                player.maxRunSpeed += 1.5f;
            }

        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<DauntlessBoots>());
            recipe.AddIngredient(ItemID.AnkhCharm);
            recipe.AddIngredient(ItemID.Magiluminescence);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.Register();

            recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<DauntlessBoots>());
            recipe.AddIngredient(ItemID.AnkhShield);
            recipe.AddIngredient(ItemID.Magiluminescence);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.Register();

            recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.TerrasparkBoots);
            recipe.AddIngredient(ItemID.AnkhShield);
            recipe.AddIngredient(ItemID.Magiluminescence); 
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.Register();
        }
    }
}