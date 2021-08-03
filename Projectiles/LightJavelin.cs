using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace Light.Projectiles
{
    public class LightJavelin : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Light Spear");
            DisplayName.AddTranslation(GameCulture.German, "Lichtspeer");
        }


        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 18;
            projectile.aiStyle = 19;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.scale = 1f;
            projectile.hide = true;
            projectile.ownerHitCheck = true;
            projectile.melee = true;
            projectile.alpha = 0;
        }
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection){
			if(target.modNPC != null && target.modNPC.mod.DisplayName.Contains("Calamity Mod")){
				damage += (int)(target.lifeMax * 0.02);
			}
		}

        public float movementFactor
        {
            get { return projectile.ai[0]; }
            set { projectile.ai[0] = value; }
        }

        public override void AI()
        {
            Player projOwner = Main.player[projectile.owner];
            Player player = Main.player[projectile.owner];
            LightPlayer modPlayer = player.GetModPlayer<LightPlayer>();
			Color color = modPlayer.lightColor;
            //red | green| blue
            Lighting.AddLight(projectile.Center, color.R/255, color.G/255, color.B/255);  //this defines the projectile light color
            Vector2 ownerMountedCenter = projOwner.RotatedRelativePoint(projOwner.MountedCenter, true);
            projectile.direction = projOwner.direction;
            projOwner.heldProj = projectile.whoAmI;
            projOwner.itemTime = projOwner.itemAnimation;
            projectile.position.X = ownerMountedCenter.X - (float)(projectile.width / 2);
            projectile.position.Y = ownerMountedCenter.Y - (float)(projectile.height / 2);
            if (!projOwner.frozen)
            {
                if (movementFactor == 0f)
                {
                    movementFactor = 3f;
                    projectile.netUpdate = true;
                }
                if (projOwner.itemAnimation < projOwner.itemAnimationMax / 3.1)
                {
                    movementFactor -= 2.4f;
                }
                else
                {
                    movementFactor += 2.1f;
                }
            }

            projectile.position += projectile.velocity * movementFactor;

            /*if (projOwner.itemAnimation == 0)
            {
                projectile.Kill();
            }*/

			if (projOwner.itemAnimation <= 1)
            {
                projectile.Kill();
            }

            if (Main.rand.Next(2) == 0)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 267, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 100, color, 0.75f);
				Main.dust[dust].noGravity = true;
                //or 20 instead of 15
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
    }
}