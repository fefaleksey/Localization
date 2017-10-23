using System;
using System.Collections.Generic;

namespace Localization
{
	class Map
	{
		public int[,,] map = new int[8, 8, 5]
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

		public List<List<int>> Hypothesis = new List<List<int>>(); // x,y,direction

		//way: x, y, directions
		public List<List<int>> BestWays = new List<List<int>>();

		//x,y, maybe else direction

		/**************************************************************************************/

		public void Localization()
		{
			StartInit();

			var handingOfAllCases = new HandingOfAllCases();
			var way = new Way();
			//var Robot = new Robot();
			handingOfAllCases.Handing(this, way);

			ListFiltration(ref BestWays);
			//PrintMap();
			var solution = new Solution();
			var map = this;
			var finalWays = new FinalWays();
			/*
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
			*/
			//solution.GetWays(ref map, ref BestWays, ref finalWays);
			var solutionForRobot = new SolutionForRobot();
			solutionForRobot.SimulationOfLocalization(ref map, ref BestWays, ref finalWays);
			var ruleOfOneHand = new RuleOfTheRightAndLeftHand();
			ruleOfOneHand.SimulationOfLocalization(ref map, ref finalWays, true);
			ruleOfOneHand.SimulationOfLocalization(ref map, ref finalWays, false);
			finalWays.PrintResult();
			//test.TimeOfFinalWays(ref finalWays, ref map, robot);
		}

		public void HypothesisFilter() //, int step 
		{
			HypothesisInit();
			Hypothesis1New();
			var robot = new Robot {InitialDirection = 3};
			for (var i = 0; i < Hypothesis[0].Count; i++)
			{
				int x = Hypothesis[0][i],
					y = Hypothesis[1][i],
					direction = Hypothesis[2][i];
				if (!CheckWalls(x, y, direction, robot))
				{
					Hypothesis[0].RemoveAt(i);
					Hypothesis[1].RemoveAt(i);
					Hypothesis[2].RemoveAt(i);
					i--;
				}
			}
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

		private void StartInit()
		{
			for (int i = 0; i < 3; i++)
			{
				Hypothesis.Add(new List<int>());
			}
		}

		public void HypothesisInit()
		{
			Hypothesis.Clear();
			StartInit();
			for (var i = 0; i < Wight; i++)
			{
				for (var j = 0; j < Height; j++)
				{
					for (var k = 1; k < 5; k++)
					{
						Hypothesis[0].Add(i);
						Hypothesis[1].Add(j);
						Hypothesis[2].Add(k);
					}
				}
			}
		}

		private void SensorsInit(int down, int left, int up, int right, Robot robot)
		{
			Sensors[0] = down;
			Sensors[1] = left;
			Sensors[2] = up;
			Sensors[3] = right;
			robot.SetSensors(Sensors);
			robot.GetSensors(ref Sensors);
			/*
			Robot.Sensors = _sensors;
			_sensors = Robot.Sensors;
			*/
		}

		//сравнивает показания датчиков с полем на карте (сравнивает стены)
		//(робот двигался (x0,y0)->(x1,y1)
		public bool CheckWalls(int x, int y, int direction, Robot robot)
		{
			//Robot.Sensors = _sensors;
			if (robot.InitialDirection == 1)
			{
				if (direction > 2) direction -= 2;
				else direction += 2;
			}


			//_sensors = Robot.Sensors;
			if (direction == Down)
			{
				if (map[x, y, 1] != Sensors[2]) return false;
				if (map[x, y, 2] != Sensors[3]) return false;
				if (map[x, y, 3] != Sensors[0]) return false;
				if (map[x, y, 4] != Sensors[1]) return false;
				return true;
			}
			else if (direction == Left)
			{
				if (map[x, y, 1] != Sensors[1]) return false;
				if (map[x, y, 2] != Sensors[2]) return false;
				if (map[x, y, 3] != Sensors[3]) return false;
				if (map[x, y, 4] != Sensors[0]) return false;
				return true;
			}
			else if (direction == Up)
			{
				if (map[x, y, 1] != Sensors[0]) return false;
				if (map[x, y, 2] != Sensors[1]) return false;
				if (map[x, y, 3] != Sensors[2]) return false;
				if (map[x, y, 4] != Sensors[3]) return false;
				return true;
			}
			else
			{
				if (map[x, y, 1] != Sensors[3]) return false;
				if (map[x, y, 2] != Sensors[0]) return false;
				if (map[x, y, 3] != Sensors[1]) return false;
				if (map[x, y, 4] != Sensors[2]) return false;
				return true;
			}
		}

		// 0 <= quantity <= 4
		//Сюда ещё можно добавить расположение стен (| | или _|)
		public void Hypothesis1(int quantity)
		{
			for (var i = 0; i < Height; ++i)
			{
				for (var j = 0; j < Wight; ++j)
				{
					if (map[i, j, 0] == quantity)
					{
						Hypothesis[0].Add(i);
						Hypothesis[1].Add(j);
						Hypothesis[2].Add(3);
					}
				}
			}
			//передвигаемся прямо (если занято, то направо/налево/назад)
		}

		public void Hypothesis1New()
		{
			var quantity = Sensors[0] + Sensors[1] + Sensors[2] + Sensors[3];
			for (var i = 0; i < Hypothesis[0].Count; ++i)
			{
				var x = Hypothesis[0][i];
				var y = Hypothesis[1][i];
				if (map[x, y, 0] != quantity)
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
		public void Hypothesis3(int direction, bool beginWay, Motion motion, Robot robot)
		{
			int i, quantity = Sensors[0] + Sensors[1] + Sensors[2] + Sensors[3];

			for (i = 0; i < Hypothesis[0].Count; ++i)
			{
				var newDir = motion.GetNewDir(Hypothesis[2][i], direction, beginWay);
				var fl = true;
				switch (newDir)
				{
					case Down:
					{
						//int i, quantity = _sensors[0] + _sensors[1] + _sensors[2] + _sensors[3];
						//var fl = true;
						int x = Hypothesis[0][i], y = Hypothesis[1][i];

						if (x + 1 < Height && map[x + 1, y, 0] == quantity)
						{
							if (map[x, y, Down] == 0 &&
							    CheckWalls(x + 1, y, Down, robot))
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

						if (y > 0 && map[x, y - 1, 0] == quantity)
						{
							if (map[x, y, Left] == 0 && CheckWalls(x, y - 1, Left, robot))
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

						if (x > 0 && map[x - 1, y, 0] == quantity)
						{
							if (map[x, y, Up] == 0 && CheckWalls(x - 1, y, Up, robot))
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

						if (y + 1 < Wight && map[x, y + 1, 0] == quantity)
						{
							if (map[x, y, Right] == 0 && CheckWalls(x, y + 1, Right, robot))
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
				/*
				if (!fl && Robot.InitialDirection==1)
				{
					if (Hypothesis[2][i] > 2) Hypothesis[2][i] -= 2;
					else Hypothesis[2][i] += 2;
				}
				*/
				if (fl)
				{
					Hypothesis[0].RemoveAt(i);
					Hypothesis[1].RemoveAt(i);
					Hypothesis[2].RemoveAt(i);
					i--;
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
					value = 1000 * map[x, y, 1] + 100 * map[x, y, 2] +
					        10 * map[x, y, 3] + map[x, y, 4];
					if (!differentInd[k].Contains(value)) differentInd[k].Add(value); //0
					break;
				case Left:
					value = 1000 * map[x, y, 2] + 100 * map[x, y, 3] +
					        10 * map[x, y, 4] + map[x, y, 1];
					if (!differentInd[k].Contains(value)) differentInd[k].Add(value); //1
					break;
				case Up:
					value = 1000 * map[x, y, 3] + 100 * map[x, y, 4] +
					        10 * map[x, y, 1] + map[x, y, 2];
					if (!differentInd[k].Contains(value)) differentInd[k].Add(value); //2
					break;
				case Right:
					value = 1000 * map[x, y, 4] + 100 * map[x, y, 1] +
					        10 * map[x, y, 2] + map[x, y, 3];
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
			to.Clear();
			for (var i = 0; i < m; i++)
			{
				to.Add(new List<int>());
			}

			for (var i = 0; i < n; ++i)
			{
				to[0].Add(from[0][i]);
				to[1].Add(from[1][i]);
				to[2].Add(from[2][i]);
			}
			//int[,,] copy_hypothesis = new int[n, n, n];
		}

		public void SensorsRead(int x, int y, int direction, Robot robot)
		{
			/*
			if (beginWay && robot.InitialDirection == 1)
			{
				if (direction > 2)
				{
					direction -= 2;
				}
				else direction += 2;
			}
			*/
			switch (direction)
			{
				case Down:
					Sensors[2] = map[x, y, 1];
					Sensors[3] = map[x, y, 2];
					Sensors[0] = map[x, y, 3];
					Sensors[1] = map[x, y, 4];
					robot.SetSensors(Sensors);
					robot.GetSensors(ref Sensors);
					/*
					Robot.Sensors = _sensors;
					_sensors = Robot.Sensors;
					*/
					break;
				case Left:
					Sensors[2] = map[x, y, 2];
					Sensors[3] = map[x, y, 3];
					Sensors[0] = map[x, y, 4];
					Sensors[1] = map[x, y, 1];
					robot.SetSensors(Sensors);
					robot.GetSensors(ref Sensors);
					/*
					Robot.Sensors = _sensors;
					_sensors = Robot.Sensors;
					*/
					break;
				case Up:
					Sensors[0] = map[x, y, 1];
					Sensors[1] = map[x, y, 2];
					Sensors[2] = map[x, y, 3];
					Sensors[3] = map[x, y, 4];
					robot.SetSensors(Sensors);
					robot.GetSensors(ref Sensors);
					/*
					Robot.Sensors = _sensors;
					_sensors = Robot.Sensors;
					*/
					break;
				case Right:
					Sensors[0] = map[x, y, 2];
					Sensors[1] = map[x, y, 3];
					Sensors[2] = map[x, y, 4];
					Sensors[3] = map[x, y, 1];
					robot.SetSensors(Sensors);
					robot.GetSensors(ref Sensors);
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
	}
}