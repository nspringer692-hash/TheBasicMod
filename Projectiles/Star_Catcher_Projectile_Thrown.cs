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
        NPC target = null; // The targeted enemy closest, mostly calculated by findClosest() function.
        // Projectile is destroyed killoing said enemy and the projectile will never switch targets.
        bool hitTarget = false; // Track if the projectile has hit the target this frame
        float currSpeed = 20; // The speed the projectile is traveling at (exponentially gets slower as it moves, but a factor 0.095 each frame).
        int numHitsDone = 0; // Number of hits done to the target
        int currTime = 0; // The current time/frame for AI() (could have used LocalAI[])
        bool dustInitialized = false; // Used to determine if dust has been initialized.
        bool hasHit = false; // Track if the projectile has hit the target at least once
        Vector2 dustLocation; // The locations of the dust particles spurt from the projectile (one inner and one outer).
        Vector2 dustLocation2;

        
        // for finding the closest enemy/dummy, enemies have a higher priority.
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
        // Projectiles are immediately centered to follow their velocity.
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
        // Testing whether the enemy is alive and if the enemy is locked. This determines that other enemies won't be killed by 
        // projectile.
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
        // Every frame this function will calculate the projectile's velocity within the circular motion, to confirm
        // the projectile is moving inwards.
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
        // Same use case as the previous but for dust particles instead.
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
        // Tracks whether the enemy was hit, so the projectile can be teleported a distance away to come in again.
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

            // After OnSpawn() this was put in, making the projectile visable. 
            if (Projectile.velocity != Vector2.Zero)
            {

                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.Pi;
                Projectile.alpha = 0;
            }

            // Half the produced projectiles were just cosmetic (.ai[0] meant damaging, .ai[1] meant cosmetic)
            // This check just creates that.
            if (Projectile.ai[0] == 1 && currTime == 0)
            {
                Projectile.friendly = false;
                Projectile.hostile = false;
                Projectile.damage = 0;
            }

            // Entire code block used for the first frame.
            if (currTime == 0) 
            {
                target = findClosest(Main.npc);

                // If enemy dies, kill the projectile.
                if (target == null)
                {
                    Projectile.Kill();
                    return;
                }

                float currX = Math.Abs(player.Center.X - target.Center.X);
                float currY = Math.Abs(player.Center.Y - target.Center.Y);
                // Projectiles aren't allowed to target far away enemies.
                if (currX > 1366f || currY > 768f)
                {
                    Projectile.Kill();
                    return;
                }

                // The initial velocity will be determined by the player's position and facing (may be redundant, needs to be tested).
                // if (target.Center.X < player.Center.X)
                // {
                //     player.direction = -1;
                //     Projectile.velocity.X = -20f; // Move left
                // }
                // else
                // {
                //     player.direction = 1;
                //     Projectile.velocity.X = 20f; // Move right
                // }
            }
            // Kill projectile is enemy is dead.
            if (target == null || !target.active)
            {
                Projectile.Kill();
                return;
            }
            if (hasHit) // Every 20 ticks after the first hit
            {
                // Dust only starts to spawns once the enemy has been hit at least once.
                if (!dustInitialized)
                {
                    // Only initialize things after the his has occured and only once.
                    float xPos1 = Main.rand.Next(-60, -30);
                    float yPos1 = Main.rand.Next(-60, -30);
                    float xPos2 = Main.rand.Next(20, 40);
                    float yPos2 = Main.rand.Next(-20, 40);
                    dustLocation = new Vector2(xPos1,yPos1);
                    dustLocation2 = new Vector2(xPos2,yPos2);
                    dustInitialized = true;
                }

                // Summon the dust particles and set their velocity vectors every frame.
                Vector2 spawnPosition1 = Projectile.Center + dustLocation.RotatedBy(Projectile.rotation);
                Vector2 spawnPosition2 = Projectile.Center + dustLocation2.RotatedBy(Projectile.rotation);

                int dust1 = Dust.NewDust(spawnPosition1, 0, 0, DustID.Venom, 0f, 0f, 100, new Color(220, 170, 20), 1.5f);
                int dust2 = Dust.NewDust(spawnPosition2, 0, 0, DustID.Venom, 0f, 0f, 100, new Color(220, 170, 20), 1.5f);

                Main.dust[dust1].noGravity = true;
                Main.dust[dust2].noGravity = true;

                calculateNextDustSpeed(target, currSpeed, Main.dust[dust1]);
                calculateNextDustSpeed(target, currSpeed, Main.dust[dust2]);
                

            }

            // Again, kill the projectile if the enemy is undamagable, passive, or if the projectile has slowed down enough
            if (target.friendly || !target.active || target.dontTakeDamage || (currSpeed <= 1.5f && hitTarget) || currSpeed <= 1f)
            {
                Projectile.Kill();
                return;
            }
            hitThisFrame(target);

            // If the enemy was just hit, calculated from above (hitThisFrame()).
            if (hitTarget)
            {
                hitTarget = false;
                
                float randX = Main.rand.Next(200, 250); // <-- Here until
                float randY = Main.rand.Next(200, 250);

                int switcherX = Main.rand.Next(0, 2);
                int switcherY = Main.rand.Next(0, 2);

                if (switcherX == 1) randX *= -1;
                if (switcherY == 1) randY *= -1; // <-- here calcuated the new location of the projectile once the enemy is hit.

                Projectile.Center = new Vector2(target.Center.X + randX, target.Center.Y + randY);
                // Set the new velocity of the projectile
                calculateNextSpeed(target, currSpeed, numHitsDone);
                numHitsDone++;

                // Once the projectile is moved, the velocity has changed, so the direction the projectile is facing most 
                // move with it (so it doesn't just look jittery first frame it is moved.)
                if (target != null)
                {
                    Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.Pi;
                }
                Projectile.alpha = 0;
            } else
            {
                // Just the general use where the projectile follows the set path, with its velocity slowing down
                // each frame.
                calculateNextSpeed(target, currSpeed, numHitsDone);
                currSpeed *= 0.995f; 
            }
             
            // Set the new frame
            currTime++;
        }
    }
}