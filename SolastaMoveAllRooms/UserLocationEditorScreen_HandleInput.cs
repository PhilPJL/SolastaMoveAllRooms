using HarmonyLib;
using System.Linq;
using System.Reflection;

namespace SolastaMoveAllRooms
{
    [HarmonyPatch(typeof(UserLocationEditorScreen), "HandleInput")]
    internal static class UserLocationEditorScreen_HandleInput
    {
        public static void Prefix(UserLocationEditorScreen __instance, ref bool ___anythingModified, InputCommands.Id command)
        {
            if (__instance == null)
                return;

            var xOffset = 0;
            var yOffset = 0;

            var panel = __instance.UserLocationViewPanel;
            var location = panel.UserLocation;
            var rooms = location.UserRooms;

            // check for location boundaries
            var minx = rooms.Min(ur => (int?)ur.Position.x) ?? 0;
            var maxx = rooms.Max(ur => (int?)(ur.Position.x + ur.OrientedWidth)) ?? 0;
            var miny = rooms.Min(ur => (int?)ur.Position.y) ?? 0;
            var maxy = rooms.Max(ur => (int?)(ur.Position.y + ur.OrientedHeight)) ?? 0;

            var size = 0;

            switch (location.Size)
            {
                case UserLocationDefinitions.Size.Small:
                    size = 50;
                    break;
                case UserLocationDefinitions.Size.Medium:
                    size = 70;
                    break;
                case UserLocationDefinitions.Size.Large:
                    size = 100;
                    break;
                default:
                    Main.Error($"Unknown size {location.Size}");
                    break;
            }

            switch (command)
            {
                case InputCommands.Id.SelectCharacter1:
                    if (maxx < size) xOffset = 1;
                    break;

                case InputCommands.Id.SelectCharacter2:
                    if (minx > 0) xOffset = -1;
                    break;

                case InputCommands.Id.SelectCharacter3:
                    if (maxy < size) yOffset = 1;
                    break;

                case InputCommands.Id.SelectCharacter4:
                    if (miny > 0) yOffset = -1;
                    break;
            }

            if (xOffset != 0 || yOffset != 0)
            {
                Main.Log($"xmin={minx}, ymin={miny}, xmax={maxx}, ymax={maxy}, size={size}, xoff={xOffset}, yoff={yOffset}");

                foreach (var ur in rooms)
                {
                    ur.Position = new UnityEngine.Vector2Int(ur.Position.x + xOffset, ur.Position.y + yOffset);
                }

                ___anythingModified = true;
                panel.RefreshRooms();
                RefreshButtons();
            }

            void RefreshButtons()
            {
                var rb = typeof(UserLocationEditorScreen).GetMethod("RefreshButtons", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                Main.Log($"calling refreshbuttons {rb != null}");
                rb?.Invoke(__instance, null);
            }
        }
    }
}