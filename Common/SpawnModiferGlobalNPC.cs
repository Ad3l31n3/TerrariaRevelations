using Terraria;
using Terraria.ModLoader;
using Revelations.Content.Items.Accessories;

namespace Revelations.Common
{
    public class SpawnModifierGlobalNPC : GlobalNPC
    {
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            if (player.GetModPlayer<CharismaticPhotoPlayer>().SpawnBoost)
            {
                // Lower spawnRate = faster spawns
                spawnRate = (int)(spawnRate/30); // 30x faster
                if (spawnRate < 1) spawnRate = 1; //ensures it never goes below 0

                // Higher maxSpawns = more simultaneous enemies
                maxSpawns = (int)(maxSpawns * 30f); // 30x more
                if (maxSpawns < 1) maxSpawns = 1; //ensures it never goes below 0
            }
        }
    }
}