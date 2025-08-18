using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ID;

namespace Revelations.Extensions
{
    public static class PlayerExtensions
    {
        public static bool IsOnGround(this Player player)
        {
            Point tilePos = player.Bottom.ToTileCoordinates();

            for (int i = -1; i <= 1; i++)
            {
                Tile tile = Framing.GetTileSafely(tilePos.X + i, tilePos.Y);
                if (tile.HasTile && Main.tileSolid[tile.TileType])
                    return true;
            }

            return false;
        }
    }
}