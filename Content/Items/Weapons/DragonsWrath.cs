using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Revelations.Content.Projectiles;
using Terraria.DataStructures;
using Terraria.Audio; // Required for SoundEngine
using Microsoft.Xna.Framework;

namespace Revelations.Content.Items.Weapons
{ 
	// This is a basic item template.
	// Please see tModLoader's ExampleMod for every other example:
	// https://github.com/tModLoader/tModLoader/tree/stable/ExampleMod
	public class DragonsWrath : ModItem
	{
		// The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.Revelations.hjson' file.
		public override void SetDefaults()
		{
			Item.damage = 45;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 35;
			Item.useAnimation = 35;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 9;
			Item.value = Item.buyPrice(gold: 2);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shootSpeed = 15f;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			int projDamage = 95; // set to your desired projectile damage
			Projectile.NewProjectile(source, position, velocity, type, projDamage, knockback, player.whoAmI);
			return false; // return false so the default projectile isn't spawned
		}
		int shotCooldown = 4;
		public override bool CanUseItem(Player player)
		{
			if (shotCooldown <= 0)
			{
				SoundEngine.PlaySound(SoundID.Item15, player.Center);
				Item.shoot = ModContent.ProjectileType<DragonShot>();
				shotCooldown = 4; // Shoots every 5 swings
			}
			else
			{
				Item.shoot = ProjectileID.None;
				shotCooldown--;
			}
			return base.CanUseItem(player);
		}
	}
}
