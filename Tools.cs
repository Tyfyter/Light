using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
        public static HotkeyState GetHotkeyState(this ModHotKey hotkey) {
            return (hotkey.Current?HotkeyState.JustPressed:0)|(hotkey.Old?HotkeyState.JustReleased:0);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LinearSmoothing(ref float smoothed, float target, float rate) {
            if(target!=smoothed) {
                if(Math.Abs(target-smoothed)<rate) {
                    smoothed = target;
                } else {
                    if(target>smoothed) {
                        smoothed+=rate;
                    }else if(target<smoothed) {
                        smoothed-=rate;
                    }
                }
            }
        }
        public static bool ItemExists(Item item) {
            return !(item?.IsAir ?? true);
        }
        public static Item ItemFromID(int id) {
            Item item = new Item();
            item.SetDefaults(id);
            return item;
        }
        public static T[] WithLength<T>(this T[] input, int length) {
            T[] output = new T[length];
            if(length>input.Length) {
                length = input.Length;
            }
            for(int i = 0; i < length; i++) {
                output[i] = input[i];
            }
            return output;
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
    public enum HotkeyState {
        Up,
        JustPressed,
        JustReleased,
        Held
    }
}
