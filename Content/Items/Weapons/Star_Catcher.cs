using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheBasicMod.Projectiles;

namespace TheBasicMod.Content.Items.Weapons
{
    public class Star_Catcher : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 20;
			Item.DamageType = DamageClass.Magic;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 6;
			Item.value = Item.buyPrice(silver: 1);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shootSpeed = 10f;
            Item.shoot = ModContent.ProjectileType<Star_Catcher_Projectile_Base>();
			Item.mana = 5;
			Item.scale = 1.2f;
        }
    }
}