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
			int x = Map.hypothesis[0][number], y = Map.hypothesis[1][number],
				direction = Map.hypothesis[2][number];
			var newDir = Map.hypothesis[2][number];
			for (int k = 0; k < way.Count; k++)
			{
				bool fl = true;
				//int a = hypothesis[2][i], b = ways[j][k];
				newDir = Map.ChooseDir(newDir, way[k]);
				switch (newDir)
				{
					case Map.Down:
					{
						if (x + 1 < Map.height && Map._map[x, y, Map.Down] == 0) 
							// && CheckWalls(x, y + 1, Down))
						{
							fl = false;
							//++x;
							Map.SensorsRead(x+1, y, Map.Down);
						}
						break;
					}
					case Map.Left:
					{
						if (y > 0 && Map._map[x, y, Map.Left] == 0) // && CheckWalls(x, y - 1, Left))
						{
							fl = false;
							//--y;
							Map.SensorsRead(x, y-1, Map.Left);
						}
						break;
					}
					case Map.Up:
					{
						if (x > 0 && Map._map[x, y, Map.Up] == 0) //&& CheckWalls(x - 1, y , Up))
						{
							fl = false;
							//--x;
							Map.SensorsRead(x-1, y, Map.Up);
						}
						break;
					}
					case Map.Right:
					{
						if (y + 1 < Map.wight && Map._map[x, y, Map.Right] == 0) // && 
							// CheckWalls(x, y + 1, Right))
						{
							fl = false;
							//++y;
							Map.SensorsRead(x, y+1, Map.Right);
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
					Map.best_ways[n].Add(x);
					Map.best_ways[n].Add(y);
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

		/*
		private static int NumberOfSteps(int x, int y, int j, int i)
		{
			int x_coord = hypothesis[0][i], y_coord = hypothesis[1][i],
			 direction = hypothesis[2][i];
			//Console.WriteLine("Number of steps");
			//Заменить на цикл "пока не локализуюсь"
			var newDir = hypothesis[2][i];
			for (int k = 0; k < ways[j].Count; k++)
			{
				bool fl = true;
				//int a = hypothesis[2][i], b = ways[j][k];
				newDir = ChooseDir(newDir, ways[j][k]);
				switch (newDir)
				{
					case Down:
					{
						if (x + 1 < height && _map[x, y, Down] == 0) 
						// && CheckWalls(x, y + 1, Down))
						{
							fl = false;
							++x;
							SensorsRead(x, y, Down);
						}
						break;
					}
					case Left:
					{
						if (y > 0 && _map[x, y, Left] == 0) // && CheckWalls(x, y - 1, Left))
						{
							fl = false;
							--y;
							SensorsRead(x, y, Left);
						}
						break;
					}
					case Up:
					{
						if (x > 0 && _map[x, y, Up] == 0) //&& CheckWalls(x - 1, y , Up))
						{
							fl = false;
							--x;
							SensorsRead(x, y, Up);
						}
						break;
					}
					case Right:
					{
						if (y + 1 < wight && _map[x, y, Right] == 0) // && 
							// CheckWalls(x, y + 1, Right))
						{
							fl = false;
							++y;
							SensorsRead(x, y, Right);
						}
						break;
					}
				}
				if (fl)
				{
					_sensors[0] = -1;
					_sensors[1] = -1;
					_sensors[2] = -1;
					_sensors[3] = -1;
					return -1;
				}

				//Console.WriteLine("FFFUUUUUCKKKKKKKKKK" + hypothesis[0].Count);
				Hypothesis3(ways[j][k]);

				if (hypothesis[0].Count == 1)
				{
					var n = best_ways.Count;
					best_ways.Add(new List<int>());
					best_ways[n].Add(x_coord);
					best_ways[n].Add(y_coord);
					best_ways[n].Add(direction);
					for (var l = 0; l <= k; l++)
					{
						best_ways[n].Add(ways[j][l]);
						//	Console.Write(best_ways[n][l] + ",");
					}
					//Console.WriteLine(" quantity of steps: " + k);
					return k;
				}
			}
			//Console.WriteLine("HC: " + hypothesis[0].Count);
			return hypothesis[0].Count;
		}
		*/

		/*
		private static int Prognosis(int lengthOfWay)
		{
			Console.WriteLine("Prognosis");
			List<List<int>> copy_hypothesis = new List<List<int>>();

			HypothesisInit();

			for (int i = 0; i < hypothesis.Count; i++)
			{
				copy_hypothesis.Add(new List<int>());
			}

			Copy_Lists(ref copy_hypothesis, hypothesis);

			int n = hypothesis[0].Count;

			ways.Clear();
			WaysInit(lengthOfWay);
			GenerateWays(lengthOfWay);

			for (var i = 0; i < n; ++i)
			{
				for (int j = 0; j < ways.Count; j++)
				{
					int x = hypothesis[0][i], y = hypothesis[1][i];
					_quantityOfHypothises = hypothesis[0].Count;
					NumberOfSteps(x, y, j, i);
					hypothesis[0].Clear();
					hypothesis[1].Clear();
					hypothesis[2].Clear();
					Copy_Lists(ref hypothesis, copy_hypothesis);
				}
			}
			return 0;
		}

		private static void WaysInit(Int64 length)
		{
			length = (Int64) Math.Pow(4, length);
			for (var i = 0; i < length; ++i)
			{
				ways.Add(new List<int>());
			}
		}

		private static int GetDirectionFromWay(Int64 length)
		{
			length = (Int64) Math.Pow(4, length);
			Int64 k = length / 4, s = k;
			int direction = 1;
			//ways.Clear();

			while (k >= 1)
			{
				for (var i = 0; i < length; i++)
				{
					if (i == s)
						if (direction < 4)
						{
							direction++;
							s += k;
						}
						else
						{
							direction = 1;
							s += k;
						}
					ways[i].Add(direction);
				}
				k /= 4;
				s = k;
			}
			return 0;
		}

		private static void GenerateWays(long length)
		{
			length = (Int64) Math.Pow(4, length);
			Int64 k = length / 4, s = k;
			int direction = 1;
			//ways.Clear();

			while (k >= 1)
			{
				for (var i = 0; i < length; i++)
				{
					if (i == s)
						if (direction < 4)
						{
							direction++;
							s += k;
						}
						else
						{
							direction = 1;
							s += k;
						}
					ways[i].Add(direction);
				}
				k /= 4;
				s = k;
			}
		}
		*/

		//Хотя не факт
		/* Убрать генерацию путей!!!!! 
		 * Заменить на функцию, которая будет по индексу возвращать направление!!!
		 * Не забыть учесть изменения в методе Prognosis */
	}
}