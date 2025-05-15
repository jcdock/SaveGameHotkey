using System;
using MelonLoader;
using UnityEngine;
using Il2CppScheduleOne.Persistence;
using ModManagerPhoneApp;
using System.Runtime.InteropServices;


namespace SaveGameHotkey
{
    public class SaveGameHotkey : MelonMod
    {
       
        public override void OnInitializeMelon()
        {
           SaveGameHotkey.Category = MelonPreferences.CreateCategory("SaveGameHotkey_Main", "Save Game Hotkey Settings");
            SaveGameHotkey.Keybind = SaveGameHotkey.Category.CreateEntry<KeyCode>("Keybind", KeyCode.F8, "Keybind to Save the Game.",null,false,false,null,null);
           SaveGameHotkey.CrtlModifier = SaveGameHotkey.Category.CreateEntry<bool>("CrtlModifier", false, "Use Ctrl as a modifier key.", null, false, false, null, null);
           SaveGameHotkey.ShiftModifier = SaveGameHotkey.Category.CreateEntry<bool>("ShiftModifier", false, "Use Shift as a modifier key.", null, false, false, null, null);
           SaveGameHotkey.AltModifier = SaveGameHotkey.Category.CreateEntry<bool>("AltModifier", false, "Use Alt as a modifier key.", null, false, false, null, null);

            

            SubscribeToModManagerEvents();

           LoggerInstance.Msg(base.Info.Name +  " v" + base.Info.Version + " Initialized!");
        }

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            //var _keybind = SaveGameHotkey.Keybind.Value;
            //var isKeyValid = IsKeyBindValid(_keybind);
            //if (!isKeyValid)
            //{
            //    LoggerInstance.Error($"Keybind not set or is set to invalid key. Defaulting to F8.");
            //    SaveGameHotkey.Keybind.Value = "F8";
            //}

        }

        public override void OnDeinitializeMelon()
        {
            UnsubscribeFromModManagerEvents();
        }

        public override void OnUpdate()
        {

                var crtlModifier = SaveGameHotkey.CrtlModifier.Value;
                var shiftModifier = SaveGameHotkey.ShiftModifier.Value;
                var altModifier = SaveGameHotkey.AltModifier.Value;

                if (crtlModifier && shiftModifier && altModifier)
                {
                    if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown((SaveGameHotkey.Keybind.Value)))
                    {
                        SaveGame();
                    }
                } else if (crtlModifier && shiftModifier)
                {
                    if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown((SaveGameHotkey.Keybind.Value)))
                    {
                        SaveGame();
                    }
                }
                else if (crtlModifier && altModifier)
                {
                    if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown((SaveGameHotkey.Keybind.Value)))
                    {
                        SaveGame();
                    }
                }
                else if (shiftModifier && altModifier)
                {
                    if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown((SaveGameHotkey.Keybind.Value)))
                    {
                        SaveGame();
                    }
                }
                else if (crtlModifier)
                {
                    if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown((SaveGameHotkey.Keybind.Value)))
                    {
                        SaveGame();
                    }
                }
                else if (shiftModifier)
                {
                    if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown((SaveGameHotkey.Keybind.Value)))
                    {
                        SaveGame();
                    }
                }
                else if (altModifier)
                {
                    if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown((SaveGameHotkey.Keybind.Value)))
                    {
                        SaveGame();
                    }
                }
            }  
      

        private void SaveGame()
        {
            var SaveManager = GameObject.FindObjectOfType<SaveManager>();
            if (SaveManager != null)
            {
                SaveManager.Save();
                LoggerInstance.Msg("Game Saved");
            }
            else
            {
                LoggerInstance.Error("SaveManager not found!");
            }
        }

        private KeyCode ParseKeybind(string keybind)
        {
            KeyCode keyCode;
            bool flag = Enum.TryParse<KeyCode>(keybind, out keyCode);
            KeyCode result;
            if (flag)
            {
                result = keyCode;
            }
            else
            {
                result = KeyCode.F9;
            }
            return result;
        }

        // All credit for the following code goes to the author of the Mod Manager, Prowiler.
        private void UnsubscribeFromModManagerEvents()
        {
            //LoggerInstance.Msg("Attempting to unsubscribe from Mod Manager events...");
            try { ModManagerPhoneApp.ModSettingsEvents.OnPhonePreferencesSaved -= HandleSettingsUpdate; } catch { /* Ignore */ }
            try { ModManagerPhoneApp.ModSettingsEvents.OnMenuPreferencesSaved -= HandleSettingsUpdate; } catch { /* Ignore */ }
        }

        private void SubscribeToModManagerEvents()
        {
            // Subscribe to Phone App saves
            try
            {
                ModSettingsEvents.OnPhonePreferencesSaved += HandleSettingsUpdate;
            }
            // Catch potential runtime errors during subscription
            catch (Exception ex)
            {
                LoggerInstance.Error($"Error subscribing to OnPhonePreferencesSaved: {ex}");
            }

            // Subscribe to Main Menu Config saves
            try
            {
                ModSettingsEvents.OnMenuPreferencesSaved += HandleSettingsUpdate; // Can use the SAME handler
            }
            catch (Exception ex)
            {
                LoggerInstance.Error($"Error subscribing to OnMenuPreferencesSaved: {ex}");
            }
        }

        private bool IsKeyBindValid(string keybind)
        {
            bool flag = Enum.TryParse<KeyCode>(keybind, out KeyCode keyCode);
            return flag;
        }

        private void HandleSettingsUpdate() // Can be static if it only accesses static fields/methods
        {
        
            try
            {
                SaveGameHotkey.Keybind = SaveGameHotkey.Category.GetEntry<KeyCode>("Keybind");
                SaveGameHotkey.CrtlModifier = SaveGameHotkey.Category.GetEntry<bool>("CrtlModifier");
                SaveGameHotkey.ShiftModifier = SaveGameHotkey.Category.GetEntry<bool>("ShiftModifier");
                SaveGameHotkey.AltModifier = SaveGameHotkey.Category.GetEntry<bool>("AltModifier");

               
            }
            catch (System.Exception ex) { LoggerInstance.Error($"Error applying updated settings after save: {ex}"); }
        }

        private static MelonPreferences_Category Category;
        private static MelonPreferences_Entry<KeyCode> Keybind;
        private static MelonPreferences_Entry<bool> CrtlModifier;
        private static MelonPreferences_Entry<bool> ShiftModifier;
        private static MelonPreferences_Entry<bool> AltModifier;
        private static MelonPreferences_Entry<KeyCode> key;

    }
}