#if false
using HarmonyLib;

// Experimenting with character export
namespace SolastaMoveAllRooms
{
    [HarmonyPatch(typeof(RulesetActor), "SerializeAttributes")]
    internal static class RulesetActor_SerializeAttributes
    {
        public static bool Prefix(IAttributesSerializer serializer,IVersionProvider versionProvider)
        {
            Main.Log($"RulesetActor_SerializeAttributes: serializer={serializer}, mode={serializer?.Mode}, versionprovider={versionProvider}");

            var service = ServiceRepository.GetService<IRulesetEntityService>();
            Main.Log($"service={service}");

            if (serializer != null)
            {
                ulong guid = 0;
                guid = serializer.SerializeAttribute("Guid", guid);
                Main.Log($"RulesetActor_SerializeAttributes: guid={guid}");

                if(guid == 0UL)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
#endif
