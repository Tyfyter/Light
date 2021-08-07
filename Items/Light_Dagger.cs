using System;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.GameInput;
using static Terraria.ModLoader.ModContent;
using Light.Projectiles;
using Light.Buffs;

namespace Light.Items
{
	public class Light_Dagger : LightItem
	{
		int maxcharge = 25;
		int knives = 0;
		int maxknives = 10;
		int duration = 300;
		int range = 240;
		bool stealthinit = false;
        public static short customGlowMask = 0;
		public static string AbilityName = "";
        public override bool CloneNewInstances => true;

        public override int PointsUsed => 4;

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Light Dagger");
			Tooltip.SetDefault(@"Right click in your invetory to dispel,
			DisplayCharge2
			DisplayCharge1");
            customGlowMask = Light.SetStaticDefaultsGlowMask(this);
            RegisterLightItem();
		}
		public override void SetDefaults()
		{
			item.damage = 50;
			item.thrown = true;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.width = 40;
			item.height = 40;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = 4;
			item.knockBack = 6;
			item.value = 0;
			item.rare = 2;
			item.UseSound = SoundID.Item1;
			item.autoReuse = false;
            item.shoot = ProjectileType<LightDagger>();
			item.shootSpeed = 12.5f;
            item.glowMask = customGlowMask;
			item.shopCustomPrice = 30;
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
                if ((tooltips[i].text.Contains("DisplayCharge2") || tooltips[i].Name == "DisplayCharge2") && modPlayer.LightStealthMax != 0)
                {
                    TooltipLine tip;
					//tooltips[i].text.Substring(8, tooltips[i].text.Length-8);
                    tip = new TooltipLine(mod, "DisplayCharge2",
					"current charge level: " + charge+";"+Math.Round((float)(modPlayer.LightStealth/modPlayer.LightStealthMax)*100)+";"+player.aggro);
                    //tip.overrideColor = new Color(255, 32, 174, 200);
					tip.overrideColor = modPlayer?.lightColor;
                    tooltips.RemoveAt(i);
                    tooltips.Insert(i, tip);
                }else if ((tooltips[i].text.Contains("DisplayCharge2") || tooltips[i].Name == "DisplayCharge2") && modPlayer.LightStealthMax == 0)
                {
                    TooltipLine tip;
					//tooltips[i].text.Substring(8, tooltips[i].text.Length-8);
                    tip = new TooltipLine(mod, "DisplayCharge2",
					"current charge level: " + charge);
                    //tip.overrideColor = new Color(255, 32, 174, 200);
					tip.overrideColor = modPlayer?.lightColor;
                    tooltips.RemoveAt(i);
                    tooltips.Insert(i, tip);
                }else if (tooltips[i].text.Contains("throwing"))
                {
					String[] SplitText = tooltips[i].text.Split();
                    TooltipLine tip;
					//tooltips[i].text.Substring(8, tooltips[i].text.Length-8);
                    tip = new TooltipLine(mod, "throwing",
                        SplitText[0]+" light damage");
                    //tip.overrideColor = new Color(255, 32, 174, 200);
					tip.overrideColor = modPlayer?.lightColor;
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
					tip.overrideColor = modPlayer?.lightColor;
                    tooltips.RemoveAt(i);
                    tooltips.Insert(i, tip);
                }else if(tooltips[i].text.Contains("Light Dagger")){
					TooltipLine tip;
					tip = new TooltipLine(mod, "", tooltips[i].text);
					tip.overrideColor = modPlayer?.lightColor;
                    tooltips.RemoveAt(i);
                    tooltips.Insert(i, tip);
				}else if(tooltips[i].text.Contains("Rainbow Dagger")){
					item.rare = -2;
					item.expert = true;
				}/*else if ((tooltips[i].text.Contains("DisplayCharge1") || tooltips[i].Name == "DisplayCharge1") && charge < maxcharge) {
                    TooltipLine tip;
					//tooltips[i].text.Substring(8, tooltips[i].text.Length-8);
                    tip = new TooltipLine(mod, "DisplayCharge1",
                        "Hold " +Light.ChargeKey+" to charge.");
                    //tip.overrideColor = new Color(255, 32, 174, 200);
					tip.overrideColor = modPlayer?.lightColor;
                    tooltips.RemoveAt(i);
					if(charge < maxcharge-1){
                    	tooltips.Insert(i, tip);
					}
                }else if ((tooltips[i].text.Contains("DisplayCharge1") || tooltips[i].Name == "DisplayCharge1") && charge >= maxcharge) {
                    TooltipLine tip;
					//tooltips[i].text.Substring(8, tooltips[i].text.Length-8);
                    tip = new TooltipLine(mod, "DisplayCharge1",
                        "Press (insert ultimate hotkey here) to use radial blind");
                    //tip.overrideColor = new Color(255, 32, 174, 200);
					tip.overrideColor = modPlayer?.lightColor;
                    tooltips.RemoveAt(i);
					if(charge < maxcharge-1){
                    	tooltips.Insert(i, tip);
					}
                }//*/
            }
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
		public override void UpdateInventory(Player player){
			item.shopCustomPrice = 30+charge;
			if(player.HeldItem.type!=item.type)item.damage = 50+charge;
		}
		public override void HoldItem (Player player)
		{
            LightPlayer modPlayer = player.GetModPlayer<LightPlayer>();
			if (player.itemAnimation == 0)
            {
				item.damage = 50+charge;
				stealthinit = false;
			}
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
			if(item.thrown){
				item.useStyle = 5;
				item.shoot = ProjectileType<LightDagger>();
				item.noUseGraphic = true;
			}else{
				modPlayer.LightStealthMax += 250;
				modPlayer.LightStealthMaxPercent = 0.75f;
				item.useStyle = 3;
				item.shoot = 0;
				item.noUseGraphic = true;
				player.moveSpeed *= 1.15f;
			}
			if(charge/maxcharge >= 0.75){
				duration = (int)(300+(15*(charge-(maxcharge*0.75))));
			}
			//}

			if(modPlayer.Ulting && modPlayer.UltCD <= 0 && charge/maxcharge >= 0.75){
				for(int i2 = 0; i2 < Main.npc.Length; i2++){
					NPC target2 = Main.npc[i2];
					if(target2.Distance(player.Center) < range && !((target2.friendly || target2.damage == 0) || target2.immortal || !target2.chaseable) && target2.active && !target2.boss){
						//int a = Projectile.NewProjectile(target2.position.X, target2.position.Y, 0, -5, ProjectileType<RadialJavelin>(), item.damage*(6+(0.6*(charge-(maxcharge*0.75)))), 0, player.whoAmI);
						target2.AddBuff(BuffType<Blind>(), duration, true);
						modPlayer.UltCD = (int)Math.Min(modPlayer.UltCD+(300.0f-modPlayer.UltCD)/7.5,300);
					}
				}
				for(int i3 = 0; i3 < Main.player.Length; i3++){
					Player target3 = Main.player[i3];
					if(target3.Distance(player.Center) < range && (target3.team != player.team || target3 == player)){
						//int a = Projectile.NewProjectile(target2.position.X, target2.position.Y, 0, -5, ProjectileType<RadialJavelin>(), item.damage*(6+(0.6*(charge-(maxcharge*0.75)))), 0, player.whoAmI);
						target3.AddBuff(BuffType<Blind>(), duration, true);
						modPlayer.UltCD = (int)Math.Min(modPlayer.UltCD+(300.0f-modPlayer.UltCD)/7.5,300);
					}
					if(target3.team == player.team){
						target3.AddBuff(BuffID.Hunter, duration+60);
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

        public override bool CanUseItem(Player player)
        {
            LightPlayer modPlayer = player.GetModPlayer<LightPlayer>();
			if(player.altFunctionUse == 2 && item.thrown){
				stealthinit = true;
				item.thrown = false;
				item.melee = true;
			}else if(player.altFunctionUse == 0 && item.melee){
				item.damage = (int)((50+charge)*(1+(modPlayer.LightStealth/500)));
				item.thrown = true;
				item.melee = false;
				//modPlayer.LightStealth = 0;
			}else if(player.altFunctionUse == 2 && item.melee){
				item.damage = (int)((50+charge)*(2+(modPlayer.LightStealth/500)));
				item.useStyle = 3;
				item.noMelee = false;
				item.noUseGraphic = false;
				//modPlayer.LightStealth = 0;
			}
            return base.CanUseItem(player);
        }

		public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit){
            LightPlayer modPlayer = player.GetModPlayer<LightPlayer>();
			if(stealthinit){
				modPlayer.LightStealth = modPlayer.LightStealthMax;
				damage *= 2;
			}else{
				modPlayer.LightStealth /= 2;
			}
			if(target.modNPC != null && target.modNPC.mod.DisplayName.Contains("Calamity Mod")){
				damage += (int)(target.lifeMax * 0.02);
			}
		}
	}
}
