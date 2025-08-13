using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Revelations.Systems
{
    public class RevelationsUtil : ModSystem
    {
        //Checks if the player has the specified Accessory
        public static bool HasAccessoryEquipped(Player player, int[] accessoryTypes)
        {
            int MaxAccessorySlots = player.extraAccessory ? 11 : 10;

            for (int i = 3; i < MaxAccessorySlots; i++) // skip armor slots
            {
                Item item = player.armor[i];
                if (!item.IsAir && accessoryTypes.Contains(item.type))
                {
                    return true;
                }
            }

            return false;
        }
        //Checks if there is solid ground ahead of an NPC
        public static bool IsSolidAhead(NPC npc)
        {
            int directionX = Math.Sign(npc.velocity.X);
            int directionY = Math.Sign(npc.velocity.Y);

            // Check 3 tiles ahead in the direction of movement
            int checkDistance = 3;

            for (int i = 1; i <= checkDistance; i++)
            {
                int tileX = (int)((npc.Center.X + directionX * i * 16f) / 16f);
                int tileY = (int)((npc.Center.Y + directionY * i * 16f) / 16f);

                // ✅ Clamp check
                if (tileX < 0 || tileX >= Main.maxTilesX || tileY < 0 || tileY >= Main.maxTilesY)
                    continue; // skip out-of-bounds tile safely

                if (WorldGen.SolidTile(tileX, tileY))
                {
                    return true;
                }
            }

            return false;
        }
        // Finding the closest NPC to attack within maxDetectDistance range
        // If not found then returns null
        public static NPC FindClosestNPC(float maxDetectDistance, Projectile projectile)
        {
            NPC closestNPC = null;

            // Using squared values in distance checks will let us skip square root calculations, drastically improving this method's speed.
            float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

            // Loop through all NPCs
            foreach (var target in Main.ActiveNPCs)
            {
                // Check if NPC able to be targeted. 
                if (IsValidTarget(target, projectile))
                {
                    // The DistanceSquared function returns a squared distance between 2 points, skipping relatively expensive square root calculations
                    float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, projectile.Center);

                    // Check if it is within the radius
                    if (sqrDistanceToTarget < sqrMaxDetectDistance)
                    {
                        sqrMaxDetectDistance = sqrDistanceToTarget;
                        closestNPC = target;
                    }
                }
            }

            return closestNPC;
        }

        public static bool IsValidTarget(NPC target, Projectile projectile)
        {
            // This method checks that the NPC is:
            // 1. active (alive)
            // 2. chaseable (e.g. not a cultist archer)
            // 3. max life bigger than 5 (e.g. not a critter)
            // 4. can take damage (e.g. moonlord core after all it's parts are downed)
            // 5. hostile (!friendly)
            // 6. not immortal (e.g. not a target dummy)
            // 7. doesn't have solid tiles blocking a line of sight between the projectile and NPC
            return target.CanBeChasedBy() && Collision.CanHit(projectile.Center, 1, 1, target.position, target.width, target.height);
        }
        
        public static Player FindClosestPlayer(float maxDetectDistance, Projectile projectile)
        {
            Player closestPlayer = null;
            float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

            foreach (Player player in Main.player)
            {
                if (player != null && player.active && player.statLife > 0)
                {
                    if (IsValidPlayerTarget(player, projectile))
                    {
                        float sqrDistanceToPlayer = Vector2.DistanceSquared(player.Center, projectile.Center);
                        if (sqrDistanceToPlayer < sqrMaxDetectDistance)
                        {
                            sqrMaxDetectDistance = sqrDistanceToPlayer;
                            closestPlayer = player;
                        }
                    }
                }
            }

            if (closestPlayer != null)
                Main.NewText($"Found player: {closestPlayer.name}");

            return closestPlayer;
        }

        public static bool IsValidPlayerTarget(Player player, Projectile projectile)
        {
            // Checks to make sure player can be targeted:
            // - Active and alive checked already in caller
            // - Not the owner of the projectile (if you want to exclude self)
            // - Not dead or ghosted
            // - Line of sight not blocked

            if (player.dead || player.ghost)
                return false;

            // Optionally exclude projectile owner:
            if (projectile.owner == player.whoAmI)
                return false;

            // Line of sight check (can add more parameters if needed)
            return Collision.CanHit(projectile.Center, 1, 1, player.position, player.width, player.height);
        }

    }
}