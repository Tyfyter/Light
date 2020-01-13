using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace Light.Commands
{
	public class ColorCommand : ModCommand
	{
		public override CommandType Type
		{
			get { return CommandType.Chat; }
		}

		public override string Command
		{
			get { return "l"; }
		}

		public override string Usage
		{
			get { return "/l number number number"; }
		}

		public override string Description 
		{
			get { return "change light color (R G B)"; }
		}

		public override void Action(CommandCaller player, string input, string[] args)
		{
			if(args.Length == 4){
				player.Player.GetModPlayer<LightPlayer>().LightColor = new Color(Int32.Parse(args[0]), Int32.Parse(args[1]), Int32.Parse(args[2]), Int32.Parse(args[3]));
			}else if(args.Length == 3){
				player.Player.GetModPlayer<LightPlayer>().LightColor = new Color(Int32.Parse(args[0]), Int32.Parse(args[1]), Int32.Parse(args[2]));
			}else if(args.Length==0){
				player.Player.chatOverhead.NewMessage(player.Player.GetModPlayer<LightPlayer>().LightColor.ToString(), 240);
			}else{
				Main.NewText("You're using it wrong, "+player.Player.name);
			}
		}
	}
}