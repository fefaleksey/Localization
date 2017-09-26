using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
/*
	Добавить вариант: Если картины одинаковые при движении во все стороны, расчитать следующий
	шаг
*/
public class Map
{
	private static int[,,] _map 	= new int[8, 8, 5]{
										{{2,0,1,1,0},{2,1,0,1,0},{1,0,0,1,0},{2,1,0,1,0},
											{2,1,0,1,0},{2,0,0,1,1},{4,1,1,1,1},{3,0,1,1,1}},
														
										{{2,0,1,0,1},{4,1,1,1,1},{2,0,1,0,1},{3,0,1,1,1},
											{4,1,1,1,1},{1,0,1,0,0},{2,1,0,1,0},{2,1,0,0,1}},
														
										{{2,1,1,0,0},{2,1,0,1,0},{1,0,0,0,1},{1,0,1,0,0},
											{2,1,0,1,0},{1,0,0,0,1},{4,1,1,1,1},{3,0,1,1,1}},
														
										{{3,0,1,1,1},{4,1,1,1,1},{2,0,1,0,1},{3,1,1,0,1},
											{4,1,1,1,1},{1,0,1,0,0},{2,1,0,1,0},{1,0,0,0,1}},
														
										{{1,0,1,0,0},{2,1,0,1,0},{0,0,0,0,0},{2,1,0,1,0},
											{1,0,0,1,0},{1,0,0,0,1},{4,1,1,1,1},{3,1,1,0,1}},
														
										{{3,1,1,0,1},{4,1,1,1,1},{2,0,1,0,1},{4,1,1,1,1},
											{2,1,1,0,0},{0,0,0,0,0},{2,1,0,1,0},{2,0,0,1,1}},
														
										{{2,0,1,1,0},{2,1,0,1,0},{0,0,0,0,0},{2,0,0,1,1},
											{4,1,1,1,1},{3,1,1,0,1},{4,1,1,1,1},{2,0,1,0,1}},
														
										{{3,1,1,0,1},{4,1,1,1,1},{2,1,1,0,0},{1,1,0,0,0},
											{2,1,0,1,0},{2,1,0,1,0},{2,1,0,1,0},{2,1,0,0,1}}};
										
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

    
	private static int height = 3,
		wight = 3, //размеры карты (_map)
		_lengthOfListStart = 0;

	private static List<List<int>> start = new List<List<int>>(); // x,y,quantity of ways in way

	private static List<List<int>> way = new List<List<int>>();

	private static List<List<int>> endOfWay = new List<List<int>>(); //x,y, maybe else direction

	private static int _quantityOfWays = 0;

	/******************************************************************************************/

	public static void Localization(int quantity)
	{
		StartInit();
		//MapInit();
		//PrintMap();
		//quantity = 3;
		Console.WriteLine("localization");
		StartInit();
		SensorsInit(1, 1, 0, 0); //Потом пригодится, когда доработаю Hypothesis1
		Hypothesis1(quantity);
		//go in random direction
		SensorsInit(0, 1, 0, 0);
		Hypothesis2();
		PrintMap();
		PrintWays();
		Solution();
	}

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
		int i, j, k = start[0].Count, n = 0;
		for (i = 0; i < height; ++i)
		{
			for (j = 0; j < wight; ++j)
			{

				if (n < k && i == start[0][n] && j == start[1][n])
				{
					Console.Write(_map[i, j, 0]);
					++n;
				}
				else Console.Write("0");
			}
			Console.WriteLine();
		}
	}

	public static void PrintWays()
	{
		int i; //,l=quantityOfWays;
		for (i = 0; i < _quantityOfWays; ++i)
		{
			Console.Write(i + ". ");
			int n = way[i].Count;
			for (int j = 0; j < n; ++j)
			{
				Console.Write(way[i][j] + " ");
			}
			Console.WriteLine();
		}
		foreach (var k in start[2])
		{
			Console.Write(k + " ");
		}
	}

	private static void StartInit()
	{
		for (int i = 0; i < 3; i++)
		{
			start.Add(new List<int>());
		}
	}

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
		for (var i = 0; i < _lengthOfListStart; ++i)
		{
			x = start[0][i];
			y = start[1][i];
			for (var j = 0; j < start[2][i]; ++j)
			{
				for (k = 0; k < way[numberOfWay].Count; ++k)
				{
					switch (way[numberOfWay][k])
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
				Console.Write("EndOfWayInit \nCount:" + way[numberOfWay].Count);
				endOfWay[0].Add(x);
				endOfWay[1].Add(y);
				endOfWay[2].Add(way[numberOfWay][k]);
				Console.WriteLine("coord:"+ x + " " + y);
				Console.WriteLine("  end direction:"+endOfWay[2][k]);
				++numberOfWay;
			}
		}
		
		endOfWay[2][0] = 4;
		endOfWay[2][1] = 1;
		endOfWay[2][2] = 3;
		Console.WriteLine("EndOfWayInit2 \n" +endOfWay[2][0]+" "+endOfWay[2][1]+" "+endOfWay[2][2]);
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
					start[0].Add(i);
					start[1].Add(j);
					start[2].Add(0);

					_lengthOfListStart++;
				}
			}
		}
		//передвигаемся прямо (если занято, то направо/налево/назад)
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

	//Проследить за корректностью размера массива(должен быть 4)
	private static void Hypothesis2()
	{
		int i, quantity = _sensors[0] + _sensors[1] + _sensors[2] + _sensors[3];
		for (i = 0; i < _lengthOfListStart; ++i)
		{
			var fl = true;
			//to test code below
			//down
			Console.Write(start[0][i] + " " + start[1][i] + " " + _lengthOfListStart + ";   ");
			if (start[0][i] + 1 < height && _map[start[0][i] + 1, start[1][i], 0] == quantity)
			{
				if (_map[start[0][i], start[1][i], Down] == 0 &&
				    CheckWalls(start[0][i] + 1, start[1][i], Down))
				{
					way.Add(new List<int>());
					way[_quantityOfWays].Add(1);
					++_quantityOfWays;
					var x = ++start[2][i];
					fl = false;
				}
			}

			//left
			if (start[1][i] > 0 && _map[start[0][i], start[1][i] - 1, 0] == quantity)
			{
				if (_map[start[0][i], start[1][i], Left] == 0 &&
				    CheckWalls(start[0][i], start[1][i] - 1, Left))
				{
					way.Add(new List<int>());
					way[_quantityOfWays].Add(2);
					++_quantityOfWays;
					var x = ++start[2][i];
					fl = false;
				}
			}

			//up
			if (start[0][i] > 0 && _map[start[0][i] - 1, start[1][i], 0] == quantity)
			{
				if (_map[start[0][i], start[1][i], Up] == 0 &&
				    CheckWalls(start[0][i] - 1, start[1][i], Up))
				{
					way.Add(new List<int>());
					way[_quantityOfWays].Add(3);
					++_quantityOfWays;
					var x = ++start[2][i];
					fl = false;
				}
			}

			//right
			if (start[1][i] + 1 < wight && _map[start[0][i], start[1][i] + 1, 0] == quantity)
			{
				if (_map[start[0][i], start[1][i], Right] == 0 &&
				    CheckWalls(start[0][i], start[1][i] + 1, Right))
				{
					way.Add(new List<int>());
					way[_quantityOfWays].Add(4);
					++_quantityOfWays;
					var x = ++start[2][i];
					fl = false;
				}
			}
			if (fl)
			{
				start[0].RemoveAt(i);
				start[1].RemoveAt(i);
				start[2].RemoveAt(i);
				i--;
				_lengthOfListStart--;
			}
		}
		Console.WriteLine();
	}

	/*
	private static int NumberOfStepsForLocalization(int direction)
	{
		int i;
		int[,] start_copy = new int[start.Count,start[0].Count];
		int[,] endOfWay_copy = new int[endOfWay.Count, endOfWay[0].Count];
		
	}
	*/
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
	
	private static int ToRoundDir(int direction)
	{
		if (direction > 2) return direction - 2;
		return direction + 2;
	}
	
	private static int CheckDir(int currentDirection, int direction)
	{
		if (direction == ToRightDir(currentDirection)) return Right;
		if (direction == ToLeftDir(currentDirection)) return Left;
		if (direction == ToRoundDir(currentDirection)) return Down;
		return Up;
	}
	
	private static int ChooseBestDirection()
	{
		int i; //, n = endOfWay[0].Count;
		/*
		int[,] start_copy = new int[start.Count,start[0].Count];
		int[,] endOfWay_copy = new int[endOfWay.Count, endOfWay[0].Count];
		*/

		List<List<int>> differentIndications = new List<List<int>>();
		for (i = 0; i < 5; ++i)
		{
			differentIndications.Add(new List<int>());

		}

		//List<List<int>> differentIndications = new List<List<int>>();
		
		
		for (i = 0; i < endOfWay[0].Count; ++i)
		{
			int x = endOfWay[0][i], y = endOfWay[1][i];
			if (_map[endOfWay[0][i], endOfWay[1][i], 1] == 0)
			{
				CheckIndications(endOfWay[0][i] + 1, endOfWay[1][i], endOfWay[2][i]/*1*/,
					ref differentIndications,1);
			}

			if (_map[endOfWay[0][i], endOfWay[1][i], 2] == 0)
			{
				CheckIndications(endOfWay[0][i], endOfWay[1][i] - 1, endOfWay[2][i]/*2*/,
					ref differentIndications,2);
			}

			if (_map[endOfWay[0][i], endOfWay[1][i], 3] == 0)
			{
				CheckIndications(endOfWay[0][i] - 1, endOfWay[1][i], endOfWay[2][i]/*3*/,
					ref differentIndications,3);
			}

			if (_map[endOfWay[0][i], endOfWay[1][i], 4] == 0)
			{
				CheckIndications(endOfWay[0][i], endOfWay[1][i] + 1, endOfWay[2][i]/*4*/,
					ref differentIndications,4);
			}
		}
		//Работает не правильно! Исправить
		Console.WriteLine("final dir:" + differentIndications[0].Count + differentIndications[1].Count
		                  +differentIndications[2].Count+differentIndications[3].Count);
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
		Console.WriteLine("oookkokkok "+k);
		return k;
	}
	/*У робота:
		в-1000
		п-100
		н-10
		л-1
	*/
	private static void CheckIndications  (	int x, int y, int direction,
											ref List<List<int>> differentInd,int goToDirection )
	{
		int value, //k=direction-1;
			k = CheckDir(direction, goToDirection)-1;
		switch (goToDirection)
		{
			case Down:
				value = 1000 * _map[x, y, 1] + 100 * _map[x, y, 2] +
				        10 * _map[x, y, 3] + _map[x, y, 4];
				if (!differentInd[k].Contains(value)) differentInd[k].Add(value);//0
				break;
			case Left:
				value = 1000 * _map[x, y, 2] + 100 * _map[x, y, 3] +
				        10 * _map[x, y, 4] + _map[x, y, 1];
				if (!differentInd[k].Contains(value)) differentInd[k].Add(value);//1
				break;
			case Up:
				value = 1000 * _map[x, y, 3] + 100 * _map[x, y, 4] +
				        10 * _map[x, y, 1] + _map[x, y, 2];
				if (!differentInd[k].Contains(value)) differentInd[k].Add(value);//2
				break;
			case Right:
				value = 1000 * _map[x, y, 4] + 100 * _map[x, y, 1] +
				        10 * _map[x, y, 2] + _map[x, y, 3];
				if (!differentInd[k].Contains(value)) differentInd[k].Add(value);//3
				break;
			default:
				Console.WriteLine("OOOOOPPPSSSSS");
				break;
		}
		Console.WriteLine("kek");
		Console.Write(differentInd[0].Count);
		Console.Write(differentInd[1].Count);
		Console.Write(differentInd[2].Count);
		Console.Write(differentInd[3].Count);
		Console.WriteLine();
		
		/*
		if (direction == Down)
		{
			value = 1000 * _map[x, y, 3] + 100 * _map[x, y, 4] +
			        10 * _map[x, y, 1] + _map[x, y, 2];
			if (!differentInd[0].Contains(value)) differentInd[0].Add(value);
		}
		else if (direction == Left)
		{
			value = 1000 * _map[x, y, 4] + 100 * _map[x, y, 1] +
			        10 * _map[x, y, 2] + _map[x, y, 3];
			if (!differentInd[1].Contains(value)) differentInd[1].Add(value);
		}
		else if (direction == Up)
		{
			value = 1000 * _map[x, y, 1] + 100 * _map[x, y, 2] +
			        10 * _map[x, y, 3] + _map[x, y, 4];
			if (!differentInd[2].Contains(value)) differentInd[2].Add(value);
		}
		else
		{
			value = 1000 * _map[x, y, 2] + 100 * _map[x, y, 3] +
			        10 * _map[x, y, 4] + _map[x, y, 1];
			if (!differentInd[3].Contains(value)) differentInd[3].Add(value);
		}
		*/
	}

	private static int Solution()
	{
		int k = 0, quantity = 1, i;
		
		while(quantity==1)
		{
			quantity = 0;
			for(i = 0; i < 4; ++i)
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
		
		EndOfWayInit();
		int dir = ChooseBestDirection();
		
		return dir;
	}
}