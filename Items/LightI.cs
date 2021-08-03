using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Light.Items {
	public class LightI : ModItem {
		public override bool CloneNewInstances => true;
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Light");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults() {
			item.width = 40;
			item.height = 40;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = 0;
			item.knockBack = 6;
			item.value = 0;
			item.rare = 2;
            item.maxStack = 999;
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float  scale, int whoAmI) {
			Texture2D texture = Main.itemTexture[item.type];
			Main.spriteBatch.Draw(Main.itemTexture[item.type], new Vector2(item.position.X - Main.screenPosition.X + item.width * 0.5f, item.position.Y - Main.screenPosition.Y + item.height - texture.Height * 0.5f + 2f), new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f,scale, SpriteEffects.None, 0f);
		}
	}
}
