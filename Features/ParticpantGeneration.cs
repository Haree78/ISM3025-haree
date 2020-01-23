using System;
using System.Collections.Generic;
using System.Linq;
using BattleTech;
using Harmony;

namespace ISM3025.Features
{
    public static class ParticpantGeneration
    {
        private static double GetDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
        }

        //public static int DoFive = 5;

        public static void TryAddParticipants(StarSystemDef def)
        {
            if (Main.Settings.SetContractParticipants &&
                def.Description.Name != "Terra")
            {
                Dictionary<string, double> majorDistances = new Dictionary<string, double>();
                Dictionary<string, double> minorDistances = new Dictionary<string, double>();

                foreach (string majorPower in Main.Settings.MajorFactionPositions.Keys)
                {
                    majorDistances.Add(majorPower, GetDistance(Main.Settings.MajorFactionPositions[majorPower][0], Main.Settings.MajorFactionPositions[majorPower][1], def.Position.x, def.Position.y));
                }

                foreach (string minorPower in Main.Settings.MinorFactionPositions.Keys)
                {
                    minorDistances.Add(minorPower, GetDistance(Main.Settings.MinorFactionPositions[minorPower][0], Main.Settings.MinorFactionPositions[minorPower][1], def.Position.x, def.Position.y));
                }

                /*if(DoFive >0)
                {
                    Main.HBSLog.Log($"ISM Major Distances: {majorDistances.Count}");
                    Main.HBSLog.Log($"ISM Minor Distances: {minorDistances.Count}");

                    Main.HBSLog.Log($"ISM This Position: {def.Position.x}, {def.Position.y}");

                    foreach (string majorPower in Main.Settings.MajorFactionPositions.Keys)
                    {
                        Main.HBSLog.Log($"ISM Faction Distance: {majorPower}, {majorDistances[majorPower]}");
                    }

                    foreach (string minorPower in Main.Settings.MinorFactionPositions.Keys)
                    {
                        Main.HBSLog.Log($"ISM Faction Distance: {minorPower}, {minorDistances[minorPower]}");
                    }

                    DoFive--;
                }*/

                if (Main.Settings.MajorFactionPositions.ContainsKey(def.OwnerValue.Name) ||
                    Main.Settings.MinorFactionPositions.ContainsKey(def.OwnerValue.Name))
                {
                    //Main.HBSLog.Log($"ISM Owner of planet: {def.OwnerValue.Name}");
                    double ownerDistance;

                    if (majorDistances.ContainsKey(def.OwnerValue.Name))
                    {
                        ownerDistance = majorDistances[def.OwnerValue.Name];
                    }
                    else
                    {
                        ownerDistance = minorDistances[def.OwnerValue.Name];
                    }

                    foreach (string majorPower in majorDistances.Keys)
                    {
                        if (majorPower != def.OwnerValue.Name)
                        {
                            if (majorDistances[majorPower] / Main.Settings.MajorInfluence < ownerDistance)
                            {
                                //Main.HBSLog.Log($"ISM Adding Contract Employer: {majorPower}");
                                if (!def.ContractEmployerIDList.Contains(majorPower))
                                {
                                    def.ContractEmployerIDList.Add(majorPower);
                                }
                            }
                        }
                    }

                    double smallest = double.MaxValue;
                    string smallestName = "";
                    foreach (string minorPower in minorDistances.Keys)
                    {
                        var thisDistance = minorDistances[minorPower];
                        if (thisDistance < smallest)
                        {
                            smallest = thisDistance;
                            smallestName = minorPower;
                        }
                    }
                    if (smallestName != def.OwnerValue.Name)
                    {
                        if (minorDistances[smallestName] / Main.Settings.MinorInfluence < ownerDistance)
                        {
                            //Main.HBSLog.Log($"ISM Adding Contract Employer: {smallestName}");
                            if (!def.ContractEmployerIDList.Contains(smallestName))
                            {
                                def.ContractEmployerIDList.Add(smallestName);
                            }
                        }
                    }
                }
                else
                {
                    //Main.HBSLog.Log($"ISM No Owner of planet");
                    if (!def.Tags.Contains("planet_pop_none"))
                    {
                        double smallestMajorD = double.MaxValue;
                        string smallestMajorName = "";
                        foreach (string majorPower in majorDistances.Keys)
                        {
                            var thisDistance = majorDistances[majorPower];
                            if (thisDistance < smallestMajorD)
                            {
                                smallestMajorD = thisDistance;
                                smallestMajorName = majorPower;
                            }
                        }

                        double smallestMinorD = double.MaxValue;
                        string smallestMinorName = "";
                        foreach (string minorPower in minorDistances.Keys)
                        {
                            var thisDistance = minorDistances[minorPower];
                            if (thisDistance < smallestMinorD)
                            {
                                smallestMinorD = thisDistance;
                                smallestMinorName = minorPower;
                            }
                        }

                        if(smallestMinorD < smallestMajorD)
                        {
                            if (!def.ContractEmployerIDList.Contains(smallestMinorName))
                            {
                                def.ContractEmployerIDList.Add(smallestMinorName);
                            }
                        }

                        if (!def.ContractEmployerIDList.Contains(smallestMajorName))
                        {
                            def.ContractEmployerIDList.Add(smallestMajorName);
                        }
                    }
                }
            }
        }
    }
}
