using Terraria;
using Terraria.ID;
using Terraria.Enums;
using Terraria.ModLoader;
using Revelations.Content.Tiles.Banners;

namespace Revelations.Content.Items.Banners
{
    public class DragonSkullBanner : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
			Item.DefaultToPlaceableTile(ModContent.TileType<EnemyBanner>(), (int)EnemyBanner.StyleID.DragonSkull);
			Item.width = 10;
			Item.height = 24;
			Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(silver: 10));
		}
    }
}