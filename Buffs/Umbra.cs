using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Light.Buffs
{
	public class Umbra : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Umbra");
			Description.SetDefault("");
			Main.buffNoTimeDisplay[Type] = true;
		}
		public override void Update(NPC npc, ref int buffIndex)
		{
			if(npc.buffTime[buffIndex]%4 == 0 && !(npc.HasBuff(BuffType<DisabledDebuff>()) || npc.HasBuff(BuffType<DisabledTempDebuff>()))){
				if(npc.life < npc.lifeMax){
					npc.life++;
				}else{
					npc.buffTime[buffIndex]=(int)MathHelper.Min(npc.buffTime[buffIndex]+8,npc.lifeMax);
				}
			}
		}
	}
}