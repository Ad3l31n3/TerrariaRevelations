using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.DataStructures;
using Revelations.Content.Items.Weapons;
using Revelations.Content.Tiles.Banners;

namespace Revelations.Content.NPCs.Enemies
{
    public class DragonSkull : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 3; //Keeps track of the number of animation frames
        }

        public override void SetDefaults()
        {
            NPC.width = 82;
            NPC.height = 58;
            NPC.damage = 25;
            NPC.defense = 10;
            NPC.lifeMax = 100;
            NPC.HitSound = SoundID.NPCHit5; //Skeleton hit sound
            NPC.DeathSound = SoundID.NPCDeath14; //Skeleton death sound
            NPC.value = 500f; //The number of copper coins the enemy drops (this one drops 5 silver)
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = 22; //Cursed Skull AI style
            AIType = NPCID.CursedSkull;
            NPC.noGravity = true;
            NPC.noTileCollide = true;

            Banner = Type;
            BannerItem = ModContent.ItemType<Items.Banners.DragonSkullBanner>();
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return spawnInfo.Player.ZoneDungeon ? 0.05f : 0f; //Makes the enemy spawn only if you are in the dungeon with a weight of 0.05
        }

        //Makes it so the sprite flips vertically when on the player's left (useful for enemies who always look at you like Cursed Skull)
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects effects = SpriteEffects.None;

            if (NPC.rotation > MathHelper.PiOver2 || NPC.rotation < -MathHelper.PiOver2)
            {
                effects |= SpriteEffects.FlipVertically;
            }

            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            Rectangle frame = NPC.frame;
            Vector2 origin = frame.Size() / 2f;

            spriteBatch.Draw(texture, NPC.Center - screenPos, frame, drawColor, NPC.rotation, origin, 1f, effects, 0f);

            return false; // handled manually
        }

        public override void AI()
        {
            Player target = Main.player[NPC.target];
            Vector2 toPlayer = target.Center - NPC.Center;

            // Flip sprite direction
            NPC.spriteDirection = toPlayer.X < 0 ? -1 : 1;

            // Face the player smoothly
            float desiredRotation = toPlayer.ToRotation() + MathHelper.Pi;
            desiredRotation = MathHelper.WrapAngle(desiredRotation);
            NPC.rotation = NPC.rotation.AngleLerp(desiredRotation, 0.1f);

            // Light emission
            Lighting.AddLight(NPC.Center, 0.9f, 0.9f, 1.0f); // bluish-white glow
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
            npcLoot.Add(ItemDropRule.Common(ItemID.Bone, 1, 1, 3)); //the 2nd and third numbers are the minimum and maximum of the item the enemy can drop respectively
            npcLoot.Add(ItemDropRule.Common(ItemID.GoldenKey, 10)); // 10% chance

            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<DragonsWrath>(), 20)); //5% chance (1/x where x is the first number inserted)
        }

        //Sets the beastiary information
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon,
                new FlavorTextBestiaryInfoElement("The haunted skull of dragons long passed.")
            });
        }
    }
}