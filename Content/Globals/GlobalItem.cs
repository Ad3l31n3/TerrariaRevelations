using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Revelations.Content.Items.Weapons;

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