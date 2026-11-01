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
            Projectile.timeLeft = 90; 
            Projectile.tileCollide = true;
            //Projectile.damage = 0;
            // Projectile.rotation = 10;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return true;
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

            if (Projectile.ai[0] == 60)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X, Projectile.position.Y, 0f, 0f, ModContent.ProjectileType<Star_Catcher_Projectile_Thrown>(), (int)(Projectile.damage * 0.6f), 0f, Main.myPlayer);
            }

                
            
            
        }
    }
}