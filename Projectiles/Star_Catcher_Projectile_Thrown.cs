using Terraria;
using System;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.UI;
using System.Linq;
using Terraria.DataStructures;

namespace TheBasicMod.Projectiles
{
    public class Star_Catcher_Projectile_Thrown : Star_Catcher_Projectile_Base
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.aiStyle = 0;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 10000;
            Projectile.light = 0.5f;
            Projectile.timeLeft = 360; 
            Projectile.tileCollide = false;
            //Projectile.rotation = 10;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 8; 
            if (Projectile.ai[0] == 1)
            {
                Projectile.friendly = false; 
                Projectile.hostile = false;
                Projectile.damage = 0;
            } else
            {
                Projectile.friendly = true;
            }
            Projectile.alpha = 255;
            
            //Projectile.damage = (int)(Projectile.damage * 0.6f); // Reduce damage by 40%
        }

         


        // the enemy that the projectile is currently targeting
        //int hitsLeft = 7; // Number of hits left before the projectile is destroyed
        NPC target = null;
        bool hitTarget = false; // Track if the projectile has hit the target this frame
        float currSpeed = 20;
        int numHitsDone = 0; // Number of hits done to the target
        int currTime = 0;
        bool hasHit = false; // Track if the projectile has hit the target at least once
        Vector2 dustLocation;
        Vector2 dustLocation2;

        // short[] npcs = new short[]
        // {
        //     NPCID.BurningSphere, NPCID.VileSpitEaterOfWorlds, NPCID.VileSpit, NPCID.ChaosBall, NPCID.WaterSphere, NPCID.SpikeBall, NPCID.ChaosBallTim
        // };
        // bool hasAvoidences(NPC npc)
        // {   
        //     if (Array.IndexOf(npcs, npc.type) == -1)
        //     {
        //         return true;
        //     }
        //     return false;
        // }
        
        //for finding the closest enemy/dummy, enemies have a higher priority.
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
                if (npc.active && !npc.friendly && !npc.dontTakeDamage && !(npc.lifeMax < 5))
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
        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);

            // Find the target immediately upon spawning
             target = findClosest(Main.npc);

            if (target != null)
            {
            // Immediately face the target on spawn frame before the first render
            Projectile.rotation = (target.Center - Projectile.Center).ToRotation() + MathHelper.Pi;
            Projectile.alpha = 255;
            }
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
                //Vector2 direction = target.Center - Projectile.Center;
                Vector2 customOffset = new Vector2(-20f, 0f);
                Vector2 orbitCenter = target.Center + customOffset;
                Vector2 direction = orbitCenter - Projectile.Center;
                direction.Normalize();
                float currentRadius = speed * 2.75f;
                Vector2 circle = new Vector2(-direction.Y, direction.X) * currentRadius * 0.4f;
                Projectile.velocity = (direction * speed) + circle;

                
            }
        }
        public void calculateNextDustSpeed(NPC target, float speed, Dust dust)
        {
            if (target != null)
            {
                Vector2 direction = target.Center - Projectile.Center;
                direction.Normalize();
                float currentRadius = speed * 2.75f;
                Vector2 circle = new Vector2(-direction.Y, direction.X) * currentRadius * 0.4f;
                dust.velocity = (direction * speed) + circle;
            }
        }
        private void hitThisFrame(NPC contact)

        {
            if (contact != null && Projectile.Hitbox.Intersects(contact.Hitbox))
            {
                hitTarget = true;
                hasHit = true;
                
            }
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (Projectile.velocity != Vector2.Zero)
            {

                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.Pi;
                Projectile.alpha = 0;
            }
            if (Projectile.ai[0] == 1 && currTime == 0) // Run once on frame 0
            {
                Projectile.friendly = false;
                Projectile.hostile = false;
                Projectile.damage = 0;
            }

            if (currTime == 0) 
            {
                target = findClosest(Main.npc);
                if (target == null)
                {
                    Projectile.Kill();
                    return;
                }
                float currX = Math.Abs(player.Center.X - target.Center.X);
                float currY = Math.Abs(player.Center.Y - target.Center.Y);
                
                if (currX > 1366f || currY > 768f)
                {
                    Projectile.Kill();
                    return;
                }

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
            if (target == null || !target.active)
            {
                Projectile.Kill();
                return;
            }
            if (hasHit) // Every 20 ticks after the first hit
            {
                if (currTime == 0)
                {
                    float xPos1 = Main.rand.Next(-60, -30);
                    float yPos1 = Main.rand.Next(-60, -30);
                    dustLocation = new Vector2(xPos1,yPos1);
                    float xPos2 = Main.rand.Next(20, 40);
                    float yPos2 = Main.rand.Next(-20, 40);
                    dustLocation2 = new Vector2(xPos2,yPos2);
                }
                Vector2 spawnPosition1 = Projectile.Center + dustLocation.RotatedBy(Projectile.rotation);
                Vector2 spawnPosition2 = Projectile.Center + dustLocation2.RotatedBy(Projectile.rotation);
                int dust1 = Dust.NewDust(spawnPosition1, 0, 0, DustID.Venom, 0f, 0f, 100, new Color(220, 170, 20), 1.5f);
                int dust2 = Dust.NewDust(spawnPosition2, 0, 0, DustID.Venom, 0f, 0f, 100, new Color(220, 170, 20), 1.5f);
                Main.dust[dust1].noGravity = true;
                Main.dust[dust2].noGravity = true;
                calculateNextDustSpeed(target, currSpeed, Main.dust[dust1]);
                calculateNextDustSpeed(target, currSpeed, Main.dust[dust2]);
                

            }

            if (target.friendly || !target.active || target.dontTakeDamage || (currSpeed <= 1.5f && hitTarget) || currSpeed <= 1f)
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
                //setting the new location of the projectile.
                Projectile.Center = new Vector2(target.Center.X + randX, target.Center.Y + randY);
                calculateNextSpeed(target, currSpeed, numHitsDone);
                numHitsDone++;

                if (target != null)
                {
                    Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.Pi;
                }
                Projectile.alpha = 0;
            } else
            {
                calculateNextSpeed(target, currSpeed, numHitsDone);
                currSpeed *= 0.995f; // Losing speed over time.
            }
            // Get the player's facing direction
            // Additional AI logic for the thrown projectile can be added here if needed.

            currTime++;
        }
    }
}