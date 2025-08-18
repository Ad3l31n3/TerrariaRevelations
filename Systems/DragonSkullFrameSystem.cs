// File: Revelations/Systems/DragonSkullFrameSystem.cs
using Terraria;
using Terraria.ModLoader;
using Revelations.Content.NPCs.Enemies;

namespace Revelations.Systems
{
    public class DragonSkullFrameSystem : ModSystem
    {
        public override void PostSetupContent()
        {
            Main.npcFrameCount[ModContent.NPCType<DragonSkull>()] = 3;
        }
    }
}