using System;
// TODO: пролог, мат логика
//http://fruct.org/node/366613
namespace Localization
{
	public class Generate
	{
		//TODO: это фигня, исправить, теряется очень много случаев
		private int[,] _coordinats = new int[(int) Math.Pow(2, 4 * Robot.RobotSensors.QualitySensors), 3];
		public int HashtableLength(FinalWays finalWays)
		{
			var max = 0;
			var counter = 0;
			for (var i = 0; i < finalWays.Ways.Count; i++)
			{
				while (finalWays.Ways[i][counter] != 8888888)
				{
					counter++;
				}
				if (max < counter) max = counter;
				counter = 0;
			}
			return max-3;
		}

		public int[,] GenerateHashtable(FinalWays finalWays)
		{
			var directions = new int[(int) Math.Pow(2,
					4 * Robot.RobotSensors.QualitySensors /*Math.Pow(2, Robot.RobotSensors.QualitySensors)*/),
				HashtableLength(finalWays)];
			var robot = new Robot();
			var motion = new Motion();
			var map = new HandlingHypotheses();
			var solutionForRobot = new SolutionForRobot();
			for (var i = 0; i < finalWays.Ways.Count; i++)
			{
				var x = finalWays.Ways[i][0];
				var y = finalWays.Ways[i][1];
				var initialdirection = finalWays.Ways[i][3];
				var newDir = finalWays.Ways[i][2];
				robot.InitialDirection = 3;
				robot.RSensors.Read(x, y, newDir /*direction*/, robot, map);
				if (initialdirection == 8888888) // Записать координаты!
				{
					ToAddCoordinats(robot, x, y, newDir);
					continue;
				}
				var step = 0;
				var beginWay = true;
				robot.InitialDirection = initialdirection;
				for (var j = 3; j < finalWays.Ways[i].Count && finalWays.Ways[i][j] != 8888888; j++)
				{
					var value = robot.RSensors.GetSensorsValue(robot);
					directions[value, step] = finalWays.Ways[i][j];
					newDir = motion.GetNewDir(newDir, finalWays.Ways[i][j], beginWay);
					switch (newDir)
					{
						case HandlingHypotheses.Down:
						{
							++x;
							break;
						}
						case HandlingHypotheses.Left:
						{
							--y;
							break;
						}
						case HandlingHypotheses.Up:
						{
							--x;
							break;
						}
						case HandlingHypotheses.Right:
						{
							++y;
							break;
						}
					}
					step++;
					robot.RSensors.Read(x, y, newDir, robot, map);
					beginWay = false;
					if (solutionForRobot.Impasse(robot))
					{
						beginWay = true;
						if (robot.InitialDirection == 1)
						{
							robot.InitialDirection = 3;
							newDir = solutionForRobot.OppositeDirection(newDir);
						}
						else
						{
							robot.InitialDirection = 1;
						}
					}
				}
				ToAddCoordinats(robot, x, y, newDir);
			}
			//PrintResult(directions);
			PrintReleaseResult(directions);
			return directions;
		}

		private void ToAddCoordinats(Robot robot, int x, int y, int direction)
		{
			var index = robot.RSensors.GetSensorsValue(robot);
			_coordinats[index, 0] = x;
			_coordinats[index, 1] = y;
			_coordinats[index, 2] = direction;
		}

		public void PrintResult(int[,] directions)
		{
			int test = 0, test1 = 0;
			//тут вроде не правильный цикл
			for (var i = 0; i < (int) Math.Pow(2, 4 * Robot.RobotSensors.QualitySensors); i++)
				//Math.Pow(2, Robot.RobotSensors.QualitySensors)); i++)
			{
				for (var j = 0; j < 8; j++)
				{
					test++;
					if (directions[i, j] != 0)
					{
						test1++;
						Console.WriteLine("key = " + i + " step = " + j + " value = " + directions[i, j]);
					}
				}
			}
			Console.WriteLine("test = " + test + " test1 = " + test1);
		}

		private void PrintReleaseResult(int[,] directions)
		{
			Console.WriteLine("var myhashtable = [");
			for (var i = 0; i < directions.GetLength(0); i++)
			{
				Console.Write("[");
				for (var j = 0; j < directions.GetLength(1); j++)
				{
					Console.Write(directions[i,j]+",");
				}
				Console.Write(directions[i,4]+"],");
				Console.WriteLine();
			}
			Console.WriteLine("];");
			
			Console.WriteLine("var coordinats = [");
			for (var i = 0; i < _coordinats.GetLength(0); i++)
			{
				Console.Write("[");
				for (var j = 0; j < _coordinats.GetLength(1)-1; j++)
				{
					Console.Write(_coordinats[i,j]+",");
				}
				Console.Write(_coordinats[i,2] + "],");
				Console.WriteLine();
			}
			/*
			for (var index0 = 0; index0 < directions.GetLength(0); index0++)
			for (var index1 = 0; index1 < directions.GetLength(1); index1++)
			{
				var a = directions[index0, index1];
				a = 2;
			}
			*/
			Console.WriteLine("];");
		}
	}
}