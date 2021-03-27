using HarmonyLib;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace SolastaMoveAllRooms
{
    [HarmonyPatch(typeof(UserLocationEditorScreen), "HandleInput")]
    internal static class UserLocationEditorScreen_HandleInput
    {
        public static void Prefix(UserLocationEditorScreen __instance, ref bool ___anythingModified, InputCommands.Id command)
        {
            if (__instance == null)
                return;

            var panel = __instance.UserLocationViewPanel;
            var location = panel.UserLocation;
            var rooms = location.UserRooms;

            // get extents
            var minx = rooms.Min(ur => (int?)ur.Position.x) ?? 0;
            var maxx = rooms.Max(ur => (int?)(ur.Position.x + ur.OrientedWidth)) ?? 0;
            var miny = rooms.Min(ur => (int?)ur.Position.y) ?? 0;
            var maxy = rooms.Max(ur => (int?)(ur.Position.y + ur.OrientedHeight)) ?? 0;

            if (!UserLocationDefinitions.CellsBySize.TryGetValue(location.Size, out var size))
            {
                Main.Error($"Unknown room size: {location.Size}");
                return;
            }

            switch (command)
            {
                case InputCommands.Id.SelectCharacter1:
                    if (maxx < size) MoveAll(1, 0, ref ___anythingModified);
                    break;

                case InputCommands.Id.SelectCharacter2:
                    if (minx > 0) MoveAll(-1, 0, ref ___anythingModified);
                    break;

                case InputCommands.Id.SelectCharacter3:
                    if (maxy < size) MoveAll(0, 1, ref ___anythingModified);
                    break;

                case InputCommands.Id.SelectCharacter4:
                    if (miny > 0) MoveAll(0, -1, ref ___anythingModified);
                    break;

                case InputCommands.Id.RotateCCW:
                    Rotate(-90f, ref ___anythingModified);
                    break;

                case InputCommands.Id.RotateCW:
                    Rotate(90f, ref ___anythingModified);
                    break;
            }

            #region Local functions
            void Rotate(float rotationAngle, ref bool anythingModified)
            {
                var rotation = Quaternion.Euler(0.0f, 0.0f, -rotationAngle);
                Main.Log($"angle={rotationAngle}, rotation={rotation}");

                var dungeonCenter = new Vector3(size / 2, size / 2);
                Main.Log($"dungeon center ({dungeonCenter.x}, {dungeonCenter.y})");

                foreach (var ur in rooms)
                {
                    Main.Log($"room orientation before ({ur.Orientation}, {ur.OrientedWidth}, {ur.OrientedHeight})");
                    Main.Log($"current room position ({ur.Position.x}, {ur.Position.y})");

                    var currentRoomCenter = new Vector3(ur.Position.x + ur.OrientedWidth / 2, ur.Position.y + ur.OrientedHeight / 2);
                    Main.Log($"current room center ({currentRoomCenter.x}, {currentRoomCenter.y})");

                    var newRoomCenter = rotation * (currentRoomCenter - dungeonCenter) + dungeonCenter;
                    Main.Log($"new room center ({newRoomCenter.x}, {newRoomCenter.y})");

                    ur.Rotate(rotationAngle);
                    Main.Log($"room orientation after {ur.Orientation}, {ur.OrientedWidth}, {ur.OrientedHeight}");

                    ur.Position = new Vector2Int(Mathf.RoundToInt(newRoomCenter.x - ur.OrientedWidth / 2), Mathf.RoundToInt(newRoomCenter.y - ur.OrientedHeight / 2));
                    Main.Log($"new room position ({ur.Position.x}, {ur.Position.y})");
                }

                Update(ref anythingModified);
            }

            void MoveAll(int xOffset, int yOffset, ref bool anythingModified)
            {
                Main.Log($"xmin={minx}, ymin={miny}, xmax={maxx}, ymax={maxy}, size={size}, xoff={xOffset}, yoff={yOffset}");

                foreach (var ur in rooms)
                {
                    Main.Log($"{ur.Position.x}, {ur.Position.y}");
                    ur.Position = new Vector2Int(ur.Position.x + xOffset, ur.Position.y + yOffset);
                    Main.Log($"{ur.Position.x}, {ur.Position.y}");
                }

                Update(ref anythingModified);
            }

            void Update(ref bool anythingModified)
            {
                anythingModified = true;
                panel.RefreshRooms();
                RefreshButtons();
            }

            void RefreshButtons()
            {
                var rb = typeof(UserLocationEditorScreen).GetMethod("RefreshButtons", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                Main.Log($"calling refreshbuttons {rb != null}");
                rb?.Invoke(__instance, null);
            }
            #endregion
        }
    }
}