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

namespace Light {
    public/*internal*/ class LightPlayer : ModPlayer {
		public int Points_Of_Light = 0;
		public bool LightArmor = false;
		public int channeling = 0;
		public int Ult = 0;
		public bool Ulting = false;
		public int UltCD = 0;
		public float LightStealth = 0;
		public float LightStealthMax = 0;
		public float LightStealthMaxPercent = 0;
		public int PointsFrom = 0;
		public int ShadeCure = 0;
		public int lightdamage = 1;
        public int ascendtype = 0;        
		public Color LightColor = new Color(255, 255, 255);
        public float meleedmgmult = 1;
        public bool phasing = false;
        public int PointsInUse = 0;
        public int tempcharge = 0;
        private Version LastVer;
        public override bool Autoload(ref string name) {
            return true;
        }

        public override void ResetEffects()
        {
			Points_Of_Light = -PointsInUse;
			LightArmor = false;
			channeling = Math.Max(channeling-1, 0);
			ShadeCure = Math.Max(ShadeCure-1, 0);
			UltCD = Math.Max(UltCD-1, 0);
			Ult = Math.Max(Ult-1, 0);
			Ulting = Ult == 1;
			LightStealth = Math.Min(LightStealth+1, LightStealthMax);
            lightdamage = 1;
            ascendtype = 0;
            meleedmgmult = 1;
			if(LightStealthMax != 0){
				player.aggro = (int)(player.aggro * (1-(LightStealth/LightStealthMax)));
				player.stealth = 1-((LightStealth/LightStealthMax)*LightStealthMaxPercent);
				player.shroomiteStealth = true;
			}
			LightStealthMax = 0;
			if (player.active)
			{
				for (int j = 0; j < player.inventory.Length; j++)
				{
					if (player.inventory[j].type == ItemType<Point_Of_Light>())
					{
						Points_Of_Light += player.inventory[j].stack;
						//Points_Of_Light++;
					}
				}
			}
        }
        public override void UpdateEquips(ref bool wallSpeedBuff, ref bool tileSpeedBuff, ref bool tileRangeBuff){
            for(int i = 0; i < 5; i++){
                IMiscEquip item = (player.miscEquips[i].modItem) as IMiscEquip;
                if(item!=null){
                    item.UpdateMisc(player);
                }
            }
        }
        public override bool ShiftClickSlot(Item[] inventory, int context, int slot){
            if(player.HeldItem.type == mod.GetItem("Light_Javelin").item.type && inventory[slot].type == ItemID.SoulofMight && inventory[slot].stack >= 20){
                //((Items.Soul_Light_Javelin)player.HeldItem).ascend(5);
                //ascendtype = 5;
                player.chatOverhead.NewMessage(inventory[slot].HoverName+":"+(ascendtype-1), 60);
                //player.HeldItem.modItem.ascend();
                return true;
            }
            return false;
        }
		public override TagCompound Save()
		{
			/*
			return new TagCompound
			{
				["LightColor"] = SplitColor(LightColor);
			};//*/
			return new TagCompound
			{
				{"LightColor",LightColor},
                {"PointsInUse",PointsInUse},
                {"PointsFrom",PointsFrom},
                {"LastVer",new int[]{mod.Version.Major,mod.Version.Minor,mod.Version.Build,mod.Version.Revision}}
				//{"LightColorR",LightColor.R},
				//{"LightColorG",LightColor.G},
				//{"LightColorB",LightColor.B}
				//["LightColor"] = SplitColor(LightColor);
			};
		}

		public override void Load(TagCompound tag)
		{
			LightColor = tag.GetTag<Color>("LightColor");
            PointsInUse = tag.GetTag<int>("PointsInUse");
            PointsFrom = tag.GetTag<int>("PointsFrom");
			//LightColor = new Color(tag.GetInt("LightColorR"),tag.GetInt("LightColorG"),tag.GetInt("LightColorB"));
			//LightColor = ReformColor(tag.GetString("LightColor"));
		}

		/*private String SplitColor(Color color){
			return color.R+","+color.G+","+color.B;
		}
		
		private Color ReformColor(String color){
			
			String[] SplitText = color.Split(new Char[] {','});
			//return new Color(ToInt32(SplitText[0]),ToInt32(SplitText[1]),ToInt32(SplitText[2]));
			return new Color(SplitText[0],SplitText[1],SplitText[2]);
		}*/
		/*private Color ReformColor(String color){
			
			String[] SplitText = color.Split(new Char[] {','});
			//return new Color(ToInt32(SplitText[0]),ToInt32(SplitText[1]),ToInt32(SplitText[2]));
			return new Color(SplitText[0],SplitText[1],SplitText[2]);
		}*/
        /*public override void ModifyHitByProjectile(Projectile proj, ref int damage, ref bool crit)
        {
            ElementalPlayer modPlayer = player.GetModPlayer<ElementalPlayer>(mod);
            if (modPlayer.LightArmor)
            {
                Main.projectile[proj.whoAmI].owner = player.whoAmI;
                Main.projectile[proj.whoAmI].hostile = false;
                Main.projectile[proj.whoAmI].friendly = true;
                if (Math.Abs(Main.projectile[proj.whoAmI].velocity.ToRotation() - (float)Math.Atan2((player.Center - Main.MouseWorld).Y, (player.Center - Main.MouseWorld).X)) <= 20)
                {
                    Vector2 vel = player.Center - Main.MouseWorld;
                    vel.Normalize();
                    vel *= -Main.projectile[proj.whoAmI].velocity.Length();
                    Main.projectile[proj.whoAmI].velocity = vel;
                }
                //Main.projectile[proj.whoAmI].velocity *= -1;
                damage = 0;
                crit = true;
                player.immuneTime = 0;
                player.immune = false;
            }
        }

        public override bool CanBeHitByProjectile(Projectile proj)
        {
            ElementalPlayer modPlayer = player.GetModPlayer<ElementalPlayer>(mod);
            if (modPlayer.LightArmor)
            {
                return false;
            }
            return base.CanBeHitByProjectile(proj);
        }
		*/
        public override bool? CanHitNPC(Item item, NPC target)
        {
            if (target.type == NPCID.Bunny || target.type == NPCID.BunnySlimed || target.type == NPCID.BunnyXmas || target.type == NPCID.GoldBunny || target.type == NPCID.PartyBunny || target.type == NPCID.CorruptBunny || target.type == NPCID.CrimsonBunny)
            {
                return false;
            }
            return base.CanHitNPC(item, target);
        }

        public override bool? CanHitNPCWithProj(Projectile proj, NPC target)
        {
            if (target.type == NPCID.Bunny || target.type == NPCID.BunnySlimed || target.type == NPCID.BunnyXmas || target.type == NPCID.GoldBunny || target.type == NPCID.PartyBunny || target.type == NPCID.CorruptBunny || target.type == NPCID.CrimsonBunny)
            {
                return false;
            }
            return base.CanHitNPCWithProj(proj, target);
        }

        public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot)
        {
            if(npc.type == NPCID.CorruptBunny || npc.type == NPCID.CrimsonBunny)
            {
                return false;
            }
            return base.CanBeHitByNPC(npc, ref cooldownSlot);
        }
		
		/*
		public override void ModifyHitByNPC(NPC npc, int damage, bool crit){
			if (npc.HasBuff(BuffType<Umbra>()) && Main.rand.Next(2) == 0)
            {
				player.AddBuff(BuffType<ShadeDebuff>(), 900);
			}
		}//*/
		/*
        public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
        {
            Main.NewText(stone + "");
            if (stone)
            {
                npc.life -= (int)(player.velocity - npc.velocity).Length();
                damage = 0;
            }
        }
		*/
		public override void PostUpdateEquips()
		{
			/*if(winddebuffed){
				player.noKnockback = false;
				player.autoJump = false;
			}
			if(waterdebuffed){
				player.buffImmune[4] = true;
				player.buffImmune[34] = true;
				player.merman = false;
				player.buffImmune[69] = false;
				player.buffImmune[70] = false;
				player.wingTimeMax = (int)(player.wingTimeMax/4);
			}
			player.rangedCrit = (int)(player.rangedCrit * multiplyCrit);
			player.magicCrit = (int)(player.magicCrit * multiplyCrit);
			player.thrownCrit = (int)(player.thrownCrit * multiplyCrit);
			player.meleeCrit = (int)(player.meleeCrit * multiplyCrit);*/
            player.meleeDamage *= meleedmgmult;
		}

        /*public override void ModifyScreenPosition()
        {
            base.ModifyScreenPosition();
        }
        /*public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource) {
            if(((CheatHotkeys)mod).GodMode) {
                return false;
            }

            return base.PreHurt(pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref damageSource);
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource) {
            if(((CheatHotkeys)mod).GodMode) {
                return false;
            }

            return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genGore, ref damageSource);
        }

        public override void PreUpdateBuffs() {
            CheatHotkeys chmod = (CheatHotkeys)mod;

            if(chmod.GodMode) {
                chmod.RemoveDebuffs();
                player.statMana = player.statManaMax;
            }

            base.PreUpdateBuffs();
        }

        public override bool CanBeHitByProjectile(Projectile proj) {
            if(((CheatHotkeys)mod).GodMode) {
                return false;
            }

            return base.CanBeHitByProjectile(proj);
        }

        public override bool ConsumeAmmo(Item weapon, Item ammo) {
            if(((CheatHotkeys)mod).UnlimitedAmmo) {
                return false;
            }

            return base.ConsumeAmmo(weapon, ammo);
        }*/
    }
}
