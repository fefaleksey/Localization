using System;
using System.Collections.Generic;

namespace Localization
{
    class Tests
    {
        public List<List<int>> Test = new List<List<int>>();

        public void TimeOfFinalWays(ref FinalWays ways, ref Map map, Robot robot)
        {
            //PathAdjustment(ref ways);
            map.HypothesisInit();
            var hypothesisCopy = new List<List<int>>();
            map.Copy_Lists(ref hypothesisCopy, map.Hypothesis);

            for (var i = 0; i < map.Hypothesis[0].Count; i++)
            {
                Test.Add(new List<int>());
                Test[i].Add(map.Hypothesis[0][i]);
                Test[i].Add(map.Hypothesis[1][i]);
                Test[i].Add(map.Hypothesis[2][i]);
            }


            //map.Copy_Lists(ref Test, map.Hypothesis);
            var sensor = new int[4];

            var motion = new Motion();

            int x, y, direction;
            //int x = hypothesisCopy[0][i], y = hypothesisCopy[1][i], direction = hypothesisCopy[2][i];
            /*
            map.SensorsRead(x, y, direction, robot);
            map.Hypothesis1New();
            */
            for (var i = 0; i < hypothesisCopy[0].Count; i++)
            {
                var beginWay = true;
                int time = 0, step = 0;
                x = hypothesisCopy[0][i];
                y = hypothesisCopy[1][i];
                direction = hypothesisCopy[2][i];
                robot.InitialDirection = 3;
                map.SensorsRead(x, y, direction, robot);
                map.Hypothesis1New();
                var localize = false;
                //Test.Add(new List<int>());
                //TODO: возможно не учел начало пути!!(bool), проверить; не учел Robot.InitialDirection
                while (!localize)
                {
                    var sensorValue = GetValueOfSensor(robot.Sensors);
                    direction = ways.Ways[step][sensorValue];
                    //if (step > 0) hg
                    //GoTo(ref map, i, direction); // TODO: СДЕЛАТЬ!!!
                    if (direction == 1 || direction == 3) time++;
                    else time += 2;
                    //TODO: Сначала нужно считать показания сенсоров, а уже потом идти туда.
                    //TODO: Учесть проблему с поворотами (на листике написано подробно)
                    /*
                    var newDir = motion.GetNewDir(Hypothesis[2][i], direction, beginWay);
                    map.SensorsRead(x, y, direction, robot);
                    */
                    // ref на направление, в котором стоит робот
                    //TODO: учесть это в ReadNextSensors. И вообще дописать ее!!!
                    var currentDirection = hypothesisCopy[2][i];
                    ReadNextSensors(x, y, direction, ref currentDirection, robot, ref map, beginWay); //???
                    map.Hypothesis3(direction, beginWay, motion, robot); //???
                    step++;
                    beginWay = false;
                    Test[i].Add(direction);

                    if (map.Hypothesis[0].Count == 1)
                    {
                        localize = true;
                        Test[i].Add(hypothesisCopy[0][i]);
                        Test[i].Add(hypothesisCopy[1][i]);
                        Test[i].Add(hypothesisCopy[2][i]);

                        Test[i].Add(time);
                    }
                    if (map.Hypothesis[0].Count == 0)
                    {
                        Test[i].Add(-1);
                        Test[i].Add(-1);
                        Test[i].Add(-1);
                        break;
                    }
                    if (step == 1)
                    {
                        robot.InitialDirection = direction;
                        if (direction == 1) PathAdjustment(ref ways);
                    }
                    //beginWay = false;
                }

                map.Hypothesis[0].Clear();
                map.Hypothesis[1].Clear();
                map.Hypothesis[2].Clear();
                map.Copy_Lists(ref map.Hypothesis, hypothesisCopy);
            }
            for (var i = 0; i < Test.Count; i++)
            {
                Console.Write(i + "   ");
                for (var j = 0; j < Test[i].Count; j++)
                {
                    Console.Write(Test[i][j] + " ");
                }
                Console.WriteLine();
            }
        }


        private void ReadNextSensors(int x, int y, int direction, ref int currentDirection,
            Robot robot, ref Map map, bool beginWay)
        {
            var Motion = new Motion();
            //var newDir = currentDirection;
            var fl = true;
            currentDirection = Motion.GetNewDir(currentDirection, direction, beginWay); //Map.ChooseDir(newDir, way[k]);
            switch (currentDirection)
            {
                case Map.Down:
                {
                    if (x + 1 < map.Height && map.map[x, y, Map.Down] == 0)
                        // && CheckWalls(x, y + 1, Down))
                    {
                        fl = false;
                        ++x;
                        map.SensorsRead(x, y, Map.Down, robot);
                    }
                    break;
                }
                case Map.Left:
                {
                    if (y > 0 && map.map[x, y, Map.Left] == 0)
                        //&& CheckWalls(x, y - 1, Map.Left))
                    {
                        fl = false;
                        --y;
                        map.SensorsRead(x, y, Map.Left, robot);
                    }
                    break;
                }
                case Map.Up:
                {
                    if (x > 0 && map.map[x, y, Map.Up] == 0) //&& CheckWalls(x - 1, y , Up))
                    {
                        fl = false;
                        --x;
                        map.SensorsRead(x, y, Map.Up, robot);
                    }
                    break;
                }
                case Map.Right:
                {
                    if (y + 1 < map.Wight && map.map[x, y, Map.Right] == 0) // && 
                        // CheckWalls(x, y + 1, Right))
                    {
                        fl = false;
                        ++y;
                        map.SensorsRead(x, y, Map.Right, robot);
                    }
                    break;
                }
            }
        }

        private void PathAdjustment(ref FinalWays ways)
        {
            for (var i = 0; i < ways.Ways.Count; i++)
            {
                for (var j = 0; j < ways.Ways[i].Count; j++)
                {
                    if (ways.Ways[i][j] == 2)
                    {
                        ways.Ways[i][j] = 4;
                    }
                    else if (ways.Ways[i][j] == 4)
                    {
                        ways.Ways[i][j] = 2;
                    }
                }
            }
        }


        private int GetValueOfSensor(int[] sensor)
        {
            var value = 0;
            for (var i = 0; i < 4; i++)
            {
                value += (int) Math.Pow(2, i) * sensor[i];
            }
            return value;
        }

        private void GoTo(ref Map map, int index, int direction)
        {
            //TODO: получить новое направление и делать switch по нему
            switch (direction)
            {
                case 1: //Down
                {
                    map.Hypothesis[0][index]++;
                    break;
                }
            }
        }

        public void RuleOfTheLeftHand(FinalWays ways, Map map)
        {

        }

        public void RuleOfTheRightHand(FinalWays ways, Map map)
        {

        }
    }
}