using Terraria;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.DataStructures;
using Revelations.Systems;

namespace Revelations.Content.Projectiles.FriendlyProjectiles
{
    public class ArchDemonScytheProjectile : ModProjectile
    {
        public float frameCounter = 0;
        private bool isDying = false; // Track "death" state
        private bool playedDeathSound = false; // Track sound playing

        // Store the target NPC using Projectile.ai[0]
		private NPC HomingTarget {
			get
            {
                int index = (int)Projectile.ai[0] - 1;
                if (index < 0 || index >= Main.npc.Length)
                    return null; // out of bounds → no target

                NPC npc = Main.npc[index];
                if (npc == null || !npc.active)
                    return null; // inactive or null NPC → no target

                return npc;
            }
            set
            {
                Projectile.ai[0] = value == null ? 0 : value.whoAmI + 1;
            }
		}

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Magic;
            Projectile.damage = 53;
            Projectile.penetrate = 3; // Will hit 3 enemies before disappearing
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            if (isDying)
            {
                Projectile.tileCollide = false;
            }
            else
            {
                Projectile.tileCollide = true;
            }
            Projectile.netImportant = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10; // 10 ticks = 1/6 second
        }
        public override bool? CanHitNPC(NPC target)
        {
            return !target.friendly; // Only hit hostile NPCs
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
            float maxDetectRadius = 400f; // The maximum radius at which a projectile can detect a target
            float maxSpeed = 15f;
            float acceleration = 0.15f; // how fast it accelerates

            // Increment local counter
            frameCounter++;

            // Every 5 frames, rotate by 90 degrees (π/2 radians)
            if (frameCounter % 3 == 0)
            {
                Projectile.rotation += MathHelper.PiOver2;
            }

            // Emit light
            Lighting.AddLight(Projectile.Center, 0.3f, 1f, 0.3f);

            // Find or validate homing target
            if (HomingTarget == null || !RevelationsUtil.IsValidTarget(HomingTarget, Projectile))
            {
                HomingTarget = RevelationsUtil.FindClosestNPC(maxDetectRadius, Projectile);
            }
            

            if (frameCounter < 40) // First 20 ticks = no movement
            {
                Projectile.velocity *= 0f; // Stop moving
            }
            
            Vector2 desiredDirection;

            if (HomingTarget != null)
            {
                // Direction to target (homing)
                desiredDirection = HomingTarget.Center - Projectile.Center;
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
    }
}