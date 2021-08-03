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
			byte? pointIndex = null;
			switch(npc.type) {
				case NPCID.EyeofCthulhu:
				pointIndex = 0;
				break;

				case NPCID.DD2DarkMageT1:
				pointIndex = 1;
				break;

				case NPCID.DD2DarkMageT3:
				pointIndex = 2;
				break;

				case NPCID.Retinazer:
				pointIndex = 3;
				break;

				case NPCID.Spazmatism:
				pointIndex = 4;
				break;

				case NPCID.LunarTowerVortex:
				pointIndex = 5;
				break;

				case NPCID.LunarTowerStardust:
				pointIndex = 6;
				break;

				case NPCID.LunarTowerNebula:
				pointIndex = 7;
				break;

				case NPCID.LunarTowerSolar:
				pointIndex = 8;
				break;

				case NPCID.WallofFlesh:
				pointIndex = 9;
				break;

				case NPCID.Pinky:
				if(Main.hardMode)pointIndex = 10;
				break;

				default:
				break;
			}
            if(pointIndex.HasValue) {
                if(Main.netMode == NetmodeID.Server) {
                    for(int i = 0; i < Main.player.Length; i++) {
                        if(Main.player[i].active) {
                            LightPlayer modPlayer = Main.player[i].GetModPlayer<LightPlayer>();
                            if((Main.rand.Next(4) == 0 || npc.HasBuff(BuffType<Lux>())) && !modPlayer.PointsCollected[pointIndex.Value]) {
                                int item = Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Point_Of_Light>(), 1);

                                switch(npc.type) {
                                    case NPCID.DD2DarkMageT3:
                                    Main.item[item].value = modPlayer.PointsCollected[1] ? 2 : 1;
                                    break;
                                    case NPCID.Pinky:
                                    Main.item[item].color = Color.HotPink;
                                    goto default;
                                    default:
                                    Main.item[item].value = pointIndex.Value;
                                    break;
                                }
                            }
                        }
                    }
                }
            }else if(npc.HasBuff(BuffType<Lux>())){
				for(int i1 = 0; i1 < Main.player.Length; i1++) {
					if(Main.player[i1].active){
						LightPlayer modPlayer = Main.player[i1].GetModPlayer<LightPlayer>();
						if (Main.rand.Next(4) == 0 && !modPlayer.PointsCollected[31]) {
							int i = Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Point_Of_Light>(), 1); //this is where you set what item to drop, ItemType("CustomSword>() is an example of how to add your custom item. and 1 is the amount
							Main.item[i].value = 31;
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