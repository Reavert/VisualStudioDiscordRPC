using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using VisualStudioDiscordRPC.Shared.Slots.TextSlots.Custom;
using VisualStudioDiscordRPC.Shared.Utils;

namespace VisualStudioDiscordRPC.Shared.Slots
{

    public class CustomSlotsSettings
    {
        private const string CustomSlotsSettingsPath = "custom_slots_settings.json";

        public List<CustomizableTextSlotSettings> CustomizableTextSlots = 
            new List<CustomizableTextSlotSettings>();

        public static CustomSlotsSettings Read()
        {
            string customSlotsSettingsPath = GetSettingsPath();

            if (File.Exists(customSlotsSettingsPath))
            {
                string json = File.ReadAllText(customSlotsSettingsPath);
                CustomSlotsSettings deserializedObject = JsonConvert.DeserializeObject<CustomSlotsSettings>(json);

                if (deserializedObject != null)
                {
                    return deserializedObject;
                }
            }

            return new CustomSlotsSettings();
        }

        public static void Write(CustomSlotsSettings customSlotsSettings)
        {
            string customSlotsSettingsPath = GetSettingsPath();

            string json = JsonConvert.SerializeObject(customSlotsSettings);
            File.WriteAllText(customSlotsSettingsPath, json);
        }

        private static string GetSettingsPath()
        {
            return Path.Combine(
                PathHelper.GetApplicationDataPath(),
                CustomSlotsSettingsPath);
        }
    }
}
