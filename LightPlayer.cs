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

namespace Light {
    public class LightPlayer : ModPlayer {
		public bool LightArmor = false;
		public int channeling = 0;
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

        public override void ResetEffects() {
			//pointsTotal = -PointsInUse;
			LightArmor = false;
            if(channeling>0) {
                channeling--;
            }
            if(shadeCure>0) {
                shadeCure--;
            }
            if(UltCD>0) {
                UltCD--;
            }
            if(Ult>0) {
                Ult--;
            }
			Ulting = Ult == 1;
            lightdamage = 1f;
            ascendtype = 0;
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
		public override TagCompound Save() {
            string versionString = mod.Version.ToString();
			return new TagCompound {
                { "LightColor", lightColor },
                { "PointsCollected", PointsCollected.Pack() },
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
            string a = player.name;
            string versionString = "0.0.0.0";
            try {
                versionString = tag.GetString("LastVer");
            } catch(Exception e) {
                if(e is IOException) {
                    try {
                        int[] vers = tag.Get<int[]>("LastVer");
                        versionString = $"{vers[0]}.{vers[1]}.{vers[2]}.{vers[3]}";
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
