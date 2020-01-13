using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using Light;
using Terraria.ID;

namespace Light.Commands
{
	public class LuxCommand : ModCommand
	{
		public override bool Autoload(ref string name){
			return ModLoader.GetMod("DevHelp")!=null;
		}
		public override CommandType Type
		{
			get { return CommandType.Chat; }
		}

		public override string Command
		{
			get { return "lux"; }
		}

		public override string Usage
		{
			get { return "/lux NPCID"; }
		}

		public override string Description 
		{
			get { return ""; }
		}

		public override void Action(CommandCaller player, string input, string[] args)
		{
			int type = 0;
			if(!Int32.TryParse(args[0], out type)){
				Type idtype = typeof(NPCID);
				if(int.TryParse(idtype.GetField(args[0]).GetRawConstantValue().ToString(),out type)){
				Main.NewText("You're using it wrong, "+player.Player.name+", it's /lux NPCID, NPCID can either be the numeric ID or the field name in the NPCID class");
				}
			}
			int spawnedNPC = NPC.NewNPC((int)Main.MouseWorld.X, (int)Main.MouseWorld.Y, type);
			//Main.npc[spawnedNPC].AddBuff(BuffType<Lux>(), 600);
			if(ModLoader.GetMod("DevHelp")==null)Main.npc[spawnedNPC].dontTakeDamage = true;
			Light.ApplyLuxBoosts(ref Main.npc[spawnedNPC]);
		}
	}
	public class UmbraCommand : ModCommand
	{
		public override bool Autoload(ref string name){
			return ModLoader.GetMod("DevHelp")!=null;
		}
		public override CommandType Type
		{
			get { return CommandType.Chat; }
		}

		public override string Command
		{
			get { return "umbra"; }
		}

		public override string Usage
		{
			get { return "/umbra NPCID"; }
		}

		public override string Description 
		{
			get { return ""; }
		}

		public override void Action(CommandCaller player, string input, string[] args)
		{
			int type = 0;
			if(!Int32.TryParse(args[0], out type)){
				Type idtype = typeof(NPCID);
				if(int.TryParse(idtype.GetField(args[0]).GetRawConstantValue().ToString(),out type)){
				Main.NewText("You're using it wrong, "+player.Player.name+", it's /umbra NPCID, NPCID can either be the numeric ID or the field name in the NPCID class");
				}
			}
			int spawnedNPC = NPC.NewNPC((int)Main.MouseWorld.X, (int)Main.MouseWorld.Y, type);
			//Main.npc[spawnedNPC].AddBuff(BuffType<Umbra>(), 600);
			if(ModLoader.GetMod("DevHelp")==null)Main.npc[spawnedNPC].dontTakeDamage = true;
			Light.ApplyShadeBoosts(ref Main.npc[spawnedNPC]);
		}
	}
	public class PointCommand : ModCommand
	{
		public override CommandType Type
		{
			get { return CommandType.Chat; }
		}

		public override string Command
		{
			get { return "point"; }
		}

		public override string Usage
		{
			get { return "/point <set> <index> <value>\n/point <get> [index]"; }
		}

		public override string Description 
		{
			get { return "get or set an index in PointsFrom"; }
		}

		public override void Action(CommandCaller player, string input, string[] args)
		{
			if(args[0].ToLower()=="get"){
				if(args.Length==1){
					Main.NewText(player.Player.GetModPlayer<LightPlayer>().PointsFrom);
				}else{
					int i;
					if(!int.TryParse(args[1], out i))Main.NewText("you're doing it wrong");
					Main.NewText(Light.GetBitFromInt(player.Player.GetModPlayer<LightPlayer>().PointsFrom, i));
				}
			}else if(ModLoader.GetMod("DevHelp")!=null){
				int i;
				if(!int.TryParse(args[1], out i))Main.NewText("you're doing it wrong");
				bool valtoset = false;
				switch (args[2].ToLower()){
					case "0":
					break;
					case "1":
					valtoset = true;
					break;
					case "true":
					valtoset = true;
					break;
					case "false":
					break;
					default:
					valtoset = !Light.GetBitFromInt(player.Player.GetModPlayer<LightPlayer>().PointsFrom, i);
					return;
				}
				if(valtoset){
					Light.SetBitToInt(ref player.Player.GetModPlayer<LightPlayer>().PointsFrom, i);
				}else{
                	player.Player.GetModPlayer<LightPlayer>().PointsFrom = player.Player.GetModPlayer<LightPlayer>().PointsFrom&~(1<<i);
				}
				Main.NewText(player.Player.GetModPlayer<LightPlayer>().PointsFrom);
			}
		}
	}
	public class PointsCommand : PointCommand{

		public override string Command
		{
			get { return "points"; }
		}

		public override string Usage
		{
			get { return "/points <set> <index> <value>\n/points <get> [index]"; }
		}
	}
}