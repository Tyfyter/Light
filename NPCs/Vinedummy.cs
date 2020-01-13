using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace Light.NPCs            //We need this to basically indicate the folder where it is to be read from, so you the texture will load correctly
{
    public class Vinedummy : ModNPC
    {
        /*public override bool Autoload(ref string name, ref string texture, ref string[] altTextures)
        {
            name = "Custom Town NPC";
            return mod.Properties.Autoload;
        }//*/
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("");
		}
		
        public override void SetDefaults()
        {
            //npc.name = "Light Forge";   //the name displayed when hovering over the npc ingame.
            npc.width = 18; //the npc sprite width
            npc.height = 46;  //the npc sprite height
            npc.aiStyle = 0; //this is the npc ai style, 7 is Pasive Ai
            npc.defense = 25;  //the npc defense
            npc.lifeMax = 30;// the npc life
			npc.noGravity = true;
			npc.dontTakeDamage = true;
            npc.HitSound = null;  //the npc sound when is hit
            npc.DeathSound = null;  //the npc sound when he dies
            npc.knockBackResist = 1f;  //the npc knockback resistance
            /*Main.npcFrameCount[npc.type] = 2; //this defines how many frames the npc sprite sheet has
            NPCID.Sets.ExtraFramesCount[npc.type] = 9;
            NPCID.Sets.AttackFrameCount[npc.type] = 4;
            NPCID.Sets.DangerDetectRange[npc.type] = 150; //this defines the npc danger detect range
            NPCID.Sets.AttackType[npc.type] = 2; //this is the attack type,  0 (throwing), 1 (shooting), or 2 (magic). 3 (melee) 
            NPCID.Sets.AttackTime[npc.type] = 30; //this defines the npc attack speed
            NPCID.Sets.AttackAverageChance[npc.type] = 100;//this defines the npc atack chance
            NPCID.Sets.HatOffsetY[npc.type] = 4; //this defines the party hat position
            animationType = NPCID.Guide;  //this copy the guide animation*/
        }
		public override void AI(){
			Lighting.AddLight(npc.Center, Color.DarkCyan.R/100, Color.DarkCyan.G/50, Color.DarkCyan.B/100);
            npc.life--;
            npc.checkDead();
		}

    }
}