using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Light.Tools;
using static Terraria.ModLoader.ModContent;
using Light.Projectiles;

namespace Light.Items
{
	public class Soul_Light_Javelin : Light_Javelin
	{
		int charge = 0;
		int[] charges = new int[7];
		int maxcharge = 100;
		int range = 100;
		public int type = 0;
        //public static short customGlowMask = 0;
		private static string[] TypeNames = new string[7];
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul Boosted Light Javelin");
			Tooltip.SetDefault(@"Right click to throw a volley of javelins
			Right click in your invetory to dispel
			DisplayCharge2
			DisplayCharge1");
            customGlowMask = Light.SetStaticDefaultsGlowMask(this);
			TypeNames[0] = "";
			TypeNames[1] = "";
			TypeNames[2] = "";
			TypeNames[3] = "Draconian";
			TypeNames[4] = "";
			TypeNames[5] = "";
			TypeNames[6] = "";
			//TypeNames[7] = "";
            //RegisterLightItem();
		}
		public override void SetDefaults()
		{
			item.damage = 225;
            item.useStyle = 5;
            item.shootSpeed = 2;
            item.width = 90;
            item.height = 90;
            item.scale = 1f;
            item.noMelee = true;
            item.noUseGraphic = true;
			item.melee = true;
			item.useTime = 20;
			item.useAnimation = 20;
			item.knockBack = 6;
			item.value = 0;
			item.rare = 5;
            item.shoot = ProjectileType<LightJavelin>();
			item.UseSound = SoundID.Item1;
			item.autoReuse = false;
			item.shopCustomPrice = 45;
		}

		public override TagCompound Save()
		{

			return new TagCompound {
				{"charge",charge},
				{"charges",charges},
				{"type", type}
			};
		}

		public override void Load(TagCompound tag)
		{
			charge = tag.GetInt("charge");
			charges = tag.GetIntArray("charges");
			type = tag.GetInt("type");
		}

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(tooltips);
            for (int i = 0; i < tooltips.Count; i++){
                if(tooltips[i].text.Contains("Light Javelin")){
                    if(type>0) {
					    TooltipLine tip = tooltips[i];
                        tip.text = TypeNames[type - 1] + " " + tip.text;
					    tip.overrideColor = LightConfig.Instance.LightColor;
                    }
                    break;
				}
            }
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }


        public override bool CanUseItem(Player player)
        {
            LightPlayer modPlayer = player.GetModPlayer<LightPlayer>();
			if(player.altFunctionUse == 2){
				item.useTime = 5;
				item.useAnimation = 35;
				item.shootSpeed = 12.5f;
				item.reuseDelay = 20;
				item.shoot = ProjectileType<LightJavelin2>();
				item.autoReuse = false;
			}else if(player.altFunctionUse == 0 && player.itemAnimation == 0){
				item.useTime = 20;
				item.useAnimation = 20;
				item.shootSpeed = 2.5f;
				item.reuseDelay = 0;
				item.shoot = ProjectileType<LightJavelin>();
				item.autoReuse = true;
			}
            return base.CanUseItem(player);
        }
		public override void UpdateInventory(Player player){
			item.shopCustomPrice = 45+charge;
			item.damage = 225+charge;
		}
		public override void HoldItem (Player player)
		{
            LightPlayer modPlayer = player.GetModPlayer<LightPlayer>();
			item.damage = 225;
            item.holdStyle = 0;
			if (player.altFunctionUse == 0 && player.itemAnimation == 0)     //2 is right click
            {
				item.useTime = 20;
				item.useAnimation = 20;
			}else{
				item.damage = 45+(charge/5);
			}
			item.noUseGraphic = true;
			if(charge/maxcharge >= 0.75){
				range = (int)(640+(16*(charge-(maxcharge*0.75))));
			}
			if(modPlayer.Ulting && modPlayer.UltCD <= 0){
				for(int i2 = 0; i2 < Main.npc.Length; i2++){
					NPC target2 = Main.npc[i2];
					if(target2.Distance(player.Center) < range && !((target2.friendly || target2.damage == 0) || target2.immortal || !target2.chaseable) && target2.active){
						//int a = Projectile.NewProjectile(target2.position.X, target2.position.Y, 0, -5, ProjectileType<RadialJavelin>(), item.damage*(6+(0.6*(charge-(maxcharge*0.75)))), 0, player.whoAmI);
						target2.velocity = new Vector2(0, 0);
						int a = Projectile.NewProjectile((float)target2.Center.X, (float)target2.Center.Y+150, 0, -5, ProjectileType<RadialJavelin>(), (int)(item.damage*((0.006*(charge-(maxcharge*0.75))))), 0, player.whoAmI, i2);
						modPlayer.UltCD = (int)Math.Min(modPlayer.UltCD+(600.0f-modPlayer.UltCD)/5,600);
					}
				}
				if(modPlayer.UltCD > 0){
                	Main.NewText("Ultimate now on cooldown for "+Math.Round((float)modPlayer.UltCD/60, 1)+" seconds.");
				}
				modPlayer.Ulting = false;
			}else if(modPlayer.Ulting && modPlayer.UltCD > 0){
                Main.NewText("Ultimate on cooldown, "+Math.Round((float)modPlayer.UltCD/60, 1)+" seconds left.");
			}
		}
		public override void RightClick (Player player){
            LightPlayer modPlayer = player.GetModPlayer<LightPlayer>();
			//modPlayer.PointsInUse = Math.Max(modPlayer.PointsInUse-4, 0);
			//Item.NewItem((int)player.position.X, (int)player.position.Y, player.width, player.height, ItemType<LightI>(), (int)item.shopCustomPrice);
			Item.NewItem((int)player.position.X, (int)player.position.Y, player.width, player.height, ItemID.SoulofLight, charges[0]);
			Item.NewItem((int)player.position.X, (int)player.position.Y, player.width, player.height, ItemID.SoulofNight, charges[1]);
			Item.NewItem((int)player.position.X, (int)player.position.Y, player.width, player.height, ItemID.SoulofFright, charges[2]);
			Item.NewItem((int)player.position.X, (int)player.position.Y, player.width, player.height, ItemID.SoulofMight, charges[3]);
			Item.NewItem((int)player.position.X, (int)player.position.Y, player.width, player.height, ItemID.SoulofSight, charges[4]);
			Item.NewItem((int)player.position.X, (int)player.position.Y, player.width, player.height, ItemID.SoulofFlight, charges[5]);
			Item.NewItem((int)player.position.X, (int)player.position.Y, player.width, player.height, ItemType<SoulOfInosite>(), charges[6]);
		}

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            LightPlayer modPlayer = player.GetModPlayer<LightPlayer>();
			Color color = modPlayer.lightColor;
            Vector2 velocity;
            if (player.altFunctionUse == 2 && player.itemAnimation < 25) //Right click
            {
				Main.PlaySound(SoundID.Item1, (int)player.position.X, (int)player.position.Y);
				//Main.PlaySound(32, (int)player.position.X, (int)player.position.Y, 21, 5f);
				//Main.PlaySound(13, (int)player.position.X, (int)player.position.Y, 1);
				//Main.PlaySound(42, (int)player.position.X, (int)player.position.Y, 63);
				for (int i = 0; i < 20; i++) {
                     //Random rotation, random distance from the player
					position = player.MountedCenter + (Main.rand.NextVector2CircularEdge(1, 1) * Main.rand.NextFloat(10, 50));

					if (Collision.CanHit(player.MountedCenter, 18, 18, position, 0, 0)) break;
				}
                //velocity = new Vector2(player.direction * item.shootSpeed, 0); //Straight in the direction the player is facing
                velocity =  Vector2.Lerp(Main.MouseWorld - player.Center, Main.MouseWorld - position, 0.5f).OfLength(item.shootSpeed+1); //Direction is a middlepoint between straight from the player to the cursor and straight from the sword to the cursor
				Projectile.NewProjectile(position, velocity, type, damage, knockBack, player.whoAmI);
				for (int i = 0; i < 7; i++)
				{
					int dust = Dust.NewDust(position-new Vector2(4, 8), 8, 16, 267, 0f, 0f, 0, color, 1f);
					Main.dust[dust].noGravity = true;
				}
				return false; //So it doesn't shoot normally
            }
			return true;
        }
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale){

			Texture2D texture = Main.itemTexture[item.type];
			//Texture2D texture = mod.GetTexture("Items/"+TypeNames[type-1]+(this.GetType().Name.Remove(0, 4)));
			Main.spriteBatch.Draw(Main.itemTexture[item.type], new Vector2(item.position.X - Main.screenPosition.X + item.width * 0.5f, item.position.Y - Main.screenPosition.Y + item.height - texture.Height * 0.5f + 2f), new Rectangle(0, 0, texture.Width, texture.Height), Color.White, 0, texture.Size() * 0.5f,scale, SpriteEffects.None, 0f);
			return false;
		}
	}
}
