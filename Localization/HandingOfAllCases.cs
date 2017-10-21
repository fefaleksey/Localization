using System;
using System.Collections.Generic;

namespace Localization
{
	class HandingOfAllCases
	{
		private bool _wayExist = true, _localiz = false;
		
		private void CopyWay(List<int> from,ref List<int> to)
		{
			to.Clear();
			for (var i = 0; i < from.Count; ++i)
			{
				to.Add(from[i]);
			}
		}
		
		public void Handing(Map Map, Way Way)
		{
			//Console.WriteLine("Handing");
			var copyHypothesis = new List<List<int>>();
			Map.HypothesisInit();
			var way = new List<int>();
				
			for (var i = 0; i < Map.Hypothesis.Count; i++)
			{
				copyHypothesis.Add(new List<int>());
			}
			var fl = 1;
			int test = 0;
			Map.Copy_Lists(ref copyHypothesis, Map.Hypothesis);
			var n = Map.Hypothesis[0].Count;
			/*
			Map.hypothesis[0][0] = 5;
			Map.hypothesis[1][0] = 5;
			Map.hypothesis[2][0] = 1;
			*/
			var Motion = new Motion();
			var Robot = new Robot();
			for (var i = 0; i < n; ++i)
			{
				Way.CurentWayInit();
				while (fl != 0)
				{
					++test;
					CopyWay(Way.CurentWay, ref way);
					Map.SensorsRead(Map.Hypothesis[0][i], Map.Hypothesis[1][i],
									Map.Hypothesis[2][i],Robot); //!!
					HandingHypothesis(i, way, Map, Motion, Way, Robot);
					fl = Way.NextDirection(_wayExist, _localiz, Robot);
					Map.Hypothesis[0].Clear();
					Map.Hypothesis[1].Clear();
					Map.Hypothesis[2].Clear();
					Map.Copy_Lists(ref Map.Hypothesis, copyHypothesis);
					
					//////////////////////////////
					/*
					Map.hypothesis[0][0] = 5;
					Map.hypothesis[1][0] = 5;
					Map.hypothesis[2][0] = 1;
					*///////////////////////////////
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
										Map Map, Motion Motion, Way Way, Robot Robot)
		{
			int x = Map.Hypothesis[0][number],
				y = Map.Hypothesis[1][number],
				direction = Map.Hypothesis[2][number],
				xStart = x,
				yStart = y;
			var newDir = Map.Hypothesis[2][number];
			Way.BeginWay = true;
			//Map.Hypothesis1New();
			Map.Hypothesis1New();
			for (int k = 0; k < way.Count; k++)
			{
				bool fl = true;
				if (k == 1)
				{
					Way.BeginWay = false;
				}
				//int a = hypothesis[2][i], b = ways[j][k];
				newDir = Motion.GetNewDir(newDir, way[k], Way.BeginWay);//Map.ChooseDir(newDir, way[k]);
				switch (newDir)
				{
					case Map.Down:
					{
						if (x + 1 < Map.Height && Map._map[x, y, Map.Down] == 0) 
							// && CheckWalls(x, y + 1, Down))
						{
							fl = false;
							++x;
							Map.SensorsRead(x, y, Map.Down,Robot);
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
							Map.SensorsRead(x, y, Map.Left,Robot);
						}
						break;
					}
					case Map.Up:
					{
						if (x > 0 && Map._map[x, y, Map.Up] == 0) //&& CheckWalls(x - 1, y , Up))
						{
							fl = false;
							--x;
							Map.SensorsRead(x, y, Map.Up,Robot);
						}
						break;
					}
					case Map.Right:
					{
						if (y + 1 < Map.Wight && Map._map[x, y, Map.Right] == 0) // && 
							// CheckWalls(x, y + 1, Right))
						{
							fl = false;
							++y;
							Map.SensorsRead(x, y, Map.Right,Robot);
						}
						break;
					}
				}
				if (fl)
				{
					Map.Sensors[0] = -1;
					Map.Sensors[1] = -1;
					Map.Sensors[2] = -1;
					Map.Sensors[3] = -1;
					_wayExist = false;
					return ; //-1
				}

				//Console.WriteLine("FFFUUUUUCKKKKKKKKKK" + hypothesis[0].Count);
				Map.Hypothesis3(way[k], Way.BeginWay,Motion, Robot);
				/*
				for (int i = 0; i < way.Count; i++)
				{
					Console.Write(way[i]);
				}
				Console.WriteLine();
				*/
				if (Map.Hypothesis[0].Count == 1)
				{
					_localiz = true;
					var n = Map.BestWays.Count;
					Map.BestWays.Add(new List<int>());
					Map.BestWays[n].Add(xStart);
					Map.BestWays[n].Add(yStart);
					Map.BestWays[n].Add(direction);
					for (var l = 0; l <= k; l++)
					{
						Map.BestWays[n].Add(way[l]);
						//	Console.Write(best_ways[n][l] + ",");
					}
					//Console.WriteLine(" quantity of steps: " + k);
					return; // k;
				}

				if (Map.Hypothesis[0].Count == 0)
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