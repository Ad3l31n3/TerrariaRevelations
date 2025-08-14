using Terraria;
using Terraria.ModLoader;
using Revelations.Content.Players;
using System;
using Microsoft.Xna.Framework;

namespace Revelations.Content.Buffs.Pets
{
    public class KiraraPetBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
            Main.vanityPet[Type] = true; // Important: marks this as a pet buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000; // Refresh duration constantly
            player.GetModPlayer<RevelationsPlayer>().hasKirara = true;
            // Spawn the projectile if it doesn’t exist yet
            if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Pets.Kirara>()] <= 0 && player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(player.GetSource_Buff(buffIndex),
                    player.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<Projectiles.Pets.Kirara>(),
                    0,
                    0f,
                    player.whoAmI);
            }
        }
    }
}