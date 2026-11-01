using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheBasicMod.Projectiles;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

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

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    	{
        	// Define whatever custom damage you want your projectile to have here (e.g., 40 damage)
        	int customProjectileDamage = 40; 

        	// Manually spawn the projectile with your custom damage instead of the item's 0 damage
        	Projectile.NewProjectile(source, position, velocity, type, customProjectileDamage, knockback, player.whoAmI);

        	// Return false so Terraria doesn't also try to spawn a default 0-damage projectile
        	return false;
    	}
    }
}