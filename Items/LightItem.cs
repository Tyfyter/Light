using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Light.Items {
    public abstract class LightItem : ModItem {
		public int charge = 0;
		public virtual int MaxCharge { get => 10; set { } }
		public virtual int PointsUsed => 1;
        public override void ModifyTooltips(List<TooltipLine> tooltips) {

        }
        protected void ModifyTooltips(List<TooltipLine> tooltips, LightPlayer lightPlayer) {
            if(tooltips[0].text.Contains("Rainbow")){
                tooltips[0].overrideColor = Main.DiscoColor;
			}else {
                tooltips[0].overrideColor = lightPlayer?.LightColor;
			}
            for (int i = 0; i < tooltips.Count; i++) {
                if (tooltips[i].text.Contains("DisplayCharge2")) {
                    TooltipLine tip;
					//tooltips[i].text.Substring(8, tooltips[i].text.Length-8);
                    tip = new TooltipLine(mod, "DisplayCharge2",
					"current charge level: " + charge);
                    //tip.overrideColor = new Color(255, 32, 174, 200);
					tip.overrideColor = lightPlayer?.LightColor;
                    tooltips.RemoveAt(i);
                    tooltips.Insert(i, tip);
                }else if (tooltips[i].text.Contains("current charge level")) {
                    TooltipLine tip;
					//tooltips[i].text.Substring(8, tooltips[i].text.Length-8);
                    tip = new TooltipLine(mod, "DisplayCharge2",
					"current charge level: " + charge);
                    //tip.overrideColor = new Color(255, 32, 174, 200);
					tip.overrideColor = lightPlayer?.LightColor;
                    tooltips.RemoveAt(i);
                    tooltips.Insert(i, tip);
                }else if (tooltips[i].text.Contains("melee")) {
					string[] SplitText = tooltips[i].text.Split();
                    TooltipLine tip;
					//tooltips[i].text.Substring(8, tooltips[i].text.Length-8);
                    tip = new TooltipLine(mod, "melee",
                        SplitText[0]+" light damage");
                    //tip.overrideColor = new Color(255, 32, 174, 200);
					tip.overrideColor = lightPlayer?.LightColor;
                    tooltips.RemoveAt(i);
                    tooltips.Insert(i, tip);
                }else if (tooltips[i].text.Contains("DisplayCharge1")) {
                    TooltipLine tip;
                    tip = new TooltipLine(mod, "DisplayCharge1",
                        "Hold " +Light.ChargeKey+" to charge.");
					tip.overrideColor = lightPlayer?.LightColor;
                    tooltips.RemoveAt(i);
					if(charge < MaxCharge){
                    	tooltips.Insert(i, tip);
					}
                }
            }
        }
    }
}
