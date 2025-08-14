using Terraria;
using System;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Microsoft.Xna.Framework;

namespace Revelations.Content.Projectiles
{
    public class SlimeShot : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Melee;
            Projectile.damage = 6;
            Projectile.width = 10;
            Projectile.height = 20;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            Projectile.light = 0.25f;
        }
        public override void AI()
        {
            // Example: slow down over time (drag)
            Projectile.velocity *= 0.98f;

            // Example: clamp speed
            float maxSpeed = 11f;
            if (Projectile.velocity.Length() > maxSpeed)
            {
                Projectile.velocity = Vector2.Normalize(Projectile.velocity) * maxSpeed;
            }
            // Apply gravity
            Projectile.velocity.Y += 0.2f;

            // Optional: Limit max fall speed
            if (Projectile.velocity.Y > 16f)
            { 
                Projectile.velocity.Y = 16f;
            }

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
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