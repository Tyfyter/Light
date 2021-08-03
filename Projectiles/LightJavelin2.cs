using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace Light.Projectiles
{
    public class LightJavelin2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Light Javelin");
            DisplayName.AddTranslation(GameCulture.German, "Lichtspeer");
        }


        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 18;
            projectile.aiStyle = 1;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.tileCollide = true;
            projectile.scale = 1f;
            projectile.ownerHitCheck = false;
            projectile.melee = true;
            projectile.extraUpdates = 1;
        }
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection){
			if(target.modNPC != null && target.modNPC.mod.DisplayName.Contains("Calamity Mod")){
				damage += (int)(target.lifeMax * 0.002);
			}
		}

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            LightPlayer modPlayer = player.GetModPlayer<LightPlayer>();
            projectile.rotation = projectile.velocity.ToRotation() + 0.785f;
			Color color = modPlayer.lightColor;
            //red | green| blue
            Lighting.AddLight(projectile.Center, color.R/255, color.G/255, color.B/255);

            if (Main.rand.Next(2) == 0)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 267, projectile.velocity.X * 0.33f, projectile.velocity.Y * 0.33f, 100, color, 0.75f);
				Main.dust[dust].noGravity = true;
            }

        }

	    public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
            Player player = Main.player[projectile.owner];
            LightPlayer modPlayer = player.GetModPlayer<LightPlayer>();
			//Redraw the projectile with the color not influenced by light
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			Color color = modPlayer.lightColor;
			/*for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				//Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}//*/
			spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.position, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			return true;
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit){
            target.immune[0] = 0;
        }
    }

}