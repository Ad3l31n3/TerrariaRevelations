using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Revelations.Content.Items.Pets
{
    public class DeliveryBox : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ZephyrFish);
            Item.buffType = ModContent.BuffType<Buffs.Pets.KiraraPetBuff>();
            Item.shoot = ModContent.ProjectileType<Projectiles.Pets.Kirara>();

            Item.UseSound = SoundID.Item2;
            Item.rare = ItemRarityID.LightPurple;
            Item.value = Item.sellPrice(gold: 1);
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.HoldUp;
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (player.whoAmI == Main.myPlayer && !player.HasBuff(Item.buffType))
            {
                player.AddBuff(Item.buffType, 3600);
            }
        }
    }
}