using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;

namespace Light.Items
{
	public class Light_Drill : ModItem
	{
		int charge = 0;
		int maxcharge = 10;
		int PointsUsed = 1;
		public override bool CloneNewInstances{
			get { return true; }
		}
        public static short customGlowMask = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Light Drill");
			Tooltip.SetDefault(@"Right click in inventory to dispel
DisplayCharge2
DisplayCharge1");
            customGlowMask = Light.SetStaticDefaultsGlowMask(this);
		}
		public override void SetDefaults()
		{
			item.damage = 5;
			item.melee = true;
			item.noMelee = false;
			item.noUseGraphic = true;
			item.width = 40;
			item.height = 40;
			item.useTime = 10;
			item.useAnimation = 10;
			item.pick = 40;
			item.useStyle = 3;
			item.knockBack = 6;
			item.value = 0;
			item.rare = 2;
			item.UseSound = SoundID.Item9;
			item.autoReuse = true;
			item.useTurn = true;
			item.shopCustomPrice = 10;
			item.shopSpecialCurrency = Light.LightCurrencyID;
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
                }else if(tooltips[i].text.Contains("Light Drill")){
					TooltipLine tip;
					tip = new TooltipLine(mod, "", tooltips[i].text);
					tip.overrideColor = modPlayer.LightColor;
                    tooltips.RemoveAt(i);
                    tooltips.Insert(i, tip);
				}else if(tooltips[i].text.Contains("Rainbow Drill")){
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
					//if(charge < maxcharge){
                    	tooltips.Insert(i, tip);
					//}
                }
            }
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

		public override void UseItemHitbox(Player player, ref Rectangle hitbox, ref bool noHitbox){
			hitbox.Width = 16;
			hitbox.Height = 16;
			hitbox.X = (int)(new Vector2(Main.mouseX, Main.mouseY)+player.position-new Vector2(Main.screenWidth/2, Main.screenHeight/2)).X;
			hitbox.Y = (int)(new Vector2(Main.mouseX, Main.mouseY)+player.position-new Vector2(Main.screenWidth/2, Main.screenHeight/2)).Y+12;
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
            LightPlayer modPlayer = player.GetModPlayer<LightPlayer>();
			int dust = Dust.NewDust(hitbox.Center(), 0, 0, /*DustID.Vortex*/267, 0, 0, 25, modPlayer.LightColor);
			Main.dust[dust].noGravity = true;
		}
		public override void UpdateInventory(Player player){
			item.shopCustomPrice = 10+charge;
			if(player.HeldItem.type!=item.type)item.pick = 40+charge;
		}
		public override void HoldItem (Player player)
		{
            LightPlayer modPlayer = player.GetModPlayer<LightPlayer>();
            item.holdStyle = 0;
			item.pick = 40+charge;
			item.damage = 5+(int)((float)charge/10);
			item.useTime = 10;
			item.useAnimation = 10;
			maxcharge = 10;
			if(NPC.downedBoss1){
				item.useTime -= 1;
				item.useAnimation -= 1;
				maxcharge += 5;
			}
			if(NPC.downedBoss3){
				item.useTime -= 1;
				item.useAnimation -= 1;
			}
			if(NPC.downedQueenBee){
				maxcharge += 2;
			}
			if(NPC.downedSlimeKing){
				item.useTime -= 1;
				item.useAnimation -= 1;
				maxcharge += 2;
			}
			if(NPC.downedBoss2){
				maxcharge += 10;
			}
			if(Main.hardMode){
				maxcharge += 60;
				if(Main.expertMode){
					maxcharge += 21;
				}
			}
			if(NPC.downedMechBoss1){
				maxcharge += 25;
			}
			if(NPC.downedMechBoss2){
				maxcharge += 25;
			}
			if(NPC.downedMechBoss3){
				maxcharge += 25;
			}
			if(NPC.downedGolemBoss){
				maxcharge += 50;
			}else{
				maxcharge = Math.Min(maxcharge, 169);
			}
			
            if (modPlayer.channeling > 0)
            {
                item.holdStyle = 1;
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
							//item.damage = 50+charge;
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