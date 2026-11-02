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
            Projectile.penetrate = 10000;
            Projectile.light = 0.5f;
            Projectile.timeLeft = 360; 
            Projectile.tileCollide = false;
            Projectile.rotation = 10;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10; 
            //Projectile.damage = (int)(Projectile.damage * 0.6f); // Reduce damage by 40%
        }

         


        // the enemy that the projectile is currently targeting
        //int hitsLeft = 7; // Number of hits left before the projectile is destroyed
        NPC target = null;
        bool hitTarget = false; // Track if the projectile has hit the target
        float currSpeed = 20;
        int numHitsDone = 0; // Number of hits done to the target
        

        private NPC findClosest(NPC[] npcs)
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
                        // using the distance formula to find the closest dummy entity to the projectile base's position.
                        float distanceDummy = Vector2.Distance(npc.Center, new Vector2(Projectile.ai[1], Projectile.ai[2]));
                        if (distanceDummy < closestDummyDistance)
                        {
                            closestDummyDistance = distanceDummy;
                            closestDummy = npc;
                        }
                        continue;
                    }
                    float distance = Vector2.Distance(npc.Center,  new Vector2(Projectile.ai[1], Projectile.ai[2]));
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
        public override bool? CanHitNPC(NPC target)
        {
            if (target != null && target.active)
            {
                if (target.whoAmI == this.target?.whoAmI)
                {
                    return true; // Can hit the targeted NPC
                }
            }
            
            return false;
        }  
        private void calculateNextSpeed(NPC target, float speed, int numHitsDone)
        {
            
            if (target != null)
            {
                // the direction moves the projectile towards and the circle vector loops the projectile across the target.
                Vector2 direction = target.Center - Projectile.Center;
                direction.Normalize();
                float currentRadius = speed * 2.75f;
                Vector2 circle = new Vector2(-direction.Y, direction.X) * currentRadius * 0.4f;
                Projectile.velocity = (direction * speed) + circle;

                
            }
        }

        private void hitThisFrame(NPC contact)
        {
            if (contact != null && Projectile.Hitbox.Intersects(contact.Hitbox))
            {
                hitTarget = true;
                
            }
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (Projectile.ai[0] == 0) 
            {
                target = findClosest(Main.npc);
            }
            if (target == null || !target.active)
            {
                Projectile.Kill();
                return;
            }
            if (Projectile.ai[0] == 0) 
            {
                
                if (target.Center.X < player.Center.X)
                {
                    player.direction = -1;
                    Projectile.velocity.X = -20f; // Move left
                }
                else
                {
                    player.direction = 1;
                    Projectile.velocity.X = 20f; // Move right
                }
            }
            

            if (target.friendly || !target.active || target.dontTakeDamage || (currSpeed <= 1.5f && hitTarget) || currSpeed <= 0.5f)
            {
                Projectile.Kill();
                return;
            }
            hitThisFrame(target);
            if (hitTarget)
            {
                hitTarget = false;
                
                float randX = Main.rand.Next(200, 250);
                float randY = Main.rand.Next(200, 250);

                int switcherX = Main.rand.Next(0, 2);
                int switcherY = Main.rand.Next(0, 2);

                if (switcherX == 1) randX *= -1;
                if (switcherY == 1) randY *= -1;

                Projectile.Center = new Vector2(target.Center.X + randX, target.Center.Y + randY);
                Projectile.velocity.X = 0;
                Projectile.velocity.Y += 0;
                numHitsDone++;
            } else
            {
                calculateNextSpeed(target, currSpeed, numHitsDone);
                currSpeed *= 0.99f; // Losing speed over time.
            }
            // Get the player's facing direction
            // Additional AI logic for the thrown projectile can be added here if needed.

            Projectile.ai[0] += 1;
        }
    }
}