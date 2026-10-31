using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBasicMod.DamageChanges
{
    public class Mush_Spores_Change : GlobalNPC
    {
        public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
        {
            Player player = Main.player[Main.myPlayer];
            Item item = player.HeldItem;

            if (item.type == ModContent.ItemType<Content.Items.Weapons.Spore_Sprayer>() && (npc.type == 13 || npc.type == 14 || npc.type == 267)) // Check if the player is using the Spore Sprayer and the target is Eye of Cthulhu
            {
                modifiers.ArmorPenetration += -8;
                
            }
        }
    }
}