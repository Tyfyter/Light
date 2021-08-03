using System;
using Light.Buffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Light.Projectiles
{
    public class LightDagger : ModProjectile
    {
        public override void SetDefaults()
        {
            //projectile.name = "Light Dagger";//Name of the projectile, only shows this if you get killed by it
            projectile.width = 8;  //Set the hitbox width
            projectile.height = 8;  //Set the hitbox height
            projectile.aiStyle = 0; //How the projectile works
            projectile.friendly = true;  //Tells the game whether it is friendly to players/friendly npcs or not
            projectile.hostile = false; //Tells the game whether it is hostile to players or not
            projectile.tileCollide = true; //Tells the game whether or not it can collide with a tile
            projectile.ignoreWater = true; //Tells the game whether or not projectile will be affected by water
            projectile.ranged = true;   //Tells the game whether it is a ranged projectile or not
            projectile.penetrate = -1; //Tells the game how many enemies it can hit before being destroyed
            projectile.timeLeft = 400; //The amount of time the projectile is alive for
            projectile.extraUpdates = 1;
            projectile.light = 0.25f; //This defines the projectile light
            //aiType = 0; // this is the projectile ai style . 1 is for arrows style
        }
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Light Dagger");
		}
		public Color GetColor()
		{
            Player player = Main.player[projectile.owner];
            LightPlayer modPlayer = player.GetModPlayer<LightPlayer>();
			return modPlayer.lightColor;
		}
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            LightPlayer modPlayer = player.GetModPlayer<LightPlayer>();
            projectile.rotation = projectile.velocity.ToRotation() + 0.875f;
			Color color = modPlayer.lightColor;
			/*if(ModLoader.GetMod("CustomizerMod") != null){
				if(ModLoader.GetMod("CustomizerMod").mod.ammoShaders[projectile.owner] == GameShaders.Armor.GetShaderIdFromItemId(3556)){

				}
			}*/
            //red | green| blue
            Lighting.AddLight(projectile.Center, color.R/255, color.G/255, color.B/255);  //this defines the projectile light color
            //int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Fire);   //this adds a vanilla terraria dust to the projectile
            int dust2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, /*DustID.AncientLight*/267, 0f, 0f, 0, color);
            //Main.dust[dust].velocity /= 30f;  //this modify the velocity of the first dust
            Main.dust[dust2].velocity /= 30f; //this modify the velocity of dust2
            //Main.dust[dust].scale = 1f;  //this modify the scale of the first dust
            Main.dust[dust2].scale = 1f; //this modify the scale of the dust2
			Main.dust[dust2].noGravity = true;
			if(projectile.ai[0] != 0 && projectile.ai[0] != -1 && Main.npc[(int)projectile.ai[0]].life > 0){
				NPC target2 = Main.npc[(int)projectile.ai[0]];
				Vector2 newvel = new Vector2(target2.Center.X - projectile.position.X, target2.Center.Y - projectile.position.Y);
				newvel.Normalize();
				newvel *= projectile.velocity.Length();
				projectile.velocity = newvel;
			}
        }

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection){
			if(target.modNPC != null && target.modNPC.mod.DisplayName.Contains("Calamity Mod")){
				damage += (int)(target.lifeMax * 0.002);
			}
		}

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) //When you hit an NPC
        {
            Player player = Main.player[projectile.owner];
            LightPlayer modPlayer = player.GetModPlayer<LightPlayer>();
			projectile.damage += 5;
			float distancetotarget2 = 640;
			float maxdistance = 640;
            for (int i2 = 0; i2 < Main.npc.Length; i2++)
            {
                NPC target2 = Main.npc[i2];
                if (projectile.ai[0] != -1 && ((target2.Distance(projectile.Center) < distancetotarget2)||(target2.Distance(projectile.Center) < maxdistance && target2.type != 488))/*Math.Abs(Math.Sqrt(Math.Pow(target2.position.X + projectile.position.X, 2) + Math.Pow(target2.position.Y + projectile.position.Y, 2))/100) <= distancetotarget2*/ && !(target2.type == NPCID.Bunny || target2.type == NPCID.BunnySlimed || target2.type == NPCID.BunnyXmas || target2.type == NPCID.GoldBunny || target2.type == NPCID.PartyBunny || target2.type == NPCID.CorruptBunny || target2.type == NPCID.CrimsonBunny || target2.type == 0) && !target2.friendly && target2 != target && !target2.boss)
                {
					//target2.position = projectile.position;
					/*{
						player.position = target2.position;
					}//*/
					distancetotarget2 = (float)Math.Sqrt(Math.Pow(target2.position.X + projectile.position.X, 2) + Math.Pow(target2.position.Y + projectile.position.Y, 2))/100;
					//Main.NewText(Math.Sqrt(Math.Pow(target2.position.X + projectile.position.X, 2) + Math.Pow(target2.position.Y + projectile.position.Y, 2))/100+"");
					Vector2 newvel = new Vector2(target2.Center.X - projectile.position.X, target2.Center.Y - projectile.position.Y);
					newvel.Normalize();
					newvel *= projectile.velocity.Length();
					projectile.velocity = newvel;
					projectile.ai[0] = i2;
                }
            }
			projectile.timeLeft += (int)(distancetotarget2/projectile.velocity.Length());
			if(ModLoader.GetMod("CalamityMod") != null){
				//see anything fammiliar, Drew?
				//this is probably really generic code And I just never knew what to do to get info from other mods, so maybe it doesn't really seem too familliar
				if(target.type == ModLoader.GetMod("CalamityMod").NPCType("DevourerofGodsBody") || target.type == ModLoader.GetMod("CalamityMod").NPCType("DevourerofGodsHead") || target.type == ModLoader.GetMod("CalamityMod").NPCType("DevourerofGodsTail")){
					NPC npc = target;
					npc.damage = 0;
					npc.defense = 0;
					npc.behindTiles = false;
					npc.noGravity = false;
					npc.noTileCollide = false;
					projectile.penetrate = -2;
					target.AddBuff(BuffType<DisabledDebuff>(), 600);
					target.scale = (float)Math.Max(target.scale-0.0025f, 0.99f);

					if(target.type != ModLoader.GetMod("CalamityMod").NPCType("DevourerofGodsHead")){
						NPC target2 = Main.npc[(int)npc.ai[1]];
						Vector2 newvel = new Vector2(target2.Center.X - projectile.position.X, target2.Center.Y - projectile.position.Y);
						newvel.Normalize();
						newvel *= projectile.velocity.Length();
						projectile.velocity = newvel;
						projectile.ai[0] = target2.whoAmI;
						projectile.tileCollide = false;
						Color color = modPlayer.lightColor;
						int dust2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 226, 0f, 0f, 0, color);
						Main.dust[dust2].velocity /= 10f;
						Main.dust[dust2].scale = 1f;
					}else{
						target.life = Math.Max(target.life-50000,1);
						npc.HitEffect(0, 50000.0);
						Color color = modPlayer.lightColor;
						for(int i = 0; i < 10; i++){
						Dust.NewDust(projectile.position, projectile.width, projectile.height, 226, 0f, 0f, 0, color);
						}
						projectile.Kill();
					}
				}
				//if(target.type == ModLoader.GetMod("CalamityMod").NPCType("DevourerofGodsBody") || target.type == ModLoader.GetMod("CalamityMod").NPCType("DevourerofGodsHead") || target.type == ModLoader.GetMod("CalamityMod").NPCType("DevourerofGodsTail")){}
			}
			if(target.noTileCollide){
				projectile.tileCollide = false;
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

        //After the projectile is dead
        public override void Kill(int timeLeft)
        {
            //Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 0, 0, ProjectileID.DeathSickle, (int)(projectile.damage * 1.5), projectile.knockBack, Main.myPlayer); // This spawns a projectile after this projectile is dead
        }
		/*
			maybe you'll recognize this better...

			if (ModLoader.GetMod("ThoriumMod") != null)
			{
				if (npc.type == ModLoader.GetMod("ThoriumMod").NPCType("TheGrandThunderBird") ||
				    npc.type == ModLoader.GetMod("ThoriumMod").NPCType("Cacklor") ||
				    npc.type == ModLoader.GetMod("ThoriumMod").NPCType("QueenJelly") ||
					npc.type == ModLoader.GetMod("ThoriumMod").NPCType("QueenJellyDiverless") ||
					npc.type == ModLoader.GetMod("ThoriumMod").NPCType("GraniteEnergyStorm") ||
					npc.type == ModLoader.GetMod("ThoriumMod").NPCType("SlagFury") ||
					npc.type == ModLoader.GetMod("ThoriumMod").NPCType("Omnicide") ||
					npc.type == ModLoader.GetMod("ThoriumMod").NPCType("Aquaius") ||
					npc.type == ModLoader.GetMod("ThoriumMod").NPCType("Aquaius2") ||
					npc.type == ModLoader.GetMod("ThoriumMod").NPCType("ThePrimeScouter") ||
					npc.type == ModLoader.GetMod("ThoriumMod").NPCType("FallenDeathBeholder") ||
					npc.type == ModLoader.GetMod("ThoriumMod").NPCType("PhaseBeing") ||
					npc.type == ModLoader.GetMod("ThoriumMod").NPCType("Lich") ||
					npc.type == ModLoader.GetMod("ThoriumMod").NPCType("LichHeadless"))
				{
					npc.buffImmune[BuffType<GlacialState>()] = true;
					npc.buffImmune[BuffType<TemporalSadness>()] = true;
					npc.buffImmune[BuffType<ArmorCrunch>()] = true;
				}
			}
		*/
    }
}