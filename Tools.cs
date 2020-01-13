using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameInput;

namespace Light
{

    public static class Tools
    {
        //Constants

        public const float FullCircle = 2 * (float)Math.PI;


        //Vectors

        public static Vector2 Normalized(this Vector2 vector) //Shorthand for Terraria's SafeNormalize
        {
            return vector.SafeNormalize(Vector2.UnitX);
        }
		
        public static Vector2 OfLength(this Vector2 vector, float length) //Returns a vector of the same direction but with the provided absolute length
        {
            return vector.Normalized() * length;
        }


        //Random

        public static int RandomInt(int min, int max) //Inclusive min and max
        {
            return Main.rand.Next(min, max + 1);
        }
        public static int RandomInt(int max) //Exclusive max
        {
            return Main.rand.Next(max);
        }
		
        public static float RandomFloat(float min, float max) //Inclusive min and max
        {
            return (float)Main.rand.NextDouble() * (max - min) + min;
        }
        public static float RandomFloat(float max = 1)
        {
            return (float)Main.rand.NextDouble() * max;
        }
    }
}
