using Microsoft.Xna.Framework;
using Terraria;

namespace Revelations.Extensions
{
    public static class ProjectileExtensions
    {
        public static bool IsOnGround(this Projectile projectile)
        {
            // Check directly below the projectile
            Vector2 checkPos = projectile.Bottom + Vector2.UnitY;
            return Collision.SolidCollision(checkPos, projectile.width, 2);
        }
    }
}