using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.UI;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.GameInput;

namespace Light.UI {
    public static class ForgingHotbarUI {
        public static void Draw() {
            if (Main.LocalPlayer.ghost) {
		        return;
	        }
            Player player = Main.LocalPlayer;
            LightPlayer lightPlayer = player.GetModPlayer<LightPlayer>();
            Item forgeSelectedItem = lightPlayer.forgeItems[lightPlayer.forgeSelectedItem];
            Texture2D backTexture = Main.inventoryBack13Texture;
	        string text = "";
	        if (!string.IsNullOrEmpty(forgeSelectedItem.Name)) {
		        text = forgeSelectedItem.AffixName();
	        }
	        Vector2 vector = Main.fontMouseText.MeasureString(text) / 2f;
	        Main.spriteBatch.DrawString(Main.fontMouseText, text, new Vector2(236f - vector.X, 0f), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
	        int posX = 20;
	        for (int i = 0; i < 10; i++) {
		        if (i == lightPlayer.forgeSelectedItem) {
			        if (Main.hotbarScale[i] < 1f) {
				        Main.hotbarScale[i] += 0.05f;
			        }
		        } else if (Main.hotbarScale[i] > 0.75) {
			        Main.hotbarScale[i] -= 0.05f;
		        }
		        float hotbarScale = Main.hotbarScale[i];
		        int posY = (int)(20f + 22f * (1f - hotbarScale));
		        int a = (int)(75f + 150f * hotbarScale);
		        Color lightColor = new Color(255, 255, 255, a);
		        if (!player.hbLocked && !PlayerInput.IgnoreMouseInterface && Main.mouseX >= posX && (float)Main.mouseX <= (float)posX + (float)backTexture.Width * Main.hotbarScale[i] && Main.mouseY >= posY && (float)Main.mouseY <= (float)posY + (float)backTexture.Height * Main.hotbarScale[i] && !player.channel) {
			        player.mouseInterface = true;
			        player.showItemIcon = false;
			        if (Main.mouseLeft && !player.hbLocked && !Main.blockMouse) {
				        lightPlayer.forgeSelectedItem = i;
			        }
			        Main.hoverItemName = player.inventory[i].AffixName();
		        }
		        float oldInventoryScale = Main.inventoryScale;
		        Main.inventoryScale = hotbarScale;
                UITools.DrawColoredItemSlot(
                    Main.spriteBatch,
                    ref lightPlayer.forgeItems[i],
                    new Vector2(posX, posY),
                    backTexture,
                    lightPlayer.lightColor,
                    lightColor);
		        Main.inventoryScale = oldInventoryScale;
		        posX += (int)(backTexture.Width * Main.hotbarScale[i]) + 4;
	        }
        }
    }
}
