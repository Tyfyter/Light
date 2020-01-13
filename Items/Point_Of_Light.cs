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
		int droppedby = 0;
		public override bool CloneNewInstances{
			get { return true; }
		}
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
			item.value = 0;
			item.rare = 2;
            item.maxStack = 999;
			item.UseSound = null;
			item.autoReuse = true;
		}
		
 
        public override bool CanUseItem(Player player)
        {
            LightPlayer modPlayer = player.GetModPlayer<LightPlayer>();
			int a = NPC.NewNPC((int)Main.MouseWorld.X, (int)Main.MouseWorld.Y, mod.NPCType("LightForgeNpc"));
			((LightForgeNpc)Main.npc[a].modNPC).owner = modPlayer;
			Main.npc[a].GivenName = ((LightForgeNpc)Main.npc[a].modNPC).TownNPCName();
            return base.CanUseItem(player);
        }
		
		/*public override bool CanRightClick(){
			return true;
		}
		public override void RightClick (Player player){
			NPC.NewNPC((int)player.position.X, (int)player.position.Y, mod.NPCType("LightForgeNpc"));
			item.stack++;
		}*/
		
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
		public override bool OnPickup (Player player){
            LightPlayer modPlayer = player.GetModPlayer<LightPlayer>();
			int droppedby = item.value;
			item.value = 0;
			Light.SetBitToInt(ref modPlayer.PointsFrom, droppedby);
			return true;
		}
		public override bool CanPickup (Player player){
            LightPlayer modPlayer = player.GetModPlayer<LightPlayer>();
			if(Light.GetBitFromInt(modPlayer.PointsFrom,item.value) && item.value != 0){
				return false;
			}
			return true;
		}
	}
}
