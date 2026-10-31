using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBasicMod.Content.Items.Armor.Basic_Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class Test_Helmet : ModItem
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
            Item.defense = 5; // The amount of defense the helmet will give when equipped
        }
    }
}
    // This is a basic item template.
    // Please see tModLoader's ExampleMod for every other example:
    //d