using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

//this item name shall be a monument of my refusal to name a soul item something that doesn't end in -ite for eternity.
namespace Light.Items
{
	public class SoulOfInosite : ModItem
	{
		int rng;
		int time = 0;
		public override bool CloneNewInstances => true;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul Of Inosite");
			Tooltip.SetDefault("");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 4));
			ItemID.Sets.AnimatesAsSoul[item.type] = true;
			ItemID.Sets.ItemIconPulse[item.type] = true;
			ItemID.Sets.ItemNoGravity[item.type] = true;
		}
		public override void SetDefaults()
		{
			rng = Main.rand.Next(0,25);
			Item refItem = new Item();
			refItem.SetDefaults(ItemID.SoulofSight);
			item.width = refItem.width;
			item.height = refItem.height;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = 0;
			item.knockBack = 6;
			item.value = 0;
			item.rare = 2;
            item.maxStack = 999;
			item.noGrabDelay = 0;
		}
		public override bool OnPickup(Player player){
			rng = Main.rand.Next(0,25);
			time = 0;
			return true;
		}
		public override void GrabRange(Player player, ref int grabRange)
		{
			grabRange *= 5;
		}
		/*public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float  scale, int whoAmI)
		{
			Texture2D texture = Main.itemTexture[item.type];
			Main.spriteBatch.Draw(Main.itemTexture[item.type], new Vector2(item.position.X - Main.screenPosition.X + item.width * 0.5f, item.position.Y - Main.screenPosition.Y + item.height - texture.Height * 0.5f + 2f), new Rectangle(0, 0, texture.Width, texture.Height), Color.DarkCyan, rotation, texture.Size() * 0.5f,scale, SpriteEffects.None, 0f);
		}*/
		public override bool GrabStyle(Player player)
		{
			time+=2;
			Vector2 vectorItemToPlayer = player.Center - item.Center;
			Vector2 movement = -vectorItemToPlayer.SafeNormalize(default(Vector2)) * 0.1f;
			//item.velocity = item.velocity + movement;
			//item.velocity = Collision.TileCollision(item.position, item.velocity, item.width, item.height);
			//player.velocity = player.velocity + movement*10;
			if(time == 60 || time == 61){
				int b = NPC.NewNPC((int)item.position.X, (int)item.position.Y, mod.GetNPC("Vinedummy").npc.type);
				int a = Projectile.NewProjectile(item.position, movement*-25, ProjectileType<VineHookProjectile2>(), 25+(player.statDefense/3), 0, 0, b);
				Main.projectile[a].friendly = false;
				Main.projectile[a].hostile = true;
				time = Main.rand.Next(-60, 0);
			}

			if(time >= 60){
				time = Main.rand.Next(-60, 0);
			}
			return true;
		}

		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI){
			if(time > 0){
				time--;
			}
			if(rng == 25){
				Lighting.AddLight(item.position, Color.DarkCyan.R/100, Color.DarkCyan.G/50, Color.DarkCyan.B/100);
			}else{
				Lighting.AddLight(item.position, Color.DarkCyan.R/250, Color.DarkCyan.G/125, Color.DarkCyan.B/250);
			}
			return base.PreDrawInWorld(spriteBatch, Color.White, alphaColor, ref rotation, ref scale, whoAmI);
			//mod.GetPrefix("").
		}
	}
}
