using System;

namespace Localization
{
	public class Robot
	{
		public int[,] Sensors;
		public RobotSensors RSensors;
		private const int Down = 0;
		private const int Left = 1;
		private const int Up = 2;
		private const int Right = 3;
		public bool BeginWay = true;

		public int InitialDirection { get; set; }

		public Robot()
		{
			RSensors = new RobotSensors();
			Sensors = new int[RobotSensors.QualitySensors, 4];
			InitialDirection = 1;
		}

		public class RobotSensors
		{
			public const int QualitySensors = 2; // количество клеток, на которых сенсоры работают адекватно

			private const int IDown = 1;
			private const int ILeft = 2;
			private const int IUp = 3;
			private const int IRight = 4;

			public void Read(int x, int y, int direction, Robot robot, HandlingHypotheses handlingHypotheses)
			{
				var i = 0;
				int startX = x, startY = y;
				for (var a = 0; a < QualitySensors; a++)
				{
					for (var b = 0; b < 4; b++)
					{
						robot.Sensors[a, b] = 0;
					}
				}
				//Down
				while (i < QualitySensors && x + 1 <= HandlingHypotheses.Height)
				{
					var j = GetIndex(direction, IDown);
					robot.Sensors[i, j] = handlingHypotheses.Map[x, y, IDown];
					if (robot.Sensors[i, j] == 1) break;
					i++;
					x++;
				}
				x = startX;
				y = startY;
				i = 0;
				//Left
				while (i < QualitySensors && y >= 0)
				{
					var j = GetIndex(direction, ILeft);
					robot.Sensors[i, j] = handlingHypotheses.Map[x, y, ILeft];
					if (robot.Sensors[i, j] == 1) break;
					i++;
					y--;
				}
				x = startX;
				y = startY;
				i = 0;
				//Up
				while (i < QualitySensors && x >= 0)
				{
					var j = GetIndex(direction, IUp);
					robot.Sensors[i, j] = handlingHypotheses.Map[x, y, IUp];
					if (robot.Sensors[i, j] == 1) break;
					i++;
					x--;
				}
				x = startX;
				y = startY;
				i = 0;
				//Right
				while (i < QualitySensors && y + 1 <= HandlingHypotheses.Width)
				{
					var j = GetIndex(direction, IRight);
					robot.Sensors[i, j] = handlingHypotheses.Map[x, y, IRight];
					if (robot.Sensors[i, j] == 1) break;
					i++;
					y++;
				}
				if (robot.InitialDirection == 1)
				{
					for (var j = 0; j < QualitySensors; j++)
					{
						var value1 = robot.Sensors[j, Down];
						var value2 = robot.Sensors[j, Left];
						robot.Sensors[j, Down] = robot.Sensors[j, Up];
						robot.Sensors[j, Left] = robot.Sensors[j, Right];
						robot.Sensors[j, Up] = value1;
						robot.Sensors[j, Right] = value2;
					}
				}
			}

			/// <summary>
			/// Only for method SensorRead
			/// </summary>
			/// <param name="direction"> absolute current direction </param>
			/// <param name="newDirection"></param>
			/// <returns> new index </returns>
			public int GetIndex(int direction, int newDirection)
			{
				if (newDirection == direction + 2 || newDirection == direction - 2)
					return 0;
				if (newDirection == direction)
					return 2;
				if (newDirection == direction + 1 || newDirection == direction - 3)
					return 3;
				return 1;
			}

			/// <summary>
			/// </summary>
			/// <param name="robot"> robot</param>
			/// <returns> hash key </returns>
			public int GetSensorsValue(Robot robot)
			{
				var value = 0;
				var n = QualitySensors * 4;
				for (var i = 0; i < QualitySensors; i++)
				{
					for (var j = 0; j < 4; j++)
					{
						n--;
						value += (int) Math.Pow(2, n) * robot.Sensors[i, j];
					}
				}
				return value;
			}
		}
	}
}