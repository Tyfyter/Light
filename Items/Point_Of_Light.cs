using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Light;
using Light.NPCs;

namespace Light.Items
{
	public class Point_Of_Light : ModItem
	{
        public override bool CloneNewInstances => true;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Point Of Light");
			Tooltip.SetDefault(@"Use to start forging.");
		}
		public override void SetDefaults()
		{
			item.width = 40;
			item.height = 40;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = 1;
			item.knockBack = 6;
			item.value = -1;
			item.rare = 2;
            item.maxStack = 999;
			item.UseSound = null;
			item.autoReuse = true;
		}

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            for (int i = 0; i < tooltips.Count; i++)
            {
                TooltipLine tip;
                tip = new TooltipLine(mod, "", tooltips[i].text);
                tip.overrideColor = new Color(255, 255, 255, 200);
                tooltips.RemoveAt(i);
                tooltips.Insert(i, tip);
            }
        }
		public override bool OnPickup(Player player){
            LightPlayer lightPlayer = player.GetModPlayer<LightPlayer>();
			int droppedby = item.value;
			item.value = -1;
            lightPlayer.PointsCollected[item.value] = true;
			return true;
		}
		public override bool CanPickup(Player player){
            LightPlayer lightPlayer = player.GetModPlayer<LightPlayer>();
			if(lightPlayer.PointsCollected[item.value]){
				return false;
			}
			return true;
		}
	}
}
