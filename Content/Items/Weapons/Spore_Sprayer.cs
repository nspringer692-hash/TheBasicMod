using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheBasicMod.Projectiles;

namespace TheBasicMod.Content.Items.Weapons
{
    public class Spore_Sprayer : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 6;
            Item.DamageType = DamageClass.Magic;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 4;
            Item.useAnimation = 4;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 6;
            Item.value = Item.buyPrice(silver: 1);
            Item.shoot = ModContent.ProjectileType<Mush_Spore>();
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shootSpeed = 1f;
            Item.mana = 1;
            Item.scale = 1.2f;
            Item.ArmorPenetration = 10; // Set the armor penetration value to 20
            Item.noMelee = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DirtBlock, 10);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
    