using Revelations.Content.Items.Accessories;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

public class LuckyCoinGlobalNPC : GlobalNPC
{
    public override void OnKill(NPC npc)
    {
        // Only run if killed by a player with the accessory
        int killer = npc.lastInteraction;
        if (killer < 0 || killer >= Main.maxPlayers)
            return;

        Player player = Main.player[killer];
        if (!player.active || player.dead)
            return;

        if (player.GetModPlayer<PokerChipPlayer>().PokerChipEquipped)
        {
            // Calculate base coin value
            int coinValue = (int)npc.value; // normally NPC.value controls coin drops

            // Prevent normal coin drops
            npc.value = 0;


            if (coinValue > 0)
            {
                if (Main.rand.NextBool(2))
                {
                    // 50% chance: Triple the coins
                    coinValue *= 3;
                }
                else
                {
                    // 50% chance: Force 1 copper
                    coinValue = 1;
                }

                // Drop the coins
                if (coinValue >= 1000000)
                {
                    int platinum = coinValue / 1000000;
                    Item.NewItem(npc.GetSource_Loot(), npc.Hitbox, ItemID.PlatinumCoin, platinum);
                    coinValue -= platinum * 1000000;
                }
                if (coinValue >= 10000)
                {
                    int gold = coinValue / 10000;
                    Item.NewItem(npc.GetSource_Loot(), npc.Hitbox, ItemID.GoldCoin, gold);
                    coinValue -= gold * 10000;
                }
                if (coinValue >= 100)
                {
                    int silver = coinValue / 100;
                    Item.NewItem(npc.GetSource_Loot(), npc.Hitbox, ItemID.SilverCoin, silver);
                    coinValue -= silver * 100;
                }
                Item.NewItem(npc.GetSource_Loot(), npc.Hitbox, ItemID.CopperCoin, coinValue);
            }
        }
    }
}