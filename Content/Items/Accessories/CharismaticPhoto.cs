using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;
using Revelations.Content.Players;
using Revelations.Systems;

namespace Revelations.Content.Items.Accessories
{
    public class CharismaticPhoto : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.accessory = true;
            Item.value = Item.sellPrice(gold: 20);
            Item.rare = ItemRarityID.LightRed;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<CharismaticPhotoPlayer>().SpawnBoost = true;
        }
    }
    public class CharismaticPhotoPlayer : ModPlayer
    {
        public bool SpawnBoost;

        public override void ResetEffects()
        {
            // Reset every tick; the accessory will set it again if equipped
            SpawnBoost = false;
        }
    }
}