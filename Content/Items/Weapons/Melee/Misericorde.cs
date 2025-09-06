using Terraria;
using Terraria.ID;
using Terraria.UI;
using Terraria.ModLoader;
using Revelations.Content.Projectiles.Shortswords;
using Terraria.DataStructures;
using Terraria.Audio; // Required for SoundEngine
using Microsoft.Xna.Framework;

namespace Revelations.Content.Items.Weapons.Melee
{
    public class Misericorde : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 15;
            Item.ArmorPenetration = 6;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<MisericordeProjectile>();
            Item.shootSpeed = 2.1f;
            Item.width = 32;
            Item.height = 32;
            Item.useTime = Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.knockBack = 4f;
            Item.value = Item.buyPrice(silver: 10);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
        }
    }
}