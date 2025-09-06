using Terraria;
using Terraria.ModLoader;

namespace Revelations.Content.Players
{
    public class RevelationsPlayer : ModPlayer
    {
        public int AccessoryDefenseBoost;
        public bool hasKirara;
        public bool hasStarwalker;

        public override void ResetEffects()
        {
            AccessoryDefenseBoost = 0;
            hasKirara = false;
            hasStarwalker = false;
        } 
        public override void PostUpdateEquips()
        {
            Player.statDefense += AccessoryDefenseBoost;
        }
    }
}