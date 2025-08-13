using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Revelations.Content.Items.Pets;
using Revelations.Content.Items.Accessories;

namespace Revelations.Content.Globals
{
    public class EnemyDrops : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            // Adds Handwarmers to base game drop rates
            if (npc.type == NPCID.SpikedIceSlime || npc.type == NPCID.IceBat || npc.type == NPCID.IceGolem || npc.type == NPCID.IcyMerman)
            {
                npcLoot.Add(ItemDropRule.Common(ItemID.HandWarmer, 100)); // 1 in 100 chance
            }
            // Adds DeliveryBox to Snatchers and Man Eaters
            if (npc.type == NPCID.Snatcher || npc.type == NPCID.ManEater)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<DeliveryBox>(), 100)); //1% chance drop
            }
            // Adds Rubber Gloves to base game drop rates
            if (npc.type == 387|| npc.type == 389)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<RubberGloves>(), 50)); // 1 in 50 chance
            }
        }   
    }
}