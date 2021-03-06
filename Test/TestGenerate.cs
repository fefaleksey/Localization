﻿using System;
using Xunit;
using Localization;

namespace Test
{
	public class TestGenerateForQualitySensors4
	{
		[Fact]
		public void TestGenerateHashtable()
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
			directions=generate.GenerateHashtable(finalWays);
			Assert.True(directions[36896, 0] == 3);
			finalWays.PrintResult();
		}
	}
}