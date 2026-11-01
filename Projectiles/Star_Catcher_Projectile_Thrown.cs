using Terraria;
using System;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.UI;
using System.Linq;

namespace TheBasicMod.Projectiles
{
    public class Star_Catcher_Projectile_Thrown : Star_Catcher_Projectile_Base
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.aiStyle = 0;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 3;
            Projectile.light = 0.5f;
            Projectile.timeLeft = 90; 
            Projectile.tileCollide = true;
            //Projectile.damage = (int)(Projectile.damage * 0.6f); // Reduce damage by 40%
        }

        public override bool? CanHitNPC(NPC target)
        {
            return true;
        }   


        // the enemy that the projectile is currently targeting
        NPC target = null;

        private NPC findClosest(NPC[] npcs, Player player)
        {
            // tracking the nearest enemy/npc with dummy entities coming after the real enemies
           NPC closest = null;
           NPC closestDummy = null;
            float closestDistance = float.MaxValue;
            float closestDummyDistance = float.MaxValue;
            // iterating through the enities array until the closest calculated enemy is found.
            foreach (NPC npc in npcs)
            {
                if (npc.active && !npc.friendly && !npc.dontTakeDamage)
                {
                    if (npc.type == NPCID.TargetDummy)
                    {
                        float distanceDummy = Vector2.Distance(npc.Center, player.Center);
                        if (distanceDummy < closestDummyDistance)
                        {
                            closestDummyDistance = distanceDummy;
                            closestDummy = npc;
                        }
                        continue;
                    }
                    float distance = Vector2.Distance(npc.Center, player.Center);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closest = npc;
                    }
                }
            }
            if (closest != null) {
                return closest;
            } 
            return closestDummy;
        }

        private void calculateNextSpeed(NPC target, float speed)
        {
            if (target != null)
            {
                Vector2 direction = target.Center - Projectile.Center;
                direction.Normalize();
                Projectile.velocity = direction * speed;
            }
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (Projectile.ai[0] == 0) 
            {
                target = findClosest(Main.npc, Main.player[Projectile.owner]);
                if (target.Center.X < player.Center.X)
                {
                    player.direction = -1;
                    Projectile.velocity.X = -10f; // Move left
                }
                else
                {
                    player.direction = 1;
                    Projectile.velocity.X = 5f; // Move right
                }
            }

            if (target.friendly || !target.active || target.dontTakeDamage || Projectile.ai[0] > 90)
            {
                Projectile.Kill();
            } else
            {
                calculateNextSpeed(target, 10f);
            }
            // Get the player's facing direction
            // Additional AI logic for the thrown projectile can be added here if needed.

            Projectile.ai[0] += 1;
        }
    }
}