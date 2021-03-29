#if false
using HarmonyLib;
using System.Linq;

// experimenting with dungeon validation
namespace SolastaMoveAllRooms
{
    [HarmonyPatch(typeof(UserLocationEditorScreen), "OnSaveCb")]
    internal static class UserLocationEditorScreen_OnSaveCb
    {
        public static bool Prefix(UserLocationEditorScreen __instance)
        {
            if (__instance == null)
                return true;

            var panel = __instance.UserLocationViewPanel;
            var location = panel.UserLocation;
            var rooms = location.UserRooms;

            if (!UserLocationDefinitions.CellsBySize.TryGetValue(location.Size, out var size))
            {
                Main.Error($"Unknown room size: {location.Size}");
                return true;
            }

            // get extents
            var minx = rooms.Min(ur => (int?)ur.Position.x) ?? 0;
            var maxx = rooms.Max(ur => (int?)(ur.Position.x + ur.OrientedWidth)) ?? 0;
            var miny = rooms.Min(ur => (int?)ur.Position.y) ?? 0;
            var maxy = rooms.Max(ur => (int?)(ur.Position.y + ur.OrientedHeight)) ?? 0;

            Main.Log($"xMin={minx}, yMin={miny}, xMax={maxx}, yMax={maxy}.");

            // Check all rooms are inside dungeon
            if (minx < 0 || miny < 0 || maxx > size || maxy > size)
            {
                Gui.GuiService.ShowMessage(MessageModal.Severity.Serious3,
                    "Save error",
                    "One or more rooms overlap the edge of the dungeon.  Please fix.", "OK", null, null, null);

                return false;
            }

            return true;
        }
    }
}
#endif
