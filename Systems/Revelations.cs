using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Revelations.Content.Items.Accessories;

namespace Revelations.Systems
{
    public class RevelationsSystem : ModSystem
    {
        public override void PostWorldGen()
        {
            // Loop through all chests
            for (int chestIndex = 0; chestIndex < Main.maxChests; chestIndex++)
            {
                Chest chest = Main.chest[chestIndex];
                if (chest == null)
                    continue;

                // Loop through each item in the chest
                for (int itemSlot = 0; itemSlot < Chest.maxItems; itemSlot++)
                {
                    Item item = chest.item[itemSlot];

                    // Check if the item is the one you want to replace (e.g., Magic Mirror)
                    if (item != null && item.type == ItemID.CobaltShield)
                    {
                        // 50% chance to replace
                        if (Main.rand.NextFloat() < 0.50f)
                        {
                            item.SetDefaults(ModContent.ItemType<PalladiumShield>());
                        }
                    }
                }
            }
        }
    }
}