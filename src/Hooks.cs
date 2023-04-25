using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using UnityEngine;


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


        private static bool isInit = false;

        private static void RainWorld_OnModsInit(On.RainWorld.orig_OnModsInit orig, RainWorld self)
        {
            try
            {
                if (isInit) return;
                isInit = true;

                MachineConnector.SetRegisteredOI(Plugin.MOD_ID, Options.instance);


                // Creatures
                IL.BigEel.Update += BigEel_Update;
                IL.BigSpider.Update += BigSpider_Update;
                IL.Centipede.Update += Centipede_Update;
                IL.Cicada.Update += Cicada_Update;
                IL.DaddyLongLegs.Update += DaddyLongLegs_Update;

                IL.DropBug.Update += DropBug_Update;
                IL.EggBug.Update += EggBug_Update;
                IL.Fly.Update += Fly_Update; // F to fling instead of B
                IL.GarbageWorm.Update += GarbageWorm_Update;
                IL.JellyFish.Update += JellyFish_Update;
                IL.JetFish.Update += JetFish_Update;
                IL.Leech.Update += Leech_Update;
                IL.Lizard.Update += Lizard_Update;
                IL.NeedleWorm.Update += NeedleWorm_Update;
                IL.Vulture.Update += Vulture_Update; // G to fling skywards
                IL.Scavenger.Update += Scavenger_Update;
                IL.Spider.Update += Spider_Update;
                IL.TempleGuard.Update += TempleGuard_Update;
                IL.LanternMouse.Update += LanternMouse_Update;

                IL.PoleMimic.Update += PoleMimic_Update;
                IL.TentaclePlant.Update += TentaclePlant_Update;

                IL.Deer.Update += Deer_Update;
                IL.DeerAI.Update += DeerAI_Update;

                IL.MirosBird.Update += MirosBird_Update;
                //IL.MirosBird.Act += MirosBird_Act; // this is completely broken

                IL.Player.Update += Player_Update;

                IL.MoreSlugcats.Inspector.Update += Inspector_Update;
                IL.MoreSlugcats.StowawayBug.Update += StowawayBug_Update; // no clue if this works LMAO
                IL.MoreSlugcats.BigJellyFish.DebugDrag += BigJellyFish_DebugDrag;
                IL.MoreSlugcats.Yeek.Update += Yeek_Update;



                // Objects
                IL.NeedleEgg.Update += NeedleEgg_Update;
                IL.DangleFruit.Update += DangleFruit_Update;
                IL.EggBugEgg.Update += EggBugEgg_Update;
                IL.SlimeMold.Update += SlimeMold_Update;
                IL.Snail.Update += Snail_Update;
                IL.VultureGrub.Update += VultureGrub_Update;
                IL.Hazer.Update += Hazer_Update;
                IL.SporePlant.Update += SporePlant_Update;
                IL.SwollenWaterNut.Update += SwollenWaterNut_Update;
                IL.TubeWorm.Update += TubeWorm_Update;

                IL.MoreSlugcats.FireEgg.Update += FireEgg_Update;
                IL.MoreSlugcats.DandelionPeach.Update += DandelionPeach_Update;
                IL.MoreSlugcats.GlowWeed.Update += GlowWeed_Update;
                IL.MoreSlugcats.GooieDuck.Update += GooieDuck_Update;


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
            finally
            {
                orig(self);
            }
        }



        private static bool wasDevToolsActive = false;

        private static void RainWorld_PostModsInit(On.RainWorld.orig_PostModsInit orig, RainWorld self)
        {
            orig(self);

            try
            {
                wasDevToolsActive = Options.devToolsEnabledByDefault.Value;
            }
            catch (Exception ex)
            {
                Plugin.Logger.LogError("PostModsInit: " + ex);
            }
        }

        private static void RainWorldGame_ctor(On.RainWorldGame.orig_ctor orig, RainWorldGame self, ProcessManager manager)
        {
            orig(self, manager);

            try
            {
                if (!ModManager.DevTools) return;

                if (Options.rememberIfEnabled.Value)
                {
                    self.devToolsActive = wasDevToolsActive;
                    self.devToolsLabel.isVisible = self.devToolsActive;
                }
                else
                {
                    self.devToolsActive = Options.devToolsEnabledByDefault.Value;
                    self.devToolsLabel.isVisible = self.devToolsActive;
                }
            }
            catch (Exception ex)
            {
                Plugin.Logger.LogError("RainWorldGame_ctor: " + ex);
            }
        }



        private static void RebindKeyIL(ILCursor c, string key, Func<bool> inputAction, bool getKeyDown=false, int? keyCode=null)
        {
            int rebinds = 0;

            try
            {
                c.Index = 0;

                if (keyCode == null)
                {
                    while (c.TryGotoNext(MoveType.After,
                        x => x.MatchLdstr(key),
                        x => x.MatchCallOrCallvirt<Input>(getKeyDown ? nameof(Input.GetKeyDown) : nameof(Input.GetKey))))
                    {
                        c.Emit(OpCodes.Pop);
                        c.EmitDelegate(inputAction);
                        
                        rebinds++;
                    }
                }
                else
                {
                    while (c.TryGotoNext(MoveType.After,
                        x => x.MatchLdcI4((int)keyCode),
                        x => x.MatchCallOrCallvirt<Input>(getKeyDown ? nameof(Input.GetKeyDown) : nameof(Input.GetKey))))
                    {
                        c.Emit(OpCodes.Pop);
                        c.EmitDelegate(inputAction);

                        rebinds++;
                    }
                }
            }
            catch (Exception ex)
            {
                Plugin.Logger.LogWarning($"Error IL Rebinding Key! ({key})\n" + ex);
            }


            // n is intentionally attempted to be rebound more than it should be, for all creatures and objects, just in case it is used in a camera offset
            if (rebinds == 0 && key != "n")
                Plugin.Logger.LogWarning($"Key was not rebound at all! ({key})");
        }

        private static void RebindKeyDownIL(ILCursor c, string key, Func<bool> inputAction) => RebindKeyIL(c, key, inputAction, true);

        private static void RebindKeyCodeIL(ILCursor c, int keyCode, Func<bool> inputAction) => RebindKeyIL(c, keyCode.ToString(), inputAction, false, keyCode);



        private static void RainWorldGame_RawUpdate(ILContext il)
        {
            ILCursor c = new(il);
            
            RebindKeyIL(c, "h", () => Input.GetKey(Options.toggleDevToolsInterface.Value));

            RebindKeyIL(c, "a", () => Input.GetKey(Options.slowDownTime.Value));
            RebindKeyIL(c, "s", () => Input.GetKey(Options.speedUpTime.Value));
            
            RebindKeyIL(c, "l", () => Input.GetKey(Options.quarterPrecycleTime.Value));

            RebindKeyIL(c, "p", () => Input.GetKey(Options.toggleTileAccessibility.Value));
            RebindKeyIL(c, "m", () => Input.GetKey(Options.toggleDebugInfo.Value));
            RebindKeyIL(c, "k", () => Input.GetKey(Options.toggleConsoleLog.Value));
            
            RebindKeyIL(c, "q", () => Input.GetKey(Options.unloadRooms.Value));
            RebindKeyIL(c, "e", () => Input.GetKey(Options.setAIDestination.Value));


            // Toggle Dev Tools
            try
            {
                c.Index = 0;

                while (c.TryGotoNext(MoveType.After,
                    x => x.MatchLdstr("o"),
                    x => x.MatchCallOrCallvirt<Input>("GetKey")))
                {
                    c.Emit(OpCodes.Pop);
                 
                    c.Emit(OpCodes.Ldarg_0);
                    c.EmitDelegate<Func<RainWorldGame, bool>>((self) =>
                    {
                        wasDevToolsActive = self.devToolsActive;
                        return Input.GetKey(Options.toggleDevTools.Value);
                    });
                }
            }
            catch (Exception ex)
            {
                Plugin.Logger.LogWarning($"Error IL Rebinding Toggle Dev Tools! Exception:\n" + ex);
            }
        }

        private static void RainWorldGame_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "r", () => Input.GetKey(Options.restartCycle.Value));
        }



        // Weird shennanagins goes on in here 
        private static void Player_ProcessDebugInputs(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyDownIL(c, "0", () => Input.GetKeyDown(Options.cycleJumper.Value));
            RebindKeyDownIL(c, "9", () => Input.GetKeyDown(Options.resetRain.Value));
            RebindKeyDownIL(c, "8", () => Input.GetKeyDown(Options.killAllCreatures.Value));

            RebindKeyDownIL(c, "5", () => Input.GetKeyDown(Options.spawnSpearmasterPearl.Value));
            RebindKeyDownIL(c, "6", () => Input.GetKeyDown(Options.spawnHunterNeuron.Value));
            RebindKeyDownIL(c, "7", () => Input.GetKeyDown(Options.spawnRivuletCell.Value));


            RebindKeyCodeIL(c, 304, () => Input.GetKey(Options.earlyCycle.Value));
            RebindKeyCodeIL(c, 308, () => Input.GetKey(Options.midCycle.Value));
            RebindKeyCodeIL(c, 306, () => Input.GetKey(Options.lateCycle.Value));


            // Enable/Disable Pipe Jumping
            try
            {
                c.Index = 0;
                ILLabel dest = null!;
            
                if (c.TryGotoNext(MoveType.Before,
                    x => x.MatchLdloc(1),
                    x => x.MatchCallOrCallvirt<Input>(nameof(Input.GetKey)),
                    x => x.MatchBrfalse(out dest)))
                {
                    c.MoveAfterLabels();
                    c.Emit(OpCodes.Ldarg_0);

                    c.EmitDelegate<Func<RainWorldGame, bool>>((self) => Options.entranceJumperEnabled.Value);
                
                    c.Emit(OpCodes.Brfalse, dest);
                }
            }
            catch (Exception ex)
            {
                Plugin.Logger.LogWarning($"Error IL Rebinding Pipe Jumper! Exception:\n" + ex);
            }
        }



        private static void MiniMap_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "n", () => Input.GetKey(Options.changeMinimapLayer.Value));
        }

        private static void MapPage_Signal(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "n", () => Input.GetKey(Options.changeMinimapLayer.Value));
        }

        private static void MapPage_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "j", () => Input.GetKey(Options.toggleViewNodeLabels.Value));
        }

        private static void Handle_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "b", () => Input.GetKey(Options.changeHandleColor.Value));
        }

        private static void CustomDecalRepresentation_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "l", () => Input.GetKey(Options.setHandles.Value));
            RebindKeyIL(c, "k", () => Input.GetKey(Options.increaseHandles.Value));
            RebindKeyIL(c, "j", () => Input.GetKey(Options.decreaseHandles.Value));
        }

        private static void AboveCloudsView_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "t", () => Input.GetKey(Options.moveCloudsViewObject.Value));
        }



        private static void Player_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "v", () => Input.GetKey(Options.teleportSlugcat.Value));
            RebindKeyIL(c, "w", () => Input.GetKey(Options.flingSlugcat.Value));
            RebindKeyIL(c, "q", () => Input.GetKey(Options.feedSlugcat.Value));
        }



        private static void VirtualMicrophone_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "u", () => Input.GetKey(Options.reloadAllSounds.Value));
            RebindKeyIL(c, "i", () => Input.GetKey(Options.visualizeSounds.Value));
        }

        private static void RoomCamera_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "n", () => Input.GetKey(Options.offsetCamera.Value));
        }

        private static void Menu_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "a", () => Input.GetKey(Options.slowDownTime.Value));
            RebindKeyIL(c, "s", () => Input.GetKey(Options.speedUpTime.Value));
        }

        private static void ForcedVisibilityVisualizer_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "n", () => Input.GetKey(Options.offsetCamera.Value));


            // wtf ???
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
        }



        private static void SlugcatSelectMenu_StartGame(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "s", () => Input.GetKey(Options.speedUpStartGame.Value));
        }

        private static void CameraMovementEditor_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "j", () => Input.GetKey(Options.addPoint.Value));
            RebindKeyIL(c, "k", () => Input.GetKey(Options.removePoint.Value));
            RebindKeyIL(c, "l", () => Input.GetKey(Options.moveAllPoints.Value));
            RebindKeyIL(c, "o", () => Input.GetKey(Options.changeDepthOfPoint.Value));
        }

        private static void SlideShowMenuScene_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "n", () => Input.GetKey(Options.moveMenuScene.Value));
            RebindKeyIL(c, "b", () => Input.GetKey(Options.saveMenuScene.Value));
            RebindKeyIL(c, "m", () => Input.GetKey(Options.moveSceneEditor.Value));
            RebindKeyIL(c, "i", () => Input.GetKey(Options.testPlayScene.Value));
        }

        private static void MenuScene_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "n", () => Input.GetKey(Options.moveMenuScene.Value));
            RebindKeyIL(c, "b", () => Input.GetKey(Options.saveMenuScene.Value));
        }



        private static void DeerAI_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "e", () => Input.GetKey(Options.setMigratoryDesination.Value));
            RebindKeyIL(c, "n", () => Input.GetKey(Options.offsetCamera.Value));
        }

        private static void MirosBird_Act(ILContext il)
        {
            ILCursor c = new(il);

            // Remove Miros Bird Gravity (wtf?)
            RebindKeyIL(c, "t", () => Input.GetKey(Options.mirosAntiGravity.Value));
        }



        #region Creatures

        private static void DaddyLongLegs_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "b", () => Input.GetKey(Options.dragEntities.Value));
            RebindKeyIL(c, "n", () => Input.GetKey(Options.offsetCamera.Value));
        }

        private static void Cicada_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "b", () => Input.GetKey(Options.dragEntities.Value));
            RebindKeyIL(c, "n", () => Input.GetKey(Options.offsetCamera.Value));
        }

        private static void Centipede_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "b", () => Input.GetKey(Options.dragEntities.Value));
            RebindKeyIL(c, "n", () => Input.GetKey(Options.offsetCamera.Value));
        }

        private static void BigSpider_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "b", () => Input.GetKey(Options.dragEntities.Value));
            RebindKeyIL(c, "n", () => Input.GetKey(Options.offsetCamera.Value));
        }

        private static void BigEel_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "b", () => Input.GetKey(Options.dragEntities.Value));
            RebindKeyIL(c, "n", () => Input.GetKey(Options.offsetCamera.Value));
        }



        private static void LanternMouse_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "b", () => Input.GetKey(Options.dragEntities.Value));
            RebindKeyIL(c, "n", () => Input.GetKey(Options.offsetCamera.Value));
        }

        private static void Vulture_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "b", () => Input.GetKey(Options.dragEntities.Value));
            RebindKeyIL(c, "n", () => Input.GetKey(Options.offsetCamera.Value));

            RebindKeyIL(c, "g", () => Input.GetKey(Options.flingVultures.Value));
        }

        private static void TentaclePlant_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "b", () => Input.GetKey(Options.dragEntities.Value));
            RebindKeyIL(c, "n", () => Input.GetKey(Options.offsetCamera.Value));
        }

        private static void TempleGuard_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "b", () => Input.GetKey(Options.dragEntities.Value));
            RebindKeyIL(c, "n", () => Input.GetKey(Options.offsetCamera.Value));
        }

        private static void Spider_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "b", () => Input.GetKey(Options.dragEntities.Value));
            RebindKeyIL(c, "n", () => Input.GetKey(Options.offsetCamera.Value));
        }

        private static void Scavenger_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "b", () => Input.GetKey(Options.dragEntities.Value));
            RebindKeyIL(c, "n", () => Input.GetKey(Options.offsetCamera.Value));
        }

        private static void PoleMimic_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "b", () => Input.GetKey(Options.dragEntities.Value));
            RebindKeyIL(c, "n", () => Input.GetKey(Options.offsetCamera.Value));
        }

        private static void NeedleWorm_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "b", () => Input.GetKey(Options.dragEntities.Value));
            RebindKeyIL(c, "n", () => Input.GetKey(Options.offsetCamera.Value));
        }

        private static void NeedleEgg_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "b", () => Input.GetKey(Options.dragEntities.Value));
            RebindKeyIL(c, "n", () => Input.GetKey(Options.offsetCamera.Value));
        }

        private static void Yeek_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "b", () => Input.GetKey(Options.dragEntities.Value));
            RebindKeyIL(c, "n", () => Input.GetKey(Options.offsetCamera.Value));
        }

        private static void StowawayBug_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "b", () => Input.GetKey(Options.dragEntities.Value));
            RebindKeyIL(c, "n", () => Input.GetKey(Options.offsetCamera.Value));
        }

        private static void Inspector_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "b", () => Input.GetKey(Options.dragEntities.Value));
            RebindKeyIL(c, "n", () => Input.GetKey(Options.offsetCamera.Value));
        }

        private static void BigJellyFish_DebugDrag(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "b", () => Input.GetKey(Options.dragEntities.Value));
            RebindKeyIL(c, "n", () => Input.GetKey(Options.offsetCamera.Value));
        }

        private static void MirosBird_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "b", () => Input.GetKey(Options.dragEntities.Value));
            RebindKeyIL(c, "n", () => Input.GetKey(Options.offsetCamera.Value));
        }

        private static void Lizard_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "b", () => Input.GetKey(Options.dragEntities.Value));
            RebindKeyIL(c, "n", () => Input.GetKey(Options.offsetCamera.Value));
        }

        private static void Leech_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "b", () => Input.GetKey(Options.dragEntities.Value));
            RebindKeyIL(c, "n", () => Input.GetKey(Options.offsetCamera.Value));
        }

        private static void JetFish_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "b", () => Input.GetKey(Options.dragEntities.Value));
            RebindKeyIL(c, "n", () => Input.GetKey(Options.offsetCamera.Value));
        }

        private static void GarbageWorm_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "b", () => Input.GetKey(Options.dragEntities.Value));
            RebindKeyIL(c, "n", () => Input.GetKey(Options.offsetCamera.Value));
        }

        private static void DropBug_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "b", () => Input.GetKey(Options.dragEntities.Value));
            RebindKeyIL(c, "n", () => Input.GetKey(Options.offsetCamera.Value));
        }

        private static void Deer_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "b", () => Input.GetKey(Options.dragEntities.Value));
            RebindKeyIL(c, "n", () => Input.GetKey(Options.offsetCamera.Value));
        }

        private static void EggBug_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "b", () => Input.GetKey(Options.dragEntities.Value));
            RebindKeyIL(c, "n", () => Input.GetKey(Options.offsetCamera.Value));
        }



        private static void Fly_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "f", () => Input.GetKey(Options.pullBatflies.Value));
        }

        #endregion



        #region Objects
        
        private static void VultureGrub_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "b", () => Input.GetKey(Options.dragObjects.Value));
            RebindKeyIL(c, "n", () => Input.GetKey(Options.offsetCamera.Value));
        }
        
        private static void GooieDuck_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "b", () => Input.GetKey(Options.dragObjects.Value));
            RebindKeyIL(c, "n", () => Input.GetKey(Options.offsetCamera.Value));
        }

        private static void GlowWeed_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "b", () => Input.GetKey(Options.dragObjects.Value));
            RebindKeyIL(c, "n", () => Input.GetKey(Options.offsetCamera.Value));
        }

        private static void FireEgg_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "b", () => Input.GetKey(Options.dragObjects.Value));
            RebindKeyIL(c, "n", () => Input.GetKey(Options.offsetCamera.Value));
        }

        private static void DandelionPeach_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "b", () => Input.GetKey(Options.dragObjects.Value));
            RebindKeyIL(c, "n", () => Input.GetKey(Options.offsetCamera.Value));
        }

        private static void EggBugEgg_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "b", () => Input.GetKey(Options.dragObjects.Value));
            RebindKeyIL(c, "n", () => Input.GetKey(Options.offsetCamera.Value));
        }

        private static void DangleFruit_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "b", () => Input.GetKey(Options.dragObjects.Value));
            RebindKeyIL(c, "n", () => Input.GetKey(Options.offsetCamera.Value));
        }

        private static void JellyFish_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "b", () => Input.GetKey(Options.dragObjects.Value));
            RebindKeyIL(c, "n", () => Input.GetKey(Options.offsetCamera.Value));
        }

        private static void Hazer_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "b", () => Input.GetKey(Options.dragObjects.Value));
            RebindKeyIL(c, "n", () => Input.GetKey(Options.offsetCamera.Value));
        }

        private static void Snail_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "b", () => Input.GetKey(Options.dragObjects.Value));
            RebindKeyIL(c, "n", () => Input.GetKey(Options.offsetCamera.Value));
        }

        private static void SlimeMold_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "b", () => Input.GetKey(Options.dragObjects.Value));
            RebindKeyIL(c, "n", () => Input.GetKey(Options.offsetCamera.Value));
        }

        private static void SwollenWaterNut_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "b", () => Input.GetKey(Options.dragObjects.Value));
            RebindKeyIL(c, "n", () => Input.GetKey(Options.offsetCamera.Value));
        }

        private static void SporePlant_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "b", () => Input.GetKey(Options.dragObjects.Value));
            RebindKeyIL(c, "n", () => Input.GetKey(Options.offsetCamera.Value));
        }

        private static void TubeWorm_Update(ILContext il)
        {
            ILCursor c = new(il);

            RebindKeyIL(c, "b", () => Input.GetKey(Options.dragObjects.Value));
            RebindKeyIL(c, "n", () => Input.GetKey(Options.offsetCamera.Value));
        }

        #endregion
    }
}
