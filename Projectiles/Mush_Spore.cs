using Terraria;
using System;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;

namespace TheBasicMod.Projectiles
{
    public class Mush_Spore : ModProjectile
    {  
        public override void SetDefaults()
        {
            Projectile.width = 15;
            Projectile.height = 15;
            Projectile.friendly = true;
            Projectile.aiStyle = 0;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 5;
            Projectile.light = 0.2f;
            Projectile.timeLeft = 35; 
            Projectile.tileCollide = false;
            Projectile.scale = 2f;
            Projectile.Opacity = 0.65f;
           
        }


       // For increasing the hitbox size as the actual projectile increases in size
       // Will cycle along each frame with AI().
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {

            int newWidth = (int)(Projectile.width * Projectile.scale);
            int newHeight = (int)(Projectile.height * Projectile.scale);
            hitbox.X = (int)Projectile.Center.X - (newWidth / 2);
            hitbox.Y = (int)Projectile.Center.Y - (newWidth / 2);
            hitbox.Width = newWidth;
            hitbox.Height = newHeight;
            
            
        }
        
        // AI will run each tick, and is used to determine the behavior of the projectile.
        public override void AI()
        {
            // Starting velocity
            if (Projectile.ai[0] == 0)
            {
                // Get the mouse position and the player's position for the projectile's direction
                Vector2 mousePosition = Main.MouseWorld;
                Vector2 playerPosition = Main.player[Projectile.owner].Center;
                Vector2 direction = mousePosition - playerPosition;
                direction.Normalize();
                Projectile.velocity = direction * 32f;
                Projectile.position = playerPosition - new Vector2(10, 10); // Set the projectile's position to the player's position

            }

            

            Projectile.velocity *= 0.875f; // Slow down the projectile over time
            Projectile.rotation += 0.25f; // Rotate the projectile
            Projectile.scale *= 1.03f; // Increase the projectile over time
            

            Projectile.ai[0] += 1f;
            Projectile.Opacity -= 0.015f;
            if (Projectile.ai[0] < 35f && Projectile.ai[0] % 2 == 0)
            {
                // Color color = new Color(6, 69, 0);
                // int dust = Dust.NewDust(Projectile.position, Projectile.width * (2/3), Projectile.height * (2/3), 121, 0f, 0f, 0, color, 1.5f);
                // Main.dust[dust].velocity = Vector2.Zero; // Make the dust stationary
                // Main.dust[dust].noGravity = true; // Make the dust ignore gravity
            }

        }
    }
}