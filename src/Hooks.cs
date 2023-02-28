using IL.Menu.Remix.MixedUI;
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
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace RebindDevTools
{
    internal static class Hooks
    {
        public static void ApplyHooks()
        {
            On.RainWorld.OnModsInit += RainWorld_OnModsInit;
            On.RainWorld.PostModsInit += RainWorld_PostModsInit;
            On.RainWorldGame.ctor += RainWorldGame_ctor;
        }

        private static void RainWorld_PostModsInit(On.RainWorld.orig_PostModsInit orig, RainWorld self)
        {
            orig(self);

            wasDevToolsActive = Options.devToolsEnabledByDefault.Value;
        }

        private static bool wasDevToolsActive;

        private static void RainWorldGame_ctor(On.RainWorldGame.orig_ctor orig, RainWorldGame self, ProcessManager manager)
        {
            orig(self, manager);

            if (Options.rememberIfEnabled.Value)
            {
                self.devToolsActive = wasDevToolsActive;
                self.devToolsLabel.isVisible = self.devToolsActive;
                return;
            }

            self.devToolsActive = Options.devToolsEnabledByDefault.Value;
            self.devToolsLabel.isVisible = self.devToolsActive;
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
                // Creatures
                IL.BigEel.Update += BigEel_Update; // FAIL
                IL.BigSpider.Update += BigSpider_Update; // FAIL
                IL.Centipede.Update += Centipede_Update; // FAIL
                IL.Cicada.Update += Cicada_Update; // FAIL
                IL.DaddyLongLegs.Update += DaddyLongLegs_Update; // FAIL

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
                IL.DeerAI.Update += DeerAI_Update;

                IL.MirosBird.Update += MirosBird_Update; // Works
                //IL.MirosBird.Act += MirosBird_Act;

                IL.Player.Update += Player_Update;

                IL.MoreSlugcats.Inspector.Update += Inspector_Update; // Works
                IL.MoreSlugcats.StowawayBug.Update += StowawayBug_Update; // IDK
                IL.MoreSlugcats.BigJellyFish.DebugDrag += BigJellyFish_DebugDrag; // Works
                IL.MoreSlugcats.Yeek.Update += Yeek_Update; // Works



                // Objects
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


                // Misc
                IL.AboveCloudsView.Update += AboveCloudsView_Update;
                IL.DevInterface.CustomDecalRepresentation.Update += CustomDecalRepresentation_Update;
                IL.DevInterface.Handle.Update += Handle_Update;
                IL.DevInterface.MapPage.Update += MapPage_Update;
                IL.DevInterface.MapPage.Signal += MapPage_Signal;
                IL.DevInterface.MiniMap.Update += MiniMap_Update;


                // Meta
                IL.RainWorldGame.Update += RainWorldGame_Update;
                IL.RainWorldGame.RawUpdate += RainWorldGame_RawUpdate;
                IL.ForcedVisibilityVisualizer.Update += ForcedVisibilityVisualizer_Update;
                IL.RoomCamera.Update += RoomCamera_Update;
                IL.VirtualMicrophone.Update += VirtualMicrophone_Update;



                // Menus
                IL.Menu.Menu.Update += Menu_Update;
                IL.Menu.MenuScene.Update += MenuScene_Update;
                IL.Menu.SlideShowMenuScene.Update += SlideShowMenuScene_Update;
                IL.Menu.SlideShowMenuScene.CameraMovementEditor.Update += CameraMovementEditor_Update;
                IL.Menu.SlugcatSelectMenu.StartGame += SlugcatSelectMenu_StartGame;


                IL.Player.ProcessDebugInputs += Player_ProcessDebugInputs;
            }
            catch (Exception ex)
            {
                Plugin.Logger.LogError(ex);
            }
        }

        private static void Player_ProcessDebugInputs(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            // Cycle Jumper
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("0"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.cycleJumper.Value));
            }
            c.Index = 0;

            // Reset Rain
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("9"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.resetRain.Value));
            }
            c.Index = 0;

            // Kill All Creatures
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("8"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.killAllCreatures.Value));
            }
            c.Index = 0;

            // Early Cycle
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdcI4(304),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.earlyCycle.Value));
            }
            c.Index = 0;

            // Mid Cycle
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdcI4(308),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.midCycle.Value));
            }
            c.Index = 0;

            // Late Cycle
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdcI4(306),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.lateCycle.Value));
            }
            c.Index = 0;

            ILLabel dest = null!;

            // Enable/Disable Pipe Jumping
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdloc(1),
                x => x.MatchCallOrCallvirt<Input>("GetKey"),
                x => x.MatchBrfalse(out dest)))
            {
                c.MoveAfterLabels();
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Options.entranceJumperEnabled.Value);
                c.Emit(OpCodes.Brfalse, dest);
                break;
            }
            c.Index = 0;


            // Spawn Spearmaster Pearl
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("5"),
                x => x.MatchCallOrCallvirt<Input>("GetKeyDown")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKeyDown(Options.spawnSpearmasterPearl.Value));
            }
            c.Index = 0;

            // Spawn Hunter Neuron
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("6"),
                x => x.MatchCallOrCallvirt<Input>("GetKeyDown")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKeyDown(Options.spawnHunterNeuron.Value));
            }
            c.Index = 0;

            // Spawn Rivulet Cell
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("7"),
                x => x.MatchCallOrCallvirt<Input>("GetKeyDown")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKeyDown(Options.spawnRivuletCell.Value));
            }
            c.Index = 0;
        }


        private static void MiniMap_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            // Change Minimap Layer
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("n"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.changeMinimapLayer.Value));
            }
            c.Index = 0;
        }

        private static void MapPage_Signal(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            // Change Minimap Layer (check if not pressed)
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("n"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.changeMinimapLayer.Value));
            }
            c.Index = 0;
        }

        private static void MapPage_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            // Toggle View Node Labels
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("j"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.toggleViewNodeLabels.Value));
            }
            c.Index = 0;
        }

        private static void Handle_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            // Change Handle Colour
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.changeHandleColor.Value));
            }
            c.Index = 0;
        }

        private static void CustomDecalRepresentation_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            // Set Handles
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("l"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.setHandles.Value));
            }
            c.Index = 0;

            // Increase Handles
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("k"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.increaseHandles.Value));
            }
            c.Index = 0;

            // Decrease Handles
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("j"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.decreaseHandles.Value));
            }
            c.Index = 0;
        }

        private static void AboveCloudsView_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            // Move Clouds View Object
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("t"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.moveCloudsViewObject.Value));
            }
            c.Index = 0;
        }



        private static void RainWorldGame_RawUpdate(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            // Slow Down Time
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("a"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.slowDownTime.Value));
            }
            c.Index = 0;

            // Speed Up Time
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("s"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.speedUpTime.Value));
            }
            c.Index = 0;

            // Unload Rooms
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("q"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.unloadRooms.Value));
            }
            c.Index = 0;

            // Toggle Tile Accessibility
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("p"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.toggleTileAccessibility.Value));
            }
            c.Index = 0;

            // Toggle Debug Info
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("m"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.toggleDebugInfo.Value));
            }
            c.Index = 0;

            // Toggle Console Log
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("k"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.toggleConsoleLog.Value));
            }
            c.Index = 0;

            // Toggle Main Interface
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("h"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.toggleDevToolsInterface.Value));
            }
            c.Index = 0;

            // Toggle Dev Tools
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("o"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) =>
                {
                    wasDevToolsActive = self.devToolsActive;
                    return Input.GetKey(Options.toggleDevTools.Value);
                });
            }
            c.Index = 0;

            // Quarter Precycle Time
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("l"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.quarterPrecycleTime.Value));
            }
            c.Index = 0;

            // Set AI Destination
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("e"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.setAIDestination.Value));
            }
            c.Index = 0;
        }

        private static void RainWorldGame_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            // Restart Cycle
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("r"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.restartCycle.Value));
            }
            c.Index = 0;
        }

        private static void Player_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            // Teleport Slugcat
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("v"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.teleportSlugcat.Value));
            }
            c.Index = 0;

            // Fling Slugcat
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("w"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.flingSlugcat.Value));
            }
            c.Index = 0;

            // Feed Slugcat
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("q"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.feedSlugcat.Value));
            }
            c.Index = 0;
        }

        private static void VirtualMicrophone_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            // Set AI Destination
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("u"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.reloadAllSounds.Value));
            }
            c.Index = 0;

            // Visualize Sounds
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("i"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.visualizeSounds.Value));
            }
            c.Index = 0;
        }

        private static void RoomCamera_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            // Offset Camera
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("n"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.offsetCamera.Value));
            }
            c.Index = 0;
        }

        
        private static void Menu_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            // Slow Down Time
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("a"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.slowDownTime.Value));
            }
            c.Index = 0;

            // Speed up Time
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("s"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.speedUpTime.Value));
            }
            c.Index = 0;
        }

        private static void ForcedVisibilityVisualizer_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            // ???
            //while (c.TryGotoNext(MoveType.Before,
            //    x => x.MatchLdstr("b"),
            //    x => x.MatchCallOrCallvirt<Input>("GetKey")))
            //{
            //    c.Index += 2;
            //    c.Emit(OpCodes.Pop);
            //    c.Emit(OpCodes.Ldarg_0);
            //    c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.offsetCamera.Value));
            //}
            //c.Index = 0;

            // Offset Camera
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("n"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.offsetCamera.Value));
            }
            c.Index = 0;
        }


        private static void SlugcatSelectMenu_StartGame(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            // Speed Up Start Game
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("s"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.speedUpStartGame.Value));
            }
            c.Index = 0;
        }

        private static void CameraMovementEditor_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            // Add Point
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("j"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.addPoint.Value));
            }
            c.Index = 0;

            // Remove Point
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("k"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.removePoint.Value));
            }
            c.Index = 0;

            // Move All Points
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("l"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.moveAllPoints.Value));
            }
            c.Index = 0;

            // Change Depth of Point
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("o"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.changeDepthOfPoint.Value));
            }
            c.Index = 0;
        }

        private static void SlideShowMenuScene_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            // Move Menu Scene
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("n"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.moveMenuScene.Value));
            }
            c.Index = 0;

            // Save Menu Scene
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.saveMenuScene.Value));
            }
            c.Index = 0;

            // Move Editor
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("m"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.moveSceneEditor.Value));
            }
            c.Index = 0;

            // Play Scene
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("i"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.testPlayScene.Value));
            }
            c.Index = 0;
        }

        private static void MenuScene_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            // Move Scene
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("n"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.moveMenuScene.Value));
            }
            c.Index = 0;

            // Save Scene
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.saveMenuScene.Value));
            }
            c.Index = 0;
        }



        private static void DeerAI_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            // Set Migratory Destination
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("e"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.setMigratoryDesination.Value));
            }
            c.Index = 0;

            // Offset Camera
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("n"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.offsetCamera.Value));
            }
            c.Index = 0;
        }

        private static void MirosBird_Act(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            // Remove Miros Bird Gravity (wtf?)
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("t"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.mirosAntiGravity.Value));
            }
            c.Index = 0;
        }



        private static void DaddyLongLegs_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            // Drag Entity
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.dragEntities.Value));
            }
            c.Index = 0;
        }

        private static void Cicada_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            // Drag Entity
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.dragEntities.Value));
            }
            c.Index = 0;
        }

        private static void Centipede_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            // Drag Entity
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.dragEntities.Value));
            }
            c.Index = 0;
        }

        private static void BigSpider_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            // Drag Entity
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.dragEntities.Value));
            }
            c.Index = 0;
        }

        private static void BigEel_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            // Drag Entity
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.dragEntities.Value));
            }
            c.Index = 0;
        }



        // I'm never touching this again
        // (maybe I'll redo it if I somehow find the will lol)
        #region Creatures
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

        private static void Vulture_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            // Drag Entity
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.dragEntities.Value));
            }
            c.Index = 0;

            // Fling Vultures
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("g"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.flingVultures.Value));
            }
            c.Index = 0;
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

            // Drag Entity
            while (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("b"),
                x => x.MatchCallOrCallvirt<Input>("GetKey")))
            {
                c.Index += 2;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Input.GetKey(Options.dragEntities.Value));
            }
            c.Index = 0;
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
        #endregion


        #region Objects
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
                    return Input.GetKey(Options.dragObjects.Value);
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
                    return Input.GetKey(Options.dragObjects.Value);
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
                    return Input.GetKey(Options.dragObjects.Value);
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
                    return Input.GetKey(Options.dragObjects.Value);
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
                    return Input.GetKey(Options.dragObjects.Value);
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
                    return Input.GetKey(Options.dragObjects.Value);
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
                    return Input.GetKey(Options.dragObjects.Value);
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
                    return Input.GetKey(Options.dragObjects.Value);
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
                    return Input.GetKey(Options.dragObjects.Value);
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
                    return Input.GetKey(Options.dragObjects.Value);
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
                    return Input.GetKey(Options.dragObjects.Value);
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
                    return Input.GetKey(Options.dragObjects.Value);
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
                    return Input.GetKey(Options.dragObjects.Value);
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
                    return Input.GetKey(Options.dragObjects.Value);
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
        #endregion
    }
}
