using System.Collections.Generic;
using Menu.Remix.MixedUI;
using UnityEngine;

namespace RebindDevTools;

public sealed class ModOptions : OptionsTemplate
{
    public static ModOptions Instance { get; } = new();

    public static void RegisterOI()
    {
        if (MachineConnector.GetRegisteredOI(Plugin.MOD_ID) != Instance)
        {
            MachineConnector.SetRegisteredOI(Plugin.MOD_ID, Instance);
        }
    }

    #region Options

    public static Configurable<bool> devToolsEnabledByDefault = Instance.config.Bind("devToolsEnabledByDefault", false, new ConfigurableInfo(
       "When checked, Dev Tools is enabled by default when starting a cycle.",
       null, "", "Enabled by Default?"));

    public static Configurable<bool> rememberIfEnabled = Instance.config.Bind("rememberIfEnabled", true, new ConfigurableInfo(
       "When checked, Dev Tools being enabled / disabled persists through cycles.",
       null, "", "Remember If Enabled?"));

    public static Configurable<bool> entranceJumperEnabled = Instance.config.Bind("entranceJumperEnabled", true, new ConfigurableInfo(
       "When checked, jump to a specific entrance in a room by pressing its corresponding numpad index.",
       null, "", "Pipe Jumper Enabled?"));


    public static Configurable<KeyCode> toggleDevTools = Instance.config.Bind("toggleDevTools", KeyCode.O, new ConfigurableInfo(
        "Toggles Dev Mode, indicated by yellow text at the top of the screen, showing the current room.", null, "", "Toggle Dev Tools"));

    public static Configurable<KeyCode> toggleDevToolsInterface = Instance.config.Bind("toggleDevToolsInterface", KeyCode.H, new ConfigurableInfo(
        "Toggles the main Dev Tools interface.", null, "", "Toggle Interface"));

    public static Configurable<KeyCode> toggleDebugInfo = Instance.config.Bind("toggleDebugInfo", KeyCode.M, new ConfigurableInfo(
        "Toggles various debug information.", null, "", "Toggle Debug Info"));

    public static Configurable<KeyCode> toggleTileAccessibility = Instance.config.Bind("toggleTileAccessibility", KeyCode.P, new ConfigurableInfo(
        "Toggles tile accessibility display for each creature type.", null, "", "Toggle Tile Access"));

    public static Configurable<KeyCode> feedSlugcat = Instance.config.Bind("feedSlugcat", KeyCode.Q, new ConfigurableInfo(
        "Raises slugcat's food meter by 1 pip.", null, "", "Feed Slugcat"));

    public static Configurable<KeyCode> restartCycle = Instance.config.Bind("restartCycle", KeyCode.R, new ConfigurableInfo(
        "Restarts the current cycle.", null, "", "Restart Cycle"));

    public static Configurable<KeyCode> slowDownTime = Instance.config.Bind("slowDownTime", KeyCode.A, new ConfigurableInfo(
        "Reduces physics tickrate when held.", null, "", "Slow Down Time"));

    public static Configurable<KeyCode> speedUpTime = Instance.config.Bind("speedUpTime", KeyCode.S, new ConfigurableInfo(
        "Increases physics tickrate when held", null, "", "Speed Up Time"));

    public static Configurable<KeyCode> teleportSlugcat = Instance.config.Bind("teleportSlugcat", KeyCode.V, new ConfigurableInfo(
        "Teleports slugcat to the mouse cursor's location.", null, "", "Teleport Slugcat"));

    public static Configurable<KeyCode> flingSlugcat = Instance.config.Bind("flingSlugcat", KeyCode.W, new ConfigurableInfo(
        "Flings slugcat towards the mouse cursor's location.", null, "", "Fling Slugcat"));

    public static Configurable<KeyCode> pullBatflies = Instance.config.Bind("pullBatflies", KeyCode.F, new ConfigurableInfo(
        "Pulls all batflies in the room towards the mouse cursor's location.", null, "", "Pull Batflies"));

    public static Configurable<KeyCode> dragEntities = Instance.config.Bind("dragEntities", KeyCode.B, new ConfigurableInfo(
        "Drags most types of entities towards the mouse cursor's current location." +
        "\nExcludes Slugcat, Batflies, Rocks, Spears and most rarer items.", null, "", "Drag Creatures"));


    public static Configurable<KeyCode> dragObjects = Instance.config.Bind("dragObjects", KeyCode.B, new ConfigurableInfo(
        "Drags most objects (excluding creatures) towards the mouse's position", null, "", "Drag Objects"));



    public static Configurable<KeyCode> flingVultures = Instance.config.Bind("flingVultures", KeyCode.G, new ConfigurableInfo(
        "Flings all vultures in the room skywards.", null, "", "Fling Vultures"));

    public static Configurable<KeyCode> offsetCamera = Instance.config.Bind("offsetCamera", KeyCode.N, new ConfigurableInfo(
        "Offsets the camera slightly in the direction of the mouse while held.", null, "", "Offset Camera"));

    public static Configurable<KeyCode> setMigratoryDesination = Instance.config.Bind("setMigratoryDesination", KeyCode.E, new ConfigurableInfo(
        "Sets the migratory destination of every creature in the region to the current room.", null, "", "Set Migratory Destination"));

    public static Configurable<KeyCode> reloadAllSounds = Instance.config.Bind("reloadAllSounds", KeyCode.U, new ConfigurableInfo(
        "Reloads all sound samples.", null, "", "Reload Sounds"));

    public static Configurable<KeyCode> toggleConsoleLog = Instance.config.Bind("toggleConsoleLog", KeyCode.K, new ConfigurableInfo(
        "Toggles a log displaying all UnityEngine.Debug.Log messages." +
        "\nThis is the same output as shown in ConsoleLog.txt", null, "", "Toggle Console Log"));




    public static Configurable<KeyCode> unloadRooms = Instance.config.Bind("unloadRooms", KeyCode.Q, new ConfigurableInfo(
        "Unloads all rooms that the player is not currently in.", null, "", "Unload Rooms"));

    public static Configurable<KeyCode> setAIDestination = Instance.config.Bind("setAIDestination", KeyCode.E, new ConfigurableInfo(
        "Sets the destination for all nearby creature AI to the mouse cursor's position.", null, "", "Set AI Destination"));

    public static Configurable<KeyCode> quarterPrecycleTime = Instance.config.Bind("quarterPrecycleTime", KeyCode.L, new ConfigurableInfo(
        "Cuts the current precycle (shelter failure) time down by 4 times.", null, "", "Quarter Precycle Time"));

    public static Configurable<KeyCode> visualizeSounds = Instance.config.Bind("visualizeSounds", KeyCode.I, new ConfigurableInfo(
        "Visualises all sounds emitted by creatures and toggles a log of currently playing sounds.", null, "", "Visualize Sounds"));



    public static Configurable<KeyCode> addPoint = Instance.config.Bind("addPoint", KeyCode.J, new ConfigurableInfo(
        "...", null, "", "Add Point"));

    public static Configurable<KeyCode> removePoint = Instance.config.Bind("removePoint", KeyCode.K, new ConfigurableInfo(
        "...", null, "", "Remove Point"));

    public static Configurable<KeyCode> moveAllPoints = Instance.config.Bind("moveAllPoints", KeyCode.L, new ConfigurableInfo(
        "...", null, "", "Move All Points"));

    public static Configurable<KeyCode> changeDepthOfPoint = Instance.config.Bind("changeDepthOfPoint", KeyCode.O, new ConfigurableInfo(
        "...", null, "", "Change Depth of Point"));



    public static Configurable<KeyCode> moveMenuScene = Instance.config.Bind("moveMenuScene", KeyCode.N, new ConfigurableInfo(
        "Allows the hovered part of the current menu scene to be moved freely.", null, "", "Move Menu Scene"));

    public static Configurable<KeyCode> saveMenuScene = Instance.config.Bind("saveMenuScene", KeyCode.B, new ConfigurableInfo(
        "Saves the current menu scene to disk.", null, "", "Save Menu Scene"));

    public static Configurable<KeyCode> moveSceneEditor = Instance.config.Bind("moveSceneEditor", KeyCode.M, new ConfigurableInfo(
        "...", null, "", "Move Scene Editor"));

    public static Configurable<KeyCode> testPlayScene = Instance.config.Bind("testPlayScene", KeyCode.I, new ConfigurableInfo(
        "...", null, "", "Test Play Scene"));



    public static Configurable<KeyCode> mirosAntiGravity = Instance.config.Bind("mirosAntiGravity", KeyCode.T, new ConfigurableInfo(
        "...", null, "", "Disable Miros Bird Gravity"));



    public static Configurable<KeyCode> changeMinimapLayer = Instance.config.Bind("changeMinimapLayer", KeyCode.N, new ConfigurableInfo(
        "...", null, "", "Change Minimap Layer"));

    public static Configurable<KeyCode> toggleViewNodeLabels = Instance.config.Bind("toggleViewNodeLabels", KeyCode.J, new ConfigurableInfo(
        "...", null, "", "Toggle View Node Labels"));

    public static Configurable<KeyCode> changeHandleColor = Instance.config.Bind("changeHandleColor", KeyCode.M, new ConfigurableInfo(
        "...", null, "", "Change Handle Color"));

    public static Configurable<KeyCode> setHandles = Instance.config.Bind("setHandles", KeyCode.I, new ConfigurableInfo(
        "...", null, "", "Set Handles"));

    public static Configurable<KeyCode> increaseHandles = Instance.config.Bind("increaseHandles", KeyCode.N, new ConfigurableInfo(
        "...", null, "", "Increase Handles"));

    public static Configurable<KeyCode> decreaseHandles = Instance.config.Bind("decreaseHandles", KeyCode.J, new ConfigurableInfo(
        "...", null, "", "Decrease Handles"));

    public static Configurable<KeyCode> moveCloudsViewObject = Instance.config.Bind("moveCloudsViewObject", KeyCode.T, new ConfigurableInfo(
        "Allows the hovered part of the current background AboveCloudsView to be moved freely.", null, "", "Move Clouds View Object"));

    public static Configurable<KeyCode> speedUpStartGame = Instance.config.Bind("speedUpStartGame", KeyCode.S, new ConfigurableInfo(
        "Speeds up the New Game or Continue buttons when held.", null, "", "Speed Up Start Game"));



    public static Configurable<KeyCode> cycleJumper = Instance.config.Bind("cycleJumper", KeyCode.Alpha0, new ConfigurableInfo(
        "When held, enables the cycle jumper. Press one of the following keybinds to jump to a specific point in the cycle.", null, "", "Enable Cycle Jumper"));

    public static Configurable<KeyCode> earlyCycle = Instance.config.Bind("earlyCycle", KeyCode.LeftShift, new ConfigurableInfo(
        "When the cycle jumper keybind is held, jumps to early in the cycle.", null, "", "Early Cycle"));

    public static Configurable<KeyCode> midCycle = Instance.config.Bind("midCycle", KeyCode.LeftAlt, new ConfigurableInfo(
        "When the cycle jumper keybind is held, jumps to around the middle of the cycle.", null, "", "Mid Cycle"));

    public static Configurable<KeyCode> lateCycle = Instance.config.Bind("lateCycle", KeyCode.LeftControl, new ConfigurableInfo(
        "When the cycle jumper keybind is held, jumps to late in the cycle.", null, "", "Late Cycle"));


    public static Configurable<KeyCode> resetRain = Instance.config.Bind("resetRain", KeyCode.Alpha9, new ConfigurableInfo(
        "Resets the rain timer in the current cycle.", null, "", "Reset Rain"));


    public static Configurable<KeyCode> killAllCreatures = Instance.config.Bind("killAllCreatures", KeyCode.Alpha8, new ConfigurableInfo(
        "Kills all the creatures (except the player) in the current room.", null, "", "Kill All Creatures"));


    public static Configurable<KeyCode> spawnSpearmasterPearl = Instance.config.Bind("spawnSpearmasterPearl", KeyCode.Alpha5, new ConfigurableInfo(
        "Spawns Spearmaster's Stomach Pearl at the player's location.", null, "", "Spawn Spearmaster Pearl"));

    public static Configurable<KeyCode> spawnHunterNeuron = Instance.config.Bind("spawnHunterNeuron", KeyCode.Alpha6, new ConfigurableInfo(
        "Spawns Hunter's Green Neuron (NSHSwarmer) at the player's location.", null, "", "Spawn Hunter Neuron"));


    public static Configurable<KeyCode> spawnRivuletCell = Instance.config.Bind("spawnRivuletCell", KeyCode.Alpha7, new ConfigurableInfo(
        "Spawns Rivulet's Mass Rarefaction Cell at the player's location.", null, "", "Spawn Rivulet Cell"));

    #endregion

    private const int NUMBER_OF_TABS = 6;

    public override void Initialize()
    {
        base.Initialize();

        Tabs = new OpTab[NUMBER_OF_TABS];
        var tabIndex = -1;

        InitGeneral(ref tabIndex);
        InitMovement(ref tabIndex);
        InitDebugging(ref tabIndex);
        InitCycleAndItems(ref tabIndex);
        InitDevInterface(ref tabIndex);
        InitSceneEditor(ref tabIndex);
    }

    private void InitSceneEditor(ref int tabIndex)
    {
        AddTab(ref tabIndex, "Scene Editor");
        AddNewLine(2);

        DrawKeybinder(moveMenuScene, ref Tabs[tabIndex]);
        DrawKeybinder(saveMenuScene, ref Tabs[tabIndex]);
        DrawKeybinder(moveSceneEditor, ref Tabs[tabIndex]);
        DrawKeybinder(testPlayScene, ref Tabs[tabIndex]);

        AddNewLine(2);

        DrawKeybinder(addPoint, ref Tabs[tabIndex]);
        DrawKeybinder(removePoint, ref Tabs[tabIndex]);
        DrawKeybinder(moveAllPoints, ref Tabs[tabIndex]);
        DrawKeybinder(changeDepthOfPoint, ref Tabs[tabIndex]);

        AddNewLine(2);

        DrawBox(ref Tabs[tabIndex]);
    }

    private void InitDevInterface(ref int tabIndex)
    {
        AddTab(ref tabIndex, "Dev Interface");
        AddNewLine(2);

        DrawKeybinder(moveCloudsViewObject, ref Tabs[tabIndex]);

        AddNewLine(2);

        DrawKeybinder(changeMinimapLayer, ref Tabs[tabIndex]);
        DrawKeybinder(toggleViewNodeLabels, ref Tabs[tabIndex]);

        AddNewLine(2);

        DrawKeybinder(setHandles, ref Tabs[tabIndex]);
        DrawKeybinder(increaseHandles, ref Tabs[tabIndex]);
        DrawKeybinder(decreaseHandles, ref Tabs[tabIndex]);
        DrawKeybinder(changeHandleColor, ref Tabs[tabIndex]);

        AddNewLine(2);

        DrawBox(ref Tabs[tabIndex]);
    }

    private void InitCycleAndItems(ref int tabIndex)
    {
        AddTab(ref tabIndex, "Cycle & Items");
        AddNewLine(2);

        DrawKeybinder(cycleJumper, ref Tabs[tabIndex]);

        AddNewLine(1);

        DrawKeybinder(earlyCycle, ref Tabs[tabIndex]);
        DrawKeybinder(midCycle, ref Tabs[tabIndex]);
        DrawKeybinder(lateCycle, ref Tabs[tabIndex]);

        AddNewLine(1);

        DrawKeybinder(resetRain, ref Tabs[tabIndex]);
        DrawKeybinder(quarterPrecycleTime, ref Tabs[tabIndex]);

        AddNewLine(2);

        DrawKeybinder(spawnSpearmasterPearl, ref Tabs[tabIndex]);
        DrawKeybinder(spawnHunterNeuron, ref Tabs[tabIndex]);
        DrawKeybinder(spawnRivuletCell, ref Tabs[tabIndex]);

        AddNewLine(-2);

        DrawBox(ref Tabs[tabIndex]);
    }

    private void InitDebugging(ref int tabIndex)
    {
        AddTab(ref tabIndex, "Debugging");
        AddNewLine(2);

        DrawKeybinder(toggleDebugInfo, ref Tabs[tabIndex]);
        DrawKeybinder(toggleConsoleLog, ref Tabs[tabIndex]);
        DrawKeybinder(killAllCreatures, ref Tabs[tabIndex]);

        AddNewLine(2);

        DrawKeybinder(visualizeSounds, ref Tabs[tabIndex]);
        DrawKeybinder(reloadAllSounds, ref Tabs[tabIndex]);
        DrawKeybinder(unloadRooms, ref Tabs[tabIndex]);

        AddNewLine(2);

        DrawKeybinder(toggleTileAccessibility, ref Tabs[tabIndex]);
        DrawKeybinder(setAIDestination, ref Tabs[tabIndex]);
        DrawKeybinder(setMigratoryDesination, ref Tabs[tabIndex]);
        //DrawKeybinders(mirosAntiGravity, ref Tabs[tabIndex]);

        AddNewLine(-2);

        DrawBox(ref Tabs[tabIndex]);
    }

    private void InitMovement(ref int tabIndex)
    {
        AddTab(ref tabIndex, "Movement");
        AddNewLine(2);

        DrawKeybinder(teleportSlugcat, ref Tabs[tabIndex]);
        DrawKeybinder(flingSlugcat, ref Tabs[tabIndex]);

        AddNewLine(2);

        DrawKeybinder(dragEntities, ref Tabs[tabIndex]);
        DrawKeybinder(dragObjects, ref Tabs[tabIndex]);

        DrawKeybinder(pullBatflies, ref Tabs[tabIndex]);
        DrawKeybinder(flingVultures, ref Tabs[tabIndex]);

        AddNewLine(2);

        DrawKeybinder(offsetCamera, ref Tabs[tabIndex]);

        AddNewLine(-2);

        AddCheckBox(entranceJumperEnabled, (string)entranceJumperEnabled.info.Tags[0]);
        DrawCheckBoxes(ref Tabs[tabIndex]);

        AddNewLine(1);

        DrawBox(ref Tabs[tabIndex]);
    }

    private void InitGeneral(ref int tabIndex)
    {
        AddTab(ref tabIndex, "General");
        AddNewLine(-1);

        AddCheckBox(devToolsEnabledByDefault, (string)devToolsEnabledByDefault.info.Tags[0]);
        AddCheckBox(rememberIfEnabled, (string)rememberIfEnabled.info.Tags[0]);
        DrawCheckBoxes(ref Tabs[tabIndex]);

        AddNewLine(3);

        DrawKeybinder(toggleDevTools, ref Tabs[tabIndex]);
        DrawKeybinder(toggleDevToolsInterface, ref Tabs[tabIndex]);

        AddNewLine(2);

        DrawKeybinder(feedSlugcat, ref Tabs[tabIndex]);
        DrawKeybinder(restartCycle, ref Tabs[tabIndex]);

        AddNewLine(2);

        DrawKeybinder(speedUpTime, ref Tabs[tabIndex]);
        DrawKeybinder(slowDownTime, ref Tabs[tabIndex]);
        DrawKeybinder(speedUpStartGame, ref Tabs[tabIndex]);

        AddNewLine(-1);

        DrawBox(ref Tabs[tabIndex]);
    }
}