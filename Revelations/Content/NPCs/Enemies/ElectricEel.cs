using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.DataStructures;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Revelations.Systems;
using Revelations.Content.Items.Accessories;

namespace Revelations.Content.NPCs.Enemies
{
    public class ElectricEel : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 8;
        }
        private enum AIState
        {
            Idle = 0,
            Aiming = 1,
            Charging = 2,
            Beached = 3,
            Submurge = 4
        }
        private AIState CurrentState
        {
            get => (AIState)(int)NPC.ai[0];
            set => NPC.ai[0] = (float)value;
        }
        public override void SetDefaults()
        {
            NPC.width = 60;
            NPC.height = 18;
            
            if (CurrentState == AIState.Charging || CurrentState == AIState.Aiming)
            {
                NPC.damage = 60; // High damage during charge
            }
            else
            {
                NPC.damage = 20; // Default/base damage
            }

            NPC.defense = 3;
            NPC.lifeMax = 150;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = -1; // custom AI
            NPC.HitSound = SoundID.NPCHit1; //Basic hit sound
            NPC.DeathSound = SoundID.NPCDeath13; //Jellyfish death sound
            NPC.noGravity = true;
            NPC.lavaImmune = false;
            NPC.value = 1500f;
            NPC.buffImmune[BuffID.Wet] = true; // Doesn't receive "Wet" debuff

            Banner = Type;
            BannerItem = ModContent.ItemType<Items.Banners.ElectricEelBanner>();
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            Rectangle frame = NPC.frame;
            Vector2 origin = frame.Size() / 2f;

            SpriteEffects effects = (NPC.spriteDirection == -1) ? SpriteEffects.FlipVertically : SpriteEffects.None;

            spriteBatch.Draw(
                texture,
                NPC.Center - screenPos,
                frame,
                drawColor,
                NPC.rotation,
                origin,
                1f,
                effects,
                0f
            );

            return false; // we handled the drawing
        }
        public override void AI()
        {
            // Check if the NPC is in water
            bool inWater = Collision.WetCollision(NPC.position, NPC.width, NPC.height);
            Player player = Main.player[NPC.target];
            NPC.TargetClosest();

            float detectRange = 560f; // 35 tiles
            float chargeSpeed = 18.0f;
            float idleSpeed = 2.5f;
            Vector2 cachedDir = new Vector2(NPC.ai[2], NPC.ai[3]);
            switch ((AIState)NPC.ai[0])
            {
                case AIState.Idle:

                    NPC.velocity.Y = -0.1f; //very slightly constantly goes up
                    if (NPC.rotation == MathHelper.Pi)
                    {
                        NPC.velocity.X = idleSpeed;
                    }
                    else
                    {
                        NPC.velocity.X = -idleSpeed;
                    }
                    if (NPC.collideX || RevelationsUtil.IsSolidAhead(NPC))
                    {
                        if (NPC.rotation == MathHelper.Pi)
                        {
                            NPC.rotation = 0;
                        }
                        else
                        {
                            NPC.rotation = MathHelper.Pi;
                        }
                        NPC.velocity.X *= -1;
                    }

                    NPC.ai[1]++;
                    
                    if (!inWater)
                    {
                        if (NPC.velocity.Y < 0)
                        {
                            NPC.velocity.Y = 0;
                        }
                        CurrentState = AIState.Beached;
                        NPC.ai[1] = 0;
                        NPC.netUpdate = true;
                    }
                    // Check for player
                    if (player.active && !player.dead && Vector2.Distance(NPC.Center, player.Center) < detectRange && NPC.ai[1] >= 60 && player.wet) //Gives a 1 second cooldown between entering idle and targeting
                    {
                        // Play custom sound here
                        SoundEngine.PlaySound(Revelations.ElectricEelCharge, NPC.Center);
                        CurrentState = AIState.Aiming;
                        NPC.ai[1] = 0;
                        NPC.netUpdate = true;
                    }
                    break;

                case AIState.Aiming:
                    Lighting.AddLight(NPC.Center, 0.9f, 0.9f, 1.0f); // bluish-white glow
                    NPC.velocity *= 0.5f; // slow to a stop
                    Vector2 direction = player.Center - NPC.Center;
                    // Face the player smoothly
                    float desiredRotation = direction.ToRotation() + MathHelper.Pi;
                    desiredRotation = MathHelper.WrapAngle(desiredRotation);
                    NPC.rotation = NPC.rotation.AngleLerp(desiredRotation, 0.1f);
                    direction.Normalize();
                    NPC.ai[1]++;
                    if (NPC.ai[1] >= 90) // 1.5 seconds
                    {
                        // Store direction in ai[2] and ai[3]
                        NPC.ai[2] = direction.X;
                        NPC.ai[3] = direction.Y;

                        // Set velocity once
                        NPC.velocity = direction * chargeSpeed;

                        CurrentState = AIState.Charging;
                        NPC.ai[1] = 0;
                        NPC.netUpdate = true;
                    }
                    break;

                case AIState.Charging:
                    Lighting.AddLight(NPC.Center, 0.9f, 0.9f, 1.0f); // bluish-white glow
                    cachedDir = new Vector2(NPC.ai[2], NPC.ai[3]);  // Use stored direction from ai[2] and ai[3]
                    NPC.velocity = cachedDir * chargeSpeed;

                    // After charging for a short time, return to idle
                    NPC.ai[1]++;
                    if (!inWater)
                    {
                        if (NPC.velocity.Y < 0)
                        {
                            NPC.velocity.Y = 0;
                        }
                        CurrentState = AIState.Beached;
                        NPC.ai[1] = 0;
                        NPC.netUpdate = true;
                    }
                    if (NPC.collideX || RevelationsUtil.IsSolidAhead(NPC) || NPC.ai[1] >= 120) // charge for 2 second
                    {
                        if (NPC.rotation > MathHelper.PiOver2 || NPC.rotation < -MathHelper.PiOver2)
                        {
                            NPC.rotation = MathHelper.Pi;
                        }
                        else
                        {
                            NPC.rotation = 0f;
                        }

                        CurrentState = AIState.Idle;
                        NPC.ai[1] = 0;
                        NPC.netUpdate = true;
                    }
                    break;

                case AIState.Beached:
                    NPC.velocity.X = 0;
                    NPC.velocity.Y += 0.4f; // Gravity
                    NPC.ai[1]++;
                    if (inWater)
                    {
                        NPC.rotation = -Main.rand.NextFloat(MathHelper.PiOver2 / 2, MathHelper.PiOver2 * 3 / 2);
                        Vector2 SubmurgeDirection = NPC.rotation.ToRotationVector2(); // Convert rotation to a direction vector
                        // Store direction in ai[2] and ai[3]
                        NPC.ai[2] = -SubmurgeDirection.X;
                        NPC.ai[3] = -SubmurgeDirection.Y;
                        CurrentState = AIState.Submurge;
                        NPC.ai[1] = 0;
                        NPC.netUpdate = true;
                    }
                    break;

                case AIState.Submurge:
                    cachedDir = new Vector2(NPC.ai[2], NPC.ai[3]);  // Use stored direction from ai[2] and ai[3]
                    NPC.velocity = cachedDir * idleSpeed * 2;
                    NPC.ai[1]++;

                    if (!inWater)
                    {
                        if (NPC.rotation > MathHelper.PiOver2 || NPC.rotation < -MathHelper.PiOver2)
                        {
                            NPC.rotation = MathHelper.Pi;
                        }
                        else
                        {
                            NPC.rotation = 0f;
                        }
                        CurrentState = AIState.Beached;
                        NPC.ai[1] = 0;
                        NPC.netUpdate = true;
                    }

                    if (NPC.collideX || NPC.collideY || RevelationsUtil.IsSolidAhead(NPC) || NPC.ai[1] >= 120) // swim down for 1 second
                    {
                        if (NPC.rotation > MathHelper.PiOver2 || NPC.rotation < -MathHelper.PiOver2)
                        {
                            NPC.rotation = MathHelper.Pi;
                        }
                        else
                        {
                            NPC.rotation = 0f;
                        }

                        CurrentState = AIState.Idle;
                        NPC.netUpdate = true;
                    }
                    break;
            }
            // Flip vertically if upside down
            if (NPC.rotation > MathHelper.PiOver2 || NPC.rotation < -MathHelper.PiOver2)
            {
                NPC.spriteDirection = -1;
            }
            else
            {
                NPC.spriteDirection = 1;
            }
        }
        public override void FindFrame(int frameHeight)
        {
            if ((AIState)NPC.ai[0] == AIState.Idle || (AIState)NPC.ai[0] == AIState.Beached || (AIState)NPC.ai[0] == AIState.Submurge)
            {
                if ((AIState)NPC.ai[0] == AIState.Beached)
                {
                    NPC.frameCounter += 0.1;
                }
                else
                {
                    NPC.frameCounter += 0.2;
                }
                
                if (NPC.frameCounter >= 5)
                    NPC.frameCounter = 0;

                NPC.frame.Y = (int)NPC.frameCounter * frameHeight;
            }
            else
            {
                NPC.frameCounter += 0.2;
                if (NPC.frameCounter >= 3)
                    NPC.frameCounter = 0;

                // Offset by 3 frames to skip charging ones
                NPC.frame.Y = ((int)NPC.frameCounter + 5) * frameHeight;
            }
        }
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
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            if (CurrentState == AIState.Charging || CurrentState == AIState.Aiming)
            {
                // Apply Electrified for 2 seconds
                target.AddBuff(BuffID.Electrified, 120); // 60 = 1 second
            }
        }
        //Controls the loot the enemy drops
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<RubberGloves>(), 50)); //2% chance

            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<AdesDebugAccessory>(), 1000)); //0.1% chance
        }
        
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return Main.hardMode && spawnInfo.Player.ZoneBeach && spawnInfo.Water ? 0.2f : 0f;
        }
        
        //Sets the beastiary information
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,
                new FlavorTextBestiaryInfoElement("An Eel that produces electric currents to daze threats and prey")
            });
        }
    }
}
