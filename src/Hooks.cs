using IL.MoreSlugcats;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MoreSlugcats;
using On;
using Smoke;
using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Text.RegularExpressions;
using UnityEngine;

namespace RebindDevTools
{
    internal static class Hooks
    {
        public static void ApplyHooks()
        {
            On.RainWorld.OnModsInit += RainWorld_OnModsInit;
        }

        private static bool isInit = false;

        private static void RainWorld_OnModsInit(On.RainWorld.orig_OnModsInit orig, RainWorld self)
        {
            orig(self);

            if (isInit) return;
            isInit = true;

            MachineConnector.SetRegisteredOI(Plugin.MOD_ID, Options.instance);

            try
            {
                //// Creatures
                //IL.BigEel.Update += BigEel_Update; // FAIL
                //IL.BigSpider.Update += BigSpider_Update; // FAIL
                //IL.Centipede.Update += Centipede_Update; // FAIL
                //IL.Cicada.Update += Cicada_Update; // FAIL
                //IL.DaddyLongLegs.Update += DaddyLongLegs_Update; // FAIL

                IL.DropBug.Update += DropBug_Update; // Works
                IL.EggBug.Update += EggBug_Update; // Works
                IL.Fly.Update += Fly_Update; // Works
                IL.GarbageWorm.Update += GarbageWorm_Update; // Works
                IL.JellyFish.Update += JellyFish_Update; // Works
                IL.JetFish.Update += JetFish_Update; // Works
                IL.Leech.Update += Leech_Update; // Works
                IL.Lizard.Update += Lizard_Update; // Works
                IL.NeedleWorm.Update += NeedleWorm_Update; // Works
                IL.Vulture.Update += Vulture_Update; // Works
                IL.Scavenger.Update += Scavenger_Update;
                IL.Spider.Update += Spider_Update; // Works
                IL.TempleGuard.Update += TempleGuard_Update; // Works
                IL.LanternMouse.Update += LanternMouse_Update; // Works

                IL.PoleMimic.Update += PoleMimic_Update; // FAIL
                IL.TentaclePlant.Update += TentaclePlant_Update; // Works

                IL.Deer.Update += Deer_Update; // Works
                //IL.DeerAI.Update += DeerAI_Update;
                //IL.DeerPather.Update += DeerPather_Update;

                IL.MirosBird.Update += MirosBird_Update; // Works
                //IL.MirosBird.Act += MirosBird_Act;
                //IL.MirosBirdPather.FollowPath += MirosBirdPather_FollowPath;

                IL.Player.Update += Player_Update;

                IL.MoreSlugcats.Inspector.Update += Inspector_Update; // Works
                IL.MoreSlugcats.StowawayBug.Update += StowawayBug_Update; // IDK
                IL.MoreSlugcats.BigJellyFish.DebugDrag += BigJellyFish_DebugDrag; // Works
                IL.MoreSlugcats.Yeek.Update += Yeek_Update; // Works



                //// Objects
                IL.NeedleEgg.Update += NeedleEgg_Update; // Works
                IL.DangleFruit.Update += DangleFruit_Update; // Works
                IL.EggBugEgg.Update += EggBugEgg_Update; // Works
                IL.SlimeMold.Update += SlimeMold_Update; // Works
                IL.Snail.Update += Snail_Update; // Works
                IL.VultureGrub.Update += VultureGrub_Update;
                IL.Hazer.Update += Hazer_Update; // Works
                IL.SporePlant.Update += SporePlant_Update; // Works
                IL.SwollenWaterNut.Update += SwollenWaterNut_Update;
                IL.TubeWorm.Update += TubeWorm_Update; // Works

                IL.MoreSlugcats.FireEgg.Update += FireEgg_Update; // Works
                IL.MoreSlugcats.DandelionPeach.Update += DandelionPeach_Update; // Works
                IL.MoreSlugcats.GlowWeed.Update += GlowWeed_Update; // Works
                IL.MoreSlugcats.GooieDuck.Update += GooieDuck_Update; // Works



                //// Meta
                IL.RainWorldGame.Update += RainWorldGame_Update; // Works
                IL.RainWorldGame.RawUpdate += RainWorldGame_RawUpdate;
                //IL.ForcedVisibilityVisualizer.Update += ForcedVisibilityVisualizer_Update;
                //IL.RoomCamera.Update += RoomCamera_Update;
                //IL.VirtualMicrophone.Update += VirtualMicrophone_Update;



                //// Menus
                //IL.Menu.Menu.Update += Menu_Update;
                //IL.Menu.MenuScene.Update += MenuScene_Update;
                //IL.Menu.SlideShowMenuScene.Update += SlideShowMenuScene_Update;
                //IL.Menu.SlideShowMenuScene.CameraMovementEditor.Update += CameraMovementEditor_Update;
                //IL.Menu.SlugcatSelectMenu.StartGame += SlugcatSelectMenu_StartGame;
            }
            catch (Exception ex)
            {
                Plugin.Logger.LogError(ex);
            }
        }
        


        private static void Player_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("v"),
                x => x.MatchCallOrCallvirt<Input>("GetKey"),
                x => x.MatchLdloc(32),
                x => x.MatchAnd(),
                x => x.MatchBrfalse(out _)))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<Player, bool>>((self) =>
                {
                    return Input.GetKey(Options.teleportSlugcat.Value);
                });
            }

            c.Index = 0;

            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("w"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);

                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<Player, bool>>((self) =>
                {
                    return Input.GetKey(Options.flingSlugcat.Value);
                });
            }

            c.Index = 0;

            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("q"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<Player, bool>>((self) =>
                {
                    return Input.GetKey(Options.feedSlugcat.Value);
                });
            }
        }

        private static void VirtualMicrophone_Update(ILContext il)
        {
            
        }

        private static void RoomCamera_Update(ILContext il)
        {
        }

        private static void RainWorldGame_RawUpdate(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            // Slow Down Time
            while (c.TryGotoNext(
                x => x.MatchLdstr("a"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<Player, bool>>((self) =>
                {
                    return Input.GetKey(Options.slowDownTime.Value);
                });
            }


            //// Speed Up Time
            //// Does not work
            //c.Index = 0;

            //while (c.TryGotoNext(
            //    x => x.MatchLdstr("s"),
            //    x => x.MatchCallOrCallvirt<Input>("GetKey")))
            //{
            //    // 230, should be 228?
            //    Plugin.Logger.LogWarning(c.Index);

            //    c.RemoveRange(2);
            //    c.Emit(OpCodes.Ldarg_0);

            //    c.EmitDelegate<Func<Player, bool>>((self) =>
            //    {
            //        return Input.GetKey(Options.speedUpTime.Value);
            //    });

            //    Plugin.Logger.LogWarning("Reaches this point");
            //    // Fails here?
            //}


            //// Unload Rooms
            //c.Index = 0;

            //while (c.TryGotoNext(
            //    x => x.MatchLdstr("q"),
            //    x => x.MatchCallOrCallvirt<Input>("GetKey")))
            //{
            //    c.RemoveRange(2);
            //    c.Emit(OpCodes.Ldarg_0);

            //    c.EmitDelegate<Func<Player, bool>>((self) =>
            //    {
            //        return Input.GetKey(Options.unloadRooms.Value);
            //    });
            //}


            //// Toggle Tile Access
            //c.Index = 0;

            //while (c.TryGotoNext(
            //    x => x.MatchLdstr("p"),
            //    x => x.MatchCallOrCallvirt<Input>("GetKey")))
            //{
            //    c.RemoveRange(2);
            //    c.Emit(OpCodes.Ldarg_0);

            //    c.EmitDelegate<Func<Player, bool>>((self) =>
            //    {
            //        return Input.GetKey(Options.toggleTileAccessibility.Value);
            //    });
            //}


            //// Toggle Debug Info
            //c.Index = 0;

            //while (c.TryGotoNext(
            //    x => x.MatchLdstr("m"),
            //    x => x.MatchCallOrCallvirt<Input>("GetKey")))
            //{
            //    c.RemoveRange(2);
            //    c.Emit(OpCodes.Ldarg_0);

            //    c.EmitDelegate<Func<Player, bool>>((self) =>
            //    {
            //        return Input.GetKey(Options.toggleDebugInfo.Value);
            //    });
            //}


            //// Toggle Console
            //c.Index = 0;

            //while (c.TryGotoNext(
            //    x => x.MatchLdstr("k"),
            //    x => x.MatchCallOrCallvirt<Input>("GetKey")))
            //{
            //    c.RemoveRange(2);
            //    c.Emit(OpCodes.Ldarg_0);

            //    c.EmitDelegate<Func<Player, bool>>((self) =>
            //    {
            //        return Input.GetKey(Options.toggleConsoleLog.Value);
            //    });
            //}


            //// Toggle Main Interface
            //c.Index = 0;

            //while (c.TryGotoNext(
            //    x => x.MatchLdstr("h"),
            //    x => x.MatchCallOrCallvirt<Input>("GetKey")))
            //{
            //    c.RemoveRange(2);
            //    c.Emit(OpCodes.Ldarg_0);

            //    c.EmitDelegate<Func<Player, bool>>((self) =>
            //    {
            //        return Input.GetKey(Options.toggleDevToolsInterface.Value);
            //    });
            //}


            //// Toggle Dev Tools
            //c.Index = 0;

            //while (c.TryGotoNext(
            //    x => x.MatchLdstr("o"),
            //    x => x.MatchCallOrCallvirt<Input>("GetKey")))
            //{
            //    c.RemoveRange(2);
            //    c.Emit(OpCodes.Ldarg_0);

            //    c.EmitDelegate<Func<Player, bool>>((self) =>
            //    {
            //        return Input.GetKey(Options.toggleDevTools.Value);
            //    });
            //}

            // TODO: Add L, quarters the precycle timer.
            // TODO: Add E, sets all Creature AI in the room's current destination to the mouse cursor.
        }

        private static void RainWorldGame_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            while (c.TryGotoNext(
                x => x.MatchLdstr("r"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<Player, bool>>((self) =>
                {
                    return Input.GetKey(Options.restartCycle.Value);
                });
            }
        }

        private static void Menu_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            // Slow Down Time
            while (c.TryGotoNext(
                x => x.MatchLdstr("a"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<Player, bool>>((self) =>
                {
                    return Input.GetKey(Options.slowDownTime.Value);
                });
            }

            // Speed Up Time
            c.Index = 0;

            while (c.TryGotoNext(
                x => x.MatchLdstr("s"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<Player, bool>>((self) =>
                {
                    return Input.GetKey(Options.speedUpTime.Value);
                });
            }
        }

        private static void ForcedVisibilityVisualizer_Update(ILContext il)
        {
            
        }




        // UNIQUE
        private static void SlugcatSelectMenu_StartGame(ILContext il)
        {

        }

        // UNIQUE
        private static void CameraMovementEditor_Update(ILContext il)
        {

        }

        // UNIQUE
        private static void SlideShowMenuScene_Update(ILContext il)
        {

        }

        // UNIQUE
        private static void MenuScene_Update(ILContext il)
        {

        }




        private static void DeerPather_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            while (c.TryGotoNext(
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<DeerPather, bool>>((self) =>
                {
                    return Input.GetKey(Options.dragEntities.Value);
                });
            }

            c.Index = 0;

            while (c.TryGotoNext(
                x => x.MatchLdstr("n"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<DeerPather, bool>>((self) =>
                {
                    return Input.GetKey(Options.offsetCamera.Value);
                });
            }
        }

        private static void DeerAI_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            while (c.TryGotoNext(
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<DeerAI, bool>>((self) =>
                {
                    return Input.GetKey(Options.dragEntities.Value);
                });
            }

            c.Index = 0;

            while (c.TryGotoNext(
                x => x.MatchLdstr("n"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<DeerAI, bool>>((self) =>
                {
                    return Input.GetKey(Options.offsetCamera.Value);
                });
            }
        }

        private static void MirosBird_Act(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            while (c.TryGotoNext(
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<MirosBird, bool>>((self) =>
                {
                    return Input.GetKey(Options.dragEntities.Value);
                });
            }

            c.Index = 0;

            while (c.TryGotoNext(
                x => x.MatchLdstr("n"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<MirosBird, bool>>((self) =>
                {
                    return Input.GetKey(Options.offsetCamera.Value);
                });
            }
        }

        private static void MirosBirdPather_FollowPath(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            while (c.TryGotoNext(
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<MirosBirdPather, bool>>((self) =>
                {
                    return Input.GetKey(Options.dragEntities.Value);
                });
            }

            c.Index = 0;

            while (c.TryGotoNext(
                x => x.MatchLdstr("n"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<MirosBirdPather, bool>>((self) =>
                {
                    return Input.GetKey(Options.offsetCamera.Value);
                });
            }
        }




        private static void DaddyLongLegs_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            while (c.TryGotoNext(
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<DaddyLongLegs, bool>>((self) =>
                {
                    return Input.GetKey(Options.dragEntities.Value);
                });
            }

            c.Index = 0;

            while (c.TryGotoNext(
                x => x.MatchLdstr("n"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<DaddyLongLegs, bool>>((self) =>
                {
                    return Input.GetKey(Options.offsetCamera.Value);
                });
            }
        }

        private static void Cicada_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            while (c.TryGotoNext(
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<Cicada, bool>>((self) =>
                {
                    return Input.GetKey(Options.dragEntities.Value);
                });
            }

            c.Index = 0;

            while (c.TryGotoNext(
                x => x.MatchLdstr("n"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<Cicada, bool>>((self) =>
                {
                    return Input.GetKey(Options.offsetCamera.Value);
                });
            }
        }

        private static void Centipede_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            while (c.TryGotoNext(
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<Centipede, bool>>((self) =>
                {
                    return Input.GetKey(Options.dragEntities.Value);
                });
            }

            c.Index = 0;

            while (c.TryGotoNext(
                x => x.MatchLdstr("n"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<Centipede, bool>>((self) =>
                {
                    return Input.GetKey(Options.offsetCamera.Value);
                });
            }
        }

        private static void BigSpider_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            while (c.TryGotoNext(
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<BigSpider, bool>>((self) =>
                {
                    return Input.GetKey(Options.dragEntities.Value);
                });
            }

            c.Index = 0;

            while (c.TryGotoNext(
                x => x.MatchLdstr("n"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<BigSpider, bool>>((self) =>
                {
                    return Input.GetKey(Options.offsetCamera.Value);
                });
            }
        }

        private static void BigEel_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            while (c.TryGotoNext(
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<BigEel, bool>>((self) =>
                {
                    return Input.GetKey(Options.dragEntities.Value);
                });
            }

            c.Index = 0;

            while (c.TryGotoNext(
                x => x.MatchLdstr("n"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<BigEel, bool>>((self) =>
                {
                    return Input.GetKey(Options.offsetCamera.Value);
                });
            }
        }




        // I'm never touching this again
        private static void LanternMouse_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            while (c.TryGotoNext(
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<LanternMouse, bool>>((self) =>
                {
                    return Input.GetKey(Options.dragEntities.Value);
                });
            }

            while (c.TryGotoNext(
                x => x.MatchLdstr("n"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<LanternMouse, bool>>((self) =>
                {
                    return Input.GetKey(Options.flingVultures.Value);
                });
            }
        }

        private static void VultureGrub_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            while (c.TryGotoNext(
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<VultureGrub, bool>>((self) =>
                {
                    return Input.GetKey(Options.dragEntities.Value);
                });
            }

            while (c.TryGotoNext(
                x => x.MatchLdstr("g"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<Vulture, bool>>((self) =>
                {
                    return Input.GetKey(Options.flingVultures.Value);
                });
            }
        }

        private static void Vulture_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            while (c.TryGotoNext(
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<Vulture, bool>>((self) =>
                {
                    return Input.GetKey(Options.dragEntities.Value);
                });
            }

            c.Index = 0;

            while (c.TryGotoNext(
                x => x.MatchLdstr("g"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<Vulture, bool>>((self) =>
                {
                    return Input.GetKey(Options.flingVultures.Value);
                });
            }
        }

        private static void TubeWorm_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            while (c.TryGotoNext(
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<TubeWorm, bool>>((self) =>
                {
                    return Input.GetKey(Options.dragEntities.Value);
                });
            }

            c.Index = 0;

            while (c.TryGotoNext(
                x => x.MatchLdstr("n"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<TubeWorm, bool>>((self) =>
                {
                    return Input.GetKey(Options.offsetCamera.Value);
                });
            }
        }

        private static void TentaclePlant_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            while (c.TryGotoNext(
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<TentaclePlant, bool>>((self) =>
                {
                    return Input.GetKey(Options.dragEntities.Value);
                });
            }

            c.Index = 0;

            while (c.TryGotoNext(
                x => x.MatchLdstr("n"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<TentaclePlant, bool>>((self) =>
                {
                    return Input.GetKey(Options.offsetCamera.Value);
                });
            }
        }

        private static void TempleGuard_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            while (c.TryGotoNext(
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<TempleGuard, bool>>((self) =>
                {
                    return Input.GetKey(Options.dragEntities.Value);
                });
            }

            c.Index = 0;

            while (c.TryGotoNext(
                x => x.MatchLdstr("n"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<TempleGuard, bool>>((self) =>
                {
                    return Input.GetKey(Options.offsetCamera.Value);
                });
            }
        }

        private static void SwollenWaterNut_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            while (c.TryGotoNext(
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<SwollenWaterNut, bool>>((self) =>
                {
                    return Input.GetKey(Options.dragEntities.Value);
                });
            }

            c.Index = 0;

            while (c.TryGotoNext(
                x => x.MatchLdstr("n"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<SwollenWaterNut, bool>>((self) =>
                {
                    return Input.GetKey(Options.offsetCamera.Value);
                });
            }
        }

        private static void SporePlant_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            while (c.TryGotoNext(
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<SporePlant, bool>>((self) =>
                {
                    return Input.GetKey(Options.dragEntities.Value);
                });
            }

            c.Index = 0;

            while (c.TryGotoNext(
                x => x.MatchLdstr("n"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<SporePlant, bool>>((self) =>
                {
                    return Input.GetKey(Options.offsetCamera.Value);
                });
            }
        }

        private static void Spider_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            while (c.TryGotoNext(
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<Spider, bool>>((self) =>
                {
                    return Input.GetKey(Options.dragEntities.Value);
                });
            }

            c.Index = 0;

            while (c.TryGotoNext(
                x => x.MatchLdstr("n"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<Spider, bool>>((self) =>
                {
                    return Input.GetKey(Options.offsetCamera.Value);
                });
            }
        }

        private static void Snail_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            while (c.TryGotoNext(
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<Snail, bool>>((self) =>
                {
                    return Input.GetKey(Options.dragEntities.Value);
                });
            }

            c.Index = 0;

            while (c.TryGotoNext(
                x => x.MatchLdstr("n"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<Snail, bool>>((self) =>
                {
                    return Input.GetKey(Options.offsetCamera.Value);
                });
            }
        }

        private static void SlimeMold_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            while (c.TryGotoNext(
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<SlimeMold, bool>>((self) =>
                {
                    return Input.GetKey(Options.dragEntities.Value);
                });
            }

            c.Index = 0;

            while (c.TryGotoNext(
                x => x.MatchLdstr("n"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<SlimeMold, bool>>((self) =>
                {
                    return Input.GetKey(Options.offsetCamera.Value);
                });
            }
        }

        private static void Scavenger_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            while (c.TryGotoNext(
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<Scavenger, bool>>((self) =>
                {
                    return Input.GetKey(Options.dragEntities.Value);
                });
            }

            c.Index = 0;

            while (c.TryGotoNext(
                x => x.MatchLdstr("n"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<Scavenger, bool>>((self) =>
                {
                    return Input.GetKey(Options.offsetCamera.Value);
                });
            }
        }

        private static void PoleMimic_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            while (c.TryGotoNext(
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<PoleMimic, bool>>((self) =>
                {
                    return Input.GetKey(Options.dragEntities.Value);
                });
            }

            c.Index = 0;

            while (c.TryGotoNext(
                x => x.MatchLdstr("n"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<PoleMimic, bool>>((self) =>
                {
                    return Input.GetKey(Options.offsetCamera.Value);
                });
            }
        }

        private static void NeedleWorm_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            while (c.TryGotoNext(
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<NeedleWorm, bool>>((self) =>
                {
                    return Input.GetKey(Options.dragEntities.Value);
                });
            }

            c.Index = 0;

            while (c.TryGotoNext(
                x => x.MatchLdstr("n"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<NeedleWorm, bool>>((self) =>
                {
                    return Input.GetKey(Options.offsetCamera.Value);
                });
            }
        }

        private static void NeedleEgg_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            while (c.TryGotoNext(
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<NeedleEgg, bool>>((self) =>
                {
                    return Input.GetKey(Options.dragEntities.Value);
                });
            }

            c.Index = 0;

            while (c.TryGotoNext(
                x => x.MatchLdstr("n"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<NeedleEgg, bool>>((self) =>
                {
                    return Input.GetKey(Options.offsetCamera.Value);
                });
            }
        }

        private static void Yeek_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            while (c.TryGotoNext(
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<MoreSlugcats.Yeek, bool>>((self) =>
                {
                    return Input.GetKey(Options.dragEntities.Value);
                });
            }

            c.Index = 0;

            while (c.TryGotoNext(
                x => x.MatchLdstr("n"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<MoreSlugcats.Yeek, bool>>((self) =>
                {
                    return Input.GetKey(Options.offsetCamera.Value);
                });
            }
        }

        private static void StowawayBug_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            while (c.TryGotoNext(
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<MoreSlugcats.StowawayBug, bool>>((self) =>
                {
                    return Input.GetKey(Options.dragEntities.Value);
                });
            }

            c.Index = 0;

            while (c.TryGotoNext(
                x => x.MatchLdstr("n"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<MoreSlugcats.StowawayBug, bool>>((self) =>
                {
                    return Input.GetKey(Options.offsetCamera.Value);
                });
            }
        }

        private static void Inspector_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            while (c.TryGotoNext(
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<MoreSlugcats.Inspector, bool>>((self) =>
                {
                    return Input.GetKey(Options.dragEntities.Value);
                });
            }

            c.Index = 0;

            while (c.TryGotoNext(
                x => x.MatchLdstr("n"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<MoreSlugcats.Inspector, bool>>((self) =>
                {
                    return Input.GetKey(Options.offsetCamera.Value);
                });
            }
        }

        private static void GooieDuck_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            while (c.TryGotoNext(
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<MoreSlugcats.GooieDuck, bool>>((self) =>
                {
                    return Input.GetKey(Options.dragEntities.Value);
                });
            }

            c.Index = 0;

            while (c.TryGotoNext(
                x => x.MatchLdstr("n"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<MoreSlugcats.GooieDuck, bool>>((self) =>
                {
                    return Input.GetKey(Options.offsetCamera.Value);
                });
            }
        }

        private static void GlowWeed_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            while (c.TryGotoNext(
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<MoreSlugcats.GlowWeed, bool>>((self) =>
                {
                    return Input.GetKey(Options.dragEntities.Value);
                });
            }

            c.Index = 0;

            while (c.TryGotoNext(
                x => x.MatchLdstr("n"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<MoreSlugcats.GlowWeed, bool>>((self) =>
                {
                    return Input.GetKey(Options.offsetCamera.Value);
                });
            }
        }

        private static void FireEgg_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            while (c.TryGotoNext(
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<MoreSlugcats.FireEgg, bool>>((self) =>
                {
                    return Input.GetKey(Options.dragEntities.Value);
                });
            }

            c.Index = 0;

            while (c.TryGotoNext(
                x => x.MatchLdstr("n"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<MoreSlugcats.FireEgg, bool>>((self) =>
                {
                    return Input.GetKey(Options.offsetCamera.Value);
                });
            }
        }

        private static void DandelionPeach_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            while (c.TryGotoNext(
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<MoreSlugcats.DandelionPeach, bool>>((self) =>
                {
                    return Input.GetKey(Options.dragEntities.Value);
                });
            }

            c.Index = 0;

            while (c.TryGotoNext(
                x => x.MatchLdstr("n"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<MoreSlugcats.DandelionPeach, bool>>((self) =>
                {
                    return Input.GetKey(Options.offsetCamera.Value);
                });
            }
        }

        private static void BigJellyFish_DebugDrag(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            while (c.TryGotoNext(
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<MoreSlugcats.BigJellyFish, bool>>((self) =>
                {
                    return Input.GetKey(Options.dragEntities.Value);
                });
            }

            c.Index = 0;

            while (c.TryGotoNext(
                x => x.MatchLdstr("n"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<MoreSlugcats.BigJellyFish, bool>>((self) =>
                {
                    return Input.GetKey(Options.offsetCamera.Value);
                });
            }
        }

        private static void MirosBird_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            while (c.TryGotoNext(
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<MirosBird, bool>>((self) =>
                {
                    return Input.GetKey(Options.dragEntities.Value);
                });
            }

            c.Index = 0;

            while (c.TryGotoNext(
                x => x.MatchLdstr("n"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<MirosBird, bool>>((self) =>
                {
                    return Input.GetKey(Options.offsetCamera.Value);
                });
            }
        }

        private static void Lizard_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            while (c.TryGotoNext(
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<Lizard, bool>>((self) =>
                {
                    return Input.GetKey(Options.dragEntities.Value);
                });
            }

            c.Index = 0;

            while (c.TryGotoNext(
                x => x.MatchLdstr("n"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<Lizard, bool>>((self) =>
                {
                    return Input.GetKey(Options.offsetCamera.Value);
                });
            }
        }

        private static void Leech_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            while (c.TryGotoNext(
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<Leech, bool>>((self) =>
                {
                    return Input.GetKey(Options.dragEntities.Value);
                });
            }

            c.Index = 0;

            while (c.TryGotoNext(
                x => x.MatchLdstr("n"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<Leech, bool>>((self) =>
                {
                    return Input.GetKey(Options.offsetCamera.Value);
                });
            }
        }

        private static void JetFish_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            while (c.TryGotoNext(
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<JetFish, bool>>((self) =>
                {
                    return Input.GetKey(Options.dragEntities.Value);
                });
            }

            c.Index = 0;

            while (c.TryGotoNext(
                x => x.MatchLdstr("n"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<JetFish, bool>>((self) =>
                {
                    return Input.GetKey(Options.offsetCamera.Value);
                });
            }
        }

        private static void JellyFish_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            while (c.TryGotoNext(
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<JellyFish, bool>>((self) =>
                {
                    return Input.GetKey(Options.dragEntities.Value);
                });
            }

            c.Index = 0;

            while (c.TryGotoNext(
                x => x.MatchLdstr("n"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<JellyFish, bool>>((self) =>
                {
                    return Input.GetKey(Options.offsetCamera.Value);
                });
            }
        }

        private static void Hazer_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            while (c.TryGotoNext(
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<Hazer, bool>>((self) =>
                {
                    return Input.GetKey(Options.dragEntities.Value);
                });
            }

            c.Index = 0;

            while (c.TryGotoNext(
                x => x.MatchLdstr("n"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<Hazer, bool>>((self) =>
                {
                    return Input.GetKey(Options.offsetCamera.Value);
                });
            }
        }

        private static void GarbageWorm_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            while (c.TryGotoNext(
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<GarbageWorm, bool>>((self) =>
                {
                    return Input.GetKey(Options.dragEntities.Value);
                });
            }

            c.Index = 0;

            while (c.TryGotoNext(
                x => x.MatchLdstr("n"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<GarbageWorm, bool>>((self) =>
                {
                    return Input.GetKey(Options.offsetCamera.Value);
                });
            }
        }

        private static void Fly_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            while (c.TryGotoNext(
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<Fly, bool>>((self) =>
                {
                    return Input.GetKey(Options.dragEntities.Value);
                });
            }

            c.Index = 0;

            while (c.TryGotoNext(
                x => x.MatchLdstr("n"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<Fly, bool>>((self) =>
                {
                    return Input.GetKey(Options.offsetCamera.Value);
                });
            }
        }

        private static void EggBugEgg_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            while (c.TryGotoNext(
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<EggBugEgg, bool>>((self) =>
                {
                    return Input.GetKey(Options.dragEntities.Value);
                });
            }

            c.Index = 0;

            while (c.TryGotoNext(
                x => x.MatchLdstr("n"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<EggBugEgg, bool>>((self) =>
                {
                    return Input.GetKey(Options.offsetCamera.Value);
                });
            }
        }

        private static void EggBug_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            while (c.TryGotoNext(
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<EggBug, bool>>((self) =>
                {
                    return Input.GetKey(Options.dragEntities.Value);
                });
            }

            c.Index = 0;

            while (c.TryGotoNext(
                x => x.MatchLdstr("n"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<EggBug, bool>>((self) =>
                {
                    return Input.GetKey(Options.offsetCamera.Value);
                });
            }
        }

        private static void DropBug_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            while (c.TryGotoNext(
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<DropBug, bool>>((self) =>
                {
                    return Input.GetKey(Options.dragEntities.Value);
                });
            }

            c.Index = 0;

            while (c.TryGotoNext(
                x => x.MatchLdstr("n"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<DropBug, bool>>((self) =>
                {
                    return Input.GetKey(Options.offsetCamera.Value);
                });
            }
        }

        private static void Deer_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            while (c.TryGotoNext(
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<Deer, bool>>((self) =>
                {
                    return Input.GetKey(Options.dragEntities.Value);
                });
            }

            c.Index = 0;

            while (c.TryGotoNext(
                x => x.MatchLdstr("n"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<Deer, bool>>((self) =>
                {
                    return Input.GetKey(Options.offsetCamera.Value);
                });
            }
        }

        private static void DangleFruit_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            while (c.TryGotoNext(
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<DangleFruit, bool>>((self) =>
                {
                    return Input.GetKey(Options.dragEntities.Value);
                });
            }

            c.Index = 0;

            while (c.TryGotoNext(
                x => x.MatchLdstr("n"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.RemoveRange(2);
                c.Emit(OpCodes.Ldarg_0);

                c.EmitDelegate<Func<DangleFruit, bool>>((self) =>
                {
                    return Input.GetKey(Options.offsetCamera.Value);
                });
            }
        }
    }
}
