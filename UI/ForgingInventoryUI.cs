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

namespace Light.UI {
    public class ForgingInventoryUI : UIState {
        bool oldHovered = false;
        public override void Draw(SpriteBatch spriteBatch) {
            LightPlayer lightPlayer = Main.LocalPlayer.GetModPlayer<LightPlayer>();
            Color color = lightPlayer.lightColor;
            Color offColor = color * 0.75f;
	        Main.inventoryScale = 0.85f;
            float slotSize = 56 * Main.inventoryScale;
	        float xPos = 20;
	        float yPos = 258+(slotSize*2);
            float xOff = 0;
            float slotWidth = Main.inventoryBackTexture.Width * Main.inventoryScale;
            float slotHeight = Main.inventoryBackTexture.Height * Main.inventoryScale;
            bool interact = !Terraria.GameInput.PlayerInput.IgnoreMouseInterface;
            Texture2D backTexture = Main.inventoryBack13Texture;
            float x;
            Item item;
            Item upItem;
            Item downItem;
            float extraHeight = oldHovered?slotHeight:0;
            oldHovered = false;
            for(int i = 0; i < 10; i++) {
                x = xPos + xOff;
                item = lightPlayer.forgeItems[i];
                if(interact && Main.mouseX >= x && Main.mouseX <= x + slotWidth && Main.mouseY >= yPos - extraHeight && Main.mouseY <= yPos + slotHeight + extraHeight) {
                    Main.LocalPlayer.mouseInterface = true;
                    oldHovered = true;
	                Main.inventoryScale = 0.65f;
                    upItem = Tools.ItemFromID(lightPlayer.GetOffsetForgeableItemType(item.type, -1));
                    UITools.DrawColoredItemSlot(spriteBatch, ref upItem, new Vector2(x+slotSize*0.1f, yPos-slotSize*0.8f), backTexture, offColor, new Color(255,255,255,150));
                    downItem = Tools.ItemFromID(lightPlayer.GetOffsetForgeableItemType(item.type, 1));
                    UITools.DrawColoredItemSlot(spriteBatch, ref downItem, new Vector2(x+slotSize*0.1f, yPos+slotSize*1.01f), backTexture, offColor, new Color(255,255,255,150));
	                Main.inventoryScale = 0.85f;
                    if(Main.mouseLeftRelease && Main.mouseLeft) {
                        if(Main.mouseY < yPos) {
                            lightPlayer.ForgeHotbarItems[i] = upItem.type;
                            lightPlayer.forgeItems[i] = upItem;
                        } else if(Main.mouseY > yPos + slotHeight){
                            lightPlayer.ForgeHotbarItems[i] = downItem.type;
                            lightPlayer.forgeItems[i] = downItem;
                        }
                    }
                    if(Main.mouseY < yPos) {
                        ItemSlot.MouseHover(ref upItem, ItemSlot.Context.InventoryItem);
                    } else if(Main.mouseY > yPos + slotHeight){
                        ItemSlot.MouseHover(ref downItem, ItemSlot.Context.InventoryItem);
                    } else {
                        ItemSlot.MouseHover(ref item, ItemSlot.Context.InventoryItem);
                    }
                }
                UITools.DrawColoredItemSlot(spriteBatch, ref item, new Vector2(x, yPos), backTexture, color, new Color(255,255,255,150));
                xOff += 56*Main.inventoryScale;
            }
        }
    }
}
