using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;
using Light.Projectiles;

namespace Light.Items
{
	public class Light_Staff : ModItem
	{
		int charge = 0;
		int maxcharge = 25;
		int PointsUsed = 2;
        public static short customGlowMask = 0;
		public override bool CloneNewInstances{
			get { return true; }
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Beam of Light");
			Tooltip.SetDefault(@"Right click in your invetory to dispel
DisplayCharge2
DisplayCharge1");
            customGlowMask = Light.SetStaticDefaultsGlowMask(this);
		}
		public override void SetDefaults()
		{
			item.damage = 50;
			item.melee = true;
			item.width = 40;
			item.height = 40;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = 1;
            item.shootSpeed = 3;
			item.knockBack = 6;
			item.value = 0;
			item.rare = 2;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.shopCustomPrice = 15;
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
            LightPlayer modPlayer = player?.GetModPlayer<LightPlayer>();
            for (int i = 0; i < tooltips.Count; i++)
            {
                if (tooltips[i].text.Contains("DisplayCharge2"))
                {
                    TooltipLine tip;
					//tooltips[i].text.Substring(8, tooltips[i].text.Length-8);
                    tip = new TooltipLine(mod, "DisplayCharge2",
					"current charge level: " + charge);
                    //tip.overrideColor = new Color(255, 32, 174, 200);
					tip.overrideColor = modPlayer?.LightColor;
                    tooltips.RemoveAt(i);
                    tooltips.Insert(i, tip);
                }else if (tooltips[i].text.Contains("current charge level"))
                {
                    TooltipLine tip;
					//tooltips[i].text.Substring(8, tooltips[i].text.Length-8);
                    tip = new TooltipLine(mod, "DisplayCharge2",
					"current charge level: " + charge);
                    //tip.overrideColor = new Color(255, 32, 174, 200);
					tip.overrideColor = modPlayer?.LightColor;
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
					tip.overrideColor = modPlayer?.LightColor;
                    tooltips.RemoveAt(i);
                    tooltips.Insert(i, tip);
                }else if(tooltips[i].text.Contains("Light Staff")){
					TooltipLine tip;
					tip = new TooltipLine(mod, "", tooltips[i].text);
					tip.overrideColor = modPlayer?.LightColor;
                    tooltips.RemoveAt(i);
                    tooltips.Insert(i, tip);
				}else if(tooltips[i].text.Contains("Rainbow Staff")){
					item.rare = -2;
					item.expert = true;
				}else if (tooltips[i].text.Contains("DisplayCharge1"))
                {
                    TooltipLine tip;
					//tooltips[i].text.Substring(8, tooltips[i].text.Length-8);
                    tip = new TooltipLine(mod, "DisplayCharge1",
                        "Hold " +Light.ChargeKey+" to charge.");
                    //tip.overrideColor = new Color(255, 32, 174, 200);
					tip.overrideColor = modPlayer?.LightColor;
                    tooltips.RemoveAt(i);
					//if(charge < maxcharge){
                    	tooltips.Insert(i, tip);
					//}
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
				item.useStyle = 5;
            	item.noMelee = true;
            	item.noUseGraphic = true;
				item.useTime = 15;
				item.useAnimation = 15;
				item.shoot = ProjectileType<LightStaff>();
			}else if(player.altFunctionUse == 0 && player.itemAnimation == 0){
				item.useStyle = 1;
            	item.noMelee = false;
            	item.noUseGraphic = false;
				item.useTime = 20;
				item.useAnimation = 20;
				item.shoot = 0;
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
			item.shopCustomPrice = 15+charge;
			if(player.HeldItem.type!=item.type)item.damage = 50+charge;
		}
		public override void HoldItem (Player player)
		{
            LightPlayer modPlayer = player.GetModPlayer<LightPlayer>();
			item.damage = 50+charge;
            item.holdStyle = 0;
			item.useTime = 20;
			item.useAnimation = 20;
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
						if (player.inventory[j].type == ItemType<LightI>())
						{
							player.inventory[j].stack--;
							charge++;
							item.damage = 50+charge;
						}
					}
				}
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
	}
}
