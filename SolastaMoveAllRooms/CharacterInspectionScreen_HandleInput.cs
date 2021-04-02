#if false
using HarmonyLib;
using System.IO;

// Experimenting with character export
namespace SolastaMoveAllRooms
{
    [HarmonyPatch(typeof(CharacterInspectionScreen), "HandleInput")]
    internal static class CharacterInspectionScreen_HandleInput
    {
        public static bool Prefix(CharacterInspectionScreen __instance, InputCommands.Id command)
        {
            switch (command)
            {
                case InputCommands.Id.RotateCCW:
                    SaveCharacter();
                    break;
            }

            return true;

            void SaveCharacter()
            {
                Main.Log("Save the character");

                var heroCharacter = __instance.InspectedCharacter.RulesetCharacterHero;

                var name = heroCharacter.Name;
                var builtin = heroCharacter.BuiltIn;
                var guid = heroCharacter.Guid;
                Main.Log($"Is built in={builtin}, guid={guid}.");

                try
                {
                    heroCharacter.Name = "Exp" + name;
                    heroCharacter.BuiltIn = false;

                    //var hero = new RulesetCharacterHero();

                    AccessTools.Field(heroCharacter.GetType(), "guid").SetValue(heroCharacter, 0UL);

                    ServiceRepository.GetService<ICharacterPoolService>().SaveCharacter(heroCharacter, true);
                    //string path = TacticalAdventuresApplication.GameCharactersDirectory;
                    //string filename = Path.Combine(path, heroCharacter.Name) + ".chr";

                    //var snapshot = new RulesetCharacterHero.Snapshot();

                    //heroCharacter.FillSnapshot(snapshot, true);

                    //if (!Directory.Exists(path))
                    //    Directory.CreateDirectory(path);

                    //if (File.Exists(filename))
                    //    File.Delete(filename);

                    //using (var fileStream = File.Create(filename))
                    //using (var binarySerializer = new BinarySerializer(fileStream, Serializer.SerializationMode.Write, new BinarySerializer.Settings()))
                    //{
                    //    binarySerializer.SerializeElement("Snapshot", snapshot);
                    //    binarySerializer.SerializeElement("HeroCharacter", heroCharacter);
                    //}

                    //if (!__instance.Pool.ContainsKey(filename))
                    //    this.pool.Add(filename, snapshot);
                    //else
                    //    this.pool[filename] = snapshot;
                    //CharacterPoolDefinitions.CharacterPoolRefreshedHandler characterPoolRefreshed = this.CharacterPoolRefreshed;
                    //if (characterPoolRefreshed == null)
                    //    return;
                    //characterPoolRefreshed();
                }
                finally
                {
                    heroCharacter.Name = name;
                    heroCharacter.BuiltIn = builtin;
                    AccessTools.Field(heroCharacter.GetType(), "guid").SetValue(heroCharacter, guid);
                }
            }
        }
    }
}
#endif
