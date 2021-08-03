using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;
using System.Collections.Generic;
using static Terraria.ModLoader.ModContent;

namespace Light.Items
{
	public class Light_Armor : ModItem
	{
		int charge = 0;
		int maxcharge = 70;
		/*
		public override string Texture
		{
			get { return "Terraria/Item_" + ItemID.HellwingBow; }
		}*/
		public override bool CloneNewInstances => true;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Light Armor");
			Tooltip.SetDefault(@"Right click in inventory to dispel, left click to equip.
			DisplayCharge");
		}
		public override void SetDefaults()
		{
			//item.defense = 30;
			item.width = 40;
			item.height = 40;
			item.useTime = 100;
			item.useAnimation = 100;
			item.useStyle = 5;
			item.knockBack = 6;
			item.value = 0;
			item.rare = 2;
			item.UseSound = SoundID.Item9;
			item.autoReuse = false;
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

		/*public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.Next(10) == 0)
			{
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, mod.DustType("Sparkle"));
			}
		}//*/
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.player[item.owner];
            LightPlayer modPlayer = player?.GetModPlayer<LightPlayer>();
            for (int i = 0; i < tooltips.Count; i++)
            {
                if (tooltips[i].text.Contains("DisplayCharge"))
                {
                    TooltipLine tip;
					//tooltips[i].text.Substring(8, tooltips[i].text.Length-8);
                    tip = new TooltipLine(mod, "DisplayCharge",
					"current defence level: " + (int)(charge+30));
                    //tip.overrideColor = new Color(255, 32, 174, 200);
					tip.overrideColor = modPlayer?.lightColor;
                    tooltips.RemoveAt(i);
                    tooltips.Insert(i, tip);
                }
            }
        }
		public override void HoldItem (Player player)
		{
            LightPlayer modPlayer = player.GetModPlayer<LightPlayer>();
            item.holdStyle = 0;
			//item.defence = 30+charge;

            if (modPlayer.channeling > 0)
            {
                item.holdStyle = 1;
				Color color = modPlayer.lightColor;
				//red | green| blue
				Lighting.AddLight(player.Center, color.R/255, color.G/255, color.B/255);  //this defines the projectile light color
				if(charge < maxcharge && base.CanUseItem(player)){
					for (int j = 0; j < player.inventory.Length; j++)
					{
						if (player.inventory[j].type == ItemType<LightI>())
						{
							player.inventory[j].stack--;
							charge++;
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
			Item.NewItem((int)player.position.X, (int)player.position.Y, player.width, player.height, ItemType<LightI>(), 10+charge);
		}
	}
}