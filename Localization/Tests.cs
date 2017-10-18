using System;
using System.Collections.Generic;

namespace Localization
{
    class Tests
    {
        public List<List<int>> Test = new List<List<int>>();
        
        public void TimeOfFinalWays(FinalWays ways,ref Map map, Robot robot)
        {
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
                    direction = ways.directions[step][sensorValue];
                    //GoTo(ref map, i, direction); // TODO: СДЕЛАТЬ!!!
                    if (direction == 1 || direction == 3) time++;
                    else time += 2;
                    map.Hypothesis3(direction, beginWay, motion, robot);
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
                    }
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