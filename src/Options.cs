﻿using System.Collections.Generic;
using Menu.Remix.MixedUI;
using UnityEngine;

namespace RebindDevTools
{
    // Based on the options script from SBCameraScroll by SchuhBaum
    // https://github.com/SchuhBaum/SBCameraScroll/blob/Rain-World-v1.9/SourceCode/MainModOptions.cs
    public class Options : OptionInterface
    {
        public static Options instance = new();
        private const string AUTHORS_NAME = "forthbridge";

        #region Options

        public static Configurable<bool> devToolsEnabledByDefault = instance.config.Bind("devToolsEnabledByDefault", false, new ConfigurableInfo(
           "When checked, Dev Tools is enabled by default when starting a cycle.",
           null, "", "Enabled by Default?"));

        public static Configurable<bool> rememberIfEnabled = instance.config.Bind("rememberIfEnabled", true, new ConfigurableInfo(
           "When checked, Dev Tools being enabled / disabled persists through cycles.",
           null, "", "Remember If Enabled?"));

        public static Configurable<bool> entranceJumperEnabled = instance.config.Bind("entranceJumperEnabled", true, new ConfigurableInfo(
           "When checked, jump to a specific entrance in a room by pressing its corresponding numpad index.",
           null, "", "Pipe Jumper Enabled?"));


        public static Configurable<KeyCode> toggleDevTools = instance.config.Bind("toggleDevTools", KeyCode.O, new ConfigurableInfo(
            "Toggles Dev Mode, indicated by yellow text at the top of the screen, showing the current room.", null, "", "Toggle Dev Tools"));

        public static Configurable<KeyCode> toggleDevToolsInterface = instance.config.Bind("toggleDevToolsInterface", KeyCode.H, new ConfigurableInfo(
            "Toggles the main Dev Tools interface.", null, "", "Toggle Interface"));

        public static Configurable<KeyCode> toggleDebugInfo = instance.config.Bind("toggleDebugInfo", KeyCode.M, new ConfigurableInfo(
            "Toggles various debug information.", null, "", "Toggle Debug Info"));

        public static Configurable<KeyCode> toggleTileAccessibility = instance.config.Bind("toggleTileAccessibility", KeyCode.P, new ConfigurableInfo(
            "Toggles tile accessibility display for each creature type.", null, "", "Toggle Tile Access"));

        public static Configurable<KeyCode> feedSlugcat = instance.config.Bind("feedSlugcat", KeyCode.Q, new ConfigurableInfo(
            "Raises slugcat's food meter by 1 pip.", null, "", "Feed Slugcat"));

        public static Configurable<KeyCode> restartCycle = instance.config.Bind("restartCycle", KeyCode.R, new ConfigurableInfo(
            "Restarts the current cycle.", null, "", "Restart Cycle"));

        public static Configurable<KeyCode> slowDownTime = instance.config.Bind("slowDownTime", KeyCode.A, new ConfigurableInfo(
            "Reduces physics tickrate when held.", null, "", "Slow Down Time"));

        public static Configurable<KeyCode> speedUpTime = instance.config.Bind("speedUpTime", KeyCode.S, new ConfigurableInfo(
            "Increases physics tickrate when held", null, "", "Speed Up Time"));

        public static Configurable<KeyCode> teleportSlugcat = instance.config.Bind("teleportSlugcat", KeyCode.V, new ConfigurableInfo(
            "Teleports slugcat to the mouse cursor's location.", null, "", "Teleport Slugcat"));

        public static Configurable<KeyCode> flingSlugcat = instance.config.Bind("flingSlugcat", KeyCode.W, new ConfigurableInfo(
            "Flings slugcat towards the mouse cursor's location.", null, "", "Fling Slugcat"));

        public static Configurable<KeyCode> pullBatflies = instance.config.Bind("pullBatflies", KeyCode.F, new ConfigurableInfo(
            "Pulls all batflies in the room towards the mouse cursor's location.", null, "", "Pull Batflies"));

        public static Configurable<KeyCode> dragEntities = instance.config.Bind("dragEntities", KeyCode.B, new ConfigurableInfo(
            "Drags most types of entities towards the mouse cursor's current location." +
            "\nExcludes Slugcat, Batflies, Rocks, Spears and most rarer items.", null, "", "Drag Creatures"));


        public static Configurable<KeyCode> dragObjects = instance.config.Bind("dragObjects", KeyCode.B, new ConfigurableInfo(
            "Drags most objects (excluding creatures) towards the mouse's position", null, "", "Drag Objects"));



        public static Configurable<KeyCode> flingVultures = instance.config.Bind("flingVultures", KeyCode.G, new ConfigurableInfo(
            "Flings all vultures in the room skywards.", null, "", "Fling Vultures"));

        public static Configurable<KeyCode> offsetCamera = instance.config.Bind("offsetCamera", KeyCode.N, new ConfigurableInfo(
            "Offsets the camera slightly in the direction of the mouse while held.", null, "", "Offset Camera"));

        public static Configurable<KeyCode> setMigratoryDesination = instance.config.Bind("setMigratoryDesination", KeyCode.E, new ConfigurableInfo(
            "Sets the migratory destination of every creature in the region to the current room.", null, "", "Set Migratory Destination"));

        public static Configurable<KeyCode> reloadAllSounds = instance.config.Bind("reloadAllSounds", KeyCode.U, new ConfigurableInfo(
            "Reloads all sound samples.", null, "", "Reload Sounds"));

        public static Configurable<KeyCode> toggleConsoleLog = instance.config.Bind("toggleConsoleLog", KeyCode.K, new ConfigurableInfo(
            "Toggles a log displaying all UnityEngine.Debug.Log messages." +
            "\nThis is the same output as shown in ConsoleLog.txt", null, "", "Toggle Console Log"));




        public static Configurable<KeyCode> unloadRooms = instance.config.Bind("unloadRooms", KeyCode.Q, new ConfigurableInfo(
            "Unloads all rooms that the player is not currently in.", null, "", "Unload Rooms"));

        public static Configurable<KeyCode> setAIDestination = instance.config.Bind("setAIDestination", KeyCode.E, new ConfigurableInfo(
            "Sets the destination for all nearby creature AI to the mouse cursor's position.", null, "", "Set AI Destination"));

        public static Configurable<KeyCode> quarterPrecycleTime = instance.config.Bind("quarterPrecycleTime", KeyCode.L, new ConfigurableInfo(
            "Cuts the current precycle (shelter failure) time down by 4 times.", null, "", "Quarter Precycle Time"));

        public static Configurable<KeyCode> visualizeSounds = instance.config.Bind("visualizeSounds", KeyCode.I, new ConfigurableInfo(
            "Visualises all sounds emitted by creatures and toggles a log of currently playing sounds.", null, "", "Visualize Sounds"));



        public static Configurable<KeyCode> addPoint = instance.config.Bind("addPoint", KeyCode.J, new ConfigurableInfo(
            "...", null, "", "Add Point"));

        public static Configurable<KeyCode> removePoint = instance.config.Bind("removePoint", KeyCode.K, new ConfigurableInfo(
            "...", null, "", "Remove Point"));

        public static Configurable<KeyCode> moveAllPoints = instance.config.Bind("moveAllPoints", KeyCode.L, new ConfigurableInfo(
            "...", null, "", "Move All Points"));

        public static Configurable<KeyCode> changeDepthOfPoint = instance.config.Bind("changeDepthOfPoint", KeyCode.O, new ConfigurableInfo(
            "...", null, "", "Change Depth of Point"));



        public static Configurable<KeyCode> moveMenuScene = instance.config.Bind("moveMenuScene", KeyCode.N, new ConfigurableInfo(
            "Allows the hovered part of the current menu scene to be moved freely.", null, "", "Move Menu Scene"));

        public static Configurable<KeyCode> saveMenuScene = instance.config.Bind("saveMenuScene", KeyCode.B, new ConfigurableInfo(
            "Saves the current menu scene to disk.", null, "", "Save Menu Scene"));

        public static Configurable<KeyCode> moveSceneEditor = instance.config.Bind("moveSceneEditor", KeyCode.M, new ConfigurableInfo(
            "...", null, "", "Move Scene Editor"));

        public static Configurable<KeyCode> testPlayScene = instance.config.Bind("testPlayScene", KeyCode.I, new ConfigurableInfo(
            "...", null, "", "Test Play Scene"));



        public static Configurable<KeyCode> mirosAntiGravity = instance.config.Bind("mirosAntiGravity", KeyCode.T, new ConfigurableInfo(
            "...", null, "", "Disable Miros Bird Gravity"));



        public static Configurable<KeyCode> changeMinimapLayer = instance.config.Bind("changeMinimapLayer", KeyCode.N, new ConfigurableInfo(
            "...", null, "", "Change Minimap Layer"));

        public static Configurable<KeyCode> toggleViewNodeLabels = instance.config.Bind("toggleViewNodeLabels", KeyCode.J, new ConfigurableInfo(
            "...", null, "", "Toggle View Node Labels"));

        public static Configurable<KeyCode> changeHandleColor = instance.config.Bind("changeHandleColor", KeyCode.M, new ConfigurableInfo(
            "...", null, "", "Change Handle Color"));

        public static Configurable<KeyCode> setHandles = instance.config.Bind("setHandles", KeyCode.I, new ConfigurableInfo(
            "...", null, "", "Set Handles"));

        public static Configurable<KeyCode> increaseHandles = instance.config.Bind("increaseHandles", KeyCode.N, new ConfigurableInfo(
            "...", null, "", "Increase Handles"));

        public static Configurable<KeyCode> decreaseHandles = instance.config.Bind("decreaseHandles", KeyCode.J, new ConfigurableInfo(
            "...", null, "", "Decrease Handles"));

        public static Configurable<KeyCode> moveCloudsViewObject = instance.config.Bind("moveCloudsViewObject", KeyCode.T, new ConfigurableInfo(
            "Allows the hovered part of the current background AboveCloudsView to be moved freely.", null, "", "Move Clouds View Object"));

        public static Configurable<KeyCode> speedUpStartGame = instance.config.Bind("speedUpStartGame", KeyCode.S, new ConfigurableInfo(
            "Speeds up the New Game or Continue buttons when held.", null, "", "Speed Up Start Game"));



        public static Configurable<KeyCode> cycleJumper = instance.config.Bind("cycleJumper", KeyCode.Alpha0, new ConfigurableInfo(
            "When held, enables the cycle jumper. Press one of the following keybinds to jump to a specific point in the cycle.", null, "", "Enable Cycle Jumper"));

        public static Configurable<KeyCode> earlyCycle = instance.config.Bind("earlyCycle", KeyCode.LeftShift, new ConfigurableInfo(
            "When the cycle jumper keybind is held, jumps to early in the cycle.", null, "", "Early Cycle"));

        public static Configurable<KeyCode> midCycle = instance.config.Bind("midCycle", KeyCode.LeftAlt, new ConfigurableInfo(
            "When the cycle jumper keybind is held, jumps to around the middle of the cycle.", null, "", "Mid Cycle"));

        public static Configurable<KeyCode> lateCycle = instance.config.Bind("lateCycle", KeyCode.LeftControl, new ConfigurableInfo(
            "When the cycle jumper keybind is held, jumps to late in the cycle.", null, "", "Late Cycle"));


        public static Configurable<KeyCode> resetRain = instance.config.Bind("resetRain", KeyCode.Alpha9, new ConfigurableInfo(
            "Resets the rain timer in the current cycle.", null, "", "Reset Rain"));


        public static Configurable<KeyCode> killAllCreatures = instance.config.Bind("killAllCreatures", KeyCode.Alpha8, new ConfigurableInfo(
            "Kills all the creatures (except the player) in the current room.", null, "", "Kill All Creatures"));


        public static Configurable<KeyCode> spawnSpearmasterPearl = instance.config.Bind("spawnSpearmasterPearl", KeyCode.Alpha5, new ConfigurableInfo(
            "Spawns Spearmaster's Stomach Pearl at the player's location.", null, "", "Spawn Spearmaster Pearl"));

        public static Configurable<KeyCode> spawnHunterNeuron = instance.config.Bind("spawnHunterNeuron", KeyCode.Alpha6, new ConfigurableInfo(
            "Spawns Hunter's Green Neuron (NSHSwarmer) at the player's location.", null, "", "Spawn Hunter Neuron"));


        public static Configurable<KeyCode> spawnRivuletCell = instance.config.Bind("spawnRivuletCell", KeyCode.Alpha7, new ConfigurableInfo(
            "Spawns Rivulet's Mass Rarefaction Cell at the player's location.", null, "", "Spawn Rivulet Cell"));

        #endregion

        #region Parameters

        private readonly float spacing = 20f;
        private readonly float fontHeight = 20f;
        private readonly int numberOfCheckboxes = 2;
        private readonly float checkBoxSize = 60.0f;

        private float CheckBoxWithSpacing => checkBoxSize + 0.25f * spacing;

        private Vector2 marginX = new();
        private Vector2 pos = new();

        private readonly List<float> boxEndPositions = new();

        private readonly List<Configurable<bool>> checkBoxConfigurables = new();
        private readonly List<OpLabel> checkBoxesTextLabels = new();

        private readonly List<OpLabel> textLabels = new();

        #endregion



        private const int NUMBER_OF_TABS = 6;

        public override void Initialize()
        {
            base.Initialize();
            Tabs = new OpTab[NUMBER_OF_TABS];
            int tabIndex = -1;

            #region General

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

            #endregion


            #region Movement

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

            #endregion


            #region Debugging

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

            #endregion


            #region Cycle & Items

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

            #endregion


            #region Dev Interface

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

            #endregion


            #region Scene Editor

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

            #endregion
        }



        #region UI Elements

        private void AddTab(ref int tabIndex, string tabName)
        {
            tabIndex++;
            Tabs[tabIndex] = new OpTab(this, tabName);
            InitializeMarginAndPos();

            AddNewLine();
            AddTextLabel(Plugin.MOD_NAME, bigText: true);
            DrawTextLabels(ref Tabs[tabIndex]);

            AddNewLine(0.5f);
            AddTextLabel("Version " + Plugin.VERSION, FLabelAlignment.Left);
            AddTextLabel("by " + AUTHORS_NAME, FLabelAlignment.Right);
            DrawTextLabels(ref Tabs[tabIndex]);

            AddNewLine();
            AddBox();
        }


        private void InitializeMarginAndPos()
        {
            marginX = new Vector2(50f, 550f);
            pos = new Vector2(50f, 600f);
        }

        private void AddNewLine(float spacingModifier = 1f)
        {
            pos.x = marginX.x;
            pos.y -= spacingModifier * spacing;
        }


        private void AddBox()
        {
            marginX += new Vector2(spacing, -spacing);
            boxEndPositions.Add(pos.y);
            AddNewLine();
        }

        private void DrawBox(ref OpTab tab)
        {
            marginX += new Vector2(-spacing, spacing);
            AddNewLine();

            float boxWidth = marginX.y - marginX.x;
            int lastIndex = boxEndPositions.Count - 1;

            tab.AddItems(new OpRect(pos, new Vector2(boxWidth, boxEndPositions[lastIndex] - pos.y)));
            boxEndPositions.RemoveAt(lastIndex);
        }


        private void DrawKeybinder(Configurable<KeyCode> configurable, ref OpTab tab)
        {
            string name = (string)configurable.info.Tags[0];

            tab.AddItems(
                new OpLabel(new Vector2(115.0f, pos.y), new Vector2(100f, 34f), name)
                {
                    alignment = FLabelAlignment.Right,
                    verticalAlignment = OpLabel.LabelVAlignment.Center,
                    description = configurable.info?.description
                },
                new OpKeyBinder(configurable, new Vector2(235.0f, pos.y), new Vector2(146f, 30f), false)
            );

            AddNewLine(2);
        }


        private void AddCheckBox(Configurable<bool> configurable, string text)
        {
            checkBoxConfigurables.Add(configurable);
            checkBoxesTextLabels.Add(new OpLabel(new Vector2(), new Vector2(), text, FLabelAlignment.Left));
        }

        private void DrawCheckBoxes(ref OpTab tab) // changes pos.y but not pos.x
        {
            if (checkBoxConfigurables.Count != checkBoxesTextLabels.Count) return;

            float width = marginX.y - marginX.x;
            float elementWidth = (width - (numberOfCheckboxes - 1) * 0.5f * spacing) / numberOfCheckboxes;
            pos.y -= checkBoxSize;
            float _posX = pos.x;

            for (int checkBoxIndex = 0; checkBoxIndex < checkBoxConfigurables.Count; ++checkBoxIndex)
            {
                Configurable<bool> configurable = checkBoxConfigurables[checkBoxIndex];

                OpCheckBox checkBox = new(configurable, new Vector2(_posX, pos.y))
                {
                    description = configurable.info?.description ?? ""
                };

                tab.AddItems(checkBox);
                _posX += CheckBoxWithSpacing;

                OpLabel checkBoxLabel = checkBoxesTextLabels[checkBoxIndex];
                checkBoxLabel.pos = new Vector2(_posX, pos.y + 2f);
                checkBoxLabel.size = new Vector2(elementWidth - CheckBoxWithSpacing, fontHeight);
                tab.AddItems(checkBoxLabel);

                if (checkBoxIndex < checkBoxConfigurables.Count - 1)
                {
                    if ((checkBoxIndex + 1) % numberOfCheckboxes == 0)
                    {
                        AddNewLine();
                        pos.y -= checkBoxSize;
                        _posX = pos.x;
                    }
                    else
                    {
                        _posX += elementWidth - CheckBoxWithSpacing + 0.5f * spacing;
                    }
                }
            }

            checkBoxConfigurables.Clear();
            checkBoxesTextLabels.Clear();
        }


        private void AddTextLabel(string text, FLabelAlignment alignment = FLabelAlignment.Center, bool bigText = false)
        {
            float textHeight = (bigText ? 2f : 1f) * fontHeight;

            if (textLabels.Count == 0)
                pos.y -= textHeight;

            OpLabel textLabel = new(new Vector2(), new Vector2(20f, textHeight), text, alignment, bigText) // minimal size.x = 20f
            {
                autoWrap = true
            };

            textLabels.Add(textLabel);
        }

        private void DrawTextLabels(ref OpTab tab)
        {
            if (textLabels.Count == 0) return;

            float width = (marginX.y - marginX.x) / textLabels.Count;
            
            foreach (OpLabel textLabel in textLabels)
            {
                textLabel.pos = pos;
                textLabel.size += new Vector2(width - 20f, 0.0f);
                tab.AddItems(textLabel);
                pos.x += width;
            }

            pos.x = marginX.x;
            textLabels.Clear();
        }

        #endregion
    }
}