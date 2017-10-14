using System;
using System.Collections.Generic;

/*
	Добавить вариант: Если картины одинаковые при движении во все стороны, расчитать следующий
	шаг
*/
namespace Localization
{
	class Map
	{
		public int[,,] _map = new int[8, 8, 5]
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

		public int[] Sensors = new int[4];

		public const int Down = 1;
		public const int Left = 2;
		public const int Up = 3;
		public const int Right = 4;

		public int Height = 8;

		public int Wight = 8;

		private int //размеры карты (_map)
			_quantityOfHypothises = 0;

		public List<List<int>> Hypothesis = new List<List<int>>(); // x,y,direction

		private List<List<int>> _ways = new List<List<int>>();

		//way: x, y, directions
		public List<List<int>> BestWays = new List<List<int>>();

		private List<List<int>> _endOfWay = new List<List<int>>();
		//x,y, maybe else direction

		private int _quantityOfWays = 0;

		/**************************************************************************************/

		public void Localization()
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
			//Prognosis(10);
			//Way.CurentWayInit();
			//Way.curentWay.Add(3);
			//Way.curent_way.Add(2);
			//int k;
			/*
			k = Motion.GetNewDir(Left, Left);
			k = Motion.GetNewDir(Left, Right);
			k = Motion.GetNewDir(Right, Left);
			k = Motion.GetNewDir(Right, Right);
			*/
			var handingOfAllCases = new HandingOfAllCases();
			var way = new Way();
			//var Robot = new Robot();
			handingOfAllCases.Handing(this, way);

			ListFiltration(ref BestWays);
			PrintMap();
			var solution = new Solution();
			solution.ChooseWay(ref BestWays, this);
			
			for (var l = 0; l < BestWays.Count; l++)
			{
				Console.WriteLine("ways " + l);
				for (int i = 0; i < BestWays[l].Count; i++)
				{
					//Console.WriteLine("ways " + l);
					Console.Write(BestWays[l][i] + ",");
					//Console.WriteLine();
				}
				Console.WriteLine();
			}
		}

		private void MapInit()
		{

		}

		public void PrintMap()
		{
			int i, j, k = Hypothesis[0].Count, n = 0;

			for (var l = 0; l < Hypothesis[2].Count; ++l)
			{
				Console.Write(Hypothesis[0][l] + " "
				              + Hypothesis[1][l] + " " + Hypothesis[2][l] + "; ");
			}
			Console.WriteLine();
			for (i = 0; i < Height; ++i)
			{
				for (j = 0; j < Wight; ++j)
				{

					if (n < k && i == Hypothesis[0][n] && j == Hypothesis[1][n])
					{
						Console.Write(Hypothesis[2][n]);
						++n;
						while (n < k && Hypothesis[0][n] == Hypothesis[0][n - 1] &&
						       Hypothesis[1][n] == Hypothesis[1][n - 1])
							++n;
					}
					else Console.Write(".");
				}
				Console.WriteLine();
			}
		}

		public void PrintWays()
		{
			int i; //,l=quantityOfWays;
			for (i = 0; i < _ways.Count; ++i)
			{
				Console.Write(i + ". ");
				int n = _ways[i].Count;
				for (int j = 0; j < n; ++j)
				{
					Console.Write(_ways[i][j] + " ");
				}
				Console.WriteLine();
			}
		}

		private void StartInit()
		{
			for (int i = 0; i < 3; i++)
			{
				Hypothesis.Add(new List<int>());
			}
		}

		public void HypothesisInit()
		{
			_quantityOfHypothises = 0;
			var n = 0;
			for (var i = 0; i < Wight; i++)
			{
				for (var j = 0; j < Height; j++)
				{
					for (var k = 1; k < 5; k++)
					{
						Hypothesis[0].Add(i);
						Hypothesis[1].Add(j);
						Hypothesis[2].Add(k);
						++n;
						++_quantityOfHypothises;
					}
				}
			}
		}

		private void SensorsInit(int down, int left, int up, int right, Robot Robot)
		{
			Sensors[0] = down;
			Sensors[1] = left;
			Sensors[2] = up;
			Sensors[3] = right;
			Robot.SetSensors(Sensors);
			Robot.GetSensors(ref Sensors);
			/*
			Robot.Sensors = _sensors;
			_sensors = Robot.Sensors;
			*/
		}

		//сравнивает показания датчиков с полем на карте (сравнивает стены)
		//(робот двигался (x0,y0)->(x1,y1)
		private bool CheckWalls(int x, int y, int direction, Robot Robot)
		{
			//Robot.Sensors = _sensors;
			if (Robot.InitialDirection == 1)
			{
				if (direction > 2) direction -= 2;
				else direction += 2;
			}


			//_sensors = Robot.Sensors;
			if (direction == Down)
			{
				if (_map[x, y, 1] != Sensors[2]) return false;
				if (_map[x, y, 2] != Sensors[3]) return false;
				if (_map[x, y, 3] != Sensors[0]) return false;
				if (_map[x, y, 4] != Sensors[1]) return false;
				return true;
			}
			else if (direction == Left)
			{
				if (_map[x, y, 1] != Sensors[1]) return false;
				if (_map[x, y, 2] != Sensors[2]) return false;
				if (_map[x, y, 3] != Sensors[3]) return false;
				if (_map[x, y, 4] != Sensors[0]) return false;
				return true;
			}
			else if (direction == Up)
			{
				if (_map[x, y, 1] != Sensors[0]) return false;
				if (_map[x, y, 2] != Sensors[1]) return false;
				if (_map[x, y, 3] != Sensors[2]) return false;
				if (_map[x, y, 4] != Sensors[3]) return false;
				return true;
			}
			else
			{
				if (_map[x, y, 1] != Sensors[3]) return false;
				if (_map[x, y, 2] != Sensors[0]) return false;
				if (_map[x, y, 3] != Sensors[1]) return false;
				if (_map[x, y, 4] != Sensors[2]) return false;
				return true;
			}
		}

		// 0 <= quantity <= 4
		//Сюда ещё можно добавить расположение стен (| | или _|)
		public void Hypothesis1(int quantity)
		{
			int i, j;
			for (i = 0; i < Height; ++i)
			{
				for (j = 0; j < Wight; ++j)
				{
					if (_map[i, j, 0] == quantity)
					{
						Hypothesis[0].Add(i);
						Hypothesis[1].Add(j);
						Hypothesis[2].Add(3);

						_quantityOfHypothises++;
					}
				}
			}
			//передвигаемся прямо (если занято, то направо/налево/назад)
		}

		public void Hypothesis1New()
		{
			var quantity = Sensors[0] + Sensors[1] + Sensors[2] + Sensors[3];
			int i, x, y;
			for (i = 0; i < Hypothesis[0].Count; ++i)
			{
				x = Hypothesis[0][i];
				y = Hypothesis[1][i];
				if (_map[x, y, 0] != quantity)
				{
					Hypothesis[0].RemoveAt(i);
					Hypothesis[1].RemoveAt(i);
					Hypothesis[2].RemoveAt(i);
					i--;
				}
			}
			//передвигаемся прямо (если занято, то направо/налево/назад)
		}

		/*
		private void Hypothesis2()
		{
			int i, quantity = Sensors[0] + Sensors[1] + Sensors[2] + Sensors[3];
			for (i = 0; i < Hypothesis[0].Count; ++i)
			{
				var fl = true;
				int x = Hypothesis[0][i], y = Hypothesis[1][i];
				//to test code below
				//down
				Console.Write(x + " " + y + " " + Hypothesis[0].Count + ";   ");
				if (x + 1 < Height && _map[x + 1, y, 0] == quantity)
				{
					if (_map[x, y, Down] == 0 &&
					    CheckWalls(x + 1, y, Down))
					{
						++_quantityOfWays;
						//var x = ++start[2][i];
						Hypothesis[0][i]++;
						Hypothesis[2][i] = Down; //ToDownDir(hypothesis[2][i]);
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
						Hypothesis[1][i]--;
						Hypothesis[2][i] = Left; //ToLeftDir(hypothesis[2][i]);
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
						Hypothesis[0][i]--;
						Hypothesis[2][i] = Up;
						fl = false;
					}
				}

				//right
				if (y + 1 < Wight && _map[x, y + 1, 0] == quantity)
				{
					if (_map[x, y, Right] == 0 &&
					    CheckWalls(x, y + 1, Right))
					{
						++_quantityOfWays;
						//var x = ++start[2][i];
						Hypothesis[1][i]++;
						Hypothesis[2][i] = Right; //ToRightDir(hypothesis[2][i]);
						fl = false;
					}
				}
				if (fl)
				{
					Hypothesis[0].RemoveAt(i);
					Hypothesis[1].RemoveAt(i);
					Hypothesis[2].RemoveAt(i);
					i--;
					_quantityOfHypothises--;
				}
			}
			Console.WriteLine("eeee " + Hypothesis[0].Count + " " + Hypothesis[1].Count
			                  + " " + Hypothesis[2].Count);
		}
		*/
		//потестить ещё
		public void Hypothesis3(int direction, bool beginWay, Motion Motion, Robot Robot)
		{
			int i, quantity = Sensors[0] + Sensors[1] + Sensors[2] + Sensors[3];

			for (i = 0; i < Hypothesis[0].Count; ++i)
			{
				var newDir = Motion.GetNewDir(Hypothesis[2][i], direction, beginWay);
				var fl = true;
				switch (newDir)
				{
					case Down:
					{
						//int i, quantity = _sensors[0] + _sensors[1] + _sensors[2] + _sensors[3];
						//var fl = true;
						int x = Hypothesis[0][i], y = Hypothesis[1][i];

						if (x + 1 < Height && _map[x + 1, y, 0] == quantity)
						{
							if (_map[x, y, Down] == 0 &&
							    CheckWalls(x + 1, y, Down, Robot))
							{
								//++_quantityOfWays;
								//var x = ++start[2][i];
								Hypothesis[0][i]++;
								Hypothesis[2][i] = Down; //ToDownDir(hypothesis[2][i]);
								fl = false;
							}
						}
						break;
					}
					case Left:
					{
						//int i, quantity = _sensors[0] + _sensors[1] + _sensors[2] + _sensors[3];
						//var fl = true;
						int x = Hypothesis[0][i], y = Hypothesis[1][i];

						if (y > 0 && _map[x, y - 1, 0] == quantity)
						{
							if (_map[x, y, Left] == 0 && CheckWalls(x, y - 1, Left, Robot))
							{
								//++_quantityOfWays;
								//var x = ++start[2][i];
								Hypothesis[1][i]--;
								Hypothesis[2][i] = Left; //ToDownDir(hypothesis[2][i]);
								fl = false;
							}
						}
						break;
					}
					case Up:
					{
						//int i, quantity = _sensors[0] + _sensors[1] + _sensors[2] + _sensors[3];
						//var fl = true;
						int x = Hypothesis[0][i], y = Hypothesis[1][i];

						if (x > 0 && _map[x - 1, y, 0] == quantity)
						{
							if (_map[x, y, Up] == 0 && CheckWalls(x - 1, y, Up, Robot))
							{
								//++_quantityOfWays;
								//var x = ++start[2][i];
								Hypothesis[0][i]--;
								Hypothesis[2][i] = Up; //ToDownDir(hypothesis[2][i]);
								fl = false;
							}
						}
						break;
					}
					case Right:
					{
						//int i, quantity = _sensors[0] + _sensors[1] + _sensors[2] + _sensors[3];
						//var fl = true;
						int x = Hypothesis[0][i], y = Hypothesis[1][i];

						if (y + 1 < Wight && _map[x, y + 1, 0] == quantity)
						{
							if (_map[x, y, Right] == 0 && CheckWalls(x, y + 1, Right, Robot))
							{
								//++_quantityOfWays;
								//var x = ++start[2][i];
								Hypothesis[1][i]++;
								Hypothesis[2][i] = Right; //ToDownDir(hypothesis[2][i]);
								fl = false;
							}
						}
						break;
					}
				}
				if (fl)
				{
					Hypothesis[0].RemoveAt(i);
					Hypothesis[1].RemoveAt(i);
					Hypothesis[2].RemoveAt(i);
					i--;
					_quantityOfHypothises--;
				}
			}
		}

		private int ToRightDir(int direction)
		{
			if (direction < 4) return direction + 1;
			return 1;
		}

		//Возвращает направление, если поворачиваем в соотв. сторону
		private int ToLeftDir(int direction)
		{
			if (direction > 1) return direction - 1;
			return 4;
		}

		private int ToDownDir(int direction, Way way)
		{

			if (way.CurentWay.Count == 1)
			{
				if (direction > 2) return direction - 2;
				return direction + 2;
			}
			else return direction;
			/*
			if (direction > 2) return direction - 2;
			return direction + 2;
			*/
			Console.WriteLine("Map.ToDownDir - bag");
			return direction; //down == up
		}

		// текущее направление в абсолютных коорд/направление движения(куда едем?)
		public int ChooseDir(int currentDirection, int newDirection, Way way)
		{
			if (newDirection == Up) return currentDirection;
			if (newDirection == Right) return ToRightDir(currentDirection);
			if (newDirection == Left) return ToLeftDir(currentDirection);
			return ToDownDir(currentDirection, way);
		}

		/*У робота:
		в-1000
		п-100
		н-10
		л-1
		*/
		private void CheckIndications(int x, int y, int direction,
			ref List<List<int>> differentInd, int newDirection, Way way)
		{
			int value, //k=direction-1;
				k = ChooseDir(direction, newDirection, way) - 1;
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

		public void Copy_Lists(ref List<List<int>> to, List<List<int>> from)
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

		private void WaysInit(Int64 length)
		{
			length = (Int64) Math.Pow(4, length);
			for (var i = 0; i < length; ++i)
			{
				_ways.Add(new List<int>());
			}
		}

		private void GenerateWays(long length)
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
					_ways[i].Add(direction);
				}
				k /= 4;
				s = k;
			}
		}

		public void SensorsRead(int x, int y, int direction, Robot Robot)
		{
			switch (direction)
			{
				case Down:
					Sensors[2] = _map[x, y, 1];
					Sensors[3] = _map[x, y, 2];
					Sensors[0] = _map[x, y, 3];
					Sensors[1] = _map[x, y, 4];
					Robot.SetSensors(Sensors);
					Robot.GetSensors(ref Sensors);
					/*
					Robot.Sensors = _sensors;
					_sensors = Robot.Sensors;
					*/
					break;
				case Left:
					Sensors[2] = _map[x, y, 2];
					Sensors[3] = _map[x, y, 3];
					Sensors[0] = _map[x, y, 4];
					Sensors[1] = _map[x, y, 1];
					Robot.SetSensors(Sensors);
					Robot.GetSensors(ref Sensors);
					/*
					Robot.Sensors = _sensors;
					_sensors = Robot.Sensors;
					*/
					break;
				case Up:
					Sensors[0] = _map[x, y, 1];
					Sensors[1] = _map[x, y, 2];
					Sensors[2] = _map[x, y, 3];
					Sensors[3] = _map[x, y, 4];
					Robot.SetSensors(Sensors);
					Robot.GetSensors(ref Sensors);
					/*
					Robot.Sensors = _sensors;
					_sensors = Robot.Sensors;
					*/
					break;
				case Right:
					Sensors[0] = _map[x, y, 2];
					Sensors[1] = _map[x, y, 3];
					Sensors[2] = _map[x, y, 4];
					Sensors[3] = _map[x, y, 1];
					Robot.SetSensors(Sensors);
					Robot.GetSensors(ref Sensors);
					/*
					Robot.Sensors = _sensors;
					_sensors = Robot.Sensors;
					*/
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

		private void ListFiltration(ref List<List<int>> list)
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
						else if (list[i].Count < list[j].Count)
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
		private void Strategy(int number)
		{
			bool down = false, left = false, up = false, right = false;
			for (var i = 0; i < BestWays.Count; i++)
			{
				for (var j = i; j < BestWays.Count; j++)
				{

				}
			}
		}
	}
}