using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.DataStructures;
using Revelations.Content.Projectiles.HostileProjectiles;
using Revelations.Content.Items.Weapons;
using Revelations.Content.Tiles.Banners;

namespace Revelations.Content.NPCs.Enemies
{
    public class ArchDemon : ModNPC
    {
        private int shootTimer;
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 4; //Keeps track of the number of animation frames
        }

        public override void SetDefaults()
        {
            NPC.width = 82;
            NPC.height = 60;
            NPC.damage = 46;
            NPC.defense = 10;
            NPC.lifeMax = 350;
            NPC.HitSound = SoundID.NPCHit1; //Skeleton hit sound
            NPC.DeathSound = SoundID.NPCDeath14; //Skeleton death sound
            NPC.value = 1600f; //The number of copper coins the enemy drops (this one drops 5 silver)
            NPC.knockBackResist = 0.4f;
            NPC.aiStyle = 22; //Flying AI style
            AIType = NPCID.Demon;
            NPC.noGravity = true;

            NPC.lavaImmune = true;      // Immune to lava damage
            NPC.buffImmune[BuffID.OnFire] = true; // Immune to On Fire! debuff
            NPC.buffImmune[BuffID.Confused] = true; // Immune to Confused debuff
            NPC.buffImmune[BuffID.ShadowFlame] = true; // Immune to ShadowFlame debuff
            NPC.buffImmune[BuffID.OnFire3] = true; // Immune to Hellfire debuff

            Banner = Type;
            BannerItem = ModContent.ItemType<Items.Banners.ArchDemonBanner>();
        }
               
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return Main.hardMode && spawnInfo.Player.ZoneUnderworldHeight ? 0.2f : 0f; //Makes the enemy spawn only if you are in the dungeon with a weight of 0.05
        }

        public override void AI()
        {
            NPC.TargetClosest(true);
            Player player = Main.player[NPC.target];
            float speed = 4f; // Increase speed
            float acceleration = 0.2f;

            Vector2 toPlayer = player.Center - NPC.Center;
            toPlayer.Normalize();
            toPlayer *= speed;
            NPC.velocity = Vector2.Lerp(NPC.velocity, toPlayer, acceleration);

            NPC.spriteDirection = (player.Center.X < NPC.Center.X) ? -1 : 1;

            // If the player is NOT below the NPC
            if (player.Center.Y < NPC.Center.Y)
            {
                // Fly upward (negative Y velocity)
                NPC.velocity.Y = MathHelper.Lerp(NPC.velocity.Y, -4f, 0.2f);
            }
            else
            {
                // Optionally hover or slow down
                NPC.velocity.Y *= 0.9f; // slow fall
            }

            // Optional: limit vertical speed
            NPC.velocity.Y = MathHelper.Clamp(NPC.velocity.Y, -8f, 8f);

            shootTimer++;

            // Shoot every 90 ticks (1.5 seconds)
            if (shootTimer >= 90 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                shootTimer = 0;

                Vector2 shootDirection = toPlayer.SafeNormalize(Vector2.Zero) * 8f;

                Projectile.NewProjectile(
                    NPC.GetSource_FromAI(),
                    NPC.Center,
                    shootDirection,
                    ModContent.ProjectileType<ArchDemonSickle>(), // Your projectile class
                    63, // damage
                    1f, // knockback
                    Main.myPlayer
                );
            }
            NPC.rotation = MathHelper.Clamp(NPC.velocity.X * 0.1f, -0.25f, 0.25f);
        }

        //Handles the death animation
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Bone);
                }
            }
        }

        //Handles the animation
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 1.0;
            if (NPC.frameCounter >= 6)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;
                if (NPC.frame.Y >= frameHeight * Main.npcFrameCount[NPC.type])
                {
                    NPC.frame.Y = 0;
                }
            }
        }

        //Controls the loot the enemy drops
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ArchDemonScythe>(), 50)); //2% chance
        }

        //Sets the beastiary information
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,
                new FlavorTextBestiaryInfoElement("Elite Demons who are far stronger than their ordinary counterparts.")
            });
        }
    }
}