using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Revelations.Content.Players
{
    public class RoseBandPlayer : ModPlayer
    {
        public bool RoseBandEquipped;
        private int cachedManaMax2;
        private int cachedManaRegen;

        public override void ResetEffects()
        {
            RoseBandEquipped = false;
            cachedManaMax2 = 0;
            cachedManaRegen = 0;
        }

        public override void UpdateEquips()
        {
            if (!RoseBandEquipped)
                return;

            if (cachedManaMax2 == 0)
                cachedManaMax2 = Player.statManaMax2;

            if (cachedManaRegen == 0)
            {
                cachedManaRegen = Player.manaRegen;
            }

            //makes sure your current HP never exceeds half your max HP rounding down
            if (Player.statLife > Player.statLifeMax2 / 2)
            {
                Player.statLife = Player.statLifeMax2 / 2;
            }
            // Apply your modifier ONCE per frame using base stats
            Player.statManaMax2 = (int)(cachedManaMax2 * 1.5f);
            Player.manaRegen = (int)(cachedManaRegen * 1.5f);
        }
    }
    
}
