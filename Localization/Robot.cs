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
		public static int[] Sensors
		{
			get => sensors;
			set
			{
				if (Way.curent_way.Count == 1)
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
			}
		}
	}
}