using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;
using Light.Items;
using System.IO;
using Terraria.GameInput;
using Terraria.UI;

namespace Light {
    public class LightPlayer : ModPlayer {
		public bool LightArmor = false;
		public int Ult = 0;
		public bool Ulting = false;
		public int UltCD = 0;
		public float LightStealth = 0;
		public float LightStealthMax = 0;
		public float LightStealthMaxPercent = 0;
		public BitSet PointsCollected { get; internal set; }
		public int shadeCure = 0;
		public float lightdamage = 1;
        public int ascendtype = 0;
		public Color lightColor = new Color(255, 255, 255);
        public bool phasing = false;
        public int tempcharge = 0;
        public float forgeGlow = 0f;
        public int forgeSelectedItem = 0;
        public bool invalidSlotForForging = false;
        public int[] ForgeHotbarItems { get; internal set; }
        public List<int> ForgeableItems { get; internal set; }
        internal Item[] forgeItems;

        public override void ResetEffects() {
            //pointsTotal = -PointsInUse;
            LightArmor = false;
            if(shadeCure > 0) {
                shadeCure--;
            }
            if(UltCD > 0) {
                UltCD--;
            }
            if(Ult > 0) {
                Ult--;
            }
            Ulting = Ult == 1;
            lightdamage = 1f;
            ascendtype = 0;
        }
        public override void SetControls() {
            bool forging = false;
            if(Main.playerInventory) {
                if(Light.ControlModeSwitch.JustPressed) {
                    SetupForgeableItems();
                    forgeItems = ForgeHotbarItems.Select(Tools.ItemFromID).ToArray();
                    Light.Instance.ToggleForgeSelectorUI();
                }
            } else switch(Light.ControlModeSwitch.GetHotkeyState()) {
                case HotkeyState.JustPressed:
                forgeSelectedItem = 0;
                invalidSlotForForging = false;
                if(PointsCollected.Count<1) {
                    break;
                }
                if(Tools.ItemExists(player.HeldItem) && !(player.HeldItem.modItem is LightItem)) {
                    invalidSlotForForging = true;
                    break;
                }
                forgeItems = ForgeHotbarItems.Select(Tools.ItemFromID).ToArray();
                goto case HotkeyState.Held;

                case HotkeyState.Held:
                if(invalidSlotForForging) {
                    break;
                }
                forging = true;
                int scrollDirection = Math.Sign(PlayerInput.ScrollWheelDelta);
                if(PlayerInput.ScrollWheelDelta*scrollDirection>=60) {
                    forgeSelectedItem = ((forgeSelectedItem - scrollDirection) + 10)%10;
                    PlayerInput.ScrollWheelDelta = 0;
                }
                #region hotbar keys
                if(PlayerInput.Triggers.Current.Hotbar1) {
					forgeSelectedItem = 0;
                    PlayerInput.Triggers.Current.Hotbar1 = false;
				}
				if (PlayerInput.Triggers.Current.Hotbar2) {
					forgeSelectedItem = 1;
                    PlayerInput.Triggers.Current.Hotbar2 = false;
				}
				if (PlayerInput.Triggers.Current.Hotbar3) {
					forgeSelectedItem = 2;
                    PlayerInput.Triggers.Current.Hotbar3 = false;
				}
				if (PlayerInput.Triggers.Current.Hotbar4) {
					forgeSelectedItem = 3;
                    PlayerInput.Triggers.Current.Hotbar4 = false;
				}
				if (PlayerInput.Triggers.Current.Hotbar5) {
					forgeSelectedItem = 4;
                    PlayerInput.Triggers.Current.Hotbar5 = false;
				}
				if (PlayerInput.Triggers.Current.Hotbar6) {
					forgeSelectedItem = 5;
                    PlayerInput.Triggers.Current.Hotbar6 = false;
				}
				if (PlayerInput.Triggers.Current.Hotbar7) {
					forgeSelectedItem = 6;
                    PlayerInput.Triggers.Current.Hotbar7 = false;
				}
				if (PlayerInput.Triggers.Current.Hotbar8) {
					forgeSelectedItem = 7;
                    PlayerInput.Triggers.Current.Hotbar8 = false;
				}
				if (PlayerInput.Triggers.Current.Hotbar9) {
					forgeSelectedItem = 8;
                    PlayerInput.Triggers.Current.Hotbar9 = false;
				}
				if (PlayerInput.Triggers.Current.Hotbar10) {
					forgeSelectedItem = 9;
                    PlayerInput.Triggers.Current.Hotbar10 = false;
				}
                #endregion
                break;

                case HotkeyState.JustReleased:
                if(Light.forgingHotbarActive && (!Tools.ItemExists(player.HeldItem) || (player.HeldItem.modItem is LightItem))) {
                    player.HeldItem.SetDefaults(ForgeHotbarItems[forgeSelectedItem]);
                    break;
                }
                break;
            }
            Light.forgingHotbarActive = forging;
        }
        public override void PostUpdateMiscEffects() {
            Tools.LinearSmoothing(ref forgeGlow, Light.forgingHotbarActive?1:0, 0.1f+(Light.forgingHotbarActive?forgeGlow*0.1f:(1-forgeGlow)*0.1f));
            if(forgeGlow>0) {
                Lighting.AddLight(player.MountedCenter, lightColor.ToVector3()*forgeGlow);
            }
        }
        public override void UpdateEquips(ref bool wallSpeedBuff, ref bool tileSpeedBuff, ref bool tileRangeBuff) {
            for(int i = 0; i < 5; i++) {
                if((player.miscEquips[i].modItem) is IMiscEquip item) {
                    item.UpdateMisc(player);
                }
            }
        }
        public override bool ShiftClickSlot(Item[] inventory, int context, int slot) {
            if(player.HeldItem.type == mod.GetItem("Light_Javelin").item.type && inventory[slot].type == ItemID.SoulofMight && inventory[slot].stack >= 20) {
                player.chatOverhead.NewMessage(inventory[slot].HoverName+":"+(ascendtype-1), 60);
                return true;
            }
            return false;
        }
        public void SetupForgeableItems() {
            int pointCount = PointsCollected.Count;
            ForgeableItems = LightItem.LightItems.Where(v => v.points <= pointCount).OrderBy(v => v.points).Select(v => v.type).ToList();
            ForgeableItems.Insert(0, 0);
        }
        public int GetOffsetForgeableItemType(int start, int offset) {
            int index = ForgeableItems.IndexOf(start);
            index += offset;
            while(index>=ForgeableItems.Count) {
                index -= ForgeableItems.Count;
            }
            while(index<0) {
                index += ForgeableItems.Count;
            }
            return ForgeableItems[index];
        }
		public override TagCompound Save() {
            string versionString = mod.Version.ToString();
			return new TagCompound {
                { "LightColor", lightColor },
                { "PointsCollected", PointsCollected?.Pack() },
                { "ForgeHotbarItems", ForgeHotbarItems },
                { "LastVer",  versionString }
			};
		}

		public override void Load(TagCompound tag) {
			lightColor = tag.Get<Color>("LightColor");
            try {
                PointsCollected = new BitSet(tag.Get<uint>("PointsCollected"));
            } catch(Exception) {
                PointsCollected = BitSet.Zero;
            }
            ForgeHotbarItems = new int[10];
            try {
                ForgeHotbarItems = tag.Get<int[]>("ForgeHotbarItems").WithLength(10);
            } catch(Exception) {}
            string a = player.name;
            string versionString = "0.0.0.0";
            try {
                versionString = tag.GetString("LastVer");
            } catch(Exception e) {
                if(e is IOException) {
                    try {
                        int[] vers = tag.Get<int[]>("LastVer");
                        versionString = $" {vers[0]}. {vers[1]}. {vers[2]}. {vers[3]}";
                    } catch(Exception) { }
                }
            }
            Version.TryParse(versionString, out Version version);
            FixVersion(version);
		}
        private void FixVersion(Version version) {

        }
    }
}
