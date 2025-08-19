using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.DataStructures;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Revelations.Systems;

namespace Revelations.Content.Projectiles.HostileProjectiles
{
    public class ArchDemonSickle : ModProjectile
    {
        private int frameCounter = 0;
        private bool isDying = false; // Track "death" state
        private bool playedDeathSound = false; // Track sound playing
        private const int maxLostTargetTime = 600; // 600 ticks = 10 seconds (at 60 FPS)
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Magic;
            Projectile.damage = 57;
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.netImportant = true;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return target.friendly; // Only hit friendly NPCs
        }
        public override bool PreDraw(ref Color lightColor)
        {
            // If dying, make invisible
            if (isDying)
                return false; // Skip drawing the sprite

            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;

            // Texture center
            Vector2 textureOrigin = new Vector2(texture.Width / 2f, texture.Height / 2f);

            // Draw at Projectile.Center so rotation pivots correctly
            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                lightColor,
                Projectile.rotation,
                textureOrigin,
                Projectile.scale,
                SpriteEffects.None,
                0
            );

            return false; // Don't draw the default sprite
        }
        public override void AI()
        {
            float maxDetectRadius = 400f;
            float maxSpeed = 10f;
            float acceleration = 0.3f;

            frameCounter++;

            if (frameCounter % 3 == 0)
            {
                Projectile.rotation += MathHelper.PiOver2;
            }

            Lighting.AddLight(Projectile.Center, 0.3f, 1f, 0.3f);

            Player target = null;
            float closestDistance = maxDetectRadius * maxDetectRadius;

            foreach (Player player in Main.player)
            {
                if (player.active && !player.dead)
                {
                    float dist = Vector2.DistanceSquared(player.Center, Projectile.Center);
                    if (dist < closestDistance)
                    {
                        closestDistance = dist;
                        target = player;
                    }
                }
            }

            if (frameCounter < 40) // First 40 ticks = no movement
            {
                Projectile.velocity *= 0f; // Stop moving
            }

            Vector2 desiredDirection;

            if (target != null)
            {
                // Direction to target (homing)
                desiredDirection = target.Center - Projectile.Center;
                desiredDirection.Normalize();
            }
            else
            {
                // No target: move in original direction (stored in ai[0], ai[1])
                desiredDirection = new Vector2(Projectile.ai[1], Projectile.ai[2]);
                if (desiredDirection == Vector2.Zero)
                    desiredDirection = Vector2.UnitY; // default direction if none stored
                desiredDirection.Normalize();
            }

            // Accelerate toward desired direction
            Projectile.velocity += desiredDirection * acceleration;

            // Clamp speed to maxSpeed
            if (Projectile.velocity.Length() > maxSpeed)
            {
                Projectile.velocity = Vector2.Normalize(Projectile.velocity) * maxSpeed;
            }
        }

        // Store firing direction when spawned
        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
            // If fired with velocity, store it in ai[0] and ai[1]
            if (Projectile.velocity != Vector2.Zero)
            {
                Projectile.ai[1] = Projectile.velocity.X;
                Projectile.ai[2] = Projectile.velocity.Y;
            }

            // Start at 0 velocity
            Projectile.velocity = Vector2.Zero;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            isDying = true;
            Projectile.timeLeft = 1; // Kills projectile next tick
            Projectile.velocity = Vector2.Zero; // Stop movement
            
            // Play sound only once when dying
            if (isDying && !playedDeathSound)
            {
                SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
                playedDeathSound = true;
            }

            for (int i = 0; i < 10; i++) // spawn multiple for effect
            {
                int dust = Dust.NewDust(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.GreenTorch,
                    Scale: 1.2f
                );
            }

            return false; // Keep it alive for 1 more frame
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (Main.rand.NextFloat() < 0.33f) // 33% chance
            {
                target.AddBuff(BuffID.Blackout, 600); // 10 seconds
            }
        }
    }
}