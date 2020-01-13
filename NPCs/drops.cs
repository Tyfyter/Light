using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Light;
using Light.Items;
using Light.Buffs;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;

namespace Light.NPCs
{
    public class NpcDrops : GlobalNPC
    {
        public override void NPCLoot(NPC npc)
        {
			if(npc.HasBuff(BuffType<Lux>())){
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<LightI>(), 10+Main.rand.Next(20));
				if(npc.boss){
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<LightI>(), 30+Main.rand.Next(60));
				}
			}
			//EoC, dark mage and dark mage, twins, pillars, WoF eyes, and pinky (hardmode only)
            if (npc.type == 4 || npc.type ==  564 || npc.type == 565 || npc.type == 125 || npc.type == 126 || npc.type == 422 || npc.type == 493 || npc.type == 507 || npc.type == 517 || npc.type == NPCID.WallofFleshEye || (npc.type == NPCID.Pinky&&Main.hardMode))   //this is where you choose the npc you want
            {
				for(int i1 = 0; i1 < Main.player.Length; i1++)
				{
					if(Main.player[i1].active){
						LightPlayer modPlayer = Main.player[i1].GetModPlayer<LightPlayer>();
						int i2 = 0;
						switch(npc.type){
							case 4:
								i2 = 1;
								break;
								
							case 564:
								i2 = 2;
								break;
								
							case 565:
								i2 = 3;
								break;
								
							case 125:
								i2 = 4;
								break;
								
							case 126:
								i2 = 5;
								break;
								
							case 422:
								i2 = 6;
								break;
								
							case 493:
								i2 = 7;
								break;
								
							case 507:
								i2 = 8;
								break;
								
							case 517:
								i2 = 9;
								break;
								
							case NPCID.WallofFleshEye:
								i2 = 10;
								break;
								
							case NPCID.Pinky:
								i2 = 11;
								break;
								
							default:
								break;
						}
						if ((Main.rand.Next(4) == 0 || npc.HasBuff(BuffType<Lux>())) && !Light.GetBitFromInt(modPlayer.PointsFrom, i2)) //this is the item rarity, so 9 is 1 in 10 chance that the npc will drop the item.
						{
							{
								int i = Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Point_Of_Light>(), 1); //this is where you set what item to drop, ItemType("CustomSword>() is an example of how to add your custom item. and 1 is the amount
							
								switch(npc.type){
									case 4:
										Main.item[i].value = 1;
										break;
										
									case 564:
										Main.item[i].value = 2;
										break;
										
									case 565:
										Main.item[i].value = 3;
										break;
										
									case 125:
										Main.item[i].value = 4;
										break;
										
									case 126:
										Main.item[i].value = 5;
										break;
										
									case 422:
										Main.item[i].value = 6;
										break;
										
									case 493:
										Main.item[i].value = 7;
										break;
										
									case 507:
										Main.item[i].value = 8;
										break;
										
									case 517:
										Main.item[i].value = 9;
										break;
										
									case NPCID.WallofFleshEye:
										Main.item[i].value = 10;
										break;
										
									case NPCID.Pinky:
										Main.item[i].value = 11;
										Main.item[i].color = Color.HotPink;
										break;
										
									default:
										break;
								}
							}
						}
					}
				}
            }else if(npc.HasBuff(BuffType<Lux>())){
				for(int i1 = 0; i1 < Main.player.Length; i1++)
				{
					if(Main.player[i1].active){
						LightPlayer modPlayer = Main.player[i1].GetModPlayer<LightPlayer>();
						int i2 = 31;
						if (Main.rand.Next(4) == 0 && !Light.GetBitFromInt(modPlayer.PointsFrom, i2)) //this is the item rarity, so 9 is 1 in 10 chance that the npc will drop the item.
						{
							{
								int i = Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Point_Of_Light>(), 1); //this is where you set what item to drop, ItemType("CustomSword>() is an example of how to add your custom item. and 1 is the amount
								Main.item[i].value = 31;
							}
						}
					}
				}
			}

			if((npc.type == NPCID.Plantera || npc.type == NPCID.PlanterasHook) && Main.rand.Next(0,19) == 0){
				Item.NewItem(npc.Center, 0, 0, ItemType<SoulOfInosite>(), Main.rand.Next(Main.expertMode ? 20 : 25,40));
			}
			if(npc.type == NPCID.PlanterasTentacle && Main.rand.Next(0,49) == 0){
				Item.NewItem(npc.Center, 0, 0, ItemType<SoulOfInosite>());
			}
        }
    }
}