using Terraria;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.ID;
using Revelations.Content.Players;

namespace Revelations.Content.Items.Accessories
{
    public class PalladiumShield : ModItem
    {
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            // Create a new tooltip line
            TooltipLine defenseLine = new TooltipLine(Mod, "DefenseBonus", "1 defense");

            // Insert the line before the "Material" tag, if present
            int materialIndex = tooltips.FindIndex(line => line.Name == "Material");
            if (materialIndex != -1)
            {
                tooltips.Insert(materialIndex, defenseLine);
            }
            else
            {
                tooltips.Add(defenseLine);
            }
        }
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.accessory = true;
            Item.value = Item.sellPrice(platinum: 1);
            Item.rare = ItemRarityID.Green;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<RevelationsPlayer>().AccessoryDefenseBoost += 1;
            player.noKnockback = true;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.PalladiumBar, 35);
            recipe.AddTile(TileID.Anvils); // This works for both Iron and Lead Anvils
            recipe.Register();
        }
    }
}