using System;
using System.Linq;

namespace RebindDevTools;

public static partial class Hooks
{
    public static void ApplyInit() => On.RainWorld.OnModsInit += RainWorld_OnModsInit;


    public static bool IsInit { get; private set; } = false;

    private static void RainWorld_OnModsInit(On.RainWorld.orig_OnModsInit orig, RainWorld self)
    {
        try
        {
            ModOptions.RegisterOI();

            if (IsInit) return;
            IsInit = true;

            var mod = ModManager.ActiveMods.FirstOrDefault(mod => mod.id == Plugin.MOD_ID);

            Plugin.MOD_NAME = mod.name;
            Plugin.VERSION = mod.version;
            Plugin.AUTHORS = mod.authors;

            ApplyHooks();
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

    private static void ApplyHooks()
    {
        ApplyRebindHooks();

        On.RainWorld.PostModsInit += RainWorld_PostModsInit;
        On.RainWorldGame.ctor += RainWorldGame_ctor;
    }


    public static bool WasDevToolsActive { get; set; } = false;

    private static void RainWorld_PostModsInit(On.RainWorld.orig_PostModsInit orig, RainWorld self)
    {
        orig(self);

        try
        {
            WasDevToolsActive = ModOptions.devToolsEnabledByDefault.Value;
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

            if (ModOptions.rememberIfEnabled.Value)
            {
                self.devToolsActive = WasDevToolsActive;
                self.devToolsLabel.isVisible = self.devToolsActive;
            }
            else
            {
                self.devToolsActive = ModOptions.devToolsEnabledByDefault.Value;
                self.devToolsLabel.isVisible = self.devToolsActive;
            }
        }
        catch (Exception ex)
        {
            Plugin.Logger.LogError("RainWorldGame_ctor: " + ex);
        }
    }
}
