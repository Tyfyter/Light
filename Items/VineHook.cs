using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using static Terraria.ModLoader.ModContent;

namespace Light.Items
{
	class VineHook : ModItem
	{
		public override bool CloneNewInstances{
			get { return true; }
		}
		public override void SetDefaults()
        {
            /*Player player = Main.player[item.owner];
            ElementalPlayer modPlayer = player.GetModPlayer<ElementalPlayer>(mod);
            /*
				this.noUseGraphic = true;
				this.damage = 0;
				this.knockBack = 7f;
				this.useStyle = 5;
				this.name = "Amethyst Hook";
				this.shootSpeed = 10f;
				this.shoot = 230;
				this.width = 18;
				this.height = 28;
				this.useSound = 1;
				this.useAnimation = 20;
				this.useTime = 20;
				this.rare = 1;
				this.noMelee = true;
				this.value = 20000;
			*/
            // Instead of copying these values, we can clone and modify the ones we want to copy
            item.CloneDefaults(ItemID.AmethystHook);
			//item.name = "Wind Hook";
			item.damage = 100;
			item.crit = 4;
			item.shootSpeed = 25f; // how quickly the hook is shot.
			item.noUseGraphic = true;
			item.useStyle = 5;
            //item.toolTip = modPlayer.pullhook + "";
            item.shoot = ProjectileType<VineHookProjectile>();
		}
		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Vine Hook");
		  Tooltip.SetDefault("Forged from the soul of the jungle.");
		}
        public override void UpdateInventory(Player player){
			player.chatOverhead.NewMessage("ham", 5);
		}
		public override void UpdateEquip(Player player){
			item.damage = 1;
		}
		public override void GetWeaponDamage(Player player, ref int damage){
			float[] damagearray = new float[4] {player.meleeDamage,player.rangedDamage,player.magicDamage,player.thrownDamage};
			Array.Sort(damagearray);

			damage = (int)(damage * damagearray[0]);
		}
		public override void GetWeaponCrit(Player player, ref int crit){
			int[] critarray = new int[4] {player.meleeCrit,player.rangedCrit,player.magicCrit,player.thrownCrit};
			Array.Sort(critarray);
			crit+=critarray[0];
		}

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<SoulOfInosite>(), 25);
            recipe.AddRecipeGroup("Light:Vines", 20);
            recipe.AddIngredient(ItemID.Hook, 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

		/*
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack){
			int hooksOut = 0;
			for (int l = 0; l < Main.projectile.Length; l++)
			{
				if (Main.projectile[l].active && Main.projectile[l].owner == Main.myPlayer && Main.projectile[l].type == projectile.type)
				{
					hooksOut++;
					if (hooksOut == 3) // This hook can have 1 hook out.
					{
						projectile.tileCollide = false;
					}
				}
			}
			return true;
		}//*/
		

    }
	class VineHookProjectile : ModProjectile
	{
		bool hashit = false;
		public override void SetDefaults()
		{
			/*	this.netImportant = true;
				this.name = "Gem Hook";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 7;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.timeLeft *= 10;
			*/
			projectile.CloneDefaults(ProjectileID.GemHookAmethyst);
			projectile.restrikeDelay /= 2;
		}

		// Use this hook for hooks that can have multiple hooks midflight: Dual Hook, Web Slinger, Fish Hook, Static Hook, Lunar Hook
		public override bool? CanUseGrapple(Player player)
		{
			int hooksOut = 0;
			for (int l = 0; l < 1000; l++)
			{
				if (Main.projectile[l].active && Main.projectile[l].owner == Main.myPlayer && Main.projectile[l].type == projectile.type)
				{
					hooksOut++;
					if (hooksOut == 3) // This hook can have 1 hook out.
					{
						Main.projectile[l].tileCollide = false;
					}
				}
			}
            if (hooksOut > 2) // This hook can have 1 hook out.
            {
                Vector2 trgtvel = player.position - projectile.position;
                trgtvel.Normalize();
                Main.npc[(int)projectile.ai[0]].velocity = -3*trgtvel;
				return false;
			}
			return true;
		}

		// Return true if it is like: Hook, CandyCaneHook, BatHook, GemHooks
		//public override bool? SingleGrappleHook(Player player)
		//{
		//	return true;
		//}

		// Use this to kill oldest hook. For hooks that kill the oldest when shot, not when the newest latches on: Like SkeletronHand
		// You can also change the projectile likr: Dual Hook, Lunar Hook
		//public override void UseGrapple(Player player, ref int type)
		//{
		//	int hooksOut = 0;
		//	int oldestHookIndex = -1;
		//	int oldestHookTimeLeft = 100000;
		//	for (int i = 0; i < 1000; i++)
		//	{
		//		if (Main.projectile[i].active && Main.projectile[i].owner == projectile.whoAmI && Main.projectile[i].type == projectile.type)
		//		{
		//			hooksOut++;
		//			if (Main.projectile[i].timeLeft < oldestHookTimeLeft)
		//			{
		//				oldestHookIndex = i;
		//				oldestHookTimeLeft = Main.projectile[i].timeLeft;
		//			}
		//		}
		//	}
		//	if (hooksOut > 1)
		//	{
		//		Main.projectile[oldestHookIndex].Kill();
		//	}
		//}

		// Amethyst Hook is 300, Static Hook is 600
		public override float GrappleRange()
		{
			return 800f;
		}

		public override void NumGrappleHooks(Player player, ref int numHooks)
		{
			numHooks = 2;
		}

		// default is 11, Lunar is 24
		public override void GrappleRetreatSpeed(Player player, ref float speed)
		{
			speed = 20f;
		}

		public override void GrapplePullSpeed(Player player, ref float speed)
		{
			speed = 35f;
		}
		
		public override void AI(){
			int hooksOut = 0;
			for (int l = 0; l < 1000; l++)
			{
				if (Main.projectile[l].active && Main.projectile[l].owner == Main.myPlayer && Main.projectile[l].type == projectile.type)
				{
					hooksOut++;
					if (hooksOut >= 3) // This hook can have 1 hook out.
					{
						Main.projectile[l].tileCollide = false;
					}
				}
			}
			for(int i = 0; i < Main.item.Length; i++){
				if((Main.item[i].position - projectile.position).Length() <= 80){
					Vector2 thatvector = (projectile.Center - Main.item[i].Center);
					Vector2 thisstaticvector = projectile.Center - Main.item[i].Center;
					thisstaticvector = thisstaticvector.Normalized() * 8;
					thisstaticvector = new Vector2(Math.Min(thatvector.X, thisstaticvector.X), Math.Min(thatvector.Y, thisstaticvector.Y));
					Main.item[i].velocity = (thatvector/5.5f) + thisstaticvector;
				}
			}
			if(projectile.ai[0]==2)projectile.Kill();
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 playerCenter = Main.player[projectile.owner].MountedCenter;
			Vector2 center = projectile.Center;
			if(projectile.hostile){
				playerCenter = Main.npc[projectile.owner].Center;
			}
			Vector2 distToProj = playerCenter - projectile.Center;
			float projRotation = distToProj.ToRotation() - 1.57f;
			float distance = distToProj.Length();
			while (distance > 30f && !float.IsNaN(distance))
			{
				distToProj.Normalize();                 //get unit vector
				distToProj *= 24f;                      //speed = 24
				center += distToProj;                   //update draw position
				distToProj = playerCenter - center;    //update distance
				distance = distToProj.Length();
				Color drawColor = lightColor;

				//Draw chain
				spriteBatch.Draw(mod.GetTexture("Items/VineHookChain"), new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
					new Rectangle(0, 0, Main.chain30Texture.Width, Main.chain30Texture.Height), drawColor, projRotation,
					new Vector2(Main.chain30Texture.Width * 0.5f, Main.chain30Texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
			}
			return true;
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection){
			damage+=target.defense/2;
			if(crit){
				damage+=target.defense/2;
				if(target.takenDamageMultiplier > 0 && target.takenDamageMultiplier < 1)damage = (int)(damage / target.takenDamageMultiplier);
			}
		}

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			target.velocity = -projectile.velocity;
			Main.player[projectile.owner].AddBuff(mod.GetBuff("VineHookBuff").Type, 90);
			target.AddBuff(BuffID.Venom, 300);
			if(target.catchItem != 0){
				target.position = projectile.Center - new Vector2(target.width/2, target.height/2);
				Item.NewItem(target.Center, new Vector2(), target.catchItem);
				target.HitEffect(0 , target.realLife);
			}
        }
		public override void OnHitPlayer(Player target, int damage, bool crit){
			/*if(projectile.damage != 0){
				projectile.velocity *= -0.25f;
				projectile.damage = 0;
			}*/
			if(!hashit){
				projectile.velocity *= -0.25f;
				projectile.damage /= 10;
				hashit = true;
			}
			target.velocity = projectile.velocity;
		}
    }
	class VineHookProjectile2 : ModProjectile
	{
		public override void SetDefaults()
		{
			/*	this.netImportant = true;
				this.name = "Gem Hook";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 7;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.timeLeft *= 10;
			*/
			projectile.CloneDefaults(ProjectileID.GemHookAmethyst);
			projectile.aiStyle = 0;
			projectile.extraUpdates = 4;
			projectile.timeLeft = 120;
			projectile.penetrate = -1;
		}

		public override void AI(){
            projectile.rotation = projectile.velocity.ToRotation();
		}


		// Return true if it is like: Hook, CandyCaneHook, BatHook, GemHooks
		//public override bool? SingleGrappleHook(Player player)
		//{
		//	return true;
		//}

		// Use this to kill oldest hook. For hooks that kill the oldest when shot, not when the newest latches on: Like SkeletronHand
		// You can also change the projectile likr: Dual Hook, Lunar Hook
		//public override void UseGrapple(Player player, ref int type)
		//{
		//	int hooksOut = 0;
		//	int oldestHookIndex = -1;
		//	int oldestHookTimeLeft = 100000;
		//	for (int i = 0; i < 1000; i++)
		//	{
		//		if (Main.projectile[i].active && Main.projectile[i].owner == projectile.whoAmI && Main.projectile[i].type == projectile.type)
		//		{
		//			hooksOut++;
		//			if (Main.projectile[i].timeLeft < oldestHookTimeLeft)
		//			{
		//				oldestHookIndex = i;
		//				oldestHookTimeLeft = Main.projectile[i].timeLeft;
		//			}
		//		}
		//	}
		//	if (hooksOut > 1)
		//	{
		//		Main.projectile[oldestHookIndex].Kill();
		//	}
		//}

		// Amethyst Hook is 300, Static Hook is 600

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			//float GrappleRange = 600f;
			Vector2 playerCenter = Main.npc[(int)projectile.ai[0]].Center;
			Vector2 center = projectile.Center;
			Vector2 distToProj = playerCenter - projectile.Center;
			float projRotation = distToProj.ToRotation() - 1.57f;
			float distance = distToProj.Length();
			while (distance > 30f && !float.IsNaN(distance))
			{
				distToProj.Normalize();                 //get unit vector
				distToProj *= 24f;                      //speed = 24
				center += distToProj;                   //update draw position
				distToProj = playerCenter - center;    //update distance
				distance = distToProj.Length();
				Color drawColor = lightColor;

				//Draw chain
				spriteBatch.Draw(mod.GetTexture("Items/VineHookChain"), new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
					new Rectangle(0, 0, Main.chain30Texture.Width, Main.chain30Texture.Height), drawColor, projRotation,
					new Vector2(Main.chain30Texture.Width * 0.5f, Main.chain30Texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
			}
			return true;
		}

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			target.velocity = -projectile.velocity;
        }
		public override void OnHitPlayer(Player target, int damage, bool crit){
			if(projectile.damage != 0){
				projectile.velocity *= -2f;
				projectile.damage = 0;
			}
			target.velocity = projectile.velocity*2.5f;
			projectile.timeLeft += 10;
		}
    }

	// Animated hook example
	// Multiple, 
	// only 1 connected, spawn mult
	// Light the path
	// Gem Hooks: 1 spawn only
	// Thorn: 4 spawns, 3 connected
	// Dual: 2/1 
	// Lunar: 5/4 -- Cycle hooks, more than 1 at once
	// AntiGravity -- Push player to position
	// Static -- move player with keys, don't pull to wall
	// Christmas -- light ends
	// Web slinger -- 9/8, can shoot more than 1 at once
	// Bat hook -- Fast reeling

}
