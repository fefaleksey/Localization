using System;
using Xunit;
using Localization;

namespace Test
{
	public class TestGenerate
	{
		[Fact]
		public void TestGenerateHashtable()
		{
			var mapp = new Map();
			mapp.StartInit();

			var handingOfAllCases = new HandingOfAllCases();
			var way = new Way();
			handingOfAllCases.Handing(mapp, way);
			mapp.ListFiltration(ref mapp.BestWays);
			var finalWays = new FinalWays();
			var solutionForRobot = new SolutionForRobot();
			solutionForRobot.SimulationOfLocalization(ref mapp, ref mapp.BestWays, ref finalWays);
			var generate = new Generate();
			generate.GenerateHashtable(finalWays);
			var directions = new int[(int) Math.Pow(2, Robot.RobotSensors.QualitySensors), generate.HashtableLength(finalWays)];

			Assert.True(directions[36896, 0] == 3);
			//Assert.True(directions[61440, 0] == 3);
			finalWays.PrintResult();
		}
	}
}