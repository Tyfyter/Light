/*
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Light.Buffs
{
	public class VineHookBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Vine Thorns");
			Description.SetDefault("");
            Main.pvpBuff[Type] = false;  //Tells the game if pvp buff or not. 
			canBeCleared = false;
			//Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(NPC npc, ref int buffIndex){
		}
		public override void Update(Player player, ref int buffIndex){
            LightPlayer modPlayer = player.GetModPlayer<LightPlayer>();
			player.thorns += 100;
			player.endurance += (player.endurance + 1)*0.66f;
			modPlayer.meleedmgmult += 0.67f;
			//player.meleeDamage *= 1.67f;
			/*Projectile heldproj = ProjectileLoader.GetProjectile(player.heldProj).projectile;
			if(player.HeldItem.melee && heldproj.timeLeft < 60){
				
			}//* /
		}
	}
	public class VineHookDebuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Vine Thorns");
			Description.SetDefault("");
            Main.pvpBuff[Type] = false;  //Tells the game if pvp buff or not. 
			canBeCleared = false;
			//Main.buffNoTimeDisplay[Type] = true;
		}
	}
}*/