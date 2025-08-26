using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.ItemDropRules;
using Revelations.Content.Items.Weapons;
using Revelations.Content.Items.Accessories;

namespace Revelations.Content.Globals
{
    public class AccessoryEffects : GlobalItem
    {
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (item.type == ItemID.AnkhShield || item.type == ItemID.AnkhCharm)
            {
                // Example: Give immunity to burning (normally not part of Ankh Shield)
                player.buffImmune[BuffID.Frozen] = true;
                player.buffImmune[BuffID.Electrified] = true;
            }
        }
    }
    public class LootBagEditor : GlobalItem
    {
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            // Check if this is Plantera’s bag
            if (item.type == ItemID.PlanteraBossBag)
            {
                // Always drop your accessory
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<RoseBand>(), 10));
            }
        }
    }
    public class ProjectileSwordGlobalItem : GlobalItem
    {
        public int localCooldown = 0;

        public override bool InstancePerEntity => true;
        public override void HoldItem(Item item, Player player)
        {
            if (item.type == ModContent.ItemType<GelatinousSword>())
            {
                if (localCooldown > 0)
                    localCooldown--;
            }
        }
    }
}