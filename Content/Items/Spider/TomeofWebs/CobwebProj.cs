﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Malignant.Content.Items.Crimson.Arterion.BurstingArtery;
using Malignant.Content.Items.Spider.SpiderNeckless;

namespace Malignant.Content.Items.Spider.TomeofWebs
{
    public class CobwebProj : ModProjectile
    {
        public override void Kill(int timeLeft)
        {

            for (int i = 0; i < 3; i++)
            {
                float speedX = Projectile.velocity.X * Main.rand.NextFloat(.4f, .7f) + Main.rand.NextFloat(-8f, 8f);
                float speedY = Projectile.velocity.Y * Main.rand.Next(40, 70) * 0.01f + Main.rand.Next(-20, 21) * 0.4f;
                Projectile proj = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.position.X + speedX, Projectile.position.Y + speedY, speedX, speedY, ModContent.ProjectileType<SpiderFangProjectile>(), (int)(Projectile.damage * 0.75), 0, Projectile.owner, 0, 0)];
                proj.hostile = false;
                proj.penetrate = 1;
                proj.friendly = true;
            }
            for (int num686 = 0; num686 < 30; num686++)
            {
                int num687 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Blood, Projectile.velocity.X, Projectile.velocity.Y, 100, default(Color), 1.7f);
                Main.dust[num687].noGravity = true;
                Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.VenomStaff, Projectile.velocity.X, Projectile.velocity.Y, 100);
            }
        }

        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 28;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;

            Projectile.timeLeft = 60;
            Projectile.scale = 1f;
        }
        public override void AI()
        {
            Dust dust;
            Vector2 position = Projectile.Center;
            dust = Main.dust[Terraria.Dust.NewDust(position, 20, 20, 5, 0, 0, 0, new Color(255, 255, 255), 0.5f)];
            Projectile.rotation += 0.5f;
            Projectile.velocity *= 0.95f;
        }
    }
}