using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace Light.Projectiles
{
    public class LightStaff : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Light Staff");
        }


        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 18;
            projectile.aiStyle = 19;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.scale = 1f;
            projectile.hide = true;
            projectile.ownerHitCheck = true;
            projectile.melee = true;
            projectile.alpha = 0;
        }
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection){
			if(target.modNPC != null && target.modNPC.mod.DisplayName.Contains("Calamity Mod")){
				damage += (int)(target.lifeMax * 0.025);
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

		public override bool OnTileCollide(Vector2 oldVelocity){
            Player player = Main.player[projectile.owner];
            LightPlayer modPlayer = player.GetModPlayer<LightPlayer>();
            float speedDiv = 1.75f;
            if(oldVelocity.X < -0.5){
                player.velocity.X = (float)Math.Max(player.velocity.X, -Math.Max(oldVelocity.X*movementFactor/speedDiv, -24));
                /*if(modPlayer.phasing || !Collision.CanHit(player.position, player.width, player.height, player.position-new Vector2((oldVelocity.X*movementFactor)-Math.Max(oldVelocity.X*movementFactor/speedDiv, -24), 0), player.width, player.height)){
                    player.position.X = player.position.X-((oldVelocity.X*movementFactor)-Math.Max(oldVelocity.X*movementFactor/speedDiv, -24));
                }//*/
                if(modPlayer.phasing)player.position.X = player.position.X-((oldVelocity.X*movementFactor)-Math.Max(oldVelocity.X*movementFactor/speedDiv, -24));
            }else if(oldVelocity.X > 0.5){
                player.velocity.X = (float)Math.Min(player.velocity.X, -Math.Min(oldVelocity.X*movementFactor/speedDiv, 24));
                /*if(modPlayer.phasing || !Collision.CanHit(player.position, player.width, player.height, player.position-new Vector2((oldVelocity.X*movementFactor)-Math.Min(oldVelocity.X*movementFactor/speedDiv, 24), 0), player.width, player.height)){
                    player.position.X = player.position.X-((oldVelocity.X*movementFactor)-Math.Min(oldVelocity.X*movementFactor/speedDiv, 24));
                }//*/
                if(modPlayer.phasing)player.position.X = player.position.X-((oldVelocity.X*movementFactor)-Math.Min(oldVelocity.X*movementFactor/speedDiv, 24));
            }
            if(oldVelocity.Y < -0.5){
                player.velocity.Y = (float)Math.Max(player.velocity.Y, -Math.Max(oldVelocity.Y*movementFactor/speedDiv, -24));
                /*if(modPlayer.phasing || !Collision.CanHit(player.position, player.width, player.height, player.position-new Vector2(0, (oldVelocity.Y*movementFactor)-Math.Max(oldVelocity.Y*movementFactor/speedDiv, -24)), player.width, player.height)){
                    player.position.Y = player.position.Y-((oldVelocity.Y*movementFactor)-Math.Min(oldVelocity.Y*movementFactor/speedDiv, 24));
                }//*/
                if(modPlayer.phasing)player.position.Y = player.position.Y-((oldVelocity.Y*movementFactor)-Math.Max(oldVelocity.Y*movementFactor/speedDiv, -24));
            }else if(oldVelocity.Y > 0.5){
                player.velocity.Y = (float)Math.Min(player.velocity.Y, -Math.Min(oldVelocity.Y*movementFactor/speedDiv, 24));
                /*if(modPlayer.phasing || !Collision.CanHit(player.position, player.width, player.height, player.position-new Vector2(0, (oldVelocity.Y*movementFactor)-Math.Min(oldVelocity.Y*movementFactor/speedDiv, 24)), player.width, player.height)){
                    player.position.Y = player.position.Y-((oldVelocity.Y*movementFactor)-Math.Min(oldVelocity.Y*movementFactor/speedDiv, 24));
                }//*/
                if(modPlayer.phasing)player.position.Y = player.position.Y-((oldVelocity.Y*movementFactor)-Math.Min(oldVelocity.Y*movementFactor/speedDiv, 24));
            }
            projectile.velocity = oldVelocity;
            player.wingTime = player.wingTimeMax;
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit){
            Player player = Main.player[projectile.owner];
            LightPlayer modPlayer = player.GetModPlayer<LightPlayer>();
            float speedDiv = 1.75f;
            Vector2 targvel = target.velocity;
            Vector2 playervel = player.velocity;
            if(projectile.velocity.X < -0.5){
                target.velocity.X = (float)Math.Max(Main.player[projectile.owner].velocity.X, Math.Max((projectile.velocity.X*movementFactor/speedDiv), -24)*Math.Max(target.knockBackResist, 0));
                Main.player[projectile.owner].velocity.X = (float)Math.Max(Main.player[projectile.owner].velocity.X, -Math.Max((projectile.velocity.X*movementFactor/speedDiv), -24)*Math.Max(1-target.knockBackResist, 0));
            }else if(projectile.velocity.X > 0.5){
                target.velocity.X = (float)Math.Min(Main.player[projectile.owner].velocity.X, Math.Min((projectile.velocity.X*movementFactor/speedDiv), 24)*Math.Max(target.knockBackResist, 0));
                Main.player[projectile.owner].velocity.X = (float)Math.Min(Main.player[projectile.owner].velocity.X, -Math.Min((projectile.velocity.X*movementFactor/speedDiv), 24)*Math.Max(1-target.knockBackResist, 0));
            }
            if(projectile.velocity.Y < -0.5){
                target.velocity.Y = (float)Math.Max(Main.player[projectile.owner].velocity.Y, Math.Max((projectile.velocity.Y*movementFactor/speedDiv), -24)*Math.Max(target.knockBackResist, 0));
                Main.player[projectile.owner].velocity.Y = (float)Math.Max(Main.player[projectile.owner].velocity.Y, -Math.Max((projectile.velocity.Y*movementFactor/speedDiv), -24)*Math.Max(1-target.knockBackResist, 0));
            }else if(projectile.velocity.Y > 0.5){
                target.velocity.Y = (float)Math.Min(Main.player[projectile.owner].velocity.Y, Math.Min((projectile.velocity.Y*movementFactor/speedDiv), 24)*Math.Max(target.knockBackResist, 0));
                Main.player[projectile.owner].velocity.Y = (float)Math.Min(Main.player[projectile.owner].velocity.Y, -Math.Min((projectile.velocity.Y*movementFactor/speedDiv), 24)*Math.Max(1-target.knockBackResist, 0));
            }
            player.chatOverhead.NewMessage(target.knockBackResist+"", 60);
            Main.player[projectile.owner].wingTime = Main.player[projectile.owner].wingTimeMax;
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