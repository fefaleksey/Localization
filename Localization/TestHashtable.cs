using System;

namespace Localization
{
    class TestHashtable
    {
        public void Test()
        {
            var mapp = new HandlingHypotheses();
            mapp.StartInit();

            var handingOfAllCases = new HandlingAllCases();
            var way = new Way();
            handingOfAllCases.Handing(mapp, way);
            mapp.ListFiltration(ref mapp.BestWays);
            var finalWays = new FinalWays();
            var solutionForRobot = new SolutionForRobot();
            solutionForRobot.SimulationOfLocalization(ref mapp, ref mapp.BestWays, ref finalWays);
            var generate = new Generate();
            var directions = new int[(int) Math.Pow(2, Math.Pow(2, Robot.RobotSensors.QualitySensors)),
                generate.HashtableLength(finalWays)];
            //directions=generate.GenerateHashtable(finalWays);
            for (var i = 0; i < 65536; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    directions[i,j] = Convert.ToInt32(Console.Read());
                }
            }
            var ppp = 0;
            while (true)
            {
                ppp = directions[36896, 0];
                ppp = directions[23040, 1];
                ppp = directions[28800, 2];
                ppp = directions[6720, 3];
                ppp = directions[23040, 4];
                ppp = directions[36896, 0];
            }
        }
    }
}