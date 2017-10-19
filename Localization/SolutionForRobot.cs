using System;
using System.Collections.Generic;

namespace Localization
{
	class SolutionForRobot
	{
		private int quantityDifferentWays = 16;
		private int way = 15;

		public void SimulationOfLocalization(ref Map map, ref List<List<int>> bestWays, ref FinalWays finalWays)
		{
			//var currentWay = new int[4] {0, 0, 0, 0};
			var timeOfWay = new TimeOfWay();
			timeOfWay.GetTime(ref bestWays, true);
			var robot = new Robot();
			//int indexDirections = -1, step = -1;
			var localization = false;
			var motion = new Motion();
			var time = 0;
			var step = 0;
			map.HypothesisInit();
			var hypothesisCopy = new List<List<int>>();
			map.Copy_Lists(ref hypothesisCopy, map.Hypothesis);
			
			for (var i = 0; i < hypothesisCopy[0].Count; i++)
			{
				map.HypothesisInit();
				var beginWay = true;
				var x = hypothesisCopy[0][i];
				var y = hypothesisCopy[1][i];
				var direction = hypothesisCopy[2][i];
				finalWays.directions.Add(new List<int>());
				finalWays.directions[i].Add(x);
				finalWays.directions[i].Add(y);
				finalWays.directions[i].Add(direction);
				robot.InitialDirection = 3;
				map.SensorsRead(x, y, direction, robot);
				map.Hypothesis1New();
				while (!localization)
				{
					//indexDirections++;
					var valueOfSensor = GetValueOfSensor(robot.Sensors);
					//TODO: newDir должно быть равно вычисленному пути!!!
					//var newDir = finalWays.directions[step][valueOfSensor];
					finalWays.directions[i].Add(newDir);
					newDir = motion.GetNewDir(newDir, direction, beginWay);
					direction = newDir;//?? вроде
					//timeOfWay.GetTime(ref ways);
					//map.SensorsRead(x, y, direction, robot);
					robot.InitialDirection = newDir;
					switch (newDir)
					{
						case Map.Down:
						{
							if (x + 1 < map.Height && map._map[x, y, Map.Down] == 0)
							{
								++x;
								map.SensorsRead(x, y, Map.Down, robot);
								time ++;
							}
							break;
						}
						case Map.Left:
						{
							if (y > 0 && map._map[x, y, Map.Left] == 0)
							{
								--y;
								map.SensorsRead(x, y, Map.Left, robot);
								time += 2;
							}
							break;
						}
						case Map.Up:
						{
							if (x > 0 && map._map[x, y, Map.Up] == 0)
							{
								--x;
								map.SensorsRead(x, y, Map.Up, robot);
								time++;
							}
							break;
						}
						case Map.Right:
						{
							if (y + 1 < map.Wight && map._map[x, y, Map.Right] == 0)
							{
								++y;
								map.SensorsRead(x, y, Map.Right, robot);
								time += 2;
							}
							break;
						}
						default:
						{
							Console.WriteLine("SolutionForRobot.SimulationOfLocalization - BAG 1111111111111111");
							break;
						}
					}
					map.Hypothesis3(newDir, beginWay, motion, robot);
					
					if (map.Hypothesis[0].Count == 1)
					{
						localization = true;
					}
					if (map.Hypothesis[0].Count == 0)
					{
						Console.WriteLine("SolutionForRobot.SimulationOfLocalization - BAG 2222222222222222222222222");
						break;
					}
					beginWay = false;
					step++;
				}
				bestWays[i].Add(time);
			}
			for (var i = 0; i < bestWays.Count; i++)
			{
				Console.Write(i+". ");
				for (var j = 0; j < bestWays[i].Count; j++)
				{
					Console.Write(bestWays[i][j]);
				}
				Console.WriteLine();
			}
		}

		private int GetValueOfSensor(int[] sensor)
		{
			var value = 0;
			for (var i = 0; i < 4; i++)
			{
				value += (int) Math.Pow(2, i) * sensor[i];
			}
			return value;
		}
		
		private void GoTo()
		{
			
		}
		
		//TODO хватаю лишние пути(из-за наличия лишних гипотез). Добавить метод фильтра гипотез.
		//во, что я понял. Мне не важно, откуда я приехал. Важны только показания датчиков
		//на настоящий момент. Необходимо сверить их с картой и выкинуть лишние гипотезы
		//Доделать это 18.10 !!!

		private void GetDirection(ref Map map, ref List<List<int>> ways)
		{
			var currentWay = new int[4] {0, 0, 0, 0};
			NextWay(ref currentWay, ref map);
			HypothesisFilter(ref map);
			var direcrion = ChooseDirection(ways, map);
			WayFilter(ref ways);
		}
		
		public void GetWays(ref Map map, ref List<List<int>> ways, ref FinalWays finalWays)
		{
			var currentWay = new int[4] {0, 0, 0, 0};
			var timeOfWay = new TimeOfWay();
			timeOfWay.GetTime(ref ways, true);
			//var finalWays = new FinalWays();
			//finalWays = new FinalWays(); TODO: ???
			var selectedPaths = new List<List<int>>();

			//0 - Down, 1- Left, 2 - Up, 3 - Right
			//var sumTimeForDirecrion = new int[4];
			var indexDirections = -1;
			var step = -1; //TODO: 0->-1 возможно надо вернуть обратно
			while (ways.Count > 0)
			{
				step++;
				indexDirections++;
				finalWays.directions.Add(new List<int>());
				timeOfWay.GetTime(ref ways);
				// TODO: После каждого шага необходимо сдвигать гипотезы, иначе получается бред!!!
				for (var i = 0; i < quantityDifferentWays; i++)
				{
					NextWay(ref currentWay, ref map);
					//HypothesisFilter(ref map);
					var direcrion = ChooseDirection(ways, map);

					finalWays.directions[indexDirections].Add(direcrion);

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

			for (var i = 0; i < finalWays.directions.Count; i++)
			{
				Console.WriteLine("step " + i);
				for (var j = 0; j < finalWays.directions[i].Count; j++)
				{
					Console.Write(finalWays.directions[i][j] + " ");
				}
				Console.WriteLine();
			}
		}

		private void HypothesisFilter(ref Map map) /*, int step*/ 
		{
			map.HypothesisInit();
			map.Hypothesis1New();
			for (var i = 0; i < map.Hypothesis[0].Count; i++)
			{
				int x = map.Hypothesis[0][i],
					y = map.Hypothesis[1][i],
					direction = map.Hypothesis[2][i];
				if (!CheckWalls(x, y, direction, map))	
				{
					map.Hypothesis[0].RemoveAt(i);
					map.Hypothesis[1].RemoveAt(i);
					map.Hypothesis[2].RemoveAt(i);
					i--;
					//TODO: удалить путь. Возможно убрать инверсию, хз. Надо посмотреть. done
				}
			}
		}

		//TODO: Добавить ещё один фильтр гипотез (который будет учитывать расположение стен)
		private int ChooseDirection(List<List<int>> ways, Map map)
		{
			var sumTimeForDirecrion = new double[4];
			var quantity = new double[4];
			for (var i = 0; i < map.Hypothesis[0].Count; i++)
			{
				int x = map.Hypothesis[0][i],
					y = map.Hypothesis[1][i],
					direction = map.Hypothesis[2][i];
				for (var j = 0; j < ways.Count; j++)
				{
					if (x == ways[j][0] && y == ways[j][1] && direction == ways[j][2])
					{
						//TODO изменить направление (нужно текущее, а не начальное) done
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
					sumTimeForDirecrion[i] = 10000000000000000000;
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

			if (sumTimeForDirecrion[indexMin] < 10000000000000000000)
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

		private void NextWay(ref int[] currentWay, ref Map map)
		{
			way++;
			if (way == 16) way = 0;
			var cway = way;
			for (var i = 0; i < 4; i++)
			{
				currentWay[i] = cway % 2;
				map.Sensors[i] = cway % 2;
				cway >>= 1;
			}
		}

		private void CopyWay(List<int> from, ref List<int> to)
		{
			to.Clear();
			for (var i = 0; i < from.Count; ++i)
			{
				to.Add(from[i]);
			}
		}

		private bool CheckWalls(int x, int y, int direction,/* int step,*/ Map map)
		{
			//Robot.Sensors = _sensors;
			//TODO: проверить. Возможно условие ниже нужно раскомментировать
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
				if (map._map[x, y, 1] != map.Sensors[2]) return false;
				if (map._map[x, y, 2] != map.Sensors[3]) return false;
				if (map._map[x, y, 3] != map.Sensors[0]) return false;
				if (map._map[x, y, 4] != map.Sensors[1]) return false;
				return true;
			}
			else if (direction == 2)
			{
				if (map._map[x, y, 1] != map.Sensors[1]) return false;
				if (map._map[x, y, 2] != map.Sensors[2]) return false;
				if (map._map[x, y, 3] != map.Sensors[3]) return false;
				if (map._map[x, y, 4] != map.Sensors[0]) return false;
				return true;
			}
			else if (direction == 3)
			{
				if (map._map[x, y, 1] != map.Sensors[0]) return false;
				if (map._map[x, y, 2] != map.Sensors[1]) return false;
				if (map._map[x, y, 3] != map.Sensors[2]) return false;
				if (map._map[x, y, 4] != map.Sensors[3]) return false;
				return true;
			}
			else
			{
				if (map._map[x, y, 1] != map.Sensors[3]) return false;
				if (map._map[x, y, 2] != map.Sensors[0]) return false;
				if (map._map[x, y, 3] != map.Sensors[1]) return false;
				if (map._map[x, y, 4] != map.Sensors[2]) return false;
				return true;
			}
		}
	}
}