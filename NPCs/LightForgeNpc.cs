using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Light.Items;

namespace Light.NPCs            //We need this to basically indicate the folder where it is to be read from, so you the texture will load correctly
{
    public class LightForgeNpc : ModNPC
    {
        /*public override bool Autoload(ref string name, ref string texture, ref string[] altTextures)
        {
            name = "Custom Town NPC";
            return mod.Properties.Autoload;
        }//*/
        public override bool CloneNewInstances => true;
        public Color color = Color.White;
        public LightPlayer owner;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Light Forge");
            Main.npcFrameCount[npc.type] = 1; //this defines how many frames the npc sprite sheet has
            NPCID.Sets.ExtraFramesCount[npc.type] = 9;
            NPCID.Sets.AttackFrameCount[npc.type] = 4;
            NPCID.Sets.DangerDetectRange[npc.type] = 150; //this defines the npc danger detect range
            NPCID.Sets.AttackType[npc.type] = 2; //this is the attack type,  0 (throwing), 1 (shooting), or 2 (magic). 3 (melee)
            NPCID.Sets.AttackTime[npc.type] = 30; //this defines the npc attack speed
            NPCID.Sets.AttackAverageChance[npc.type] = 100;//this defines the npc atack chance
		}

        public override void SetDefaults()
        {
            //npc.name = "Light Forge";   //the name displayed when hovering over the npc ingame.
            npc.townNPC = true; //This defines if the npc is a town Npc or not
            npc.friendly = true;  //this defines if the npc can hur you or not()
            npc.width = 18; //the npc sprite width
            npc.height = 46;  //the npc sprite height
            npc.aiStyle = 0; //this is the npc ai style, 7 is Pasive Ai
            npc.defense = 25;  //the npc defense
            npc.lifeMax = 250;// the npc life
			npc.noGravity = true;
			npc.dontTakeDamage = true;
            npc.HitSound = SoundID.NPCHit1;  //the npc sound when is hit
            npc.DeathSound = SoundID.NPCDeath1;  //the npc sound when he dies
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
            try{
                color = owner.lightColor;
            }catch(Exception){
				npc.life = 0;
            }
			Lighting.AddLight(npc.Center, color.R/255, color.G/255, color.B/255);
		}
        public override bool CanTownNPCSpawn(int numTownNPCs, int money) //Whether or not the conditions have been met for this town NPC to be able to move into town.
        {
            return false;
        }
        public override bool CheckConditions(int left, int right, int top, int bottom)    //Allows you to define special conditions required for this town NPC's house
        {
            return false;  //so when a house is available the npc will  spawn
        }
        public override string TownNPCName()     //Allows you to give this town NPC any name when it spawns
        {
            try{
                return owner.player.name+"'s Light Forge";
            }catch (Exception){
				npc.life = 0;
                return "Light Forge";
            }
        }

        public override void SetChatButtons(ref string button, ref string button2)  //Allows you to set the text for the buttons that appear on this town NPC's chat window.
        {
            button = "Forge";   //this defines the buy button name
            button2 = "Dispel";   //this defines the buy button name
        }
        public override void OnChatButtonClicked(bool firstButton, ref bool openShop) //Allows you to make something happen whenever a button is clicked on this town NPC's chat window. The firstButton parameter tells whether the first button or second button (button and button2 from SetChatButtons) was clicked. Set the shop parameter to true to open this NPC's shop.
        {
            if (firstButton)
            {
                openShop = true;   //so when you click on buy button opens the shop
            }else{
				npc.life = 0;
			}
        }

        public override void SetupShop(Chest shop, ref int nextSlot)
        {
            /*if (NPC.downedSlimeKing)   //this make so when the king slime is killed the town npc will sell this
            {
                shop.item[nextSlot].SetDefaults(ItemID.RecallPotion);  //an example of how to add a vanilla terraria item
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ItemID.WormholePotion);
                nextSlot++;
            }
            if (NPC.downedBoss3)   //this make so when Skeletron is killed the town npc will sell this
            {
                shop.item[nextSlot].SetDefaults(ItemID.BookofSkulls);
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ItemID.ClothierVoodooDoll);
                nextSlot++;
            }
            shop.item[nextSlot].SetDefaults(ItemID.IronskinPotion);
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ItemType<CustomSword>());  //this is an example of how to add a modded item
            nextSlot++;*/

            shop.item[nextSlot].SetDefaults(ItemType<Light_Drill>());      //
            nextSlot++;
            //if (Main.LocalPlayer.GetModPlayer<LightPlayer>().pointsTotal >= 2){
				shop.item[nextSlot].SetDefaults(ItemType<Light_Staff>());      //
                nextSlot++;
            //}
            //if (Main.LocalPlayer.GetModPlayer<LightPlayer>().pointsTotal >= 4){
				shop.item[nextSlot].SetDefaults(ItemType<Light_Dagger>());      //
                nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemType<Light_Javelin>());      //
                nextSlot++;
            //}

        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor){
            Dust d = Dust.NewDustPerfect(npc.Center, 267, null, 0, color, 0.8f);
            d.noGravity = true;
            return false;
        }
        public override string GetChat()       //Allows you to give this town NPC a chat message when a player talks to it.
        {
            return " ";
        }
        public override void TownNPCAttackStrength(ref int damage, ref float knockback)//  Allows you to determine the damage and knockback of this town NPC attack
        {
            damage = 40;  //npc damage
            knockback = 2f;   //npc knockback
        }
		//*
        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)  //Allows you to determine the cooldown between each of this town NPC's attack. The cooldown will be a number greater than or equal to the first parameter, and less then the sum of the two parameters.
        {
            cooldown = 5;
            randExtraCooldown = 10;
        }/*
        //------------------------------------This is an example of how to make the npc use a sward attack-------------------------------
        public override void DrawTownAttackSwing(ref Texture2D item, ref int itemSize, ref float scale, ref Vector2 offset)//Allows you to customize how this town NPC's weapon is drawn when this NPC is swinging it (this NPC must have an attack type of 3). Item is the Texture2D instance of the item to be drawn (use Main.itemTexture[id of item]), itemSize is the width and height of the item's hitbox
        {
            scale = 1f;
            item = Main.itemTexture[ItemType<CustomSword>()]; //this defines the item that this npc will use
            itemSize = 56;
        }

        public override void TownNPCAttackSwing(ref int itemWidth, ref int itemHeight) //  Allows you to determine the width and height of the item this town NPC swings when it attacks, which controls the range of this NPC's swung weapon.
        {
            itemWidth = 56;
            itemHeight = 56;
        }//*/

        //----------------------------------This is an example of how to make the npc use a gun and a projectile ----------------------------------
        public override void DrawTownAttackGun(ref float scale, ref int item, ref int closeness) //Allows you to customize how this town NPC's weapon is drawn when this NPC is shooting (this NPC must have an attack type of 1). Scale is a multiplier for the item's drawing size, item is the ID of the item to be drawn, and closeness is how close the item should be drawn to the NPC.
          {
            scale = 0f;
            item = ItemType<LightI>();
            closeness = 20;
          }
          public override void TownNPCAttackProj(ref int projType, ref int attackDelay)//Allows you to determine the projectile type of this town NPC's attack, and how long it takes for the projectile to actually appear
          {
            projType = 521;
            attackDelay = 1;
          }

          public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)//Allows you to determine the speed at which this town NPC throws a projectile when it attacks. Multiplier is the speed of the projectile, gravityCorrection is how much extra the projectile gets thrown upwards, and randomOffset allows you to randomize the projectile's velocity in a square centered around the original velocity
          {
            multiplier = 7f;
            // randomOffset = 4f;
          }

    }
}