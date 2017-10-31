using System;
using System.Collections.Generic;

namespace Localization
{
	public class Map
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

		public const int Height = 8;

		public const int Width = 8;

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

			var generate = new Generate();
			generate.GenerateHashtable(finalWays);
			
			
			ruleOfOneHand.SimulationOfLocalization(ref map, ref finalWays, true);
			ruleOfOneHand.SimulationOfLocalization(ref map, ref finalWays, false);

			finalWays.PrintResult();
			//test.TimeOfFinalWays(ref finalWays, ref map, robot);
		}

		public void HypothesisFilter(Robot robot) //, int step 
		{
			HypothesisInit();
			//Hypothesis1New();
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
				for (j = 0; j < Width; ++j)
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

		public void StartInit()
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
			for (var i = 0; i < Width; i++)
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

		public bool CheckWalls(int x, int y, int direction, Robot robot)
		{
			int startX = x, startY = y;
			//Robot.Sensors = _sensors;
			if (robot.InitialDirection == 1)
			{
				if (direction > 2) direction -= 2;
				else direction += 2;
			}

			var i = 0;
			//Down
			while (i < Robot.RobotSensors.QualitySensors && x + 1 <= Height)
			{
				var j = robot.RSensors.GetIndex(direction, Down);
				if (robot.Sensors[i, j] != map[x, y, Down]) return false;
				if (robot.Sensors[i, j] == 1) break;
				i++;
				x++;
			}
			x = startX;
			i = 0;
			//Left
			while (i < Robot.RobotSensors.QualitySensors && y >= 0)
			{
				var j = robot.RSensors.GetIndex(direction, Left);
				if (robot.Sensors[i, j] != map[x, y, Left]) return false;
				if (robot.Sensors[i, j] == 1) break;
				i++;
				y--;
			}
			y = startY;
			i = 0;
			//Up
			while (i < Robot.RobotSensors.QualitySensors && x >= 0)
			{
				var j = robot.RSensors.GetIndex(direction, Up);
				if (robot.Sensors[i, j] != map[x, y, Up]) return false;
				if (robot.Sensors[i, j] == 1) break;
				i++;
				x--;
			}
			x = startX;
			i = 0;
			//Right
			while (i < Robot.RobotSensors.QualitySensors && y + 1 <= Width)
			{
				var j = robot.RSensors.GetIndex(direction, Right);
				if (robot.Sensors[i, j] != map[x, y, Right]) return false;
				if (robot.Sensors[i, j] == 1) break;
				i++;
				y++;
			}
			return true;

			/*
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
			*/
		}

		// 0 <= quantity <= 4
		//Сюда ещё можно добавить расположение стен (| | или _|)
		public void Hypothesis1(int quantity)
		{
			for (var i = 0; i < Height; ++i)
			{
				for (var j = 0; j < Width; ++j)
				{
					if (map[i, j, 0] == quantity)
					{
						Hypothesis[0].Add(i);
						Hypothesis[1].Add(j);
						Hypothesis[2].Add(3);
					}
				}
			}
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
		}

		//потестить ещё
		public void Hypothesis3(int direction, bool beginWay, Motion motion, Robot robot)
		{
			int i,
				quantity = robot.Sensors[0, 0] + robot.Sensors[0, 1] +
				           robot.Sensors[0, 2] + robot.Sensors[0, 3];

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
								Hypothesis[0][i]++;
								Hypothesis[2][i] = Down;
								fl = false;
							}
						}
						break;
					}
					case Left:
					{
						int x = Hypothesis[0][i], y = Hypothesis[1][i];

						if (y > 0 && map[x, y - 1, 0] == quantity)
						{
							if (map[x, y, Left] == 0 && CheckWalls(x, y - 1, Left, robot))
							{
								Hypothesis[1][i]--;
								Hypothesis[2][i] = Left;
								fl = false;
							}
						}
						break;
					}
					case Up:
					{
						int x = Hypothesis[0][i], y = Hypothesis[1][i];

						if (x > 0 && map[x - 1, y, 0] == quantity)
						{
							if (map[x, y, Up] == 0 && CheckWalls(x - 1, y, Up, robot))
							{
								Hypothesis[0][i]--;
								Hypothesis[2][i] = Up;
								fl = false;
							}
						}
						break;
					}
					case Right:
					{
						int x = Hypothesis[0][i], y = Hypothesis[1][i];

						if (y + 1 < Width && map[x, y + 1, 0] == quantity)
						{
							if (map[x, y, Right] == 0 && CheckWalls(x, y + 1, Right, robot))
							{
								Hypothesis[1][i]++;
								Hypothesis[2][i] = Right;
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

		//правильно работает только для копирования гипотез, но для другого не используется)
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
		}

		/*
		public void SensorsRead(int x, int y, int direction, Robot robot)
		{
			switch (direction)
			{
				case Down:
					Sensors[2] = map[x, y, 1];
					Sensors[3] = map[x, y, 2];
					Sensors[0] = map[x, y, 3];
					Sensors[1] = map[x, y, 4];
					robot.SetSensors(Sensors);
					robot.GetSensors(ref Sensors);
					break;
				case Left:
					Sensors[2] = map[x, y, 2];
					Sensors[3] = map[x, y, 3];
					Sensors[0] = map[x, y, 4];
					Sensors[1] = map[x, y, 1];
					robot.SetSensors(Sensors);
					robot.GetSensors(ref Sensors);
					break;
				case Up:
					Sensors[0] = map[x, y, 1];
					Sensors[1] = map[x, y, 2];
					Sensors[2] = map[x, y, 3];
					Sensors[3] = map[x, y, 4];
					robot.SetSensors(Sensors);
					robot.GetSensors(ref Sensors);
					break;
				case Right:
					Sensors[0] = map[x, y, 2];
					Sensors[1] = map[x, y, 3];
					Sensors[2] = map[x, y, 4];
					Sensors[3] = map[x, y, 1];
					robot.SetSensors(Sensors);
					robot.GetSensors(ref Sensors);
					break;
				default:
					Console.WriteLine("SensorsRead ERROR");
					break;
			}
		}
		*/
		public void ListFiltration(ref List<List<int>> list)
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