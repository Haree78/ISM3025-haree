using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Harmony;
using BattleTech;
using ISM3025.Features;

namespace ISM3025.Patches
{
    [HarmonyPatch(typeof(FactionValue), "GetMapColor")]
    public static class StarmapRenderer_GetMapColor_Patch
    {
        public static bool Prefix(FactionValue __instance, ref Color __result)
        {
            var color = FactionColors.GetModdedFactionColor(__instance);
            if (color == null)
                return true;

            __result = color.Value;
            return false;
        }
    }

    [HarmonyPatch(typeof(FactionValue), "GetMapBorderColor")]
    public static class StarmapRenderer_GetMapBorderColor_Patch
    {
        public static bool Prefix(FactionValue __instance, ref Color __result)
        {
            var color = FactionColors.GetModdedFactionColor(__instance);
            if (color == null)
                return true;

            __result = color.Value;
            return false;
        }
    }
}
