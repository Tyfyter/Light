using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.DataStructures;
using Terraria.GameContent.UI;

namespace Light {
    public struct HotKey {

        public string Name { get; private set; }
        public Keys DefaultKey { get; private set; }

        public HotKey(string name, Keys defaultKey) {
            this.Name = name;
            this.DefaultKey = defaultKey;
        }
        public ModHotKey Register(Mod mod) {
            return mod.RegisterHotKey(Name, DefaultKey.ToString());
        }
    }
}