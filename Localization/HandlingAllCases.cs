using System.Collections.Generic;

namespace Localization
{
	public class HandlingAllCases
	{
		private bool _wayExist = true, _localiz;

		private void CopyWay(List<int> from, ref List<int> to)
		{
			to.Clear();
			for (var i = 0; i < from.Count; ++i)
			{
				to.Add(from[i]);
			}
		}

		public void Handing(HandlingHypotheses handlingHypotheses, Way way)
		{
			//Console.WriteLine("Handing");
			var copyHypothesis = new List<List<int>>();
			handlingHypotheses.HypothesisInit();
			var ways = new List<int>();

			for (var i = 0; i < handlingHypotheses.Hypothesis.Count; i++)
			{
				copyHypothesis.Add(new List<int>());
			}
			var fl = 1;
			handlingHypotheses.Copy_Lists(ref copyHypothesis, handlingHypotheses.Hypothesis);
			var n = handlingHypotheses.Hypothesis[0].Count;
			var motion = new Motion();
			var robot = new Robot();
			for (var i = 0; i < n; ++i)
			{
				way.CurentWayInit();
				while (fl != 0)
				{
					CopyWay(way.CurentWay, ref ways);
					robot.InitialDirection = 3;
					robot.RSensors.Read(handlingHypotheses.Hypothesis[0][i], handlingHypotheses.Hypothesis[1][i],
						handlingHypotheses.Hypothesis[2][i], robot, handlingHypotheses);
					HandingHypothesis(i, ways, handlingHypotheses, motion, way, robot);
					fl = way.NextDirection(_wayExist, _localiz, robot);
					handlingHypotheses.Hypothesis[0].Clear();
					handlingHypotheses.Hypothesis[1].Clear();
					handlingHypotheses.Hypothesis[2].Clear();
					handlingHypotheses.Copy_Lists(ref handlingHypotheses.Hypothesis, copyHypothesis);
					_wayExist = true;
					_localiz = false;
				}
				fl = 1;
			}
		}

		//Добавить в начало вызов Hypothesis1
		//number == number of hypothesis
		private void HandingHypothesis(int number, List<int> ways,
			HandlingHypotheses handlingHypotheses, Motion motion, Way way, Robot robot)
		{
			int x = handlingHypotheses.Hypothesis[0][number],
				y = handlingHypotheses.Hypothesis[1][number],
				direction = handlingHypotheses.Hypothesis[2][number],
				xStart = x,
				yStart = y;
			var newDir = handlingHypotheses.Hypothesis[2][number];
			way.BeginWay = true;
			//Map.Hypothesis1New();
			handlingHypotheses.HypothesisFilter(robot);
			for (int k = 0; k < ways.Count; k++)
			{
				bool fl = true;
				if (k == 1)
				{
					way.BeginWay = false;
					//robot.InitialDirection = newDir;
				}
				//int a = hypothesis[2][i], b = ways[j][k];
				newDir = motion.GetNewDir(newDir, ways[k], way.BeginWay); //Map.ChooseDir(newDir, way[k]);
				switch (newDir)
				{
					case HandlingHypotheses.Down:
					{
						if (x + 1 < HandlingHypotheses.Height && handlingHypotheses.Map[x, y, HandlingHypotheses.Down] == 0)
							// && CheckWalls(x, y + 1, Down))
						{
							fl = false;
							++x;
							robot.RSensors.Read(x, y, HandlingHypotheses.Down, robot, handlingHypotheses);
						}
						break;
					}
					case HandlingHypotheses.Left:
					{
						if (y > 0 && handlingHypotheses.Map[x, y, HandlingHypotheses.Left] == 0)
							//&& CheckWalls(x, y - 1, Map.Left))
						{
							fl = false;
							--y;
							robot.RSensors.Read(x, y, HandlingHypotheses.Left, robot, handlingHypotheses);
						}
						break;
					}
					case HandlingHypotheses.Up:
					{
						if (x > 0 && handlingHypotheses.Map[x, y, HandlingHypotheses.Up] == 0) //&& CheckWalls(x - 1, y , Up))
						{
							fl = false;
							--x;
							robot.RSensors.Read(x, y, HandlingHypotheses.Up, robot, handlingHypotheses);
						}
						break;
					}
					case HandlingHypotheses.Right:
					{
						if (y + 1 < HandlingHypotheses.Width && handlingHypotheses.Map[x, y, HandlingHypotheses.Right] == 0
						) // && CheckWalls(x, y + 1, Right))
						{
							fl = false;
							++y;
							robot.RSensors.Read(x, y, HandlingHypotheses.Right, robot, handlingHypotheses);
						}
						break;
					}
				}
				if (fl)
				{
					handlingHypotheses.Sensors[0] = -1;
					handlingHypotheses.Sensors[1] = -1;
					handlingHypotheses.Sensors[2] = -1;
					handlingHypotheses.Sensors[3] = -1;
					_wayExist = false;
					return; //-1
				}

				handlingHypotheses.Hypothesis3(ways[k], way.BeginWay, motion, robot);
				/*
				for (int i = 0; i < way.Count; i++)
				{
					Console.Write(way[i]);
				}
				Console.WriteLine();
				*/
				if (handlingHypotheses.Hypothesis[0].Count == 1)
				{
					_localiz = true;
					var n = handlingHypotheses.BestWays.Count;
					handlingHypotheses.BestWays.Add(new List<int>());
					handlingHypotheses.BestWays[n].Add(xStart);
					handlingHypotheses.BestWays[n].Add(yStart);
					handlingHypotheses.BestWays[n].Add(direction);
					for (var l = 0; l <= k; l++)
					{
						handlingHypotheses.BestWays[n].Add(ways[l]);
						//	Console.Write(best_ways[n][l] + ",");
					}
					//Console.WriteLine(" quantity of steps: " + k);
					return; // k;
				}

				if (handlingHypotheses.Hypothesis[0].Count == 0)
				{
					_wayExist = false;
					return;
				}
			}
			//Console.WriteLine("HC: " + hypothesis[0].Count);
			//return Map.hypothesis[0].Count;
		}
	}
}