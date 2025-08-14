using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;
using Revelations.Content.Players;
using Revelations.Systems;

namespace Revelations.Content.Items.Accessories
{
    public class RoseBand : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.accessory = true;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.LightRed;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<RoseBandPlayer>().RoseBandEquipped = true;
            player.GetDamage(DamageClass.Generic) *= 1.5f;
        }
    }
}