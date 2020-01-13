using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Light.Buffs
{
	public class FallDamage : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("");
			Description.SetDefault("");
			Main.buffNoTimeDisplay[Type] = true;
		}
	}
}