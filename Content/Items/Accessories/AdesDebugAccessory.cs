using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;
using Revelations.Content.Players;
using Revelations.Systems;

namespace Revelations.Content.Items.Accessories
{
    public class AdesDebugAccessory : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.accessory = true;
            Item.value = Item.sellPrice(gold: 12);
            Item.rare = -12;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.wet && !player.lavaWet && !player.honeyWet) // Adds a Jellyfish Necklace like effect while underwater
            {
                // Brighter lavender light in water
                Lighting.AddLight(player.Center, 16f, 7f, 18f);
            }        
        }
    }
}