using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Revelations.Content.Projectiles.FriendlyProjectiles;
using Revelations.Content.Globals;
using Terraria.DataStructures;
using Terraria.Audio; // Required for SoundEngine
using Microsoft.Xna.Framework;

namespace Revelations.Content.Items.Weapons
{
	// This is a basic item template.
	// Please see tModLoader's ExampleMod for every other example:
	// https://github.com/tModLoader/tModLoader/tree/stable/ExampleMod
	public class GelatinousSword : ModItem
	{
		// The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.Revelations.hjson' file.
		public override void SetDefaults()
		{
			Item.damage = 16;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 21;
			Item.useAnimation = 21;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 6;
			Item.value = Item.buyPrice(silver: 30);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<SlimeShot>();
			Item.shootSpeed = 20f;
			Item.channel = false;
			Item.noMelee = false;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.noMelee = false; // Ensure the melee hitbox is used
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			var global = Item.GetGlobalItem<ProjectileSwordGlobalItem>(); //NOTE: this goes at half the rate of your swing speed, so having the number match your swing speed will make it shoot every 2 swings

			if (global.localCooldown <= 0)
			{
				SoundEngine.PlaySound(SoundID.Item39, player.Center);

				velocity.Y -= 2f;
				velocity *= 0.9f;

				Projectile.NewProjectile(source, position, velocity, type, 6, knockback, player.whoAmI);

				global.localCooldown = 24;
			}

			return false;
		}
		public override void AddRecipes()
		{
			Recipe swordRecipe = Recipe.Create(ModContent.ItemType<Content.Items.Weapons.GelatinousSword>());
			swordRecipe.AddIngredient(ItemID.Gel, 100);
			swordRecipe.AddTile(TileID.Solidifier);
			swordRecipe.Register();
		}
	}
}
