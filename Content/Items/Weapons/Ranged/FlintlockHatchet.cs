using System;
using Microsoft.Xna.Framework;
using Revelations.Content.Projectiles.FlintlockHatchetWorkaround;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Revelations.Content.Items.Weapons.Ranged
{
    public class FlintlockHatchet : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 13;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 1f;
            Item.noMelee = true;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 10f;
            Item.useAmmo = AmmoID.Bullet;

        }

        public override bool? UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.useStyle = ItemUseStyleID.Swing;
                Item.DamageType = DamageClass.Melee;
                Item.knockBack = 5f;
                Item.noMelee = false;
                Item.noUseGraphic = false;
            }
            else
            {
                Item.useStyle = ItemUseStyleID.Shoot;
                Item.knockBack = 1f;
                Item.noMelee = true;
                Item.noUseGraphic = true;
                Item.DamageType = DamageClass.Ranged;
                Item.shoot = ProjectileID.PurificationPowder;
                Item.useAmmo = AmmoID.Bullet;
            }
            return true;
            
        }

        public override void HoldItem(Player player)
        {
            // Item.DamageType = DamageClass.Ranged;
            // Item.knockBack = 1f;
        }

        /*public override void UseAnimation(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                
            }
        }*/

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse != 2)
            {
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<FlintlockHatchetShoot>(), 0, 0, player.whoAmI);
                return true;
            }
            return false;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return player.altFunctionUse != 2;
        }

        public override bool NeedsAmmo(Player player)
        {
            return player.altFunctionUse != 2;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
    }
}