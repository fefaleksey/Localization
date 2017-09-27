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

    
	private static int height = 8, wight = 8, //размеры карты (_map)
		_lengthOfListStart = 0;

	//Переделать!!! Сделать 3 координату - направление робота
	private static List<List<int>> hypothesis = new List<List<int>>(); // x,y,quantity of ways in way

	private static List<List<int>> way = new List<List<int>>();

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
		StartInit();
		SensorsInit(1, 0, 0, 0); //Потом пригодится, когда доработаю Hypothesis1
		Hypothesis1(quantity);
		Console.WriteLine("Hypotsesis 1");
		PrintMap();
		//go in random direction
		SensorsInit(0, 1, 0, 0);
		Console.WriteLine("Hypotsesis 2");
		Hypothesis2();
		PrintMap();
		//PrintWays();
		Solution();
		SensorsInit(0, 1, 0, 1);
		Console.WriteLine("Hypotsesis 3");
		Hypothesis2();
		PrintMap();
		//PrintWays();
		Solution();
		SensorsInit(0, 0, 1, 0);
		Console.WriteLine("Hypotsesis 4");
		Hypothesis2();
		PrintMap();
		//PrintWays();
		Solution();
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
		
		for(var l=0;l<hypothesis[2].Count;++l)
		{
			Console.Write(hypothesis[0][l] + " " + hypothesis[1][l] + " " + hypothesis[2][l]+"; ");
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
		foreach (var k in hypothesis[2])
		{
			Console.Write(k + " ");
		}
	}

	private static void StartInit()
	{
		for (int i = 0; i < 3; i++)
		{
			hypothesis.Add(new List<int>());
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
			x = hypothesis[0][i];
			y = hypothesis[1][i];
			for (var j = 0; j < hypothesis[2][i]; ++j)
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

					_lengthOfListStart++;
				}
			}
		}
		//передвигаемся прямо (если занято, то направо/налево/назад)
	}


	private static void Hypothesis2()
	{
		int i, quantity = _sensors[0] + _sensors[1] + _sensors[2] + _sensors[3];
		for (i = 0; i < _lengthOfListStart; ++i)
		{
			var fl = true;
			int x=hypothesis[0][i], y=hypothesis[1][i];
			//to test code below
			//down
			Console.Write(x + " " + y + " " + _lengthOfListStart + ";   ");
			if (x + 1 < height && _map[x + 1, y, 0] == quantity)
			{
				if (_map[x, y, Down] == 0 &&
				    CheckWalls(x + 1, y, Down))
				{
					++_quantityOfWays;
					//var x = ++start[2][i];
					hypothesis[0][i]++;
					hypothesis[2][i] = Down;//ToDownDir(hypothesis[2][i]);
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
					hypothesis[2][i] = Left;//ToLeftDir(hypothesis[2][i]);
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
					hypothesis[2][i] = Right;//ToRightDir(hypothesis[2][i]);
					fl = false;
				}
			}
			if (fl)
			{
				hypothesis[0].RemoveAt(i);
				hypothesis[1].RemoveAt(i);
				hypothesis[2].RemoveAt(i);
				i--;
				_lengthOfListStart--;
			}
		}
		Console.WriteLine("eeee "+hypothesis[0].Count + " " + hypothesis[1].Count
		                  + " " + hypothesis[2].Count);
	}
/*
	private static void Hypothesis3(int direction)
	{
		int i, quantity = _sensors[0] + _sensors[1] + _sensors[2] + _sensors[3];
		switch (direction)
		{
			case Down:
			{
				//int i, quantity = _sensors[0] + _sensors[1] + _sensors[2] + _sensors[3];
				var fl = true;
				int x = hypothesis[0][i], y = hypothesis[1][i];
				for (i = 0; i < _lengthOfListStart; ++i)
				{
					if (x + 1 < height && _map[x + 1, y, 0] == quantity)
					{
						if (_map[x, y, Down] == 0 &&
						    CheckWalls(x + 1, y, Down))
						{
							++_quantityOfWays;
							//var x = ++start[2][i];
							hypothesis[0][i]++;
							hypothesis[2][i] = CheckDir(hypothesis[2][i],Down); //ToDownDir(hypothesis[2][i]);
							fl = false;
						}
					}
					
					
					
					
					//to test code below
					//down
					Console.Write(x + " " + y + " " + _lengthOfListStart + ";   ");
					if (x + 1 < height && _map[x + 1, y, 0] == quantity)
					{
						if (_map[x, y, Down] == 0 &&
						    CheckWalls(x + 1, y, Down))
						{
							way.Add(new List<int>());
							way[_quantityOfWays].Add(1);
							++_quantityOfWays;
							//var x = ++start[2][i];
							hypothesis[0][i]++;
							hypothesis[2][i] = CheckDir(hypothesis[2][i],Down); //ToDownDir(hypothesis[2][i]);
							fl = false;
						}
					}
				}
			}
					break;
			case Left:
			{
				//int i, quantity = _sensors[0] + _sensors[1] + _sensors[2] + _sensors[3];

				for (i = 0; i < _lengthOfListStart; ++i)
				{
					var fl = true;
					int x = hypothesis[0][i], y = hypothesis[1][i];
					//to test code below
					//down
					if (x + 1 < height && _map[x + 1, y, 0] == quantity)
					{
						if (_map[x, y, Left] == 0 &&
						    CheckWalls(x + 1, y, Left))
						{
							way.Add(new List<int>());
							way[_quantityOfWays].Add(1);
							++_quantityOfWays;
							//var x = ++start[2][i];
							hypothesis[0][i]++;
							hypothesis[2][i] = CheckDir(hypothesis[2][i],Down); //ToDownDir(hypothesis[2][i]);
							fl = false;
						}
					}
				}
			}
					break;
			case Up:
			{
				//int i, quantity = _sensors[0] + _sensors[1] + _sensors[2] + _sensors[3];

				for (i = 0; i < _lengthOfListStart; ++i)
				{
					var fl = true;
					int x = hypothesis[0][i], y = hypothesis[1][i];
					//to test code below
					//down
					Console.Write(x + " " + y + " " + _lengthOfListStart + ";   ");
					if (x + 1 < height && _map[x + 1, y, 0] == quantity)
					{
						if (_map[x, y, Down] == 0 &&
						    CheckWalls(x + 1, y, Down))
						{
							way.Add(new List<int>());
							way[_quantityOfWays].Add(1);
							++_quantityOfWays;
							//var x = ++start[2][i];
							hypothesis[0][i]++;
							hypothesis[2][i] = CheckDir(hypothesis[2][i],Down); //ToDownDir(hypothesis[2][i]);
							fl = false;
						}
					}
				}
			}
					break;
			case Right:
			{
				//int i, quantity = _sensors[0] + _sensors[1] + _sensors[2] + _sensors[3];

				for (i = 0; i < _lengthOfListStart; ++i)
				{
					var fl = true;
					int x = hypothesis[0][i], y = hypothesis[1][i];
					//to test code below
					//down
					Console.Write(x + " " + y + " " + _lengthOfListStart + ";   ");
					if (x + 1 < height && _map[x + 1, y, 0] == quantity)
					{
						if (_map[x, y, Down] == 0 &&
						    CheckWalls(x + 1, y, Down))
						{
							way.Add(new List<int>());
							way[_quantityOfWays].Add(1);
							++_quantityOfWays;
							//var x = ++start[2][i];
							hypothesis[0][i]++;
							hypothesis[2][i] = CheckDir(hypothesis[2][i],Down); //ToDownDir(hypothesis[2][i]);
							fl = false;
						}
					}
				}
			}
					break;
		}
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
	
	private static int ToDownDir(int direction)
	{
		if (direction > 2) return direction - 2;
		return direction + 2;
	}
	
	private static int CheckDir(int currentDirection, int direction)
	{
		if (direction == ToRightDir(currentDirection)) return Right;
		if (direction == ToLeftDir(currentDirection)) return Left;
		if (direction == ToDownDir(currentDirection)) return Down;
		return Up;
	}
	
	private static int ChooseBestDirection()
	{
		int i;
	
		List<List<int>> differentIndications = new List<List<int>>();
		for (i = 0; i < 5; ++i)
		{
			differentIndications.Add(new List<int>());

		}

		//List<List<int>> differentIndications = new List<List<int>>();
		
		
		for (i = 0; i < hypothesis[0].Count; ++i)
		{
			int x = hypothesis[0][i], y = hypothesis[1][i];
			if (_map[x, y, 1] == 0)
			{
				CheckIndications(x + 1, y, hypothesis[2][i]/*1*/,
					ref differentIndications,1);
			}

			if (_map[x, y, 2] == 0)
			{
				CheckIndications(x, y - 1, hypothesis[2][i]/*2*/,
					ref differentIndications,2);
			}

			if (_map[x, y, 3] == 0)
			{
				CheckIndications(x - 1, y, hypothesis[2][i]/*3*/,
					ref differentIndications,3);
			}

			if (_map[x, y, 4] == 0)
			{
				CheckIndications(x, y + 1, hypothesis[2][i]/*4*/,
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
		Console.WriteLine("Solution: "+k);
		return k;
	}
	/*У робота:
		в-1000
		п-100
		н-10
		л-1
	*/
	private static void CheckIndications  (	int x, int y, int direction,
											ref List<List<int>> differentInd,int newDirection )
	{
		int value, //k=direction-1;
			k = CheckDir(direction, newDirection)-1;
		switch (newDirection)
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
		
		//EndOfWayInit();
		int dir = ChooseBestDirection();
		
		return dir;
	}
}