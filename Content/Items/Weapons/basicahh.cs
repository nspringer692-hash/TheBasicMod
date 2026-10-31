using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheBasicMod.Projectiles;

namespace TheBasicMod.Content.Items.Weapons
{
	// This is a basic item template.
	// Please see tModLoader's ExampleMod for every other example:
	// https://github.com/tModLoader/tModLoader/tree/stable/ExampleMod
	public class basicahh : ModItem
	{
		// The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.TheBasicMod.hjson' file.
		// SetDefaults determines the properties that the item has, whenever a change occurs, this confirms the change
		// If you used a constructor, the memory usage would crash the game all the time.
		public override void SetDefaults()
		{
			// Note: You aren't exactly calling on an uninitialized class, but you already
			// have a reference to the class, so you can use it to set the properties of the item.
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
			Item.shoot = ModContent.ProjectileType<basicahhprojectile>();
			Item.shootSpeed = 10f;
			Item.mana = 5;
			Item.scale = 1.2f;
		}

        // Since every function inside the BasicAhh class is contained inside ModItem, you must use override
        // And its functionality is to be done when the item is held by the player.
        public override void UpdateInventory(Player player)
        {
			// HeldItem is the current item the player is holding, and Item is the item that is being held.
			// Using UpdateInventory, you can change the player's stats when the item is held, instead of using
			// HoldItem, which is used when the item is being held in the hand. This is useful for items that are not weapons, but still have an effect on the player.	
           if (player.HeldItem == Item)
			{
				player.statManaMax2 += 100; // <-- current mana increase (statMaxMana2)
			}
        }

		// AddRecipes initializes the recipe and makes a recipe object.
		// It then determines what the place you make it is and what it requires.
		// It then registers the recipe to be used in the game.
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.DirtBlock, 10);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}
