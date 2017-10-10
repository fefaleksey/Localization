using System;
using System.Collections.Generic;

namespace Localization
{
	class HandingOfAllCases
	{
		private static bool wayExist = true, localiz = false;
		
		private static void Copy_way(List<int> from,ref List<int> to)
		{
			to.Clear();
			for (var i = 0; i < from.Count; ++i)
			{
				to.Add(from[i]);
			}
		}
		
		public static void Handing()
		{
			Console.WriteLine("Handing");
			var copy_hypothesis = new List<List<int>>();
			Map.HypothesisInit();
			var way = new List<int>();
				
			for (var i = 0; i < Map.hypothesis.Count; i++)
			{
				copy_hypothesis.Add(new List<int>());
			}
			var fl = 1;
			int test = 0;
			Map.Copy_Lists(ref copy_hypothesis, Map.hypothesis);
			var n = Map.hypothesis[0].Count;
			for (var i = 0; i < n; ++i)
			{
				Way.CurentWayInit();
				while (fl != 0)
				{
					++test;
					Copy_way(Way.curent_way, ref way);
					Map.SensorsRead(Map.hypothesis[0][i], Map.hypothesis[1][i],
									Map.hypothesis[2][i]); //!!
					HandingHypothesis(i, way);
					fl = Way.NextDirection(wayExist, localiz);
					Map.hypothesis[0].Clear();
					Map.hypothesis[1].Clear();
					Map.hypothesis[2].Clear();
					Map.Copy_Lists(ref Map.hypothesis, copy_hypothesis);
					wayExist = true;
					localiz = false;
				}
				Console.Write(i);
				fl = 1;
			}
		}

		//Добавить в начало вызов Hypothesis1
		//number == number of hypothesis
		private static void HandingHypothesis(int number, List<int> way)
		{
			int x = Map.hypothesis[0][number],
				y = Map.hypothesis[1][number],
				direction = Map.hypothesis[2][number],
				x_start = x,
				y_start = y;
			var newDir = Map.hypothesis[2][number];
			//Map.Hypothesis1New();
			for (int k = 0; k < way.Count; k++)
			{
				bool fl = true;
				//int a = hypothesis[2][i], b = ways[j][k];
				newDir = Motion.GetNewDir(newDir, way[k]);//Map.ChooseDir(newDir, way[k]);
				switch (newDir)
				{
					case Map.Down:
					{
						if (x + 1 < Map.height && Map._map[x, y, Map.Down] == 0) 
							// && CheckWalls(x, y + 1, Down))
						{
							fl = false;
							++x;
							Map.SensorsRead(x, y, Map.Down);
						}
						break;
					}
					case Map.Left:
					{
						if (y > 0 && Map._map[x, y, Map.Left] == 0)
							//&& CheckWalls(x, y - 1, Map.Left))
						{
							fl = false;
							--y;
							Map.SensorsRead(x, y, Map.Left);
						}
						break;
					}
					case Map.Up:
					{
						if (x > 0 && Map._map[x, y, Map.Up] == 0) //&& CheckWalls(x - 1, y , Up))
						{
							fl = false;
							--x;
							Map.SensorsRead(x, y, Map.Up);
						}
						break;
					}
					case Map.Right:
					{
						if (y + 1 < Map.wight && Map._map[x, y, Map.Right] == 0) // && 
							// CheckWalls(x, y + 1, Right))
						{
							fl = false;
							++y;
							Map.SensorsRead(x, y, Map.Right);
						}
						break;
					}
				}
				if (fl)
				{
					Map._sensors[0] = -1;
					Map._sensors[1] = -1;
					Map._sensors[2] = -1;
					Map._sensors[3] = -1;
					wayExist = false;
					return ; //-1
				}

				//Console.WriteLine("FFFUUUUUCKKKKKKKKKK" + hypothesis[0].Count);
				Map.Hypothesis3(way[k]);

				for (int i = 0; i < way.Count; i++)
				{
					Console.Write(way[i]);
				}
				Console.WriteLine();
				if (Map.hypothesis[0].Count == 1)
				{
					localiz = true;
					var n = Map.best_ways.Count;
					Map.best_ways.Add(new List<int>());
					Map.best_ways[n].Add(x_start);
					Map.best_ways[n].Add(y_start);
					Map.best_ways[n].Add(direction);
					for (var l = 0; l <= k; l++)
					{
						Map.best_ways[n].Add(way[l]);
						//	Console.Write(best_ways[n][l] + ",");
					}
					//Console.WriteLine(" quantity of steps: " + k);
					return; // k;
				}

				if (Map.hypothesis[0].Count == 0)
				{
					wayExist = false;
					return;
				}
			}
			//Console.WriteLine("HC: " + hypothesis[0].Count);
			//return Map.hypothesis[0].Count;
		}
	}
}