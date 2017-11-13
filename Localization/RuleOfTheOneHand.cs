using System.Collections.Generic;
using System;

namespace Localization
{
	public class RuleOfTheOneHand
	{
		private const int Down = 1;
		private const int Left = 2;
		private const int Up = 3;
		private const int Right = 4;

		public void SimulationOfLocalization(ref HandlingHypotheses handlingHypotheses, ref FinalWays finalWays, bool ruleRightHand)
		{
			finalWays.Ways.Clear();
			finalWays.Ways = new List<List<int>>();
			var robot = new Robot();
			var localization = false;
			var motion = new Motion();
			handlingHypotheses.HypothesisInit();
			var hypothesisCopy = new List<List<int>>();
			handlingHypotheses.Copy_Lists(ref hypothesisCopy, handlingHypotheses.Hypothesis);
			robot.InitialDirection = 3;
			var quantitybags = 0;
			for (var i = 0; i < hypothesisCopy[0].Count; i++)
			{
				var time = 0;
				handlingHypotheses.HypothesisInit();
				var x = hypothesisCopy[0][i];
				var y = hypothesisCopy[1][i];
				var direction = hypothesisCopy[2][i];
				finalWays.Ways.Add(new List<int>());
				finalWays.Ways[i].Add(x);
				finalWays.Ways[i].Add(y);
				finalWays.Ways[i].Add(direction);
				robot.RSensors.Read(x, y, direction, robot, handlingHypotheses);
				handlingHypotheses.HypothesisFilter(robot);
				if (handlingHypotheses.Hypothesis[0].Count == 1)
				{
					finalWays.Ways[i].Add(8888888);
					finalWays.Ways[i].Add(handlingHypotheses.Hypothesis[0][0]);
					finalWays.Ways[i].Add(handlingHypotheses.Hypothesis[1][0]);
					finalWays.Ways[i].Add(handlingHypotheses.Hypothesis[2][0]);
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
						case HandlingHypotheses.Down:
						{
							if (x + 1 < HandlingHypotheses.Height && handlingHypotheses.Map[x, y, HandlingHypotheses.Down] == 0)
							{
								++x;
								robot.RSensors.Read(x, y, HandlingHypotheses.Down, robot, handlingHypotheses);
							}
							break;
						}
						case HandlingHypotheses.Left:
						{
							if (y > 0 && handlingHypotheses.Map[x, y, HandlingHypotheses.Left] == 0)
							{
								--y;
								robot.RSensors.Read(x, y, HandlingHypotheses.Left, robot, handlingHypotheses);
							}
							break;
						}
						case HandlingHypotheses.Up:
						{
							if (x > 0 && handlingHypotheses.Map[x, y, HandlingHypotheses.Up] == 0)
							{
								--x;
								robot.RSensors.Read(x, y, HandlingHypotheses.Up, robot, handlingHypotheses);
							}
							break;
						}
						case HandlingHypotheses.Right:
						{
							if (y + 1 < HandlingHypotheses.Width && handlingHypotheses.Map[x, y, HandlingHypotheses.Right] == 0)
							{
								++y;
								robot.RSensors.Read(x, y, HandlingHypotheses.Right, robot, handlingHypotheses);
							}
							break;
						}
						default:
						{
							Console.WriteLine("SolutionForRobot.SimulationOfLocalization - BAG 1");
							break;
						}
					}
					handlingHypotheses.Hypothesis3(directionOfTheNextStep, true, motion, robot);

					if (handlingHypotheses.Hypothesis[0].Count == 1)
					{
						finalWays.Ways[i].Add(8888888);
						finalWays.Ways[i].Add(handlingHypotheses.Hypothesis[0][0]);
						finalWays.Ways[i].Add(handlingHypotheses.Hypothesis[1][0]);
						finalWays.Ways[i].Add(handlingHypotheses.Hypothesis[2][0]);
						localization = true;
					}
					if (handlingHypotheses.Hypothesis[0].Count == 0)
					{
						quantitybags++;
						time = -1;
						finalWays.Ways[i].RemoveAt(finalWays.Ways[i].Count - 1);
						break;
					}
				}
				localization = false;
				finalWays.Ways[i].Add(8888888);
				finalWays.Ways[i].Add(time);
			}
			//PrintResult(finalWays);
			//Console.WriteLine(quantitybags);
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
				if (robot.Sensors[0, direction] == 0) return direction + 1;
			}
			return 1; //Hypothesis3 все равно убьет все гипотезы
		}

		public int NextDirectionL(Robot robot)
		{
			if (robot.Sensors[0, 1] == 0) return Left;
			if (robot.Sensors[0, 2] == 0) return Up;
			if (robot.Sensors[0, 3] == 0) return Right;
			return Down;
			//Hypothesis3 все равно убьет все гипотезы
		}
	}
}