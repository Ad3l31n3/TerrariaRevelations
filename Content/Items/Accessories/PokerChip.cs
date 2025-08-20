using Terraria;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.ID;
using Revelations.Content.Players;

namespace Revelations.Content.Items.Accessories
{
    public class PokerChip : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.accessory = true;
            Item.value = Item.sellPrice(copper: 1);
            Item.rare = ItemRarityID.Green;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PokerChipPlayer>().PokerChipEquipped = true;
        }
    }
    public class PokerChipPlayer : ModPlayer
    {
        public bool PokerChipEquipped;

        public override void ResetEffects()
        {
            // Reset every tick; the accessory will set it again if equipped
            PokerChipEquipped = false;
        }
    }
}