using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Revelations.Content.Items.Pets
{
    public class Starwalker : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.CompanionCube);
            Item.buffType = ModContent.BuffType<Buffs.Pets.StarwalkerPetBuff>();
            Item.shoot = ModContent.ProjectileType<Projectiles.Pets.Starwalker>();

            Item.UseSound = SoundID.Item2;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(gold: 1);
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.HoldUp;
        }

        // Pet buff stays after unequipping
        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(Item.buffType, 3600, true);
            }
        }
    }
}