#if false
using HarmonyLib;
using System.Linq;

namespace SolastaMoveAllRooms
{
    internal static partial class UserLocationEditorScreen_HandleInput
    {
        [HarmonyPatch(typeof(SettingDropListItem), "Bind")]
        internal static class SettingDropListItem_Bind
        {
            public static void Postfix(SettingDropListItem __instance,
                Setting setting, SettingItem.OnSettingChangedHandler onSettingChanged, 
                SettingTypeDropListAttribute ___settingTypeDropListAttribute, GuiDropdown ___dropList)
            {
                if (__instance == null)
                    return;

                var attribute = ___settingTypeDropListAttribute;

                if (attribute?.Name == "TextLanguage")
                {
                    Main.Log("SettingDropListItem_Bind: TextLanguage");

                    foreach(var option in ___dropList.options)
                    {
                        Main.Log($"{option.text}");
                    }

                    if(!___dropList.options.Any(o => o.text == "pt"))
                    {
                        ___dropList.options.Add(new TMPro.TMP_Dropdown.OptionData("pt", null));
                    }
                }
            }
        }
    }
}
#endif