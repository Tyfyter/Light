using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Light {
    public static class Tools {
        ///<summary>
        ///Shorthand for Terraria's SafeNormalize with Vector2.UnitY as the default value
        ///</summary>
        public static Vector2 Normalized(this Vector2 vector) {
            return vector.SafeNormalize(Vector2.UnitY);
        }
        /// <summary>
        /// Returns a vector of the same direction but with the provided absolute length
        /// </summary>
        public static Vector2 OfLength(this Vector2 vector, float length) {
            return vector.Normalized() * length;
        }
        public static string GetHotkeyBinding(this ModHotKey hotkey, string fallback = "") {
            List<string> boundKeys = hotkey.GetAssignedKeys();
            return boundKeys.Count > 0 ? boundKeys[0] : fallback;
        }
    }
    public class BitSet {
        const uint UOne = 1;
        readonly HashSet<byte> bits;
        public bool this[byte i] {
            get {
                return bits.Contains(i);
            }
            set {
                switch(value) {
                    case true:
                    bits.Add(i);
                    break;
                    case false:
                    bits.Remove(i);
                    break;
                }
            }
        }
        public bool this[int i] {
            get {
                return bits.Contains((byte)i);
            }
            set {
                switch(value) {
                    case true:
                    bits.Add((byte)i);
                    break;
                    case false:
                    bits.Remove((byte)i);
                    break;
                }
            }
        }
        public BitSet(HashSet<byte> bits) {
            this.bits = bits;
        }
        public BitSet(uint bits) {
            HashSet<byte> output = new HashSet<byte>();
            byte i = 0;
            uint value = bits;
            while(value!=0) {
                if((value&1)==1) {
                    output.Add(i);
                }
                value >>= 1;
                i++;
            }
            this.bits = output;
        }
        public BitSet() {
            bits = new HashSet<byte>();
        }
        public static BitSet Zero => new BitSet();
        public uint Pack() {
            uint output = 0;
            foreach(byte i in bits){
                output |= (UOne<<i);
            }
            return output;
        }
        public static BitSet Unpack(uint bytes) {
            return new BitSet(bytes);
        }
    }
}
