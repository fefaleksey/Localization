using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Localization
{
	public class SolutionForRobot
	{
		public void SimulationOfLocalization(ref HandlingHypotheses handlingHypotheses, ref List<List<int>> bestWays, ref FinalWays finalWays)
		{
			var timeOfWay = new TimeOfWay();
			timeOfWay.GetTime(ref bestWays, true);
			var robot = new Robot();
			var localization = false;
			var motion = new Motion();
			handlingHypotheses.HypothesisInit();
			var hypothesisCopy = new List<List<int>>();
			handlingHypotheses.Copy_Lists(ref hypothesisCopy, handlingHypotheses.Hypothesis);
			List<List<int>> bestWaysCopy = new List<List<int>>();
			CopyLists(ref bestWaysCopy, bestWays);
			var quantitybags = 0;

			for (var i = 0; i < hypothesisCopy[0].Count; i++)
			{
				var time = 0;
				var step = 0;
				handlingHypotheses.HypothesisInit();
				var beginWay = true;
				var x = hypothesisCopy[0][i];
				var y = hypothesisCopy[1][i];
				var direction = hypothesisCopy[2][i];
				finalWays.Ways.Add(new List<int>());
				finalWays.Ways[i].Add(x);
				finalWays.Ways[i].Add(y);
				finalWays.Ways[i].Add(direction);
				robot.InitialDirection = 3;
				robot.RSensors.Read(x, y, direction, robot, handlingHypotheses);
				handlingHypotheses.HypothesisFilter(robot);
				if (handlingHypotheses.Hypothesis[0].Count == 1)
				{
					finalWays.Ways[i].Add(8888888);
					finalWays.Ways[i].Add(handlingHypotheses.Hypothesis[0][0]);
					finalWays.Ways[i].Add(handlingHypotheses.Hypothesis[1][0]);
					if (robot.InitialDirection == 1)
						finalWays.Ways[i].Add(OppositeDirection(handlingHypotheses.Hypothesis[2][0]));
					else
						finalWays.Ways[i].Add(handlingHypotheses.Hypothesis[2][0]);
					localization = true;
				}
				while (!localization)
				{
					int directionForGetDirection;

					if (beginWay || robot.InitialDirection != 1)
					{
						directionForGetDirection = 3;
					}
					else
					{
						directionForGetDirection = 1;
					}
					var newDir = GetDirection(ref handlingHypotheses, ref bestWays, directionForGetDirection, beginWay, robot);
					var directionOfTheNextStep = newDir;
					finalWays.Ways[i].Add(newDir);
					newDir = motion.GetNewDir(direction, newDir, beginWay);
					direction = newDir;
					if (step == 0)
					{
						robot.InitialDirection = directionOfTheNextStep;
					}
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
							Console.WriteLine("SolutionForRobot.SimulationOfLocalization - BAG 1111111111111111");
							break;
						}
					}
					handlingHypotheses.Hypothesis3(directionOfTheNextStep, beginWay, motion, robot);

					if (handlingHypotheses.Hypothesis[0].Count == 1)
					{
						finalWays.Ways[i].Add(8888888);
						finalWays.Ways[i].Add(handlingHypotheses.Hypothesis[0][0]);
						finalWays.Ways[i].Add(handlingHypotheses.Hypothesis[1][0]);
						if (robot.InitialDirection == 1)
							finalWays.Ways[i].Add(OppositeDirection(handlingHypotheses.Hypothesis[2][0]));
						else
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
					if (directionOfTheNextStep == 1 || directionOfTheNextStep == 3)
						time++;
					else
						time += 2;
					beginWay = false;
					step++;
					if (Impasse(robot))
					{
						step = 0;
						beginWay = true;
						if (robot.InitialDirection == 1)
						{
							CorrectDirectionsInHypothesis(robot, ref handlingHypotheses);
							robot.InitialDirection = 3;
							direction = OppositeDirection(direction);
						}
					}
				}
				localization = false;
				finalWays.Ways[i].Add(8888888);
				finalWays.Ways[i].Add(time);
				CopyLists(ref bestWays, bestWaysCopy);
			}


			PrintResult(finalWays);
			finalWays.SetFinalList();
			//PrintReleaseResult(finalWays);
			Console.WriteLine(quantitybags);
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

		private void PrintReleaseResult(FinalWays finalWays)
		{
			for (var i = 0; i < finalWays.Ways.Count; i++)
			{
				//Console.Write(i+". ");
				var j = finalWays.Ways[i].Count - 1;
				Console.Write(finalWays.Ways[i][0] + " " + finalWays.Ways[i][1] + " " +
				              finalWays.Ways[i][2] + " " + finalWays.Ways[i][j]);
				Console.WriteLine();
			}
		}

		public bool Impasse(Robot robot)
		{
			var numbersOfWalls = 0;
			for (var i = 0; i < 4; i++)
			{
				if (robot.Sensors[0, i] == 1)
					numbersOfWalls++;
			}
			if (numbersOfWalls == 3)
				return true;
			return false;
		}

		public void CopyLists(ref List<List<int>> to, List<List<int>> from)
		{
			to.Clear();
			for (var i = 0; i < from.Count; i++)
			{
				to.Add(new List<int>());
				for (var j = 0; j < from[i].Count; ++j)
				{
					to[i].Add(from[i][j]);
				}
			}
		}

		private int GetDirection(ref HandlingHypotheses handlingHypotheses, ref List<List<int>> ways, int currentDirection, bool beginWay,
			Robot robot)
		{
			var timeOfWay = new TimeOfWay();
			timeOfWay.GetTime(ref ways);
			var direcrion = ChooseDirection(ways, handlingHypotheses, currentDirection, beginWay, robot);
			if (robot.InitialDirection == 1)
				if (direcrion == 2 || direcrion == 4)
					return OppositeDirection(direcrion);
			return direcrion;
		}

		public void CorrectDirectionsInHypothesis(Robot robot, ref HandlingHypotheses handlingHypotheses)
		{
			for (var i = 0; i < handlingHypotheses.Hypothesis[0].Count; i++)
			{
				if (robot.InitialDirection == 1)
				{
					handlingHypotheses.Hypothesis[2][i] = OppositeDirection(handlingHypotheses.Hypothesis[2][i]);
				}
			}
		}

		private int ChooseDirection(List<List<int>> ways, HandlingHypotheses handlingHypotheses, int currentDirection, bool beginWay, Robot robot)
		{
			var sumTimeForDirecrion = new double[4];
			var quantity = new double[4];
			for (var i = 0; i < handlingHypotheses.Hypothesis[0].Count; i++)
			{
				int x = handlingHypotheses.Hypothesis[0][i],
					y = handlingHypotheses.Hypothesis[1][i],
					direction = handlingHypotheses.Hypothesis[2][i];
				if (robot.InitialDirection == 1)
				{
					direction = OppositeDirection(direction);
				}
				for (var j = 0; j < ways.Count; j++)
				{
					if (x == ways[j][0] && y == ways[j][1] && direction == ways[j][2])
					{
						var index = ways[j][4];
						sumTimeForDirecrion[index - 1] += ways[j][3];
						quantity[index - 1]++;
					}
				}
			}
			for (var i = 0; i < 4; i++)
			{
				if (quantity[i] != 0)
				{
					sumTimeForDirecrion[i] /= quantity[i];
				}
				else
				{
					sumTimeForDirecrion[i] = 1000000000;
				}
			}
			return ChooseMin(sumTimeForDirecrion, currentDirection, beginWay);
		}

		private int ChooseMin(double[] sumTimeForDirecrion, int currentDirection, bool beginWay)
		{
			var oppositeDirection = OppositeDirection(currentDirection);
			var indices = new int[4] {1, 2, 3, 4};

			for (var i = 0; i < 4; i++)
			{
				for (var j = i + 1; j < 4; j++)
				{
					if (sumTimeForDirecrion[i] > sumTimeForDirecrion[j])
					{
						var value1 = sumTimeForDirecrion[i];
						sumTimeForDirecrion[i] = sumTimeForDirecrion[j];
						sumTimeForDirecrion[j] = value1;
						var value2 = indices[i];
						indices[i] = indices[j];
						indices[j] = value2;
					}
				}
			}

			if (beginWay) return indices[0];
			if (oppositeDirection == indices[0]) return indices[1];
			return indices[0];
		}

		public int OppositeDirection(int direction)
		{
			if (direction > 2) return direction - 2;
			return direction + 2;
		}
	}
}