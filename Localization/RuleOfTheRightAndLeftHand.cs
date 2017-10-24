using System.Collections.Generic;
using System;

namespace Localization
{
    class RuleOfTheRightAndLeftHand
    {
        
        private const int Down = 1;
        private const int Left = 2;
        private const int Up = 3;
        private const int Right = 4;
        
        
        //private List<int> CurrentWay = new List<int>();
        
        public void SimulationOfLocalization(ref Map map, ref FinalWays finalWays, bool ruleRightHand)
        {
            finalWays.Ways.Clear();
            finalWays.Ways=new List<List<int>>();
            var robot = new Robot();
            var localization = false;
            var motion = new Motion();
            map.HypothesisInit();
            var hypothesisCopy = new List<List<int>>();
            map.Copy_Lists(ref hypothesisCopy, map.Hypothesis);
            robot.InitialDirection = 3;
            var quantitybags = 0;
            for (var i = 0; i < hypothesisCopy[0].Count; i++)
            {
                var time = 0;
                map.HypothesisInit();
                var x = hypothesisCopy[0][i];
                var y = hypothesisCopy[1][i];
                var direction = hypothesisCopy[2][i];
                finalWays.Ways.Add(new List<int>());
                finalWays.Ways[i].Add(x);
                finalWays.Ways[i].Add(y);
                finalWays.Ways[i].Add(direction);
                robot.RSensors.Read(x, y, direction, robot, map);
                map.HypothesisFilter(robot);
                if (map.Hypothesis[0].Count == 1)
                {
                    finalWays.Ways[i].Add(8888888);
                    finalWays.Ways[i].Add(map.Hypothesis[0][0]);
                    finalWays.Ways[i].Add(map.Hypothesis[1][0]);
                    finalWays.Ways[i].Add(map.Hypothesis[2][0]);
                    localization = true;
                }
                while (!localization)
                {
                    var newDir = NextDirection(robot, ruleRightHand);
                    var directionOfTheNextStep = newDir;
                    finalWays.Ways[i].Add(newDir);
                    if (newDir == 3)
                        time++;
                    else if (newDir == 2 || newDir == 4)
                        time += 2;
                    else
                        time += 3;
                    
                    newDir = motion.GetNewDir(direction, newDir, true);
                    direction = newDir;
                    switch (newDir)
                    {
                        case Map.Down:
                        {
                            if (x + 1 < map.Height && map.map[x, y, Map.Down] == 0)
                            {
                                ++x;
                                robot.RSensors.Read(x, y, Map.Down, robot, map);
                                //time ++;
                            }
                            break;
                        }
                        case Map.Left:
                        {
                            if (y > 0 && map.map[x, y, Map.Left] == 0)
                            {
                                --y;
                                robot.RSensors.Read(x, y, Map.Left, robot, map);
                                //time += 2;
                            }
                            break;
                        }
                        case Map.Up:
                        {
                            if (x > 0 && map.map[x, y, Map.Up] == 0)
                            {
                                --x;
                                robot.RSensors.Read(x, y, Map.Up, robot, map);
                                //time++;
                            }
                            break;
                        }
                        case Map.Right:
                        {
                            if (y + 1 < map.Widht && map.map[x, y, Map.Right] == 0)
                            {
                                ++y;
                                robot.RSensors.Read(x, y, Map.Right, robot, map);
                                //time += 2;
                            }
                            break;
                        }
                        default:
                        {
                            Console.WriteLine("SolutionForRobot.SimulationOfLocalization - BAG 1");
                            break;
                        }
                    }
                    map.Hypothesis3(directionOfTheNextStep, true, motion, robot);

                    if (map.Hypothesis[0].Count == 1)
                    {
                        finalWays.Ways[i].Add(8888888);
                        finalWays.Ways[i].Add(map.Hypothesis[0][0]);
                        finalWays.Ways[i].Add(map.Hypothesis[1][0]);
                        finalWays.Ways[i].Add(map.Hypothesis[2][0]);
                        localization = true;
                    }
                    if (map.Hypothesis[0].Count == 0)
                    {
                        quantitybags++;
                        time = 0;
                        break;
                    }
                }
                localization = false;
                finalWays.Ways[i].Add(8888888);
                finalWays.Ways[i].Add(time);
            }
            //PrintResult(finalWays);
            Console.WriteLine(quantitybags);
            finalWays.SetFinalList();
            //if(!ruleRightHand) finalWays.PrintResult();
        }

        private void PrintResult(FinalWays finalWays)
        {
            for (var i = 0; i < finalWays.Ways.Count; i++)
            {
                Console.Write(i + ". ");
                for (var j = 0; j < finalWays.Ways[i].Count; j++)
                {
                    Console.Write(finalWays.Ways[i][j] + " ");
                }
                Console.WriteLine();
            }
        }
    
        private bool CheckWalls(int x, int y, int direction, Map map)
        {
            if (direction == 1)
            {
                if (map.map[x, y, 1] != map.Sensors[2]) return false;
                if (map.map[x, y, 2] != map.Sensors[3]) return false;
                if (map.map[x, y, 3] != map.Sensors[0]) return false;
                if (map.map[x, y, 4] != map.Sensors[1]) return false;
                return true;
            }
            else if (direction == 2)
            {
                if (map.map[x, y, 1] != map.Sensors[1]) return false;
                if (map.map[x, y, 2] != map.Sensors[2]) return false;
                if (map.map[x, y, 3] != map.Sensors[3]) return false;
                if (map.map[x, y, 4] != map.Sensors[0]) return false;
                return true;
            }
            else if (direction == 3)
            {
                if (map.map[x, y, 1] != map.Sensors[0]) return false;
                if (map.map[x, y, 2] != map.Sensors[1]) return false;
                if (map.map[x, y, 3] != map.Sensors[2]) return false;
                if (map.map[x, y, 4] != map.Sensors[3]) return false;
                return true;
            }
            else
            {
                if (map.map[x, y, 1] != map.Sensors[3]) return false;
                if (map.map[x, y, 2] != map.Sensors[0]) return false;
                if (map.map[x, y, 3] != map.Sensors[1]) return false;
                if (map.map[x, y, 4] != map.Sensors[2]) return false;
                return true;
            }
        }


        public int NextDirection(Robot robot, bool ruleOfTheRightHand)
        {
            if (ruleOfTheRightHand)
                return NextDirectionR(robot);
            else
                return NextDirectionL(robot);
        }
        
        public int NextDirectionR(Robot robot)
        {
            for (var direction = 3; direction >= 0; direction--)
            {
                if (robot.Sensors[0,direction] == 0) return direction + 1;
            }
            return 1; //Hypothesis3 все равно убьет все гипотезы
        }
        public int NextDirectionL(Robot robot)
        {
            if (robot.Sensors[0,1] == 0) return Left;
            if (robot.Sensors[0,2] == 0) return Up;
            if (robot.Sensors[0,3] == 0) return Right;
            return Down;
            //Hypothesis3 все равно убьет все гипотезы
        }
    }
}