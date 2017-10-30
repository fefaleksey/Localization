using System;
using System.Collections.Generic;

namespace Localization
{
	class HandingOfAllCases
	{
		private bool _wayExist = true, _localiz = false;

		private void CopyWay(List<int> from, ref List<int> to)
		{
			to.Clear();
			for (var i = 0; i < from.Count; ++i)
			{
				to.Add(from[i]);
			}
		}

		//TODO: доработать HypothesisFilter
		public void Handing(Map map, Way Way)
		{
			//Console.WriteLine("Handing");
			var copyHypothesis = new List<List<int>>();
			map.HypothesisInit();
			var way = new List<int>();

			for (var i = 0; i < map.Hypothesis.Count; i++)
			{
				copyHypothesis.Add(new List<int>());
			}
			var fl = 1;
			int test = 0;
			map.Copy_Lists(ref copyHypothesis, map.Hypothesis);
			var n = map.Hypothesis[0].Count;
			/*
			Map.hypothesis[0][0] = 5;
			Map.hypothesis[1][0] = 5;
			Map.hypothesis[2][0] = 1;
			*/
			var motion = new Motion();
			var robot = new Robot();
			for (var i = 0; i < n; ++i)
			{
				Way.CurentWayInit();
				while (fl != 0)
				{
					++test;
					CopyWay(Way.CurentWay, ref way);
					robot.InitialDirection = 3; //TODO: проверить!
					robot.RSensors.Read(map.Hypothesis[0][i], map.Hypothesis[1][i], map.Hypothesis[2][i], robot, map);
					HandingHypothesis(i, way, map, motion, Way, robot);
					fl = Way.NextDirection(_wayExist, _localiz, robot);
					map.Hypothesis[0].Clear();
					map.Hypothesis[1].Clear();
					map.Hypothesis[2].Clear();
					map.Copy_Lists(ref map.Hypothesis, copyHypothesis);

					//////////////////////////////
					/*
					Map.hypothesis[0][0] = 5;
					Map.hypothesis[1][0] = 5;
					Map.hypothesis[2][0] = 1;
					*/ //////////////////////////////
					_wayExist = true;
					_localiz = false;
				}
				//Console.Write(i);
				fl = 1;
			}
		}

		//Добавить в начало вызов Hypothesis1
		//number == number of hypothesis
		private void HandingHypothesis(int number, List<int> way,
			Map map, Motion motion, Way Way, Robot robot)
		{
			int x = map.Hypothesis[0][number],
				y = map.Hypothesis[1][number],
				direction = map.Hypothesis[2][number],
				xStart = x,
				yStart = y;
			var newDir = map.Hypothesis[2][number];
			Way.BeginWay = true;
			//Map.Hypothesis1New();
			map.HypothesisFilter(robot); //TODO: Косячит. Исправить HypothesisFilter!!!
			for (int k = 0; k < way.Count; k++)
			{
				bool fl = true;
				if (k == 1)
				{
					Way.BeginWay = false;
					//robot.InitialDirection = newDir;
				}
				//int a = hypothesis[2][i], b = ways[j][k];
				newDir = motion.GetNewDir(newDir, way[k], Way.BeginWay); //Map.ChooseDir(newDir, way[k]);
				switch (newDir)
				{
					case Map.Down:
					{
						if (x + 1 < map.Height && map.map[x, y, Map.Down] == 0)
							// && CheckWalls(x, y + 1, Down))
						{
							fl = false;
							++x;
							robot.RSensors.Read(x, y, Map.Down, robot, map);
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
							robot.RSensors.Read(x, y, Map.Left, robot, map);
						}
						break;
					}
					case Map.Up:
					{
						if (x > 0 && map.map[x, y, Map.Up] == 0) //&& CheckWalls(x - 1, y , Up))
						{
							fl = false;
							--x;
							robot.RSensors.Read(x, y, Map.Up, robot, map);
						}
						break;
					}
					case Map.Right:
					{
						if (y + 1 < map.Widht && map.map[x, y, Map.Right] == 0) // && CheckWalls(x, y + 1, Right))
						{
							fl = false;
							++y;
							robot.RSensors.Read(x, y, Map.Right, robot, map);
						}
						break;
					}
				}
				if (fl)
				{
					map.Sensors[0] = -1;
					map.Sensors[1] = -1;
					map.Sensors[2] = -1;
					map.Sensors[3] = -1;
					_wayExist = false;
					return; //-1
				}

				map.Hypothesis3(way[k], Way.BeginWay, motion, robot);
				/*
				for (int i = 0; i < way.Count; i++)
				{
					Console.Write(way[i]);
				}
				Console.WriteLine();
				*/
				if (map.Hypothesis[0].Count == 1)
				{
					_localiz = true;
					var n = map.BestWays.Count;
					map.BestWays.Add(new List<int>());
					map.BestWays[n].Add(xStart);
					map.BestWays[n].Add(yStart);
					map.BestWays[n].Add(direction);
					for (var l = 0; l <= k; l++)
					{
						map.BestWays[n].Add(way[l]);
						//	Console.Write(best_ways[n][l] + ",");
					}
					//Console.WriteLine(" quantity of steps: " + k);
					return; // k;
				}

				if (map.Hypothesis[0].Count == 0)
				{
					_wayExist = false;
					return;
				}
			}
			//Console.WriteLine("HC: " + hypothesis[0].Count);
			//return Map.hypothesis[0].Count;
		}
	}
}