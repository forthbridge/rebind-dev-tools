using Mono.Cecil.Cil;
using MonoMod.Cil;
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
                //IL.BigEel.Update += BigEel_Update;
                //IL.BigSpider.Update += BigSpider_Update;
                //IL.Centipede.Update += Centipede_Update;
                //IL.Cicada.Update += Cicada_Update;
                //IL.DaddyLongLegs.Update += DaddyLongLegs_Update;
                //IL.DropBug.Update += DropBug_Update;
                //IL.EggBug.Update += EggBug_Update;
                //IL.Fly.Update += Fly_Update;
                //IL.GarbageWorm.Update += GarbageWorm_Update;
                //IL.JellyFish.Update += JellyFish_Update;
                //IL.JetFish.Update += JetFish_Update;
                //IL.Leech.Update += Leech_Update;
                //IL.Lizard.Update += Lizard_Update;
                //IL.NeedleEgg.Update += NeedleEgg_Update;
                //IL.NeedleWorm.Update += NeedleWorm_Update;
                //IL.Vulture.Update += Vulture_Update;
                //IL.Scavenger.Update += Scavenger_Update;
                //IL.Spider.Update += Spider_Update;
                //IL.TempleGuard.Update += TempleGuard_Update;
                
                //IL.PoleMimic.Update += PoleMimic_Update;
                //IL.TentaclePlant.Update += TentaclePlant_Update;
                
                //IL.Deer.Update += Deer_Update;
                //IL.DeerAI.Update += DeerAI_Update;
                //IL.DeerPather.Update += DeerPather_Update;

                //IL.MirosBird.Act += MirosBird_Act;
                //IL.MirosBird.Update += MirosBird_Update;
                //IL.MirosBirdPather.FollowPath += MirosBirdPather_FollowPath;
                                
                IL.Player.Update += Player_Update;

                //IL.MoreSlugcats.Inspector.Update += Inspector_Update;
                //IL.MoreSlugcats.StowawayBug.Update += StowawayBug_Update;
                //IL.MoreSlugcats.BigJellyFish.DebugDrag += BigJellyFish_DebugDrag;
                //IL.MoreSlugcats.Yeek.Update += Yeek_Update;



                //// Objects
                //IL.DangleFruit.Update += DangleFruit_Update;
                //IL.EggBugEgg.Update += EggBugEgg_Update;
                //IL.Lantern.Update += Lantern_Update;
                //IL.SlimeMold.Update += SlimeMold_Update;
                //IL.Snail.Update += Snail_Update;
                //IL.VultureGrub.Update += VultureGrub_Update;
                //IL.Hazer.Update += Hazer_Update;
                //IL.SporePlant.Update += SporePlant_Update;
                //IL.SwollenWaterNut.Update += SwollenWaterNut_Update;
                //IL.TubeWorm.Update += TubeWorm_Update;

                //IL.MoreSlugcats.FireEgg.Update += FireEgg_Update;
                //IL.MoreSlugcats.DandelionPeach.Update += DandelionPeach_Update;
                //IL.MoreSlugcats.GlowWeed.Update += GlowWeed_Update;
                //IL.MoreSlugcats.GooieDuck.Update += GooieDuck_Update;



                //// Meta
                //IL.RainWorldGame.Update += RainWorldGame_Update;
                //IL.ForcedVisibilityVisualizer.Update += ForcedVisibilityVisualizer_Update;
                //IL.RainWorldGame.RawUpdate += RainWorldGame_RawUpdate;
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

        private static void VultureGrub_Update(ILContext il)
        {
            
        }

        private static void Vulture_Update(ILContext il)
        {
            
        }

        private static void VirtualMicrophone_Update(ILContext il)
        {
            
        }

        private static void TubeWorm_Update(ILContext il)
        {
            
        }

        private static void TentaclePlant_Update(ILContext il)
        {
            
        }

        private static void TempleGuard_Update(ILContext il)
        {
            
        }

        private static void SwollenWaterNut_Update(ILContext il)
        {
            
        }

        private static void SporePlant_Update(ILContext il)
        {
            
        }

        private static void Spider_Update(ILContext il)
        {
            
        }

        private static void Snail_Update(ILContext il)
        {
            
        }

        private static void SlimeMold_Update(ILContext il)
        {
            
        }

        private static void Scavenger_Update(ILContext il)
        {
            
        }

        private static void RoomCamera_Update(ILContext il)
        {
            
        }

        private static void RainWorldGame_RawUpdate(ILContext il)
        {
            
        }

        private static void RainWorldGame_Update(ILContext il)
        {
            
        }

        private static void PoleMimic_Update(ILContext il)
        {
            
        }

        private static void Player_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            while (c.TryGotoNext(
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

            //c.Index = 0;

            //while (c.TryGotoNext(
            //    x => x.MatchLdstr("w"),
            //    x => x.MatchCallOrCallvirt<Input>("GetKey"),
            //    x => x.MatchLdloc(32),
            //    x => x.MatchAnd(),
            //    x => x.MatchBrfalse(out _)))
            //{
            //    c.RemoveRange(2);
            //    c.Emit(OpCodes.Ldarg_0);

            //    c.EmitDelegate<Func<Player, bool>>((self) =>
            //    {
            //        return Input.GetKey(Options.flingSlugcat.Value);
            //    });
            //}

            c.Index = 0;

            while (c.TryGotoNext(
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

        private static void NeedleWorm_Update(ILContext il)
        {
            
        }

        private static void NeedleEgg_Update(ILContext il)
        {
            
        }

        private static void Yeek_Update(ILContext il)
        {
            
        }

        private static void StowawayBug_Update(ILContext il)
        {
            
        }

        private static void Inspector_Update(ILContext il)
        {
            
        }

        private static void GooieDuck_Update(ILContext il)
        {
            
        }

        private static void GlowWeed_Update(ILContext il)
        {
            
        }

        private static void FireEgg_Update(ILContext il)
        {
            
        }

        private static void DandelionPeach_Update(ILContext il)
        {
            
        }

        private static void BigJellyFish_DebugDrag(ILContext il)
        {
            
        }

        private static void MirosBirdPather_FollowPath(ILContext il)
        {
            
        }

        private static void MirosBird_Update(ILContext il)
        {
            
        }

        private static void MirosBird_Act(ILContext il)
        {
            
        }

        private static void SlugcatSelectMenu_StartGame(ILContext il)
        {
            
        }

        private static void CameraMovementEditor_Update(ILContext il)
        {
            
        }

        private static void SlideShowMenuScene_Update(ILContext il)
        {
            
        }

        private static void MenuScene_Update(ILContext il)
        {
            
        }

        private static void Menu_Update(ILContext il)
        {
            
        }

        private static void Lizard_Update(ILContext il)
        {
            
        }

        private static void Leech_Update(ILContext il)
        {
            
        }

        private static void Lantern_Update(ILContext il)
        {
            
        }

        private static void JetFish_Update(ILContext il)
        {
            
        }

        private static void JellyFish_Update(ILContext il)
        {
            
        }

        private static void Hazer_Update(ILContext il)
        {
            
        }

        private static void GarbageWorm_Update(ILContext il)
        {
            
        }

        private static void ForcedVisibilityVisualizer_Update(ILContext il)
        {
            
        }

        private static void Fly_Update(ILContext il)
        {
            
        }

        private static void EggBugEgg_Update(ILContext il)
        {
            
        }

        private static void EggBug_Update(ILContext il)
        {
            
        }

        private static void DropBug_Update(ILContext il)
        {
            
        }

        private static void DeerPather_Update(ILContext il)
        {
            
        }

        private static void DeerAI_Update(ILContext il)
        {
            
        }

        private static void Deer_Update(ILContext il)
        {
            
        }

        private static void DangleFruit_Update(ILContext il)
        {
            
        }

        private static void DaddyLongLegs_Update(ILContext il)
        {
            
        }

        private static void Cicada_Update(ILContext il)
        {
            
        }

        private static void Centipede_Update(ILContext il)
        {
            
        }

        private static void BigSpider_Update(ILContext il)
        {
            
        }

        private static void BigEel_Update(ILContext il)
        {
        }
    }
}
