using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using Revelations.Content.Items.Accessories;

namespace Revelations
{
    public class GlobalVanillaRecipes : GlobalItem
    {
        public override void AddRecipes()
        {
            // Add a new recipe to craft Cobalt Shield
            Recipe recipe = Recipe.Create(ItemID.CobaltShield);
            recipe.AddIngredient(ItemID.CobaltBar, 35);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();

            // Add a new recipe to craft Obsidian Shield
            recipe = Recipe.Create(ItemID.ObsidianShield);
            recipe.AddIngredient(ModContent.ItemType<PalladiumShield>());
            recipe.AddIngredient(ItemID.ObsidianSkull);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.Register();

            foreach (var oldrecipe in Main.recipe)
            {
                if (oldrecipe.createItem.type == ItemID.AnkhCharm)
                {
                    oldrecipe.DisableRecipe();
                }
            }

            recipe = Recipe.Create(ItemID.AnkhCharm);
            recipe.AddIngredient(ItemID.ArmorBracing);
            recipe.AddIngredient(ItemID.MedicatedBandage);
            recipe.AddIngredient(ItemID.ThePlan);
            recipe.AddIngredient(ItemID.CountercurseMantra);
            recipe.AddIngredient(ItemID.ReflectiveShades);
            recipe.AddIngredient(ModContent.ItemType<InsolatedGloves>());
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.Register();
        }
    }
}