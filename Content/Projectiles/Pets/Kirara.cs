using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Revelations.Content.Players;
using Revelations.Extensions;

namespace Revelations.Content.Projectiles.Pets
{
    public class Kirara : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projPet[Projectile.type] = true;
            Main.projFrames[Projectile.type] = 11;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(319);
            AIType = 319;
            Projectile.width = 42;
            Projectile.height = 38;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 18000;
            Projectile.ignoreWater = true;
            Projectile.netImportant = true;
        }

        private bool ShouldPassThroughBlocks()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 toPlayer = player.Center - Projectile.Center;

            bool petInSolid = Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height);
            bool tooFar = toPlayer.Length() > 400f;
            bool playerInAir = player.velocity.Y != 0f || !player.IsOnGround();

            return petInSolid || tooFar || playerInAir;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            int frameWidth = Projectile.width;

            int trueFrameHeight = 40;      // total height per frame (visible + empty lines)
            int visibleFrameHeight = 38;   // height without empty lines at the bottom

            Rectangle sourceRectangle = new Rectangle(
                0,
                Projectile.frame * trueFrameHeight,  // skip full frame height including empty lines
                frameWidth,
                visibleFrameHeight                   // only draw the visible part
            );

            // Adjust origin: horizontally center, vertically bottom-aligned minus empty lines
            Vector2 origin = new Vector2(sourceRectangle.Width / 2f, visibleFrameHeight/2);


            Vector2 drawPosition = Projectile.Center - Main.screenPosition;

            SpriteEffects effects = Projectile.spriteDirection == -1
                ? SpriteEffects.FlipHorizontally
                : SpriteEffects.None;

            Main.spriteBatch.Draw(
                texture,
                drawPosition,
                sourceRectangle,
                lightColor,
                Projectile.rotation,
                origin,
                Projectile.scale,
                effects,
                0f
            );

            return false;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            // Despawn if player is dead or pet is no longer marked as active
            if (!player.active || player.dead || !player.GetModPlayer<RevelationsPlayer>().hasKirara)
            {
                Projectile.Kill();
                return;
            }

            // Keep the pet alive
            Projectile.timeLeft = 2;
        }
        public override void PostAI()
        {
            bool onGround = IsOnGround();
            float velX = Math.Abs(Projectile.velocity.X);
            float velY = Projectile.velocity.Y;

            // Flying (airborne)
            if (!onGround && velY != 0)
            {
                if (Projectile.frame < 8 || Projectile.frame > 10)
                {
                    Projectile.frame = 8;
                    Projectile.frameCounter = 0;
                }

                Projectile.frameCounter++;
                if (Projectile.frameCounter >= 6)
                {
                    Projectile.frameCounter = 0;
                    Projectile.frame++;
                    if (Projectile.frame > 10)
                        Projectile.frame = 8;
                }
            }
            // Walking (on ground and moving)
            else if (onGround && velX > 0.5f)
            {
                if (Projectile.frame < 1 || Projectile.frame > 7)
                {
                    Projectile.frame = 1;
                    Projectile.frameCounter = 0;
                }

                Projectile.frameCounter++;
                if (Projectile.frameCounter >= 10)
                {
                    Projectile.frameCounter = 0;
                    Projectile.frame++;
                    if (Projectile.frame > 7)
                        Projectile.frame = 1;
                }
            }
            // Idle (on ground and not moving)
            else if (onGround && velX <= 0.5f)
            {
                Projectile.frame = 0;
                Projectile.frameCounter = 0;
            }

            // Flip direction
            if (Projectile.velocity.X > 0f)
                Projectile.spriteDirection = -1;
            else if (Projectile.velocity.X < 0f)
                Projectile.spriteDirection = 1;
        }


        private bool IsOnGround()
        {
            Point tilePos = (Projectile.Bottom + new Vector2(0, 2f)).ToTileCoordinates();
            int tilesWide = (Projectile.width + 15) / 16;
            for (int i = 0; i < tilesWide; i++)
            {
                int x = tilePos.X + i;
                int y = tilePos.Y;
                if (WorldGen.SolidTile(x, y))
                    return true;
            }
            return false;
        }
    }
}
