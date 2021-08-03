using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.Config;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Light {
	[Label("Client Config")]
    public class LightConfig : ModConfig {
        public static LightConfig Instance;
        public override ConfigScope Mode => ConfigScope.ClientSide;

        //[JsonIgnore]
		[Label("Light Color")]
        public Color LightColor {
            get {
                return Main.gameMenu ? DefaultLightColor : Main.LocalPlayer.GetModPlayer<LightPlayer>().lightColor;
            }
            set {
                //888 10000 10024 10
                switch(Main.menuMode) {
                    case 10000:
                    return;
                }
                if(Main.gameMenu) {
                    DefaultLightColor = value;
                } else {
                    Main.LocalPlayer.GetModPlayer<LightPlayer>().lightColor = value;
                }
            }
        }

		[Label("Default Light Color")]
        [DefaultValue(typeof(Color), "255, 255, 255, 255")]
        public Color DefaultLightColor { get; set; }
    }
}
