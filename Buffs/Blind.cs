using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Light.Buffs
{
	public class Blind : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Blind");
			Description.SetDefault("");
            Main.debuff[Type] = true;   //Tells the game if this is a buff or not.
            Main.pvpBuff[Type] = true;  //Tells the game if pvp buff or not. 
			canBeCleared = false;
			//Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(NPC npc, ref int buffIndex){
			npc.target = Main.player.Length+1;
			npc.FaceTarget();
			npc.direction = 0 - npc.direction;
			int a = Dust.NewDust(npc.position, npc.width, npc.height, 109, 0, 0, 200, Color.Black);
			Main.dust[a].noGravity = true;
			if(npc.velocity.Y == 0){
				npc.velocity.X *= 0.9f;
			}
			if(npc.velocity.Y <= 0){
				npc.velocity.Y *= 0.9f;
			}
		}
		public override void Update(Player player, ref int buffIndex){
			player.buffType[buffIndex] = BuffID.Blackout;
		}
	}
}