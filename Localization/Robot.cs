namespace Localization
{
	class Robot
	{
		private static int[] sensors = new int[4];
		public const int Down = 0, Left = 1, Up = 2, Right = 3;
		
		public static int InitialDirection { get; set; } = 1;
		/*
		public Robot()
		{
			sensors = new int[4];
		}
		*/

		public static void GetSensors(ref int[] value)
		{
			value[Down] = sensors[Down];
			value[Left] = sensors[Left];
			value[Up] = sensors[Up];
			value[Right] = sensors[Right];
		}
		
		public static unsafe void SetSensors(int[] value)
		{
			long d;
			fixed (int* p1 = value, p2 = sensors)
			{
				d = p1 - p2;
			}
			if (InitialDirection == 1)
			{
				sensors[Down] = value[Up];
				sensors[Left] = value[Right];
				sensors[Up] = value[Down];
				sensors[Right] = value[Left];
			}
			else
			{
				sensors[Down] = value[Down];
				sensors[Left] = value[Left];
				sensors[Up] = value[Up];
				sensors[Right] = value[Right];
			}
		}
		
		public static int[] Sensors
		{
			get => sensors;
			set
			{
				if (InitialDirection == 1)
				{
					sensors[Down] = value[Up];
					sensors[Left] = value[Right];
					sensors[Up] = value[Down];
					sensors[Right] = value[Left];
				}
				else
				{
					sensors[Down] = value[Down];
					sensors[Left] = value[Left];
					sensors[Up] = value[Up];
					sensors[Right] = value[Right];
				}
				/*
				if (Way.beginWay)
				{
					sensors[Down] = value[Down];
					sensors[Left] = value[Left];
					sensors[Up] = value[Up];
					sensors[Right] = value[Right];
				}
				else
				if (InitialDirection != 1)
				{
					sensors[Down] = value[Down];
					sensors[Left] = value[Left];
					sensors[Up] = value[Up];
					sensors[Right] = value[Right];
				}
				else
				{
					sensors[Down] = value[Up];
					sensors[Left] = value[Right];
					sensors[Up] = value[Down];
					sensors[Right] = value[Left];
				}
				*/
			}
		}
	}
}