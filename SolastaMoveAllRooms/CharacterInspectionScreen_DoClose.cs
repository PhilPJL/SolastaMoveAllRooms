#if false
using HarmonyLib;

// Experimenting with character export
namespace SolastaMoveAllRooms
{
    [HarmonyPatch(typeof(CharacterInspectionScreen), "DoClose")]
    internal static class CharacterInspectionScreen_DoClose
    {
        public static void Prefix(CharacterInspectionScreen __instance, ActionDefinitions.InventoryManagementMode ___inventoryManagementMode)
        {
            Main.Log($"CharacterInspectionScreen_DoClose: Mode={___inventoryManagementMode}, Name={__instance.InspectedCharacter?.RulesetCharacterHero?.Name ?? "null"}");

            ServiceRepository.GetService<ICharacterPoolService>().SaveCharacter(__instance.InspectedCharacter.RulesetCharacterHero);
        }
    }
}
#endif
