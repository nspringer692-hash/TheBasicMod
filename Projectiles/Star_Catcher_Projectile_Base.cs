using Terraria;
using System;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;

namespace TheBasicMod.Projectiles
{
    public class Star_Catcher_Projectile_Base : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.aiStyle = 0;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 3;
            Projectile.light = 0.5f;
            Projectile.timeLeft = 100; 
            Projectile.tileCollide = true;
            //Projectile.damage = 0;
            // Projectile.rotation = 10;
        }
        NPC target = null;
        int currTime = 0;

        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }

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
        public override void AI()
        {
            if (Projectile.ai[0] == 0)
            {
                Projectile.velocity.Y = -10f;
                Projectile.velocity.X = 0f;
                Projectile.rotation += 15f;
                
            }
            Projectile.ai[0] += 1;
            Projectile.velocity.Y *= 0.92f;
            Projectile.rotation *= 0.92f;
            
            
            if (currTime == 0)
            {
                target = findClosest(Main.npc);
            }
            //top right
            // && target.position.X > Projectile.position.X && target.position.Y > Projectile.position.Y
            if (target != null)
            {
                float targetX = target.position.X;
                float targetY = target.position.Y;
                float projectileX = Projectile.position.X;
                float projectileY = Projectile.position.Y;
                float Xoffset = targetX - projectileX;
                float Yoffset = targetY - projectileY;

                Vector2 localOffset = new Vector2(0, 0f); 
                Vector2 finalPos = Projectile.Center + localOffset.RotatedBy(Projectile.rotation);

                //  int cycleTime = (int)Projectile.ai[0] % 20;
                // int subAi0 = (cycleTime == 0 || cycleTime == 10) ? 0 : 1;

                
                finalPos.X -= new Random().Next(0, 70);
                finalPos.Y += new Random().Next(-30, 70);
            
                if (Projectile.ai[0] >= 60 && Projectile.ai[0] % 20 == 0)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), finalPos.X, finalPos.Y, 0f, 0f, ModContent.ProjectileType<Star_Catcher_Projectile_Thrown>(), (int)(Projectile.damage * 0.6f), 0f, Main.myPlayer, ai0: 0, ai1: Projectile.position.X, ai2: Projectile.position.Y);
                }
                else if (Projectile.ai[0] >= 60 && (Projectile.ai[0] + 5) % 20 == 0)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), finalPos.X, finalPos.Y, 0f, 0f, ModContent.ProjectileType<Star_Catcher_Projectile_Thrown>(), (int)(Projectile.damage * 0.6f), 0f, Main.myPlayer, ai0: 1, ai1: Projectile.position.X, ai2: Projectile.position.Y);
                }
                else if (Projectile.ai[0] >= 60 && (Projectile.ai[0] + 10) % 20 == 0)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), finalPos.X, finalPos.Y, 0f, 0f, ModContent.ProjectileType<Star_Catcher_Projectile_Thrown>(), (int)(Projectile.damage * 0.6f), 0f, Main.myPlayer, ai0: 0, ai1: Projectile.position.X, ai2: Projectile.position.Y);
                }
                else if (Projectile.ai[0] >= 60 && (Projectile.ai[0] + 15) % 20 == 0)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), finalPos.X, finalPos.Y, 0f, 0f, ModContent.ProjectileType<Star_Catcher_Projectile_Thrown>(), (int)(Projectile.damage * 0.6f), 0f, Main.myPlayer, ai0: 1, ai1: Projectile.position.X, ai2: Projectile.position.Y);
                }
            }
            currTime++;

                
            
            
        }
    }
}