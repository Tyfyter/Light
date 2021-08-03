using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using static Terraria.ModLoader.ModContent;
using Light.Buffs;

namespace Light.Projectiles
{
    public class RadialJavelin : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Radial Light Javelin");
            DisplayName.AddTranslation(GameCulture.German, "Lichtspeer");
        }


        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 18;
            projectile.aiStyle = 0;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 300; //The amount of time the projectile is alive for
            projectile.tileCollide = false;
            projectile.scale = 1f;
            projectile.ownerHitCheck = false;
            projectile.melee = true;
			projectile.ignoreWater = false;
			projectile.extraUpdates = 1;
            projectile.restrikeDelay = 10;
            projectile.localNPCHitCooldown = 7;
            projectile.usesLocalNPCImmunity = true;
        }
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection){
			if(target.modNPC != null && target.modNPC.mod.DisplayName.Contains("Calamity Mod")){
				damage += (int)(target.lifeMax * 0.01);
			}
		}

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            LightPlayer modPlayer = player.GetModPlayer<LightPlayer>();
            //projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 0.985f;
			projectile.rotation = -0.8f;
			Color color = modPlayer.lightColor;
            //red | green| blue
            Lighting.AddLight(projectile.Center, color.R/255, color.G/255, color.B/255);  //this defines the projectile light color
            /*if(projectile.ai[0] != (int)Math.Floor(projectile.ai[0])){
                Main.npc[(int)Math.Floor(projectile.ai[0])].position = projectile.Center - new Vector2(Main.npc[(int)projectile.ai[0]].width/2, Main.npc[(int)projectile.ai[0]].height/2);
            }*/
            if(projectile.ai[1] > 0.1f && projectile.ai[1] < 0.7f){
                Main.npc[(int)Math.Floor(projectile.ai[0])].position = projectile.Center - new Vector2(Main.npc[(int)projectile.ai[0]].width/2, (Main.npc[(int)projectile.ai[0]].height/4)*3);
            }
            if(!Main.npc[(int)projectile.ai[0]].active) projectile.Kill();
            if (Main.rand.Next(2) == 0)
            {
                int dust = Dust.NewDust(projectile.Center, projectile.width, projectile.height, 267, projectile.velocity.X * 0.33f, projectile.velocity.Y * 0.33f, 100, color, 0.75f);
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
            /*if(projectile.ai[0] >= (int)projectile.ai[0]){
                projectile.ai[0] += 0.1f;
            }
            if(projectile.ai[0] <= (int)projectile.ai[0]+0.2f){
                projectile.velocity = new Vector2();
            }*/
            if(projectile.ai[1] < 0.7f){
                projectile.ai[1] += 0.1f;
                target.velocity = projectile.velocity;
                if(projectile.ai[1] == 0.1f){
                    projectile.position = target.Center - new Vector2(projectile.width/2, 0);
                }
            }
            if(projectile.ai[1] >= 0.1f && projectile.ai[1] < 0.7f){
                projectile.velocity = new Vector2();
                target.velocity = projectile.velocity;
            }
            if(projectile.ai[1] >= 0.7f){
                projectile.velocity = new Vector2(0, 25);
                target.AddBuff(BuffType<FallDamage>(), 60, true);
                target.velocity = projectile.velocity;
                projectile.Kill();
            }
        }
    }
}