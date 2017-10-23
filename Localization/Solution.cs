using System;
using System.Collections.Generic;

namespace Localization
{
	/// <summary>
	/// Более оптимальный путь будет в том случае, если робот сам будет на каждом шаге
	/// запускать этот алгоритм, учитывая свой пройденный путь
	/// </summary>
	class Solution
	{
		private int quantityDifferentWays = 16;
		private int _way = 15;
		
		public void GetWays(ref Map map, ref List<List<int>> ways, ref FinalWays finalWays)
		{
			var currentWay = new int[4] {0, 0, 0, 0};
			var timeOfWay = new TimeOfWay();
			timeOfWay.GetTime(ref ways, true);
			//var finalWays = new FinalWays();
			//finalWays = new FinalWays(); 
			var selectedPaths = new List<List<int>>();
			
			//0 - Down, 1- Left, 2 - Up, 3 - Right
			//var sumTimeForDirecrion = new int[4];
			var indexDirections = -1;
			var step = -1;
			while (ways.Count > 0)
			{
				step++;
				indexDirections++;
				finalWays.Ways.Add(new List<int>());
				timeOfWay.GetTime(ref ways);
				for (var i = 0; i < quantityDifferentWays; i++)
				{
					NextWay(ref currentWay,ref map);
					HypothesisFilter(ref map, step);
					var direcrion = ChooseDirection(ways, map);

					finalWays.Ways[indexDirections].Add(direcrion);
					
					/*
					Console.WriteLine("Way " + i);
					for (var j = 0; j < 4; j++)
					{
						Console.Write(currentWay[j]);
					}
					Console.WriteLine();
					*/
				}
				WayFilter(ref ways);
				//timeOfWay.GetTime(ref ways);
			}
			/*
			for (var i = 0; i < finalWays.directions.Count; i++)
			{
				Console.WriteLine("step " + i);
				for (var j = 0; j < finalWays.directions[i].Count; j++)
				{
					Console.Write(finalWays.directions[i][j] + " ");
				}
				Console.WriteLine();
			}
			*/
		}
		
		
		private void HypothesisFilter(ref Map map, int step)
		{
			map.HypothesisInit();
			map.Hypothesis1New();
			for (var i = 0; i < map.Hypothesis[0].Count; i++)
			{
				int x = map.Hypothesis[0][i],
					y = map.Hypothesis[1][i],
					direction = map.Hypothesis[2][i];
				if (!CheckWalls(x, y, direction, step, map))
				{
					map.Hypothesis[0].RemoveAt(i);
					map.Hypothesis[1].RemoveAt(i);
					map.Hypothesis[2].RemoveAt(i);
					i--;
				}
			}
		}
		
		private int ChooseDirection(List<List<int>> ways, Map map)
		{
			var sumTimeForDirecrion = new double[4];
			var quantity = new double[4];
			for (var i = 0; i < map.Hypothesis[0].Count; i++)
			{
				int x = map.Hypothesis[0][i], y = map.Hypothesis[1][i],
					direction = map.Hypothesis[2][i];
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
			return ChooseMin(sumTimeForDirecrion);
		}

		
		private int ChooseMin(double[] sumTimeForDirecrion)
		{
			var min = sumTimeForDirecrion[0];
			var indexMin = 0;
			for (var i = 1; i < 4; i++)
			{
				if (min > sumTimeForDirecrion[i])
				{
					min = sumTimeForDirecrion[i];
					indexMin = i;
				}
			}
			
			if (sumTimeForDirecrion[indexMin] < 1000000)
			{
				return indexMin + 1;
			}
			return 0;
		}

		private void WayFilter(ref List<List<int>> ways)
		{
			for (var i = 0; i < ways.Count; i++)
			{
				ways[i].RemoveAt(4);
				if (ways[i].Count == 4)
				{
					ways.RemoveAt(i);
					i--;
				}
			}
		}
		
		private void NextWay(ref int[] currentWay,ref Map map)
		{
			_way++;
			if (_way == 16) _way = 0;
			var cway = _way;
			for (var i = 0; i < 4; i++)
			{
				currentWay[i] = cway % 2;
				map.Sensors[i] = cway % 2;
				cway >>= 1;
			}
		}
		
		private void CopyWay(List<int> from,ref List<int> to)
		{
			to.Clear();
			for (var i = 0; i < from.Count; ++i)
			{
				to.Add(from[i]);
			}
		}

		private bool CheckWalls(int x, int y, int direction, int step, Map map)
		{
			//Robot.Sensors = _sensors;
			/*
			if (step == 1)
			{
				if (direction > 2) direction -= 2;
				else direction += 2;
			}
			*/
			//_sensors = Robot.Sensors;
			if (direction == 1)
			{
				if (map.map[x, y, 1] != map.Sensors[2]) return false;
				if (map.map[x, y, 2] != map.Sensors[3]) return false;
				if (map.map[x, y, 3] != map.Sensors[0]) return false;
				if (map.map[x, y, 4] != map.Sensors[1]) return false;
				return true;
			}
			else if (direction == 2)
			{
				if (map.map[x, y, 1] != map.Sensors[1]) return false;
				if (map.map[x, y, 2] != map.Sensors[2]) return false;
				if (map.map[x, y, 3] != map.Sensors[3]) return false;
				if (map.map[x, y, 4] != map.Sensors[0]) return false;
				return true;
			}
			else if (direction == 3)
			{
				if (map.map[x, y, 1] != map.Sensors[0]) return false;
				if (map.map[x, y, 2] != map.Sensors[1]) return false;
				if (map.map[x, y, 3] != map.Sensors[2]) return false;
				if (map.map[x, y, 4] != map.Sensors[3]) return false;
				return true;
			}
			else
			{
				if (map.map[x, y, 1] != map.Sensors[3]) return false;
				if (map.map[x, y, 2] != map.Sensors[0]) return false;
				if (map.map[x, y, 3] != map.Sensors[1]) return false;
				if (map.map[x, y, 4] != map.Sensors[2]) return false;
				return true;
			}
		}
	}
}