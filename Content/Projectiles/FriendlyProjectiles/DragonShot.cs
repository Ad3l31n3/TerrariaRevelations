using Terraria;
using System;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Microsoft.Xna.Framework;

namespace Revelations.Content.Projectiles.FriendlyProjectiles
{
    public class DragonShot : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Melee;
            Projectile.damage = 95;
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
        }
        public override void AI()
        {
            // Make the projectile rotate over time
            Projectile.rotation += 0.3f * Projectile.direction; // Rotate clockwise or counterclockwise

            // Optional: Emit light or dust
            Lighting.AddLight(Projectile.Center, 0.7f, 0.3f, 0.9f);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            // Play block placing sound
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);

            // Optionally: kill the projectile after hitting the tile
            return true; // true = destroy projectile, false = keep it alive
        }
    }
}