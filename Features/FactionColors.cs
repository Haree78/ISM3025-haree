using System;
using BattleTech;
using UnityEngine;

namespace ISM3025.Features
{
    public static class FactionColors
    {
        public static Color? GetModdedFactionColor(FactionValue faction)
        {
            string factionString;
            if (faction.FactionDefID.StartsWith("faction_"))
            {
                factionString = faction.FactionDefID.Substring(8);
            }
            else
            {
                factionString = faction.FactionDefID;
            }
            //Main.HBSLog.Log($"Faction string for ISM: {factionString}");
            if (factionString == null || !Main.Settings.FactionColors.ContainsKey(factionString))
                return null;

            var c = Main.Settings.FactionColors[factionString];
            return new Color(c[0], c[1], c[2], c[3]);
        }
    }
}
