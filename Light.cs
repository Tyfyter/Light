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
using Terraria.UI;
using Light.UI;

//lumancy?
namespace Light {
	public class Light : Mod {
        private readonly HotKey ModeSwitch = new HotKey("Toggle Lumomancy", Keys.BrowserBack);
        //private static readonly HotKey Channel = new HotKey("Charge Item", Keys.E);
        //public static ModHotKey ChargeKey;
        public static ModHotKey ControlModeSwitch;
        //public static readonly HotKey Ult = new HotKey("Use Ultimate", Keys.Q);

        public static Light Instance { get; private set; }
		internal UserInterface UI;
		internal static bool forgingHotbarActive;

		public static void ApplyLuxBoosts(ref NPC npc) {

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
		public static void ApplyShadeBoosts(ref NPC npc) {

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
            if(npc.modNPC != null) {
                npc.modNPC.music = MusicID.PumpkinMoon;
            }
			//npc.music = 0;
			//npc.DisplayName.set("Umbra "+npc.DisplayName.Get());
		}

		public override void Load() {
            Instance = this;
            LightItem.LightItems = new HashSet<(int, int)>();
            ControlModeSwitch = ModeSwitch.Register(this);
			if (Main.netMode!=NetmodeID.Server){
				UI = new UserInterface();
			}
		}
        public override void Unload() {
            LightItem.LightItems = null;
            ControlModeSwitch = null;
            UI = null;
        }
        public Light() {
			Properties = new ModProperties() {
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};
		}
		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
            if(forgingHotbarActive) {
                if(Main.playerInventory) {
                    forgingHotbarActive = false;
                    return;
                }
			    int hotbarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Hotbar"));
			    if (hotbarIndex != -1) {
                    layers[hotbarIndex] = new LegacyGameInterfaceLayer(
                        "Light: ForgeHotbarUI",
                        delegate {
                            ForgingHotbarUI.Draw();
                            return true;
                        },
                        InterfaceScaleType.UI);
			    }
            }
			int inventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
			if (inventoryIndex != -1) {
                if(!Main.playerInventory && ForgeSelectorUIActive) {
                    UI.SetState(null);
                    return;
                }
                layers.Insert(inventoryIndex + 1, new LegacyGameInterfaceLayer(
                    "Light: ForgeInventoryUI",
                    delegate {
                        // If the current UIState of the UserInterface is null, nothing will draw. We don't need to track a separate .visible value.
						UI.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
			}
		}
        public bool ForgeSelectorUIActive => UI.CurrentState is ForgingInventoryUI;
        public void ToggleForgeSelectorUI() {
            if(UI.CurrentState is ForgingInventoryUI) {
                UI.SetState(null);
            } else {
                ForgingInventoryUI forgingUI = new ForgingInventoryUI();
                UI.SetState(forgingUI);
            }
        }
		public static bool GetBitFromInt(int data, int position) {
			int intReturn = data & (1 << position);
			return (intReturn != 0);
		}

		public static void SetBitToInt(ref int data, int position) {
			data |= (1 << position);
		}

		public static void UnsetBitToInt(ref int data, int position) {
			data &= ~(1 << position);
		}

        public override void AddRecipeGroups() {
            RecipeGroup group = new RecipeGroup(() => "Vines", new int[] {
                ItemID.Vine,
                ItemID.VineRope
            });
            RecipeGroup.RegisterGroup("Light:Vines", group);
        }

        public static short SetStaticDefaultsGlowMask(ModItem modItem) {
            if (Main.netMode!=NetmodeID.Server) {
                Texture2D[] glowMasks = new Texture2D[Main.glowMaskTexture.Length + 1];
                for (int i = 0; i < Main.glowMaskTexture.Length; i++) {
                    glowMasks[i] = Main.glowMaskTexture[i];
                }
                glowMasks[glowMasks.Length - 1] = Instance.GetTexture("Items/" + modItem.GetType().Name);
                Main.glowMaskTexture = glowMasks;
                return (short)(glowMasks.Length - 1);
            } else return 0;
        }
	}
}
