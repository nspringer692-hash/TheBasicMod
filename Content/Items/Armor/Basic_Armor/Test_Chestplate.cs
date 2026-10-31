using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBasicMod.Content.Items.Armor.Basic_Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class Test_Chestplate : ModItem
    {
        public override void UpdateEquip(Player player)
        {
            // GetDamage(DamageClass.Magic) is a method that increases the player's magic damage by a certain percentage. In this case, it increases it by 10%.
            player.GetDamage(DamageClass.Magic) += 0.1f; // Increases magic damage by 10%
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(silver: 1);
            Item.rare = ItemRarityID.Blue;
            Item.defense = 5; // The amount of defense the chestplate will give when equipped
        }
    }
}