using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;

namespace Revelations.Content.UI
{
    /*
    public class CustomResourceBarSystem : ModSystem
    {
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            Player player = Main.LocalPlayer;
            // Only draw custom health bar if the accessory is equipped (gotta fix)
            if (player.HasItem(ModContent.ItemType<Items.Accessories.RoseBand>()))
            {
                int vanillaIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
                // Remove the vanilla resource bar 
                layers.RemoveAt(vanillaIndex);
                if (vanillaIndex != -1)
                {
                    layers.Insert(vanillaIndex + 1, new LegacyGameInterfaceLayer(
                        "Revelations: Rose Band Health and Mana UI",
                        delegate
                            {
                                DrawCustomHeartsAndMana(Main.spriteBatch);
                                return true;
                            },
                        InterfaceScaleType.UI)
                    );
                }
            }
        }

        private void DrawCustomHeartsAndMana(SpriteBatch spriteBatch)
        {
            Player player = Main.LocalPlayer;
            int heartCount = player.statLifeMax2 / 20;
            int stars = player.statManaMax2 / 20;

            Texture2D heartTexture = TextureAssets.Heart.Value;
            Texture2D manaTexture = TextureAssets.Mana.Value;

            int heartsPerRow = 10;
            int heartSpacingX = 28;
            int heartSpacingY = 32;
            int totalHeartWidth = heartsPerRow * heartSpacingX;

            // --- Position Hearts centered at top
            Vector2 heartStart = new Vector2(Main.screenWidth - 80, 60);

            for (int i = 0; i < heartCount; i++)
            {
                int row = i / heartsPerRow;
                int col = i % heartsPerRow;
                Vector2 pos = heartStart - new Vector2(col * heartSpacingX, -row * heartSpacingY);
                spriteBatch.Draw(heartTexture, pos, Color.White);
            }

            // --- Position Mana stars on the top-right side of screen
            int manaSpacingY = 28;
            Vector2 manaStart = new Vector2(Main.screenWidth - 40, 60);

            for (int i = 0; i < stars; i++)
            {
                Vector2 pos = manaStart + new Vector2(0, i * manaSpacingY);
                spriteBatch.Draw(manaTexture, pos, Color.White);
            }
        }
    }
    */
}