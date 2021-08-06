using System;
using System.Collections.Generic;
using Light.Buffs;
using Light.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Light.NPCs {
	public partial class LightGlobalNPC : GlobalNPC {
		public override bool InstancePerEntity => true;
		public override bool CloneNewInstances => true;
		public List<Player> debuffed = new List<Player>() {};
		internal knockBack knockBack = new knockBack();
		public static void SetKBTime(NPC npc, int time) {
			LightGlobalNPC gNPC = npc.GetGlobalNPC<LightGlobalNPC>();
			Main.NewText($"1: {gNPC.knockBack}");
			if(time == 0) {
				Main.NewText("path 0");
				if(gNPC.knockBack.dur>0)gNPC.knockBack.dur = 1;
			} else {
				Main.NewText("path 1");
				if(gNPC.knockBack.dur == 0) {
					Main.NewText("path 2");
					gNPC.knockBack.KB = npc.knockBackResist;
				}
				npc.knockBackResist = 1;
				gNPC.knockBack.dur = time;
			}
			Main.NewText($"2: {gNPC.knockBack}");
		}
		public override void AI(NPC npc) {
			base.AI(npc);
			if(npc.HasBuff(BuffType<Blind>())) {
				npc.target = -1;
			}
			if(knockBack.dur>0) {
				//npc.knockBackResist = 1;
				if(--knockBack.dur==0) {
					//Main.NewText($" {npc.knockBackResist}; {knockBack.KB}");
					npc.knockBackResist = knockBack.KB;
					//Main.NewText($" {npc.knockBackResist}; {knockBack.KB}");
				}
				//Main.NewText($" {npc.knockBackResist}; {knockBack.KB}");
				//Main.NewText($" {knockBack}");
			}
		}
		public override void SpawnNPC(int type, int tileX, int tileY) {
			//int a = base.SpawnNPC(type, tileX, tileY);
			NPC npc = Main.npc[type];
			if (Main.rand.Next(74) == 0) {
				Light.ApplyLuxBoosts(ref npc);
				//npc.DisplayName.set("Lux "+npc.DisplayName.Get());
				//npc.AddBuff(BuffType<Lux>(), 600);
				for(int i = 0; i < Main.player.Length; i++) {
					Main.player[i].chatOverhead.NewMessage("!", 30);
				}
				//Main.NewText("!");
			} else if (Main.rand.Next(99) == 0) {
				Light.ApplyShadeBoosts(ref npc);
				//npc.DisplayName.set("Umbra "+npc.DisplayName.Get());
				//npc.AddBuff(BuffType<Umbra>(), 600);
				//npc.music = 0;
				for(int i = 0; i < Main.player.Length; i++) {
					Main.player[i].chatOverhead.NewMessage("!!!", 30);
				}
				//Main.NewText("!!!");
			}
		}//*/

        public override void DrawEffects(NPC npc, ref Color drawColor) {
            if (npc.HasBuff(BuffType<Lux>())) {
                /*if (Main.rand.Next(2) == 0) {
                    int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, DustID.Fire, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default(Color), 3.5f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1.8f;
                    Main.dust[dust].velocity.Y -= 0.5f;
                }*/
                Lighting.AddLight(npc.position, 0.7f, 0.7f, 0.7f);
				drawColor = new Color(drawColor.R+75,drawColor.G+75,drawColor.B+75,drawColor.A-125);
            }
            if (npc.HasBuff(BuffType<Umbra>())) {
                /*if (Main.rand.Next(2) == 0) {
                    int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, DustID.Fire, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default(Color), 3.5f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1.8f;
                    Main.dust[dust].velocity.Y -= 0.5f;
                }*/
                //Lighting.AddLight(npc.position, -0.7f, -0.7f, -0.7f);
				drawColor = new Color(drawColor.R-75,drawColor.G-75,drawColor.B-75,drawColor.A-175);
            }
        }
		public override void OnHitPlayer(NPC npc, Player target, int damage, bool crit) {
			if (npc.HasBuff(BuffType<Umbra>()) && Main.rand.Next(2) == 0) {
				debuffed.Add(target);
				target.AddBuff(BuffType<ShadeDebuff>(), 900);
			}
		}
		public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit) {
			if(item.type >= Terraria.ID.ItemID.Count) {
				if(item.modItem.mod == Light.mod && npc.HasBuff(BuffType<Umbra>())) {
					damage = (int)(damage*1.5f);
					crit = true;
					if(Main.rand.Next(2) == 0) {
						npc.AddBuff(BuffType<DisabledTempDebuff>(), 300);
					}
				}
			}
		}
		public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) {
			if(projectile.type >= Terraria.ID.ProjectileID.Count) {
				if (projectile.modProjectile.mod == Light.mod && npc.HasBuff(BuffType<Umbra>())) {
					damage = (int)(damage*1.5f);
					crit = true;
					if(Main.rand.Next(2) == 0) {
						npc.AddBuff(BuffType<DisabledTempDebuff>(), 300);
					}
				}
			}
		}
		/*
		public override void OnHitPlayer(Player target, int damage, bool crit) {
			if (npc.HasBuff(BuffType<Umbra>()) && Main.rand.Next(2) == 0) {
				target.AddBuff(BuffType<ShadeDebuff>(), 900);
			}
		}//*/
	}
	internal class knockBack {
		internal int dur = 0;
		internal float KB = 0;
		public override string ToString() {
			return $"KB: {KB} time: {dur}";
		}
	}
}