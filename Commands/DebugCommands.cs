using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using Light;
using Terraria.ID;

namespace Light.Commands {
	public class LuxCommand : ModCommand {
		public override bool Autoload(ref string name) {
			return ModLoader.GetMod("DevHelp")!=null;
		}
		public override CommandType Type {
			get { return CommandType.Chat; }
		}

		public override string Command {
			get { return "lux"; }
		}

		public override string Usage {
			get { return "/lux NPCID"; }
		}

		public override string Description {
			get { return ""; }
		}

		public override void Action(CommandCaller player, string input, string[] args) {
			int type = 0;
			if(!Int32.TryParse(args[0], out type)) {
				Type idtype = typeof(NPCID);
				if(int.TryParse(idtype.GetField(args[0]).GetRawConstantValue().ToString(),out type)) {
				Main.NewText("You're using it wrong, "+player.Player.name+", it's /lux NPCID, NPCID can either be the numeric ID or the field name in the NPCID class");
				}
			}
			int spawnedNPC = NPC.NewNPC((int)Main.MouseWorld.X, (int)Main.MouseWorld.Y, type);
			//Main.npc[spawnedNPC].AddBuff(BuffType<Lux>(), 600);
			if(ModLoader.GetMod("DevHelp")==null)Main.npc[spawnedNPC].dontTakeDamage = true;
			Light.ApplyLuxBoosts(ref Main.npc[spawnedNPC]);
		}
	}
	public class UmbraCommand : ModCommand {
		public override bool Autoload(ref string name) {
			return ModLoader.GetMod("DevHelp")!=null;
		}
		public override CommandType Type {
			get { return CommandType.Chat; }
		}

		public override string Command {
			get { return "umbra"; }
		}

		public override string Usage {
			get { return "/umbra NPCID"; }
		}

		public override string Description {
			get { return ""; }
		}

		public override void Action(CommandCaller player, string input, string[] args) {
            if(!int.TryParse(args[0], out int type)) {
                Type idtype = typeof(NPCID);
                if(int.TryParse(idtype.GetField(args[0]).GetRawConstantValue().ToString(), out type)) {
                    Main.NewText("You're using it wrong, " + player.Player.name + ", it's /umbra NPCID, NPCID can either be the numeric ID or the field name in the NPCID class");
                }
            }
            int spawnedNPC = NPC.NewNPC((int)Main.MouseWorld.X, (int)Main.MouseWorld.Y, type);
			//Main.npc[spawnedNPC].AddBuff(BuffType<Umbra>(), 600);
			if(ModLoader.GetMod("DevHelp")==null)Main.npc[spawnedNPC].dontTakeDamage = true;
			Light.ApplyShadeBoosts(ref Main.npc[spawnedNPC]);
		}
	}
	public class PointCommand : ModCommand {
        public override CommandType Type => CommandType.Chat;
        public override string Command => "point";
        public override string Usage => "/point <set> <index> <value>\n/point <get> [index]";
        public override string Description => "get or set an index in PointsFrom";

		public override void Action(CommandCaller player, string input, string[] args) {
			if(args.Length<1||args[0].ToLower()=="get") {
				if(args.Length<=1) {
					Main.NewText(player.Player.GetModPlayer<LightPlayer>().PointsCollected.Pack());
				} else {
                    if(int.TryParse(args[1], out int i)) {
					    Main.NewText(player.Player.GetModPlayer<LightPlayer>().PointsCollected[i]);
                    } else {
                        Main.NewText(Usage);
                    }
				}
			} else if(ModLoader.GetMod("DevHelp") is Mod) {//null check on Dev Help
				if(!int.TryParse(args[1], out int i)) {
                    Main.NewText(Usage);
                    return;
                }
				switch(args.Length>2?args[2].ToLower():null) {
					case "1":
					case "true":
                    player.Player.GetModPlayer<LightPlayer>().PointsCollected[i] = true;
                    break;
					case "0":
					case "false":
                    player.Player.GetModPlayer<LightPlayer>().PointsCollected[i] = true;
                    break;
                    default:
                    player.Player.GetModPlayer<LightPlayer>().PointsCollected[i] ^= true;
                    break;
				}
				Main.NewText(player.Player.GetModPlayer<LightPlayer>().PointsCollected.Pack());
			}
		}
	}
	public class PointsCommand : PointCommand {

		public override string Command {
			get { return "points"; }
		}

		public override string Usage {
			get { return "/points <set> <index> <value>\n/points <get> [index]"; }
		}
	}
}