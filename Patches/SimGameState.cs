using BattleTech;
using Harmony;
using ISM3025.Features;

// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace ISM3025.Patches
{
    //public static class SetParticipantPositions
    //{
    //    public static void SetPositions()
    //    {
    //        if (ParticpantGeneration.majorPositions.Count == 0)
    //        {
    //            foreach (string majorParticipant in Main.Settings.MajorFactionPositions.Keys)
    //            {
    //                double[] keyPosition = new double[2] { Main.Settings.MajorFactionPositions[majorParticipant][0], Main.Settings.MajorFactionPositions[majorParticipant][1] };
    //                ParticpantGeneration.majorPositions.Add(majorParticipant, keyPosition);
    //            }
    //        }
    //        if (ParticpantGeneration.minorPositions.Count == 0)
    //        {
    //            foreach (string minorParticipant in Main.Settings.MinorFactionPositions.Keys)
    //            {
    //                double[] keyPosition = new double[2] { Main.Settings.MinorFactionPositions[minorParticipant][0], Main.Settings.MajorFactionPositions[minorParticipant][1] };
    //                ParticpantGeneration.minorPositions.Add(minorParticipant, keyPosition);
    //            }
    //        }
    //    }
    //}

    [HarmonyPatch(typeof(SimGameState), "InitializeDataFromDefs")]
    public static class SimGameState_InitializeDataFromDefs_Patch
    {
        public static void Prefix(SimGameState __instance)
        {
            //SetParticipantPositions.SetPositions();

            foreach (var defKVP in __instance.DataManager.SystemDefs)
            {
                ShopGeneration.TryAddItemCollections(defKVP.Value);
                ParticpantGeneration.TryAddParticipants(defKVP.Value);
            }
        }
    }

    [HarmonyPatch(typeof(SimGameState), "Rehydrate")]
    public static class SimGameState_Rehydrate_Patch
    {
        public static void Prefix(SimGameState __instance)
        {
            //SetParticipantPositions.SetPositions();

            foreach (var defKVP in __instance.DataManager.SystemDefs)
            {
                ShopGeneration.TryAddItemCollections(defKVP.Value);
                ParticpantGeneration.TryAddParticipants(defKVP.Value);
            }
        }
    }
}
