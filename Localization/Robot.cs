namespace Localization
{
	class Robot
	{
		public int[,] Sensors;
		public RobotSensors RSensors;
		public const int Down = 0, Left = 1, Up = 2, Right = 3;
		public bool BeginWay = true;

		public int InitialDirection { get; set; }

		public Robot()
		{
			RSensors = new RobotSensors();
			Sensors = new int[RobotSensors.QualitySensors, 4];
			InitialDirection = 1;
		}

		/*
		public void GetSensors(ref int[] value)
		{
			value[Down] = Sensors[Down];
			value[Left] = Sensors[Left];
			value[Up] = Sensors[Up];
			value[Right] = Sensors[Right];
		}

		public void SetSensors(int[] value)
		{
			if (InitialDirection == 1)
			{
				Sensors[Down] = value[Up];
				Sensors[Left] = value[Right];
				Sensors[Up] = value[Down];
				Sensors[Right] = value[Left];
			}
			else
			{
				Sensors[Down] = value[Down];
				Sensors[Left] = value[Left];
				Sensors[Up] = value[Up];
				Sensors[Right] = value[Right];
			}
		}
		*/
		public class RobotSensors
		{
			//public int[,] Sensors = new int[QualitySensors, 4];
			public const int QualitySensors = 2; // количество клеток, на которых сенсоры работают адекватно

			private const int IDown = 1;
			private const int ILeft = 2;
			private const int IUp = 3;
			private const int IRight = 4;

			public void Read(int x, int y, int direction, Robot robot, Map map)
			{
				var i = 0;
				int startX = x, startY = y;
				//Down
				while (i < QualitySensors && x + 1 <= map.Height)
				{
					var j = GetIndex(direction, IDown);
					robot.Sensors[i, j] = map.map[x, y, IDown];
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
					robot.Sensors[i, j] = map.map[x, y, ILeft];
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
					robot.Sensors[i, j] = map.map[x, y, IUp];
					if (robot.Sensors[i, j] == 1) break;
					i++;
					x--;
				}
				x = startX;
				y = startY;
				i = 0;
				//Right
				while (i < QualitySensors && y + 1 <= map.Widht)
				{
					var j = GetIndex(direction, IRight);
					robot.Sensors[i, j] = map.map[x, y, IRight];
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
						/*
						Sensors[Down] = value[Up];
						Sensors[Left] = value[Right];
						Sensors[Up] = value[Down];
						Sensors[Right] = value[Left];
						*/
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
		}
	}
}