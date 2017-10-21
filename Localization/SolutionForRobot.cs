using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Localization
{
	class SolutionForRobot
	{
		private int quantityDifferentWays = 16;
		private int way = 15;

		// TODO: Баги при движении назад. Исправить! Возможно ещё будут баги при поворотах. Проверить.
		public void SimulationOfLocalization(ref Map map, ref List<List<int>> bestWays, ref FinalWays finalWays)
		{
			//var currentWay = new int[4] {0, 0, 0, 0};
			var timeOfWay = new TimeOfWay();
			timeOfWay.GetTime(ref bestWays, true);
			var robot = new Robot();
			//int indexDirections = -1, step = -1;
			var localization = false;
			var motion = new Motion();
			//var step = 0;
			map.HypothesisInit();
			var hypothesisCopy = new List<List<int>>();
			map.Copy_Lists(ref hypothesisCopy, map.Hypothesis);
			List<List<int>> bestWaysCopy = new List<List<int>>();
			CopyLists(ref bestWaysCopy, bestWays);
			var QUANTITYBAGS = 0;
			
			for (var i = 0; i < hypothesisCopy[0].Count; i++)
			{
				var time = 0;
				var step = 0;
				map.HypothesisInit();
				var beginWay = true;
				var x = hypothesisCopy[0][i];
				var y = hypothesisCopy[1][i];
				var direction = hypothesisCopy[2][i];
				finalWays.Directions.Add(new List<int>());
				finalWays.Directions[i].Add(x);
				finalWays.Directions[i].Add(y);
				finalWays.Directions[i].Add(direction);
				robot.InitialDirection = 3;
				map.SensorsRead(x, y, direction, robot);
				HypothesisFilter(ref map);
				//map.Hypothesis1New();
				var newDir = direction;
				var directionOfTheNextStep = direction;
				while (!localization)
				{
					//indexDirections++;
					var valueOfSensor = GetValueOfSensor(robot.Sensors);
					//TODO: newDir должно быть равно вычисленному пути!!!
					//var newDir = direction;
					int directionForGetDirection;
					
					if (beginWay || robot.InitialDirection != 1)
					{
						directionForGetDirection = 3;
					}
					else
					{
						directionForGetDirection = 1;
					}
					newDir = GetDirection(ref map, ref bestWays, robot.Sensors, directionForGetDirection, beginWay, robot);
					directionOfTheNextStep = newDir;
					finalWays.Directions[i].Add(newDir);
					newDir = motion.GetNewDir(direction, newDir, beginWay);
					direction = newDir;
					//timeOfWay.GetTime(ref ways);
					//map.SensorsRead(x, y, direction, robot);
					if (step == 0)
					{
						robot.InitialDirection = directionOfTheNextStep;
					}
					switch (newDir)
					{
						case Map.Down:
						{
							if (x + 1 < map.Height && map._map[x, y, Map.Down] == 0)
							{
								++x;
								map.SensorsRead(x, y, Map.Down, robot);
								//time ++;
							}
							break;
						}
						case Map.Left:
						{
							if (y > 0 && map._map[x, y, Map.Left] == 0)
							{
								--y;
								map.SensorsRead(x, y, Map.Left, robot);
								//time += 2;
							}
							break;
						}
						case Map.Up:
						{
							if (x > 0 && map._map[x, y, Map.Up] == 0)
							{
								--x;
								map.SensorsRead(x, y, Map.Up, robot);
								//time++;
							}
							break;
						}
						case Map.Right:
						{
							if (y + 1 < map.Wight && map._map[x, y, Map.Right] == 0)
							{
								++y;
								map.SensorsRead(x, y, Map.Right, robot);
								//time += 2;
							}
							break;
						}
						default:
						{
							Console.WriteLine("SolutionForRobot.SimulationOfLocalization - BAG 1111111111111111");
							break;
						}
					}
					//Не newDir, а направление, куда должны ехать. Вроде исправил
					map.Hypothesis3(directionOfTheNextStep, beginWay, motion, robot);
					
					if (map.Hypothesis[0].Count == 1)
					{
						finalWays.Directions[i].Add(8888888);
						finalWays.Directions[i].Add(map.Hypothesis[0][0]);
						finalWays.Directions[i].Add(map.Hypothesis[1][0]);
						if(robot.InitialDirection==1) 
							finalWays.Directions[i].Add(OppositeDirection(map.Hypothesis[2][0]));
						else
							finalWays.Directions[i].Add(map.Hypothesis[2][0]);
						localization = true;
					}
					if (map.Hypothesis[0].Count == 0)
					{
						QUANTITYBAGS++;
						//Console.WriteLine("SolutionForRobot.SimulationOfLocalization - BAG 2222222222222222222222222");
						break;
					}
					if (directionOfTheNextStep == 1 || directionOfTheNextStep == 3)
						time++;
					else
						time += 2;
					beginWay = false;
					step++;
					//TODO: проверить, не попали ли мы в тупик
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
				finalWays.Directions[i].Add(8888888);
				finalWays.Directions[i].Add(time);
				CopyLists(ref bestWays, bestWaysCopy);
			}
			
			
			PrintResult(finalWays);
			finalWays.SetFinalList(finalWays.Directions);
			//PrintReleaseResult(finalWays);
			Console.WriteLine(QUANTITYBAGS);
		}

		private void PrintResult(FinalWays finalWays)
		{
			for (var i = 0; i < finalWays.Directions.Count; i++)
			{
				Console.Write(i+". ");
				for (var j = 0; j < finalWays.Directions[i].Count; j++)
				{
					Console.Write(finalWays.Directions[i][j]+" ");
				}
				Console.WriteLine();
			}
		}

		private void PrintReleaseResult(FinalWays finalWays)
		{
			for (var i = 0; i < finalWays.Directions.Count; i++)
			{
				//Console.Write(i+". ");
				var j = finalWays.Directions[i].Count - 1;
				Console.Write(finalWays.Directions[i][0] + " " + finalWays.Directions[i][1] + " " +
				              finalWays.Directions[i][2] + " " + finalWays.Directions[i][j]);
				Console.WriteLine();
			}
		}

		private bool Impasse(Robot robot)
		{
			var numbersOfWalls = 0;
			for (var i = 0; i < robot.Sensors.Length; i++)
			{
				if (robot.Sensors[i] == 1)
					numbersOfWalls++;
			}
			if (numbersOfWalls == 3)
				return true;
			return false;
		}
		
		
		public void CopyLists(ref List<List<int>> to, List<List<int>> from)
		{
			//int m = from.Count, n = from[0].Count;
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

		private int GetDirection(ref Map map, ref List<List<int>> ways, int[] sensors, int currentDirection, bool beginWay, Robot robot)
		{
			/*
			var currentWay = new int[4] {0, 0, 0, 0};
			NextWay(ref currentWay, ref map);
			*/
			//CorrectDirectionsInHypothesis(robot, ref map);
			//HypothesisFilter(ref map); //TODO: Добавил. Проверить
			var timeOfWay = new TimeOfWay();
			timeOfWay.GetTime(ref ways);
			var direcrion = ChooseDirection(ways, map, currentDirection, beginWay, robot);
			//WayFilter(ref ways);
			//CorrectDirectionsInHypothesis(robot, ref map);
			if (robot.InitialDirection == 1)
				if (direcrion == 2 || direcrion == 4)
					return OppositeDirection(direcrion);
			return direcrion;
		}
		/*
		public void GetWay(ref Map map, ref List<List<int>> ways, ref FinalWays finalWays)
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
				/*}
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
		*/

		private void CorrectDirectionsInHypothesis(Robot robot, ref Map map)
		{
			for (var i = 0; i < map.Hypothesis[0].Count; i++)
			{
				if (robot.InitialDirection==1)
				{
					map.Hypothesis[2][i] = OppositeDirection(map.Hypothesis[2][i]);
				}
			}
		}
		
		private void HypothesisFilter(ref Map map) //, int step 
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
						//TODO: изменить направление (нужно текущее, а не начальное) done
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
			
			var min = sumTimeForDirecrion[0];
			var indexMin = 0;
			var indices = new int[4] {1, 2, 3, 4};
			
			for (var i = 0; i < 4; i++)
			{
				for (var j = i+1; j < 4; j++)
				{
					if (sumTimeForDirecrion[i]>sumTimeForDirecrion[j])
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
			//TODO: Нет проверки на то, что мы не попали в тупик
			//TODO: (хотя вроде это должно гарантироваться построением гипотез, но хз, лучше потом проверить
			if (beginWay) return indices[0];
			if (oppositeDirection == indices[0]) return indices[1];
			return indices[0];
			
			/*
			for (var i = 1; i < 4; i++)
			{
				if (min > sumTimeForDirecrion[i])
				{
					min = sumTimeForDirecrion[i];
					indexMin = i;
				}
			}
			
			if (sumTimeForDirecrion[indexMin] < 1000000000)
			{
				return indexMin + 1;
			}
			return 0;
			*/
		}

		private int OppositeDirection(int direction)
		{
			if (direction > 2) return direction - 2;
			return direction + 2;
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
		
		/*
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
		*/
		private bool CheckWalls(int x, int y, int direction,Map map)
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