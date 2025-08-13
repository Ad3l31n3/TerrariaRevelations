using Terraria;
using Terraria.ModLoader;

namespace Revelations.Content.Players
{
    public class RevelationsPlayer : ModPlayer
    {
        public int AccessoryDefenseBoost;
        public bool hasKirara;

        public override void ResetEffects()
        {
            AccessoryDefenseBoost = 0;
            hasKirara = false;
        } 
        public override void PostUpdateEquips()
        {
            Player.statDefense += AccessoryDefenseBoost;
        }
    }
}