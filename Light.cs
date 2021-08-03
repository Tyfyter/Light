using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria.GameInput;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.GameContent.UI;
using System;
using System.Linq;
using Light.Items;
using Light.Buffs;
using static Terraria.ModLoader.ModContent;

//lumancy?
namespace Light
{
	public class Light : Mod
	{
        internal static Mod mod;

		/*public static void RedundantFunc()
		{
			var something = System.Linq.Enumerable.Range(1, 10);
        }*/
        public static ModHotKey ChargeKey;
		public static void ApplyLuxBoosts(ref NPC npc){

			npc.npcSlots *= 20f;
			npc.damage = (int)(npc.damage*1.56);
			npc.defense *= 2;
			npc.lifeMax = (int)(npc.lifeMax*2.5);
			npc.life = npc.lifeMax;
			npc.AddBuff(ModContent.BuffType<Lux>(), 3*npc.life);
            npc.value = (int)(2*npc.value);
			npc.GivenName = "Lux " + npc.GivenOrTypeName;
            npc.rarity = (int)Math.Max(npc.rarity + 1, npc.rarity * 1.5f);
			//npc.DisplayName.set("Lux "+npc.DisplayName.Get());
		}
		public static void ApplyShadeBoosts(ref NPC npc){

			npc.npcSlots *= 20f;
			npc.damage = (int)(npc.damage*2.56);
			npc.defense += 15;
			npc.defense *= 2;
			npc.lifeMax = (int)(npc.lifeMax*2.5);
			npc.life = npc.lifeMax;
			npc.AddBuff(ModContent.BuffType<Umbra>(), 5*npc.life);
            npc.value = (int)(3.5*npc.value);
			npc.GivenName = "Umbra " + npc.GivenOrTypeName;
            npc.rarity = (int)Math.Max(npc.rarity + 2, npc.rarity * 2);
            //npc.modNPC.music = MusicID.PumpkinMoon;
            if(npc.modNPC != null){
                npc.modNPC.music = MusicID.PumpkinMoon;
                //npc.modNPC.music = -255;
            }
			//npc.music = 0;
			//npc.DisplayName.set("Umbra "+npc.DisplayName.Get());
		}

        private HotKey Channel = new HotKey("Charge Item", Keys.E);
        private HotKey Ult = new HotKey("Use Ultimate", Keys.Q);
		public static int LightCurrencyID;
		public override void Load(){
            mod = this;
            LightItem.LightItems = new HashSet<int>();
            ChargeKey = RegisterHotKey(Channel.Name, Channel.DefaultKey.ToString());
            RegisterHotKey(Ult.Name, Ult.DefaultKey.ToString());
			LightCurrencyID = CustomCurrencyManager.RegisterCurrency(new LightForgeData(ModContent.ItemType<LightI>(), 999L));  //this defines the item Currency, so CustomCurrencyItem now is a Currency
            //ChargeKey = Light.Cha;
		}
        public override void Unload() {
            LightItem.LightItems = null;
        }
        public Light()
		{
			Properties = new ModProperties()
			{
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};
		}

		public static bool GetBitFromInt(int data, int position){
			int intReturn = data & (1 << position);
			return (intReturn != 0);
		}

		public static void SetBitToInt(ref int data, int position){
			data |= (1 << position);
		}

		public static void UnsetBitToInt(ref int data, int position){
			data &= ~(1 << position);
		}

        public static string ReconsructString(string[] cut){
            string full = "";
            for(int i = 0; i > cut.Length; i++){
                full = full + cut[i];
            }
            return full;
        }
        public override void AddRecipeGroups()
        {
            RecipeGroup group = new RecipeGroup(() => "Vines or Vine Ropes", new int[]
            {
                ItemID.Vine,
                ItemID.VineRope
            });
            RecipeGroup.RegisterGroup("Light:Vines", group);
        }

        public override void HotKeyPressed(string name) {
            if(PlayerInput.Triggers.Current.KeyStatus[GetTriggerName(name)]) {
                if(name.Equals(Channel.Name)) {
                    ChannelF();
                }
                if(name.Equals(Ult.Name)) {
                    UltF();
                }
            }
        }

        public void ChannelF()
        {
            Player player = Main.player[Main.myPlayer];
            LightPlayer modPlayer = player.GetModPlayer<LightPlayer>();
			modPlayer.channeling = 2;
			//player.HeldItem.ReloadGun();
        }
        public void UltF()
        {
            Player player = Main.player[Main.myPlayer];
            LightPlayer modPlayer = player.GetModPlayer<LightPlayer>();
			modPlayer.Ult = 3;
			//player.HeldItem.ReloadGun();
        }

        public string GetTriggerName(string name)
        {
            return Name + ": " + name;
        }
        public static short SetStaticDefaultsGlowMask(ModItem modItem)
        {
            if (Main.netMode!=NetmodeID.Server)
            {
                Texture2D[] glowMasks = new Texture2D[Main.glowMaskTexture.Length + 1];
                for (int i = 0; i < Main.glowMaskTexture.Length; i++)
                {
                    glowMasks[i] = Main.glowMaskTexture[i];
                }
                glowMasks[glowMasks.Length - 1] = mod.GetTexture("Items/" + modItem.GetType().Name);
                Main.glowMaskTexture = glowMasks;
                return (short)(glowMasks.Length - 1);
            }
            else return 0;
        }
	}
}
