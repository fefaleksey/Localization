using System.Collections.Generic;
using System;

namespace Localization
{
    class RuleOfTheRightHand
    {
        private const int Down = 1;
        private const int Left = 2;
        private const int Up = 3;
        private const int Right = 4;

        private class Way
        {
            //private List<int> CurrentWay = new List<int>();

            public void SimulationOfLocalization(ref Map map, ref FinalWays finalWays)
            {
                var robot = new Robot();
                var localization = false;
                var motion = new Motion();
                map.HypothesisInit();
                var hypothesisCopy = new List<List<int>>();
                map.Copy_Lists(ref hypothesisCopy, map.Hypothesis);
                robot.InitialDirection = 3;
                for (var i = 0; i < hypothesisCopy[0].Count; i++)
                {
                    map.HypothesisInit();
                    var beginWay = true;
                    var x = hypothesisCopy[0][i];
                    var y = hypothesisCopy[1][i];
                    var direction = hypothesisCopy[2][i];
                    finalWays.directions.Add(new List<int>());
                    finalWays.directions[i].Add(x);
                    finalWays.directions[i].Add(y);
                    finalWays.directions[i].Add(direction);
                    map.SensorsRead(x, y, direction, robot);
                    HypothesisFilter(ref map);
                    while (!localization)
                    {
                        localization = true;
                        var newDir = NextDirection(robot.Sensors);
                        newDir = motion.GetNewDir(direction, newDir, true);
                        direction = newDir;
                        switch (newDir)
                        {
                            case Map.Down:
                            {
                                if (x + 1 < map.Height && map._map[x, y, Map.Down] == 0)
                                {
                                    ++x;
                                    map.SensorsRead(x, y, Map.Down, robot);
                                    //time ++;
                                }
                                break;
                            }
                            case Map.Left:
                            {
                                if (y > 0 && map._map[x, y, Map.Left] == 0)
                                {
                                    --y;
                                    map.SensorsRead(x, y, Map.Left, robot);
                                    //time += 2;
                                }
                                break;
                            }
                            case Map.Up:
                            {
                                if (x > 0 && map._map[x, y, Map.Up] == 0)
                                {
                                    --x;
                                    map.SensorsRead(x, y, Map.Up, robot);
                                    //time++;
                                }
                                break;
                            }
                            case Map.Right:
                            {
                                if (y + 1 < map.Wight && map._map[x, y, Map.Right] == 0)
                                {
                                    ++y;
                                    map.SensorsRead(x, y, Map.Right, robot);
                                    //time += 2;
                                }
                                break;
                            }
                            default:
                            {
                                Console.WriteLine("SolutionForRobot.SimulationOfLocalization - BAG 1111111111111111");
                                break;
                            }
                        }
                        map.Hypothesis3(newDir, true, motion, robot);
                        
                        if (map.Hypothesis[0].Count == 1)
                        {
                            
                        }
                    }
                }
            }

            private void HypothesisFilter(ref Map map) //, int step 
            {
                map.HypothesisInit();
                map.Hypothesis1New();
                for (var i = 0; i < map.Hypothesis[0].Count; i++)
                {
                    int x = map.Hypothesis[0][i],
                        y = map.Hypothesis[1][i],
                        direction = map.Hypothesis[2][i];
                    if (!CheckWalls(x, y, direction, map))
                    {
                        map.Hypothesis[0].RemoveAt(i);
                        map.Hypothesis[1].RemoveAt(i);
                        map.Hypothesis[2].RemoveAt(i);
                        i--;
                    }
                }
            }

            private bool CheckWalls(int x, int y, int direction, Map map)
            {
                if (direction == 1)
                {
                    if (map._map[x, y, 1] != map.Sensors[2]) return false;
                    if (map._map[x, y, 2] != map.Sensors[3]) return false;
                    if (map._map[x, y, 3] != map.Sensors[0]) return false;
                    if (map._map[x, y, 4] != map.Sensors[1]) return false;
                    return true;
                }
                else if (direction == 2)
                {
                    if (map._map[x, y, 1] != map.Sensors[1]) return false;
                    if (map._map[x, y, 2] != map.Sensors[2]) return false;
                    if (map._map[x, y, 3] != map.Sensors[3]) return false;
                    if (map._map[x, y, 4] != map.Sensors[0]) return false;
                    return true;
                }
                else if (direction == 3)
                {
                    if (map._map[x, y, 1] != map.Sensors[0]) return false;
                    if (map._map[x, y, 2] != map.Sensors[1]) return false;
                    if (map._map[x, y, 3] != map.Sensors[2]) return false;
                    if (map._map[x, y, 4] != map.Sensors[3]) return false;
                    return true;
                }
                else
                {
                    if (map._map[x, y, 1] != map.Sensors[3]) return false;
                    if (map._map[x, y, 2] != map.Sensors[0]) return false;
                    if (map._map[x, y, 3] != map.Sensors[1]) return false;
                    if (map._map[x, y, 4] != map.Sensors[2]) return false;
                    return true;
                }
            }


            public int NextDirection(int[] sensors)
            {
                for (var direction = 3; direction >= 0; direction--)
                {
                    if (sensors[direction] == 0) return direction + 1;
                }
                return 1; //Hypothesis3 все равно убьет все гипотезы
            }

            /*
            public void Init()
            {
                CurrentWay.Clear();
                CurrentWay = new List<int>();
            }
            */
        }
    }
}