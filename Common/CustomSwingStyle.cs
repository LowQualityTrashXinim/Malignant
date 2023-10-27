﻿using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Malignant.Common.Helper;
using Microsoft.Xna.Framework.Graphics;

namespace TheSkeletronMod.Common.Globals
{
    public enum CustomUseStyle
    {
        DefaultNoCustomSwing,
        SwipeAttack,
        PokeAttack,
    }
    public struct CustomAttack
    {
        public CustomUseStyle style = CustomUseStyle.DefaultNoCustomSwing;
        public bool SwingDownWard = false;
        public CustomAttack(CustomUseStyle style, bool SwingDownWard)
        {
            this.style = style;
            this.SwingDownWard = SwingDownWard;
        }
        public CustomAttack() { }
    }
    /// <summary>
    /// To see implementation of this, please check <br/>
    /// <see cref="SawboneSword.SetDefaults"/> or <see cref="BoneSword.SetDefaults"/> to see how to use this
    /// </summary>
    internal class ImprovedSwingSword : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public float ItemSwingDegree = 120;
        public const float PLAYERARMLENGTH = 12f;
        /// <summary>
        /// Use this to set up your own attack swing<br/>
        /// Q : Why not use List ?<br/>
        /// A : Be responsible :>
        /// </summary>
        public CustomAttack[] ArrayOfAttack = new CustomAttack[] { };

        public override bool CanUseItem(Item item, Player player)
        {
            return base.CanUseItem(item, player);
        }
        public override bool? UseItem(Item item, Player player)
        {
            if (ArrayOfAttack == null)
                return base.UseItem(item, player);
            if (ArrayOfAttack.Length <= 0)
                return base.UseItem(item, player);
            if (item.type != player.HeldItem.type)
                return base.UseItem(item, player);
            if (player.ItemAnimationJustStarted)
            {
                ImprovedSwingGlobalItemPlayer modplayer = player.GetModPlayer<ImprovedSwingGlobalItemPlayer>();
                if (++modplayer.AttackIndex >= ArrayOfAttack.Length)
                {
                    modplayer.AttackIndex = 0;
                }
            }
            return base.UseItem(item, player);
        }
        public override void UseStyle(Item item, Player player, Rectangle heldItemFrame)
        {
            if (ArrayOfAttack == null)
                return;
            if (ArrayOfAttack.Length <= 0)
                return;
            ImprovedSwingGlobalItemPlayer modplayer = player.GetModPlayer<ImprovedSwingGlobalItemPlayer>();
            if (modplayer.AttackIndex >= ArrayOfAttack.Length)
                return;
            if (item.type != player.HeldItem.type)
                return;

            if (ArrayOfAttack[modplayer.AttackIndex].style == CustomUseStyle.SwipeAttack)
            {
                SwipeAttack(player, modplayer, ItemSwingDegree, ArrayOfAttack[modplayer.AttackIndex].SwingDownWard.ToDirectionInt());
            }
            if (ArrayOfAttack[modplayer.AttackIndex].style == CustomUseStyle.PokeAttack)
            {
                PokeAttack(player, modplayer, ItemSwingDegree, ArrayOfAttack[modplayer.AttackIndex].SwingDownWard.ToDirectionInt());
            }
        }
        public override void ModifyItemScale(Item item, Player player, ref float scale)
        {
            if (ArrayOfAttack == null)
                return;
            if (ArrayOfAttack.Length <= 0)
                return;
            ImprovedSwingGlobalItemPlayer modplayer = player.GetModPlayer<ImprovedSwingGlobalItemPlayer>();
            if (modplayer.AttackIndex >= ArrayOfAttack.Length)
                return;
            if (ArrayOfAttack[modplayer.AttackIndex].style != CustomUseStyle.DefaultNoCustomSwing)
            {
                int duration = player.itemAnimationMax;
                float thirdduration = duration / 3;
                float progress;
                if (player.itemAnimation < thirdduration)
                {
                    progress = player.itemAnimation / thirdduration;
                }
                else
                {
                    progress = (duration - player.itemAnimation) / thirdduration;
                }
                scale += MathHelper.SmoothStep(-.5f, .25f, progress);
            }
        }
        private void SwipeAttack(Player player, ImprovedSwingGlobalItemPlayer modplayer, float swingdegree, int direct)
        {
            float percentDone = player.itemAnimation / (float)player.itemAnimationMax;
            float baseAngle = modplayer.data.ToRotation();
            float angle = MathHelper.ToRadians(baseAngle + swingdegree) * player.direction;
            float start = baseAngle + angle * direct;
            float end = baseAngle - angle * direct;
            Swipe(start, end, percentDone, player, modplayer, direct);
        }
        private void Swipe(float start, float end, float percentDone, Player player, ImprovedSwingGlobalItemPlayer modplayer, int direct)
        {
            float currentAngle = MathHelper.SmoothStep(start, end, percentDone);
            player.itemRotation = currentAngle;
            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, currentAngle - MathHelper.PiOver2);
            player.itemRotation += player.direction > 0 ? MathHelper.PiOver4 : MathHelper.PiOver4 * 3;
            player.itemLocation = player.Center + Vector2.UnitX.RotatedBy(currentAngle) * PLAYERARMLENGTH;
        }
        private void PokeAttack(Player player, ImprovedSwingGlobalItemPlayer modplayer, float swingdegree, int direct)
        {
            float percentDone = player.itemAnimation / (float)player.itemAnimationMax;
            float baseAngle = modplayer.data.ToRotation();
            float angle = MathHelper.ToRadians(baseAngle + swingdegree) * player.direction;
            float start = baseAngle + angle * direct;
            float end = baseAngle - angle * direct;
            Poke(start, end, percentDone, player, modplayer, direct);
        }
        private void Poke(float start, float end, float percentDone, Player player, ImprovedSwingGlobalItemPlayer modplayer, int direct)
        {
            float currentAngle = MathHelper.SmoothStep(start, end, percentDone);
            ImprovedSwingGlobalItemPlayer modPlayer = player.GetModPlayer<ImprovedSwingGlobalItemPlayer>();
            player.itemRotation = currentAngle;
            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, currentAngle - MathHelper.PiOver2);
            player.itemRotation += player.direction > 0 ? MathHelper.PiOver4 : MathHelper.PiOver4 * 3;
            if (direct == -1)
            {
                modPlayer.CustomItemRotation = currentAngle;
                modPlayer.CustomItemRotation += player.direction > 0 ? MathHelper.PiOver4 * 3 : MathHelper.PiOver4;
            }
            player.itemLocation = player.Center + Vector2.UnitX.RotatedBy(currentAngle) * PLAYERARMLENGTH;
        }
        //Credit hitbox code to Stardust
        public override void UseItemHitbox(Item item, Player player, ref Rectangle hitbox, ref bool noHitbox)
        {
            if (ArrayOfAttack == null)
                return;
            if (ArrayOfAttack.Length <= 0)
                return;
            ImprovedSwingGlobalItemPlayer modplayer = player.GetModPlayer<ImprovedSwingGlobalItemPlayer>();
            if (modplayer.AttackIndex >= ArrayOfAttack.Length)
                return;
            if (ArrayOfAttack[modplayer.AttackIndex].style != CustomUseStyle.DefaultNoCustomSwing)
            {
                Vector2 handPos = Vector2.UnitY.RotatedBy(player.compositeFrontArm.rotation);
                float length = new Vector2(item.width, item.height).Length() * player.GetAdjustedItemScale(player.HeldItem);
                Vector2 endPos = handPos;
                endPos *= length;
                handPos += player.MountedCenter;
                endPos += player.MountedCenter;
                (int X1, int X2) XVals = MethodHelper.Order(handPos.X, endPos.X);
                (int Y1, int Y2) YVals = MethodHelper.Order(handPos.Y, endPos.Y);
                hitbox = new Rectangle(XVals.X1 - 2, YVals.Y1 - 2, XVals.X2 - XVals.X1 + 2, YVals.Y2 - YVals.Y1 + 2);
            }
        }
    }
    /// <summary>
    /// DO NOT FUCK WITH THIS
    /// </summary>
    public class MeleeOverhaulSystem : ModSystem
    {
        public override void Load()
        {
            base.Load();
            On_PlayerDrawLayers.DrawPlayer_RenderAllLayers += On_PlayerDrawLayers_DrawPlayer_RenderAllLayers;
        }

        private void On_PlayerDrawLayers_DrawPlayer_RenderAllLayers(On_PlayerDrawLayers.orig_DrawPlayer_RenderAllLayers orig, ref PlayerDrawSet drawinfo)
        {
            Player player = Main.LocalPlayer;
            Item item = player.HeldItem;
            if (player.TryGetModPlayer(out ImprovedSwingGlobalItemPlayer modplayer))
            {
                if (item.TryGetGlobalItem(out ImprovedSwingSword meleeItem))
                {
                    if (modplayer.AttackIndex < meleeItem.ArrayOfAttack.Length)
                    {
                        if (meleeItem.ArrayOfAttack[modplayer.AttackIndex].style == CustomUseStyle.PokeAttack && !meleeItem.ArrayOfAttack[modplayer.AttackIndex].SwingDownWard)
                        {
                            for (int i = 0; i < drawinfo.DrawDataCache.Count; i++)
                            {
                                if (drawinfo.DrawDataCache[i].texture == TextureAssets.Item[item.type].Value)
                                {
                                    DrawData drawdata = drawinfo.DrawDataCache[i];
                                    Vector2 origin = drawdata.texture.Size() * .5f;
                                    drawdata.sourceRect = null;
                                    drawdata.ignorePlayerRotation = true;
                                    drawdata.rotation = modplayer.CustomItemRotation;
                                    drawdata.position += Vector2.UnitX.RotatedBy(modplayer.CustomItemRotation) * ((origin.Length() + ImprovedSwingSword.PLAYERARMLENGTH) * drawdata.scale.X + ImprovedSwingSword.PLAYERARMLENGTH) * -player.direction;
                                    drawinfo.DrawDataCache[i] = drawdata;
                                }
                            }
                        }
                    }
                }
            }
            orig.Invoke(ref drawinfo);
        }
    }
    public class ImprovedSwingGlobalItemPlayer : ModPlayer
    {
        public Vector2 data = Vector2.Zero;
        public Vector2 mouseLastPosition = Vector2.Zero;
        public float CustomItemRotation = 0;
        public int AttackIndex = 0;
        public override void PostUpdate()
        {
            Item item = Player.HeldItem;
            if (item.TryGetGlobalItem(out ImprovedSwingSword meleeItem))
            {
                if (AttackIndex >= meleeItem.ArrayOfAttack.Length)
                    return;
                if (meleeItem.ArrayOfAttack[AttackIndex].style == CustomUseStyle.DefaultNoCustomSwing || Player.HeldItem.noMelee)
                    return;
            }
            if (Player.ItemAnimationJustStarted)
            {
                data = (Main.MouseWorld - Player.MountedCenter).SafeNormalize(Vector2.Zero);
            }
            if (Player.ItemAnimationActive)
            {
                Player.direction = data.X > 0 ? 1 : -1;
            }
            Player.attackCD = 0;
            for (int i = 0; i < Player.meleeNPCHitCooldown.Length; i++)
            {
                if (Player.meleeNPCHitCooldown[i] > 0)
                {
                    Player.meleeNPCHitCooldown[i]--;
                }
            }
        }
        public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
        {
            base.ModifyDrawInfo(ref drawInfo);
            Item item = Player.HeldItem;
            if (item.TryGetGlobalItem(out ImprovedSwingSword meleeItem))
            {
                if (AttackIndex < meleeItem.ArrayOfAttack.Length)
                {
                    if (meleeItem.ArrayOfAttack[AttackIndex].style == CustomUseStyle.PokeAttack && !meleeItem.ArrayOfAttack[AttackIndex].SwingDownWard)
                    {
                        if (Player.direction == -1)
                        {
                            drawInfo.itemEffect = SpriteEffects.None;
                        }
                        else
                        {
                            drawInfo.itemEffect = SpriteEffects.FlipHorizontally;
                        }
                    }
                }
            }
        }
    }
}