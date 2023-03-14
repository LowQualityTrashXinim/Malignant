﻿using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Malignant.Content.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.Items.Misc.WarbladeofLight
{
    public class WarbladeThrow : ModProjectile
    {
        public override string Texture => "Malignant/Content/Items/Misc/WarbladeofLight/WarbladeofLight";

        private int framereset;
        private int timer;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.damage = 3;
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.scale = 1f;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            float rotation = Projectile.rotation;

            timer++;
            if (timer == 1)
            {
                Projectile.velocity *= 20;
            }
            if (timer <= 15)
            {
                Projectile.velocity *= 0.87f;
            }
            if (Projectile.frame == 1)
            {
                Vector3 RGB = new Vector3(2.51f, 1.83f, 0.65f);
                float multiplier = 1;
                float max = 2.25f;
                float min = 1.0f;
                RGB *= multiplier;
                if (RGB.X > max)
                {
                    multiplier = 0.5f;
                }
                if (RGB.X < min)
                {
                    multiplier = 1.5f;
                }
                Lighting.AddLight(Projectile.position, RGB.X, RGB.Y, RGB.Z);
                for (int j = 0; j < 3; j++)
                {
                    Vector2 speed = Main.rand.NextVector2Circular(1f, 1f);
                    Dust.NewDustPerfect(Projectile.Center, DustID.GoldCoin, speed * 10, 0, default, 0.7f);
                }
            }

            Projectile.rotation += (Projectile.velocity.Length() * 0.04f) * Projectile.direction;
            if (timer >= 15)
            {
                Projectile.velocity = Projectile.DirectionTo(player.Center) * 20;

            }

            if (Projectile.Hitbox.Intersects(player.Hitbox) && timer >= 15)
            {
                Projectile.Kill();

            }
            player.heldProj = Projectile.whoAmI;
            player.ChangeDir(Projectile.velocity.X < 0 ? -1 : 1);
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.itemRotation = rotation * player.direction;
            Projectile.netUpdate = true;

        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            for (int i = 0; i < 360; i += 8)
            {
                Vector2 circularLocation = new Vector2(-20, 0).RotatedBy(MathHelper.ToRadians(i));
                int num1 = Dust.NewDust(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 3), 3, 3, DustID.GoldCoin);
                Main.dust[num1].noGravity = true;
                Main.dust[num1].scale = 2.25f;
                Main.dust[num1].velocity = circularLocation * 0.20f;
            }
            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<Explosion>(), Projectile.damage, 0, Main.myPlayer);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new(texture.Width / 2, Projectile.height / 2);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(Color.Pink) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}
