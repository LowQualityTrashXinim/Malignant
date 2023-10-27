using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheSkeletronMod.Common.Globals;

namespace Malignant.Content.Items.Holy.HolyGreatsword
{
    public class HolyGreatsword : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Holy Greatsword");
            //Tooltip.SetDefault("This sword swords"); //10/10 tooltip
        }

        public override void SetDefaults()
        {
            Item.damage = 16;
            Item.DamageType = DamageClass.Melee;
            Item.width = Item.height = 90;
            Item.useTime = 50;
            Item.useAnimation = 50;
            Item.knockBack = 4;
            Item.value = 10000;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;

            Item.value = Item.sellPrice(0, 2, 50, 0);
            Item.rare = ItemRarityID.LightPurple;

            if (Item.TryGetGlobalItem(out ImprovedSwingSword meleeItem))
            {
                meleeItem.ArrayOfAttack =
                    new CustomAttack[]
                    {
                        new CustomAttack(CustomUseStyle.PokeAttack, true),
                        new CustomAttack(CustomUseStyle.PokeAttack, false)
                    };
                meleeItem.ItemSwingDegree = 150;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe(1)
                .AddIngredient(ItemID.GoldBar, 15)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
