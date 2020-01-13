using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Light.Buffs
{
	public class DisabledDebuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Disabled");
			Description.SetDefault("'That star destroyer is disabled!'");
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.buffTime[buffIndex]++;
			//npc.ai[0] = 0.2f;
			if(ModLoader.GetMod("CalamityMod") != null){
				if(npc.type == ModLoader.GetMod("CalamityMod").NPCType("DevourerofGodsBody")){
					npc.localAI[0] = 0f;
				}
			}
			
			if (Main.rand.Next(3) == 0){
				int dust2 = Dust.NewDust(npc.Center, npc.width, npc.height, 226, 0f, 0f, 0, Color.White);
				Main.dust[dust2].velocity /= 10f;
				Main.dust[dust2].scale = 1f;
			}
		}
	}
}