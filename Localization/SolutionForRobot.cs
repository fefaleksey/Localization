﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Localization
{
	public class SolutionForRobot
	{
		public void SimulationOfLocalization(ref Map map, ref List<List<int>> bestWays, ref FinalWays finalWays)
		{
			var timeOfWay = new TimeOfWay();
			timeOfWay.GetTime(ref bestWays, true);
			var robot = new Robot();
			var localization = false;
			var motion = new Motion();
			map.HypothesisInit();
			var hypothesisCopy = new List<List<int>>();
			map.Copy_Lists(ref hypothesisCopy, map.Hypothesis);
			List<List<int>> bestWaysCopy = new List<List<int>>();
			CopyLists(ref bestWaysCopy, bestWays);
			var quantitybags = 0;

			for (var i = 0; i < hypothesisCopy[0].Count; i++)
			{
				var time = 0;
				var step = 0;
				map.HypothesisInit();
				var beginWay = true;
				var x = hypothesisCopy[0][i];
				var y = hypothesisCopy[1][i];
				var direction = hypothesisCopy[2][i];
				finalWays.Ways.Add(new List<int>());
				finalWays.Ways[i].Add(x);
				finalWays.Ways[i].Add(y);
				finalWays.Ways[i].Add(direction);
				robot.InitialDirection = 3;
				robot.RSensors.Read(x, y, direction, robot, map);
				map.HypothesisFilter(robot);
				if (map.Hypothesis[0].Count == 1)
				{
					finalWays.Ways[i].Add(8888888);
					finalWays.Ways[i].Add(map.Hypothesis[0][0]);
					finalWays.Ways[i].Add(map.Hypothesis[1][0]);
					if (robot.InitialDirection == 1)
						finalWays.Ways[i].Add(OppositeDirection(map.Hypothesis[2][0]));
					else
						finalWays.Ways[i].Add(map.Hypothesis[2][0]);
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
					var newDir = GetDirection(ref map, ref bestWays, directionForGetDirection, beginWay, robot);
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
						case Map.Down:
						{
							if (x + 1 < Map.Height && map.map[x, y, Map.Down] == 0)
							{
								++x;
								robot.RSensors.Read(x, y, Map.Down, robot, map);
							}
							break;
						}
						case Map.Left:
						{
							if (y > 0 && map.map[x, y, Map.Left] == 0)
							{
								--y;
								robot.RSensors.Read(x, y, Map.Left, robot, map);
							}
							break;
						}
						case Map.Up:
						{
							if (x > 0 && map.map[x, y, Map.Up] == 0)
							{
								--x;
								robot.RSensors.Read(x, y, Map.Up, robot, map);
							}
							break;
						}
						case Map.Right:
						{
							if (y + 1 < Map.Width && map.map[x, y, Map.Right] == 0)
							{
								++y;
								robot.RSensors.Read(x, y, Map.Right, robot, map);
							}
							break;
						}
						default:
						{
							Console.WriteLine("SolutionForRobot.SimulationOfLocalization - BAG 1111111111111111");
							break;
						}
					}
					map.Hypothesis3(directionOfTheNextStep, beginWay, motion, robot);

					if (map.Hypothesis[0].Count == 1)
					{
						finalWays.Ways[i].Add(8888888);
						finalWays.Ways[i].Add(map.Hypothesis[0][0]);
						finalWays.Ways[i].Add(map.Hypothesis[1][0]);
						if (robot.InitialDirection == 1)
							finalWays.Ways[i].Add(OppositeDirection(map.Hypothesis[2][0]));
						else
							finalWays.Ways[i].Add(map.Hypothesis[2][0]);
						localization = true;
					}
					if (map.Hypothesis[0].Count == 0)
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
							CorrectDirectionsInHypothesis(robot, ref map);
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

		private int GetDirection(ref Map map, ref List<List<int>> ways, int currentDirection, bool beginWay,
			Robot robot)
		{
			var timeOfWay = new TimeOfWay();
			timeOfWay.GetTime(ref ways);
			var direcrion = ChooseDirection(ways, map, currentDirection, beginWay, robot);
			if (robot.InitialDirection == 1)
				if (direcrion == 2 || direcrion == 4)
					return OppositeDirection(direcrion);
			return direcrion;
		}

		public void CorrectDirectionsInHypothesis(Robot robot, ref Map map)
		{
			for (var i = 0; i < map.Hypothesis[0].Count; i++)
			{
				if (robot.InitialDirection == 1)
				{
					map.Hypothesis[2][i] = OppositeDirection(map.Hypothesis[2][i]);
				}
			}
		}

		private int ChooseDirection(List<List<int>> ways, Map map, int currentDirection, bool beginWay, Robot robot)
		{
			var sumTimeForDirecrion = new double[4];
			var quantity = new double[4];
			for (var i = 0; i < map.Hypothesis[0].Count; i++)
			{
				int x = map.Hypothesis[0][i],
					y = map.Hypothesis[1][i],
					direction = map.Hypothesis[2][i];
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