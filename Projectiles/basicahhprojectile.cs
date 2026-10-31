using Terraria;
using System;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace TheBasicMod.Projectiles
{
    public class basicahhprojectile : ModProjectile
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
            Projectile.timeLeft = 300; 
            Projectile.tileCollide = true;
           
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damage)
        {
            target.AddBuff(BuffID.Oiled, 300); // <-- This will set the NPC to be slimed for 5 seconds (300 ticks)
            target.AddBuff(BuffID.Ichor, 300); // <-- This will set the NPC to be ichored for 5 seconds (300 ticks)
        }
        
        // For determining the direction the player is facing, and then storing it in a dictionary for later use.
        Dictionary<int, bool> facings = new Dictionary<int, bool>();
        // AI will run each tick, and is used to determine the behavior of the projectile.
        // 60 times a second.
        public override void AI()
        {

            // Starting velocity
            if (Projectile.ai[0] == 0)
            {
                Vector2 mousePosition = Main.MouseWorld;
                // the array below is the players, with size 256 and the index is the player who shot the projectile, which is stored in Projectile.owner
                Vector2 playerPosition = Main.player[Projectile.owner].Center;
                Vector2 direction = mousePosition - playerPosition;
                direction.Normalize();
                Projectile.velocity = direction * 18f;

            }

            // Determines the player's direction for later
            Player player = Main.player[Projectile.owner];
            // The basic trail, using dust to make no custom texture, but a dirt texture.
            Projectile.velocity.Y += 0.2f; // Gravity
            Color color = new Color(26, 255, 0);
            // create a dust particle every tick.
            int curr_dust1 = Dust.NewDust(Projectile.position, Projectile.width + 30, Projectile.height + 30, 110, 0f, 0f, 0, color, 2f);
            Main.dust[curr_dust1].noGravity = true;
            Main.dust[curr_dust1].velocity *= 1.0f;
            Main.dust[curr_dust1].velocity.Y += 0.2f; // Gravity
            Main.dust[curr_dust1].scale = (float)Main.rand.Next(70, 100) * 0.013f;

            // This is used to determine the direction the player is facing, and then store it in a dictionary for later use.
            // This is so that the projectile can spawn other projectiles in the direction the player is facing, even if the player turns around after firing the projectile.
            if (!facings.ContainsKey(Projectile.whoAmI))
            {
                // Only add the projectile to the dictionary if it doesn't already exist, so that it doesn't get overwritten by the player turning around.
                facings.Add(Projectile.whoAmI, player.direction == 1 ? true : false);
            }

            Projectile.ai[0] += 1f;
            // ^ This is the timer for the projectile, which is used to determine when to spawn new projectiles. It is incremented by 1 each tick, and is used in the if statement below to determine when to spawn new projectiles.
            Random random = new Random();
            int rand30 = random.Next(20, 23); // <-- This will make the projectile spawn a new projectile every 20-22 ticks, which is about 1/3 of a second.
            if (Projectile.ai[0] % rand30 == 0)
            {
                // The below code will make a new projectile from the first projectile a couple times.
                int curr_proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X, Projectile.position.Y, 0f, 0f, 95, (int)(Projectile.damage * 0.6f), 0f, Main.myPlayer);
                // Projectile's actual value.
                int current = Projectile.whoAmI;
                Main.projectile[curr_proj].timeLeft = 120;
                Main.projectile[curr_proj].scale = 0.75f;
                if (Main.projectile.IndexInRange(curr_proj))
                {
                    // Checking the way the player is facing, and then setting the velocity of the projectile to be random in that direction.
                    if (facings[current])
                    {
                        Main.projectile[curr_proj].velocity.X = Main.rand.Next(30, 70) * 0.1f;
                        Main.projectile[curr_proj].velocity.Y = Main.rand.Next(50, 100) * 0.1f;
                    } else
                    {
                        Main.projectile[curr_proj].velocity.X = Main.rand.Next(-70, -30) * 0.1f;
                        Main.projectile[curr_proj].velocity.Y = Main.rand.Next(50, 100) * 0.1f;
                    }
                    
                }
                
            }
            
        }
    }
}