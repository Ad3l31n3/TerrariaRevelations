using Terraria;
using Terraria.ModLoader;
using Revelations.Content.Players;
using System;
using Microsoft.Xna.Framework;
using Revelations.Content.Projectiles;

namespace Revelations.Content.Buffs.Pets
{
    public class StarwalkerPetBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true; // Important: marks this as a light pet buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000; // Refresh duration constantly
            player.GetModPlayer<RevelationsPlayer>().hasStarwalker = true;

            bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Pets.Starwalker>()] <= 0;
            // Spawn the projectile if it doesn’t exist yet
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(player.GetSource_Buff(buffIndex),
                    player.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<Projectiles.Pets.Starwalker>(),
                    0,
                    0f,
                    player.whoAmI);
            }
        }
    }
}