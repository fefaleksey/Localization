using System;
using System.Collections.Generic;

/*
	Добавить вариант: Если картины одинаковые при движении во все стороны, расчитать следующий
	шаг
*/
namespace Localization
{
	public class Map
	{
		public static int[,,] _map = new int[8, 8, 5]
		{
			{
				{2, 0, 1, 1, 0}, {2, 1, 0, 1, 0}, {1, 0, 0, 1, 0}, {2, 1, 0, 1, 0},
				{2, 1, 0, 1, 0}, {2, 0, 0, 1, 1}, {4, 1, 1, 1, 1}, {3, 0, 1, 1, 1}
			},

			{
				{2, 0, 1, 0, 1}, {4, 1, 1, 1, 1}, {2, 0, 1, 0, 1}, {3, 0, 1, 1, 1},
				{4, 1, 1, 1, 1}, {1, 0, 1, 0, 0}, {2, 1, 0, 1, 0}, {2, 1, 0, 0, 1}
			},

			{
				{2, 1, 1, 0, 0}, {2, 1, 0, 1, 0}, {1, 0, 0, 0, 1}, {1, 0, 1, 0, 0},
				{2, 1, 0, 1, 0}, {1, 0, 0, 0, 1}, {4, 1, 1, 1, 1}, {3, 0, 1, 1, 1}
			},

			{
				{3, 0, 1, 1, 1}, {4, 1, 1, 1, 1}, {2, 0, 1, 0, 1}, {3, 1, 1, 0, 1},
				{4, 1, 1, 1, 1}, {1, 0, 1, 0, 0}, {2, 1, 0, 1, 0}, {1, 0, 0, 0, 1}
			},

			{
				{1, 0, 1, 0, 0}, {2, 1, 0, 1, 0}, {0, 0, 0, 0, 0}, {2, 1, 0, 1, 0},
				{1, 0, 0, 1, 0}, {1, 0, 0, 0, 1}, {4, 1, 1, 1, 1}, {3, 1, 1, 0, 1}
			},

			{
				{3, 1, 1, 0, 1}, {4, 1, 1, 1, 1}, {2, 0, 1, 0, 1}, {4, 1, 1, 1, 1},
				{2, 1, 1, 0, 0}, {0, 0, 0, 0, 0}, {2, 1, 0, 1, 0}, {2, 0, 0, 1, 1}
			},

			{
				{2, 0, 1, 1, 0}, {2, 1, 0, 1, 0}, {0, 0, 0, 0, 0}, {2, 0, 0, 1, 1},
				{4, 1, 1, 1, 1}, {3, 1, 1, 0, 1}, {4, 1, 1, 1, 1}, {2, 0, 1, 0, 1}
			},

			{
				{3, 1, 1, 0, 1}, {4, 1, 1, 1, 1}, {2, 1, 1, 0, 0}, {1, 1, 0, 0, 0},
				{2, 1, 0, 1, 0}, {2, 1, 0, 1, 0}, {2, 1, 0, 1, 0}, {2, 1, 0, 0, 1}
			}
		};

		public static int[] _sensors = new int[4];

		public const int Down = 1;
		public const int Left = 2;
		public const int Up = 3;
		public const int Right = 4;

		public static int height = 8;

		public static int wight = 8;

		private static int //размеры карты (_map)
			_quantityOfHypothises = 0;

		public static List<List<int>> hypothesis = new List<List<int>>(); // x,y,direction

		private static List<List<int>> ways = new List<List<int>>();

		//way: x, y, directions
		public static List<List<int>> best_ways = new List<List<int>>();

		private static List<List<int>> endOfWay = new List<List<int>>(); //x,y, maybe else direction

		private static int _quantityOfWays = 0;

		/******************************************************************************************/

		public static void Localization()
		{
			int quantity = 3;
			StartInit();
			/*
		Console.WriteLine("localization");
		SensorsInit(1, 0, 0, 0);
		Hypothesis1(quantity);
		Console.WriteLine("Hypotsesis 1");
		PrintMap();
		//go in random direction
		SensorsInit(0, 1, 0, 0);
		Console.WriteLine("step 1");
		Hypothesis2();
		PrintMap();
		//SensorsInit(0, 1, 0, 1);
		//Console.WriteLine("step 2");
		//Hypothesis3(Right);
		PrintMap();
		*/
//		Prognosis(10);
		HandingOfAllCases.Handing();
		ListFiltration(ref best_ways);
		PrintMap();
		for (var l = 0; l < best_ways.Count; l++)
		{
			Console.WriteLine("ways " + l);
			for (int i = 0; i < best_ways[l].Count; i++)
			{
				//Console.WriteLine("ways " + l);
				Console.Write(best_ways[l][i] + ",");
				//Console.WriteLine();
			}
			Console.WriteLine();
		}
		}

		private static void MapInit()
		{
		
		}

		public static void PrintMap()
		{
			int i, j, k = hypothesis[0].Count, n = 0;

			for (var l = 0; l < hypothesis[2].Count; ++l)
			{
				Console.Write(hypothesis[0][l] + " "
				              + hypothesis[1][l] + " " + hypothesis[2][l] + "; ");
			}
			Console.WriteLine();
			for (i = 0; i < height; ++i)
			{
				for (j = 0; j < wight; ++j)
				{

					if (n < k && i == hypothesis[0][n] && j == hypothesis[1][n])
					{
						Console.Write(hypothesis[2][n]);
						++n;
						while (n < k && hypothesis[0][n] == hypothesis[0][n - 1] &&
						       hypothesis[1][n] == hypothesis[1][n - 1])
							++n;
					}
					else Console.Write(".");
				}
				Console.WriteLine();
			}
		}

		public static void PrintWays()
		{
			int i; //,l=quantityOfWays;
			for (i = 0; i < ways.Count; ++i)
			{
				Console.Write(i + ". ");
				int n = ways[i].Count;
				for (int j = 0; j < n; ++j)
				{
					Console.Write(ways[i][j] + " ");
				}
				Console.WriteLine();
			}
		}

		private static void StartInit()
		{
			for (int i = 0; i < 3; i++)
			{
				hypothesis.Add(new List<int>());
			}
		}

		public static void HypothesisInit()
		{
			_quantityOfHypothises = 0;
			var n = 0;
			for (var i = 0; i < wight; i++)
			{
				for (var j = 0; j < height; j++)
				{
					for (var k = 1; k < 5; k++)
					{
						hypothesis[0].Add(i);
						hypothesis[1].Add(j);
						hypothesis[2].Add(k);
						++n;
						++_quantityOfHypothises;
					}
				}
			}
		}

		private static void SensorsInit(int down, int left, int up, int right)
		{
			_sensors[0] = down;
			_sensors[1] = left;
			_sensors[2] = up;
			_sensors[3] = right;
		}
	
		//сравнивает показания датчиков с полем на карте (сравнивает стены)
		//(робот двигался (x0,y0)->(x1,y1)
		private static bool CheckWalls(int x, int y, int direction)
		{
			if (direction == Down)
			{
				if (_map[x, y, 1] != _sensors[2]) return false;
				if (_map[x, y, 2] != _sensors[3]) return false;
				if (_map[x, y, 3] != _sensors[0]) return false;
				if (_map[x, y, 4] != _sensors[1]) return false;
				return true;
			}
			else if (direction == Left)
			{
				if (_map[x, y, 1] != _sensors[1]) return false;
				if (_map[x, y, 2] != _sensors[2]) return false;
				if (_map[x, y, 3] != _sensors[3]) return false;
				if (_map[x, y, 4] != _sensors[0]) return false;
				return true;
			}
			else if (direction == Up)
			{
				if (_map[x, y, 1] != _sensors[0]) return false;
				if (_map[x, y, 2] != _sensors[1]) return false;
				if (_map[x, y, 3] != _sensors[2]) return false;
				if (_map[x, y, 4] != _sensors[3]) return false;
				return true;
			}
			else
			{
				if (_map[x, y, 1] != _sensors[3]) return false;
				if (_map[x, y, 2] != _sensors[0]) return false;
				if (_map[x, y, 3] != _sensors[1]) return false;
				if (_map[x, y, 4] != _sensors[2]) return false;
				return true;
			}
		}

		// 0 <= quantity <= 4
		//Сюда ещё можно добавить расположение стен (| | или _|)
		private static void Hypothesis1(int quantity)
		{
			int i, j;
			for (i = 0; i < height; ++i)
			{
				for (j = 0; j < wight; ++j)
				{
					if (_map[i, j, 0] == quantity)
					{
						hypothesis[0].Add(i);
						hypothesis[1].Add(j);
						hypothesis[2].Add(3);

						_quantityOfHypothises++;
					}
				}
			}
			//передвигаемся прямо (если занято, то направо/налево/назад)
		}


		private static void Hypothesis2()
		{
			int i, quantity = _sensors[0] + _sensors[1] + _sensors[2] + _sensors[3];
			for (i = 0; i < hypothesis[0].Count; ++i)
			{
				var fl = true;
				int x = hypothesis[0][i], y = hypothesis[1][i];
				//to test code below
				//down
				Console.Write(x + " " + y + " " + hypothesis[0].Count + ";   ");
				if (x + 1 < height && _map[x + 1, y, 0] == quantity)
				{
					if (_map[x, y, Down] == 0 &&
					    CheckWalls(x + 1, y, Down))
					{
						++_quantityOfWays;
						//var x = ++start[2][i];
						hypothesis[0][i]++;
						hypothesis[2][i] = Down; //ToDownDir(hypothesis[2][i]);
						fl = false;
					}
				}

				//left
				if (y > 0 && _map[x, y - 1, 0] == quantity)
				{
					if (_map[x, y, Left] == 0 &&
					    CheckWalls(x, y - 1, Left))
					{
						++_quantityOfWays;
						//var x = ++start[2][i];
						hypothesis[1][i]--;
						hypothesis[2][i] = Left; //ToLeftDir(hypothesis[2][i]);
						fl = false;
					}
				}

				//up
				if (x > 0 && _map[x - 1, y, 0] == quantity)
				{
					if (_map[x, y, Up] == 0 &&
					    CheckWalls(x - 1, y, Up))
					{
						++_quantityOfWays;
						//var x = ++start[2][i];
						hypothesis[0][i]--;
						hypothesis[2][i] = Up;
						fl = false;
					}
				}

				//right
				if (y + 1 < wight && _map[x, y + 1, 0] == quantity)
				{
					if (_map[x, y, Right] == 0 &&
					    CheckWalls(x, y + 1, Right))
					{
						++_quantityOfWays;
						//var x = ++start[2][i];
						hypothesis[1][i]++;
						hypothesis[2][i] = Right; //ToRightDir(hypothesis[2][i]);
						fl = false;
					}
				}
				if (fl)
				{
					hypothesis[0].RemoveAt(i);
					hypothesis[1].RemoveAt(i);
					hypothesis[2].RemoveAt(i);
					i--;
					_quantityOfHypothises--;
				}
			}
			Console.WriteLine("eeee " + hypothesis[0].Count + " " + hypothesis[1].Count
			                  + " " + hypothesis[2].Count);
		}

		//потестить ещё
		public static void Hypothesis3(int direction)
		{
			int i, quantity = _sensors[0] + _sensors[1] + _sensors[2] + _sensors[3];

			for (i = 0; i < hypothesis[0].Count; ++i)
			{
				var newDir = ChooseDir(hypothesis[2][i], direction);
				var fl = true;
				switch (newDir)
				{
					case Down:
					{
						//int i, quantity = _sensors[0] + _sensors[1] + _sensors[2] + _sensors[3];
						//var fl = true;
						int x = hypothesis[0][i], y = hypothesis[1][i];

						if (x + 1 < height && _map[x + 1, y, 0] == quantity)
						{
							if (_map[x, y, Down] == 0 &&
							    CheckWalls(x + 1, y, Down))
							{
								//++_quantityOfWays;
								//var x = ++start[2][i];
								hypothesis[0][i]++;
								hypothesis[2][i] = Down; //ToDownDir(hypothesis[2][i]);
								fl = false;
							}
						}
						break;
					}
					case Left:
					{
						//int i, quantity = _sensors[0] + _sensors[1] + _sensors[2] + _sensors[3];
						//var fl = true;
						int x = hypothesis[0][i], y = hypothesis[1][i];

						if (y > 0 && _map[x, y - 1, 0] == quantity)
						{
							if (_map[x, y, Left] == 0 && CheckWalls(x, y - 1, Left))
							{
								//++_quantityOfWays;
								//var x = ++start[2][i];
								hypothesis[1][i]--;
								hypothesis[2][i] = Left; //ToDownDir(hypothesis[2][i]);
								fl = false;
							}
						}
						break;
					}
					case Up:
					{
						//int i, quantity = _sensors[0] + _sensors[1] + _sensors[2] + _sensors[3];
						//var fl = true;
						int x = hypothesis[0][i], y = hypothesis[1][i];

						if (x > 0 && _map[x - 1, y, 0] == quantity)
						{
							if (_map[x, y, Up] == 0 && CheckWalls(x - 1, y, Up))
							{
								//++_quantityOfWays;
								//var x = ++start[2][i];
								hypothesis[0][i]--;
								hypothesis[2][i] = Up; //ToDownDir(hypothesis[2][i]);
								fl = false;
							}
						}
						break;
					}
					case Right:
					{
						//int i, quantity = _sensors[0] + _sensors[1] + _sensors[2] + _sensors[3];
						//var fl = true;
						int x = hypothesis[0][i], y = hypothesis[1][i];

						if (y + 1 < wight && _map[x, y + 1, 0] == quantity)
						{
							if (_map[x, y, Right] == 0 && CheckWalls(x, y + 1, Right))
							{
								//++_quantityOfWays;
								//var x = ++start[2][i];
								hypothesis[1][i]++;
								hypothesis[2][i] = Right; //ToDownDir(hypothesis[2][i]);
								fl = false;
							}
						}
						break;
					}
				}
				if (fl)
				{
					hypothesis[0].RemoveAt(i);
					hypothesis[1].RemoveAt(i);
					hypothesis[2].RemoveAt(i);
					i--;
					_quantityOfHypothises--;
				}
			}
		}

		private static int ToRightDir(int direction)
		{
			if (direction < 4) return direction + 1;
			return 1;
		}

		//Возвращает направление, если поворачиваем в соотв. сторону
		private static int ToLeftDir(int direction)
		{
			if (direction > 1) return direction - 1;
			return 4;
		}

		private static int ToDownDir(int direction)
		{
			if (direction > 2) return direction - 2;
			return direction + 2;
		}

		// текущее направление в абсолютных коорд/направление движения(куда едем?)
		public static int ChooseDir(int currentDirection, int newDirection)
		{
			if (newDirection == Up) return currentDirection;
			if (newDirection == Right) return ToRightDir(currentDirection);
			if (newDirection == Left) return ToLeftDir(currentDirection);
			return ToDownDir(currentDirection);
		}

		/*У робота:
		в-1000
		п-100
		н-10
		л-1
	*/
		private static void CheckIndications(int x, int y, int direction,
			ref List<List<int>> differentInd, int newDirection)
		{
			int value, //k=direction-1;
				k = ChooseDir(direction, newDirection) - 1;
			switch (newDirection)
			{
				case Down:
					value = 1000 * _map[x, y, 1] + 100 * _map[x, y, 2] +
					        10 * _map[x, y, 3] + _map[x, y, 4];
					if (!differentInd[k].Contains(value)) differentInd[k].Add(value); //0
					break;
				case Left:
					value = 1000 * _map[x, y, 2] + 100 * _map[x, y, 3] +
					        10 * _map[x, y, 4] + _map[x, y, 1];
					if (!differentInd[k].Contains(value)) differentInd[k].Add(value); //1
					break;
				case Up:
					value = 1000 * _map[x, y, 3] + 100 * _map[x, y, 4] +
					        10 * _map[x, y, 1] + _map[x, y, 2];
					if (!differentInd[k].Contains(value)) differentInd[k].Add(value); //2
					break;
				case Right:
					value = 1000 * _map[x, y, 4] + 100 * _map[x, y, 1] +
					        10 * _map[x, y, 2] + _map[x, y, 3];
					if (!differentInd[k].Contains(value)) differentInd[k].Add(value); //3
					break;
				default:
					Console.WriteLine("CheckIndications has fallen!!!");
					break;
			}
		}

		public static void Copy_Lists(ref List<List<int>> to, List<List<int>> from)
		{
			int m = from.Count, n = from[0].Count;

			for (var i = 0; i < n; ++i)
			{
				to[0].Add(from[0][i]);
				to[1].Add(from[1][i]);
				to[2].Add(from[2][i]);
			}
			//int[,,] copy_hypothesis = new int[n, n, n];
		}

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
		
			/*
		for (var l = 0; l < best_ways.Count; l++)
		{
			Console.WriteLine("ways " + l);
			for (int i = 0; i < best_ways[l].Count; i++)
			{
				//Console.WriteLine("ways " + l);
				Console.Write(best_ways[l][i] + ",");
				//Console.WriteLine();
			}
			Console.WriteLine();
		}
		*/
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

		//Хотя не факт
		/* Убрать генерацию путей!!!!! 
	 * Заменить на функцию, которая будет по индексу возвращать направление!!!
	 * Не забыть учесть изменения в методе Prognosis */
		private static int NumberOfSteps(int x, int y, int j, int i)
		{
			int x_coord = hypothesis[0][i], y_coord = hypothesis[1][i], direction = hypothesis[2][i];
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
						if (x + 1 < height && _map[x, y, Down] == 0)// && CheckWalls(x, y + 1, Down))
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

		public static void SensorsRead(int x, int y, int direction)
		{
			switch (direction)
			{
				case Down:
					_sensors[2] = _map[x, y, 1];
					_sensors[3] = _map[x, y, 2];
					_sensors[0] = _map[x, y, 3];
					_sensors[1] = _map[x, y, 4];
					break;
				case Left:
					_sensors[2] = _map[x, y, 2];
					_sensors[3] = _map[x, y, 3];
					_sensors[0] = _map[x, y, 4];
					_sensors[1] = _map[x, y, 1];
					break;
				case Up:
					_sensors[0] = _map[x, y, 1];
					_sensors[1] = _map[x, y, 2];
					_sensors[2] = _map[x, y, 3];
					_sensors[3] = _map[x, y, 4];
					break;
				case Right:
					_sensors[0] = _map[x, y, 2];
					_sensors[1] = _map[x, y, 3];
					_sensors[2] = _map[x, y, 4];
					_sensors[3] = _map[x, y, 1];
					break;
				default:
					Console.WriteLine("SensorsRead ERROR");
					break;
			}
			/*
		Console.WriteLine("coord:" + x + " " + y);
		Console.WriteLine("sensors:" + _sensors[0] + " " + _sensors[1] + " " +
		                  _sensors[2] + " " + _sensors[3]);
		*/
		}

		private static void ListFiltration(ref List<List<int>> list)
		{
			for (int i = 0; i < list.Count; i++)
			{
				for (int j = i + 1; j < list.Count; j++)
				{
					var fl = true;
					for (int k = 0; k < list[i].Count && k < list[j].Count; k++)
					{
						if (list[i][k] != list[j][k]) fl = false;
					}
					if (fl)
					{
						if (list[i].Count == list[j].Count)
						{
							list.RemoveAt(j);
							--j;
						}
						else
						if (list[i].Count < list[j].Count)
						{
							list.RemoveAt(j);
							--j;
						}
						else
						{
							list.RemoveAt(i);
							i = j;
						}
					}
				}
			}
		}

	
	
		//number - номер шага(если считать начиная с 0)
		private static void Strategy(int number)
		{
			bool down = false, left = false, up = false, right = false;
			for (var i = 0; i < best_ways.Count; i++)
			{
				for (var j = i; j < best_ways.Count; j++)
				{
				
				}
			}
		}
	}
}