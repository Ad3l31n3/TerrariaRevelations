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
    public class Starwalker : ModProjectile
    {
        public Player Owner => Main.player[Projectile.owner];
        public Color LightColor => new Color(255, 242, 0);

        public override void SetStaticDefaults()
        {
            Main.projPet[Projectile.type] = true;
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(319);
            AIType = 319;
            Projectile.width = 74;
            Projectile.height = 72;
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
            int frameHeight = Projectile.height;

            Rectangle sourceRectangle = new Rectangle(
                0,
                0,
                frameWidth,
                frameHeight
            );

            // Adjust origin: horizontally center, vertically bottom-aligned minus empty lines
            Vector2 origin = new Vector2(sourceRectangle.Width / 2f, sourceRectangle.Height/2);


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

            return true;
        }

        public override void AI()
        {
            if (VerifyOwnerIsPresent())
            {
                return;
            }
        }

        public bool VerifyOwnerIsPresent()
        {
            // No logic should be run if the player is no longer active in the game.
            if (!Owner.active)
            {
                Projectile.Kill();
                return true;
            }

            if (Owner.dead)
                Owner.GetModPlayer<RevelationsPlayer>().hasStarwalker = false;
            if (Owner.GetModPlayer<RevelationsPlayer>().hasStarwalker)
                Projectile.timeLeft = 2;

            return false;
        }

        public override void PostAI()
        {
            Projectile.frame = 1;
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
