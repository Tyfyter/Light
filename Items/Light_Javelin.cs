using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Light.Tools;
using static Terraria.ModLoader.ModContent;
using Light.Projectiles;

namespace Light.Items
{
	public class Light_Javelin : ModItem
	{
		int charge = 0;
		int maxcharge = 100;
		int range = 100;
		int type;
		int PointsUsed = 4;
        public static short customGlowMask = 0;
		public override bool CloneNewInstances{
			get { return true; }
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Light Javelin");
			Tooltip.SetDefault(@"Right click to throw a volley of javelins 
			Right click in your invetory to dispel
			DisplayCharge2
			DisplayCharge1");
            customGlowMask = Light.SetStaticDefaultsGlowMask(this);
		}
		public override void SetDefaults()
		{
			item.damage = 125;
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
			item.shopSpecialCurrency = Light.LightCurrencyID;
		}
		
		public override TagCompound Save()
		{
			return new TagCompound {
				{"charge",charge}
			};
		}
		
		public override void Load(TagCompound tag)
		{
			charge = tag.GetInt("charge");
		}
		public override bool NewPreReforge(){
			Main.player[item.owner].GetModPlayer<LightPlayer>().tempcharge = charge;
			return true;
		}
		public override void PostReforge(){
			charge = Main.player[item.owner].GetModPlayer<LightPlayer>().tempcharge;
			Main.player[item.owner].GetModPlayer<LightPlayer>().tempcharge = 0;
		}
		
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.player[item.owner];
            LightPlayer modPlayer = player.GetModPlayer<LightPlayer>();
            for (int i = 0; i < tooltips.Count; i++)
            {
                if (tooltips[i].text.Contains("DisplayCharge2"))
                {
                    TooltipLine tip;
					//tooltips[i].text.Substring(8, tooltips[i].text.Length-8);
                    tip = new TooltipLine(mod, "DisplayCharge2",
					"current charge level: " + charge);
                    //tip.overrideColor = new Color(255, 32, 174, 200);
					tip.overrideColor = modPlayer.LightColor;
                    tooltips.RemoveAt(i);
                    tooltips.Insert(i, tip);
                }else if (tooltips[i].text.Contains("current charge level"))
                {
                    TooltipLine tip;
					//tooltips[i].text.Substring(8, tooltips[i].text.Length-8);
                    tip = new TooltipLine(mod, "DisplayCharge2",
					"current charge level: " + charge);
                    //tip.overrideColor = new Color(255, 32, 174, 200);
					tip.overrideColor = modPlayer.LightColor;
                    tooltips.RemoveAt(i);
                    tooltips.Insert(i, tip);
                }else if (tooltips[i].text.Contains("melee"))
                {
					String[] SplitText = tooltips[i].text.Split();
                    TooltipLine tip;
					//tooltips[i].text.Substring(8, tooltips[i].text.Length-8);
                    tip = new TooltipLine(mod, "melee",
                        SplitText[0]+" light damage");
                    //tip.overrideColor = new Color(255, 32, 174, 200);
					tip.overrideColor = modPlayer.LightColor;
                    tooltips.RemoveAt(i);
                    tooltips.Insert(i, tip);
                }else if(tooltips[i].text.Contains("Light Javelin")){
					TooltipLine tip;
					tip = new TooltipLine(mod, "", tooltips[i].text);
					tip.overrideColor = modPlayer.LightColor;
                    tooltips.RemoveAt(i);
                    tooltips.Insert(i, tip);
				}else if(tooltips[i].text.Contains("Rainbow Javelin")){
					item.rare = -2;
					item.expert = true;
				}else if (tooltips[i].text.Contains("DisplayCharge1"))
                {
                    TooltipLine tip;
					//tooltips[i].text.Substring(8, tooltips[i].text.Length-8);
                    tip = new TooltipLine(mod, "DisplayCharge1",
                        "Hold " +Light.ChargeKey+" to charge.");
                    //tip.overrideColor = new Color(255, 32, 174, 200);
					tip.overrideColor = modPlayer.LightColor;
                    tooltips.RemoveAt(i);
					if(charge < maxcharge){
                    	tooltips.Insert(i, tip);
					}
                }/*else if (tooltips[i].text.Contains("*"))
                {
					String[] SplitText = tooltips[i].text.Split("*");
                    TooltipLine tip;
					//tooltips[i].text.Substring(8, tooltips[i].text.Length-8);
                    tip = new TooltipLine(mod, "throwing",
                        SplitText[SplitText.Length-1]+" light damage");
                    //tip.overrideColor = new Color(255, 32, 174, 200);
					tip.overrideColor = modPlayer.LightColor;
                    tooltips.RemoveAt(i);
                    tooltips.Insert(i, tip);
				}//*/
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
			/*if(player.altFunctionUse == 2){
				item.channel = true;
				if(knives < maxknives){
					item.useStyle = 3;
					knives++;
					Dust.NewDust(player.Center, 1, 1, 226, 0, 0, 0, modPlayer.LightColor, 1.3f);
					item.shoot = 0;
				}
				if(knives >= maxknives){
					for(int i = 0; i >= knives; i++){
						Dust.NewDust(player.Center, 1, 1, 226, 0, 0, 0, modPlayer.LightColor, 1.3f);
					}
				}
			}
            /*if (modPlayer.channeling > 0)
            {  
				item.shoot = 0;
				item.useStyle = 4;
				item.noUseGraphic = false;
				item.useTime = 10;
				item.useAnimation = 10;
				if(charge < maxcharge && base.CanUseItem(player)){
					for (int j = 0; j < player.inventory.Length; j++)
					{
						if (player.inventory[j].type == ItemType<Light>())
						{
							player.inventory[j].stack--;
							charge++;
							item.damage = 50+charge;
							return true;
						}
					}
				}else{
					return false;
				}
            }else{
				item.damage = 50+charge;
            }*/
            return base.CanUseItem(player);
        }
		public override void UpdateInventory(Player player){
			//(int)(Math.Min(charge,maxcharge*0.75f)+(3*Math.Max(0,charge-(maxcharge*0.75f)))) multiplies any charge by 3 past 75% charge to match light item usage
			item.shopCustomPrice = 45+(int)(Math.Min(charge,maxcharge*0.75f)+(3*Math.Max(0,charge-(maxcharge*0.75f))));
			if(player.HeldItem.type!=item.type)item.damage = 125+charge;
		}
		public override void HoldItem (Player player)
		{
            LightPlayer modPlayer = player.GetModPlayer<LightPlayer>();
            /*
			if (player.altFunctionUse == 2)     //2 is right click
            {
				item.useTime = 5;
				item.useAnimation = 30;
				if(player.itemAnimation >= 25){
					//item.reuseDelay = 8;
				}
            }else if(player.itemAnimation == 0){
				item.useTime = 20;
				item.useAnimation = 20;
			}//*/
			item.damage = 125+charge;
            item.holdStyle = 0;
			if (player.altFunctionUse == 0 && player.itemAnimation == 0)     //2 is right click
            {
				item.useTime = 20;
				item.useAnimation = 20;
			}else{
				item.damage = 25+(charge/5);
			}
			item.noUseGraphic = true;
			if(charge/maxcharge >= 0.75){
				range = (int)(240+(16*(charge-(maxcharge*0.75))));
			}
			/*if(player.altFunctionUse == 2 && modPlayer.channeling <= 0){
			}else if(!player.channel && knives > 0 && modPlayer.channeling <= 0){
				//knives--;
				item.useTime = 1;
				item.useAnimation = 1;
				player.controlUseItem = true;
				item.shoot = ProjectileType<LightDagger>();
				//Main.NewText(knives);
			}else{//*/
			//}
            if (modPlayer.channeling > 0)
            {
                item.holdStyle = 2;
				item.noUseGraphic = false;
				Color color = modPlayer.LightColor;
				//red | green| blue
				Lighting.AddLight(player.Center, color.R/255, color.G/255, color.B/255);  //this defines the projectile light color
				if(charge < maxcharge && base.CanUseItem(player)){
					for (int j = 0; j < player.inventory.Length; j++)
					{
						if (player.inventory[j].type == ItemType<LightI>() && charge < maxcharge*0.75)
						{
							player.inventory[j].stack--;
							charge++;
							item.damage = 125+charge;
						}
						if (player.inventory[j].type == ItemType<LightI>() && charge >= maxcharge*0.75 && player.inventory[j].stack >= 3)
						{
							player.inventory[j].stack -= 3;
							charge++;
							item.damage = 125+charge;
						}
					}
				}
            }
			if (modPlayer.ascendtype != 0){
				ascend(modPlayer.ascendtype);
			}
			if(modPlayer.Ulting && modPlayer.UltCD <= 0 && charge/maxcharge >= 0.75){
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
			/*if (modPlayer.channeling <= 0)
            {
				item.shoot = ProjectileType<LightDagger>();
				item.useStyle = 4;
				item.noUseGraphic = true;
				item.useTime = 20;
				item.useAnimation = 20;
			}
			
            // From lunar emblems
            if (modPlayer.channeling > 0)
            {
                if (player.itemAnimation == 0)
                {
					player.controlUseItem = true;
					item.autoReuse = true;
                }else{
                    player.releaseUseItem = true;
				}
            }*/
		}
		public override bool CanRightClick(){
			return true;
		}
		public override void RightClick (Player player){
            LightPlayer modPlayer = player.GetModPlayer<LightPlayer>();
			modPlayer.PointsInUse = Math.Max(modPlayer.PointsInUse-PointsUsed, 0);
			Item.NewItem((int)player.position.X, (int)player.position.Y, player.width, player.height, ItemType<LightI>(), (int)item.shopCustomPrice);
		}
		
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            LightPlayer modPlayer = player.GetModPlayer<LightPlayer>();
			Color color = modPlayer.LightColor;
            Vector2 velocity;
            if (player.altFunctionUse == 2 && player.itemAnimation < 25) //Right click
            {
				Main.PlaySound(SoundID.Item1, (int)player.position.X, (int)player.position.Y);
				//Main.PlaySound(32, (int)player.position.X, (int)player.position.Y, 21, 5f);
				//Main.PlaySound(13, (int)player.position.X, (int)player.position.Y, 1);
				//Main.PlaySound(42, (int)player.position.X, (int)player.position.Y, 63);
				for (int i = 0; i < 20; i++) //Makes 20 attempts at finding a projectile position that the player can reach. Gives up otherwise.
				{
					position = player.MountedCenter + (RandomFloat(FullCircle/-2).ToRotationVector2().OfLength(RandomInt(10, 50))); //Random rotation, random distance from the player

					if (Collision.CanHit(player.MountedCenter, 18, 18, position, 0, 0)) break;
				}
                //velocity = new Vector2(player.direction * item.shootSpeed, 0); //Straight in the direction the player is facing
                velocity =  Vector2.Lerp(Main.MouseWorld - player.Center, Main.MouseWorld - position, 0.5f).OfLength(item.shootSpeed+1); //Direction is a middlepoint between straight from the player to the cursor and straight from the sword to the cursor
				Projectile.NewProjectile(position, velocity, type, damage, knockBack, player.whoAmI);
				for (int i = 0; i < 7; i++) //Makes 20 attempts at finding a projectile position that the player can reach. Gives up otherwise.
				{
					int dust = Dust.NewDust(position-new Vector2(4, 8), 8, 16, 267, 0f, 0f, 0, color, 1f);
					Main.dust[dust].noGravity = true;
				}
				return false; //So it doesn't shoot normally
            }
			return true;
        }
		public void ascend(int newtype){
			charge = 0;
			item.type = mod.GetItem("Soul_Light_Javelin").item.type;
			type = newtype;
		}
	}
}
