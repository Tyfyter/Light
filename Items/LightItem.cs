using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Light.Items {
    public abstract class LightItem : ModItem {
        public static HashSet<(int type, int points)> LightItems { get; internal set; }
		public int charge = 0;
		public virtual int MaxCharge { get => 10; set { } }
		public abstract int PointsUsed { get; }
        protected void RegisterLightItem() {
            LightItems.Add((item.type, PointsUsed));
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips) {
			Color color = LightConfig.Instance.LightColor;
            if(tooltips[0].text.Contains("Rainbow")){
                color = Main.DiscoColor;
			}
            for (int i = 0; i < tooltips.Count; i++) {
                if (tooltips[i].Name.Equals("ItemName")) {
                    tooltips[i].overrideColor = color;
                }else if (tooltips[i].Name.Equals("Damage")) {
                    TooltipLine tip;
					//tooltips[i].text.Substring(8, tooltips[i].text.Length-8);
                    tip = new TooltipLine(mod, "LightDamage", tooltips[i].text);
                    //tip.overrideColor = new Color(255, 32, 174, 200);
					tip.overrideColor = color;
                    tooltips.RemoveAt(i);
                    tooltips.Insert(i, tip);
                }/*else if (tooltips[i].Name.Equals("Tooltip1")) {
                    TooltipLine tip;
					//tooltips[i].text.Substring(8, tooltips[i].text.Length-8);
                    tip = new TooltipLine(mod, "Tooltip1",
					"Current charge level: " + charge);
                    //tip.overrideColor = new Color(255, 32, 174, 200);
					tip.overrideColor = color;
                    tooltips.RemoveAt(i);
                    tooltips.Insert(i, tip);
                }else if (tooltips[i].Name.Equals("Tooltip2")) {
                    TooltipLine tip;
                    tip = new TooltipLine(mod, "Tooltip2",
                        "Hold " +Light.ChargeKey.GetHotkeyBinding()+" to charge.");
					tip.overrideColor = color;
                    tooltips.RemoveAt(i);
					if(charge < MaxCharge){
                    	tooltips.Insert(i, tip);
					}
                }*/
            }
        }
		public override TagCompound Save(){
			return new TagCompound {
				{"charge",charge}
			};
		}
		public override void Load(TagCompound tag){
			charge = tag.GetInt("charge");
		}
    }
}
