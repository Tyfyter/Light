using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Light.Buffs
{
	public class ShadeDebuff : ModBuff
	{
		int weakduration = 1800;
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Powerless");
			Description.SetDefault("Ten thousand promises, ten thousand ways to lose");
            Main.debuff[Type] = true;   //Tells the game if this is a buff or not.
            Main.pvpBuff[Type] = true;  //Tells the game if pvp buff or not. 
			canBeCleared = false;
			//Main.buffNoTimeDisplay[Type] = true;
		}

		public virtual bool ReApply(Player player, int time, int buffIndex){
			weakduration = player.buffTime[buffIndex];
			player.buffTime[buffIndex] = time;
			return false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
            LightPlayer modPlayer = player.GetModPlayer<LightPlayer>();
			if(Main.expertMode){
				player.buffTime[buffIndex]--;
			}
			/*player.magicDamage *= player.buffTime[buffIndex]/weakduration;
			player.meleeDamage *= player.buffTime[buffIndex]/weakduration;
			player.minionDamage *= player.buffTime[buffIndex]/weakduration;
			player.thrownDamage *= player.buffTime[buffIndex]/weakduration;
			modPlayer.lightdamage *= player.buffTime[buffIndex]/(weakduration/2);*/
			if (Main.rand.Next(3) == 0){
				int dust2 = Dust.NewDust(player.Center, player.width, player.height, 226, 0f, 0f, 100, Color.Black);
				Main.dust[dust2].velocity /= 10f;
				Main.dust[dust2].scale = 1f;
			}
			if(player.buffTime[buffIndex] > 0 && modPlayer.ShadeCure >= 1){
				player.buffTime[buffIndex] = 0;
			}
			if(player.buffTime[buffIndex] <= 1 && modPlayer.ShadeCure < 1){
				player.statLife = 0;
				for (int i = 0; i < 15; i++){
					Dust.NewDust(player.position, player.width, player.height, 14, 0f, 0f, 0, new Color (255, 0, 0));
				}
				//Main.PlaySound(2, player.position, 27); //crystal smash
				//Main.PlaySound(2, player.position, 45); //inferno fork
				//Main.PlaySound(2, player.position, 46); //hydra summon
				//Main.PlaySound(2, player.position, 104); //shadowflame?
				Main.PlaySound(2, player.position, 119); //phantasm dragon roar
				//Main.PlaySound(4, player.position, 6); //etherial gasp
				//Main.PlaySound(4, player.position, 39); //spectre
				//Main.PlaySound(4, player.position, 51); //reaper
				Main.PlaySound(5, player.position, 1);//normal death
				//Main.PlaySound(2, player.position, 103);//shadowflame hex doll
				//player.dead = true;
				Terraria.DataStructures.PlayerDeathReason YUdie = new Terraria.DataStructures.PlayerDeathReason();
				YUdie.SourceCustomReason = player.name+" was edgy.";
				player.KillMe(YUdie, player.statLife, 0);
				player.respawnTimer = 300;
			}
		}
		/*
		public override bool ReApply(int buff, Player player, int time, int buffIndex){
			player.buffTime[buffIndex] = time;
			weakduration = time;
			return false;
		}//*/
	}
}