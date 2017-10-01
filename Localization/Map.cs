﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
/*
	Добавить вариант: Если картины одинаковые при движении во все стороны, расчитать следующий
	шаг
*/
public class Map
{
	private static int[,,] _map = new int[8, 8, 5]
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

	/*
		= new int[3, 3, 5]
	{
		{{2, 0, 1, 1, 0}, {1, 0, 0, 1, 0}, {2, 0, 0, 1, 1}},
		{{1, 0, 1, 0, 0}, {1, 1, 0, 0, 0}, {1, 0, 0, 0, 1}},
		{{2, 1, 1, 0, 0}, {3, 1, 0, 1, 1}, {3, 1, 1, 0, 1}}
	};
	*/
	private static int[] _sensors = new int[4];

	private const int Down = 1, Left = 2, Up = 3, Right = 4;


	private static int height = 8,
		wight = 8, //размеры карты (_map)
		_quantityOfHypothises = 0;

	private static List<List<int>> hypothesis = new List<List<int>>(); // x,y,direction

	private static List<List<int>> ways = new List<List<int>>();

	//way: x, y, directions
	private static List<List<int>> best_ways = new List<List<int>>();

	private static List<List<int>> endOfWay = new List<List<int>>(); //x,y, maybe else direction

	private static int _quantityOfWays = 0;

	/******************************************************************************************/

	public static void Localization()
	{
		int quantity = 3;
		StartInit();
		//MapInit();
		//PrintMap();
		//quantity = 3;
		Console.WriteLine("localization");
		//StartInit();
		SensorsInit(1, 0, 0, 0); //Потом пригодится, когда доработаю Hypothesis1
		Hypothesis1(quantity);
		Console.WriteLine("Hypotsesis 1");
		PrintMap();
		//go in random direction
		SensorsInit(0, 1, 0, 0);
		Console.WriteLine("step 1");
		Hypothesis2();
		PrintMap();
		//PrintWays();
		Solution();
		SensorsInit(0, 1, 0, 1);
		Console.WriteLine("step 2");
		Hypothesis3(Right);
		PrintMap();
		//PrintWays();
		Solution();
		/*
		SensorsInit(0, 0, 1, 0);
		Console.WriteLine("step 3");
		Hypothesis3(Up);
		PrintMap();
		//PrintWays();
		Solution();
		/***********/ /*
 		SensorsInit(0, 0, 0, 1);
 		Console.WriteLine("step 4");
 		Hypothesis3(Right);
 		PrintMap();
 		//PrintWays();
 		Solution();
 		/**************/ /*
 		SensorsInit(0, 1, 0, 0);
 		Console.WriteLine("step 5");
 		Hypothesis3(Up);
 		PrintMap();
 		//PrintWays();
 		Solution();
 		/**********/ /*
 		SensorsInit(0, 0, 1, 1);
 		Console.WriteLine("step 6");
 		Hypothesis3(Up);
 		PrintMap();
 		//PrintWays();
 		Solution();
 		Console.WriteLine("***************************************");
 		//ways.Clear();
 		//WaysInit(6);
 		//GenerateWays(6);
 		//PrintWays();
 		*/
		Prognosis(4);
		//PrintWays();
		PrintMap();
	}

	/*
		private static void DirectionOfRotation(int direction)
		{
			switch (direction)
			{
				case 1:
				case 2:
				case 3:
					break;
				case 4:
						
			}
		}
		*/
	private static void MapInit()
	{
		/*
		int m, n;
		Console.WriteLine("The number of rows: ");
		n=Convert.ToInt32(Console.ReadLine());
		//Console.WriteLine("The number of columns: ");
		//n=Convert.ToInt32(Console.Read());
		for (var i = 0; i < n; ++i)
		{
			var s = Convert.ToString(Console.ReadLine());
			Console.WriteLine(s);
		}
		*/
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
		/*
		foreach (var k in hypothesis[2])
		{
			Console.Write(k + " ");
		}
		*/
	}

	private static void StartInit()
	{
		for (int i = 0; i < 3; i++)
		{
			hypothesis.Add(new List<int>());
		}
	}

/*
	private static void BestWaysInit()
	{
		for (int i = 0; i < 4; i++)
		{
			
		}
	}
*/
	private static void SensorsInit(int down, int left, int up, int right)
	{
		_sensors[0] = down;
		_sensors[1] = left;
		_sensors[2] = up;
		_sensors[3] = right;
	}


	public static void EndOfWayInit()
	{
		int x, y, k = 0, numberOfWay = 0;
		for (var i = 0; i < 3; ++i)
		{
			endOfWay.Add(new List<int>());
		}
		for (var i = 0; i < _quantityOfHypothises; ++i)
		{
			x = hypothesis[0][i];
			y = hypothesis[1][i];
			for (var j = 0; j < hypothesis[2][i]; ++j)
			{
				for (k = 0; k < ways[numberOfWay].Count; ++k)
				{
					switch (ways[numberOfWay][k])
					{
						case 1:
							++x;
							break;
						case 2:
							--y;
							break;
						case 3:
							--x;
							break;
						case 4:
							++y;
							break;
					}
				}
				--k;
				Console.Write("EndOfWayInit \nCount:" + ways[numberOfWay].Count);
				endOfWay[0].Add(x);
				endOfWay[1].Add(y);
				endOfWay[2].Add(ways[numberOfWay][k]);
				Console.WriteLine("coord:" + x + " " + y);
				Console.WriteLine("  end direction:" + endOfWay[2][k]);
				++numberOfWay;
			}
		}

		endOfWay[2][0] = 4;
		endOfWay[2][1] = 1;
		endOfWay[2][2] = 3;
		Console.WriteLine("EndOfWayInit2 \n" + endOfWay[2][0] + " "
		                  + endOfWay[2][1] + " " + endOfWay[2][2]);
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
		for (i = 0; i < _quantityOfHypothises; ++i)
		{
			var fl = true;
			int x = hypothesis[0][i], y = hypothesis[1][i];
			//to test code below
			//down
			Console.Write(x + " " + y + " " + _quantityOfHypothises + ";   ");
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
	private static void Hypothesis3(int direction)
	{
		int i, quantity = _sensors[0] + _sensors[1] + _sensors[2] + _sensors[3];

		for (i = 0; i < _quantityOfHypothises; ++i)
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
				/*
				default:
				{
					hypothesis[0].RemoveAt(i);
					hypothesis[1].RemoveAt(i);
					hypothesis[2].RemoveAt(i);
					i--;
					_lengthOfListStart--;
					break;
				}
				*/
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
	private static int ChooseDir(int currentDirection, int directionOfTravel)
	{
		if (directionOfTravel == Up) return currentDirection;
		if (directionOfTravel == Right) return ToRightDir(currentDirection);
		if (directionOfTravel == Left) return ToLeftDir(currentDirection);
		return ToDownDir(currentDirection);

		/*
		if (directionOfTravel == ToRightDir(currentDirection)) return Right;
		if (directionOfTravel == ToLeftDir(currentDirection)) return Left;
		if (directionOfTravel == ToDownDir(currentDirection)) return Down;
		return currentDirection;
		*/
	}

	private static int ChooseBestDirection()
	{
		int i;

		List<List<int>> differentIndications = new List<List<int>>();
		for (i = 0; i < 5; ++i)
		{
			differentIndications.Add(new List<int>());

		}

		for (i = 0; i < hypothesis[0].Count; ++i)
		{
			int x = hypothesis[0][i], y = hypothesis[1][i];
			if (_map[x, y, 1] == 0)
			{
				CheckIndications(x + 1, y, hypothesis[2][i] /*1*/,
					ref differentIndications, 1);
			}

			if (_map[x, y, 2] == 0)
			{
				CheckIndications(x, y - 1, hypothesis[2][i] /*2*/,
					ref differentIndications, 2);
			}

			if (_map[x, y, 3] == 0)
			{
				CheckIndications(x - 1, y, hypothesis[2][i] /*3*/,
					ref differentIndications, 3);
			}

			if (_map[x, y, 4] == 0)
			{
				CheckIndications(x, y + 1, hypothesis[2][i] /*4*/,
					ref differentIndications, 4);
			}
		}
		//Работает не правильно! Исправить
		Console.WriteLine("final dir:" + differentIndications[0].Count
		                  + differentIndications[1].Count
		                  + differentIndications[2].Count + differentIndications[3].Count);

		int max = differentIndications[0].Count, k = 1;
		if (differentIndications[1].Count > max)
		{
			max = differentIndications[1].Count;
			k = 2;
		}
		if (differentIndications[2].Count > max)
		{
			max = differentIndications[2].Count;
			k = 3;
		}
		if (differentIndications[3].Count > max)
		{
			max = differentIndications[3].Count;
			k = 4;
		}
		Console.WriteLine("Solution: " + k);
		return k;
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
				Console.WriteLine("OOOOOPPPSSSSS");
				break;
		}
		/*
		Console.WriteLine("kek");
		Console.Write(differentInd[0].Count);
		Console.Write(differentInd[1].Count);
		Console.Write(differentInd[2].Count);
		Console.Write(differentInd[3].Count);
		Console.WriteLine();
		*/
	}

	private static int Solution()
	{
		int k = 0, quantity = 1, i;

		while (quantity == 1)
		{
			quantity = 0;
			for (i = 0; i < 4; ++i)
				if (_sensors[i] == 0)
				{
					++quantity;
					k = i;
				}
			if (quantity == 1)
			{
				//проехать одну клетку в данном направлении
				//добавить перемещение в путь
				//cчитать показания с сенсоров
				//фильтрануть карту
			}
		}

		//EndOfWayInit();
		int dir = ChooseBestDirection();

		return dir;
	}

	private static void Copy_Lists(ref List<List<int>> to, List<List<int>> from)
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


		for (int i = 0; i < hypothesis.Count; i++)
		{
			copy_hypothesis.Add(new List<int>());
		}


		Copy_Lists(ref copy_hypothesis, hypothesis);

		int n = _quantityOfHypothises;

		ways.Clear();
		WaysInit(lengthOfWay);
		GenerateWays(lengthOfWay);

		for (var i = 0; i < n; ++i)
		{
			for (int j = 0; j < ways.Count; j++)
			{
				int x = hypothesis[0][i], y = hypothesis[1][i];
				//GenerateWays(hypothesis[0][i], hypothesis[1][i],6);
				_quantityOfHypothises = hypothesis[0].Count;
				ways.Clear();
				WaysInit(lengthOfWay);
				GenerateWays(lengthOfWay);
				NumberOfSteps(x, y, j, i);

				hypothesis[0].Clear();
				hypothesis[1].Clear();
				hypothesis[2].Clear();
				Copy_Lists(ref hypothesis, copy_hypothesis);
			}
		}


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
		return 0;
	}

	private static void WaysInit(int length)
	{
		length = (int) Math.Pow(4, length);
		for (var i = 0; i < length; ++i)
		{
			ways.Add(new List<int>());
		}
	}

	private static void GenerateWays(int length)
	{
		length = (int) Math.Pow(4, length);
		int k = length / 4, direction = 1, s = k;
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

	private static int NumberOfSteps(int x, int y, int j, int i)
	{
		int x_coord = hypothesis[0][i], y_coord = hypothesis[1][i];
		//Console.WriteLine("Number of steps");
		for (int k = 0; k < ways[j].Count; k++)
		{
			bool fl = true;
			int a = hypothesis[2][i], b = ways[j][k];
			var newDir = ChooseDir(hypothesis[2][i], ways[j][k]);
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
				for (var l = 0; l <= k; l++)
				{
					best_ways[n].Add(ways[j][l]);
					Console.Write(best_ways[n][l] + ",");
				}
				Console.WriteLine(" quantity of steps: " + k);
				return k;
			}
		}
		Console.WriteLine("HC: " + hypothesis[0].Count);
		return hypothesis[0].Count;
	}

	private static void SensorsRead(int x, int y, int direction)
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
		Console.WriteLine("coord:" + x + " " + y);
		Console.WriteLine("sensors:" + _sensors[0] + " " + _sensors[1] + " " +
		                  _sensors[2] + " " + _sensors[3]);
	}

	//to do
	private static void BestWaysFiltration()
	{
		for (int i = 0; i < best_ways.Count; i++)
		{
			for (int j = 0; j < best_ways[i].Count; j++)
			{

			}
		}
	}

}