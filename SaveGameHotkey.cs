using System;
using MelonLoader;
using UnityEngine;
using Il2CppScheduleOne.Persistence;
using ModManagerPhoneApp;


namespace SaveGameHotkey
{
    public class SaveGameHotkey : MelonMod
    {
       
        public override void OnInitializeMelon()
        {
           SaveGameHotkey.Category = MelonPreferences.CreateCategory("SaveGameHotkey_Main", "Save Game Hotkey Settings");
           SaveGameHotkey.Keybind = SaveGameHotkey.Category.CreateEntry<string>("Keybind", "F8", "Keybind to Save the Game.",null,false,false,null,null);

           

           SubscribeToModManagerEvents();

           LoggerInstance.Msg(base.Info.Name +  " v" + base.Info.Version + " Initialized! Hotkey: " + SaveGameHotkey.Keybind.Value);
        }

        public override void OnDeinitializeMelon()
        {
            UnsubscribeFromModManagerEvents();
        }

        public override void OnUpdate()
        {

            if (Input.GetKeyDown(this.ParseKeybind(SaveGameHotkey.Keybind.Value)))
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

        private void HandleSettingsUpdate() // Can be static if it only accesses static fields/methods
        {
        
            LoggerInstance.Msg("Mod Manager saved preferences. Reloading settings...");
            try
            {
                SaveGameHotkey.Keybind = SaveGameHotkey.Category.GetEntry<string>("Keybind");

                LoggerInstance.Msg("Settings reloaded successfully.");
            }
            catch (System.Exception ex) { LoggerInstance.Error($"Error applying updated settings after save: {ex}"); }
        }

        private static MelonPreferences_Category Category;
        private static MelonPreferences_Entry<string> Keybind;


    }
}