namespace Localization
{
	class Robot
	{
		public int[] Sensors = new int[4];
		public const int Down = 0, Left = 1, Up = 2, Right = 3;
		public bool BeginWay = true;
		
		public int InitialDirection { get; set; } = 1;
		
		public Robot()
		{
			Sensors = new int[4];
			InitialDirection = 1;
		}
		
		
		public void GetSensors(ref int[] value)
		{
			value[Down] = Sensors[Down];
			value[Left] = Sensors[Left];
			value[Up] = Sensors[Up];
			value[Right] = Sensors[Right];
		}
		
		public unsafe void SetSensors(int[] value)
		{
			long d;
			fixed (int* p1 = value, p2 = Sensors)
			{
				d = p1 - p2;
			}
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
		/*
		public int[] Sensors
		{
			get => _sensors;
			set
			{
				if (InitialDirection == 1)
				{
					_sensors[Down] = value[Up];
					_sensors[Left] = value[Right];
					_sensors[Up] = value[Down];
					_sensors[Right] = value[Left];
				}
				else
				{
					_sensors[Down] = value[Down];
					_sensors[Left] = value[Left];
					_sensors[Up] = value[Up];
					_sensors[Right] = value[Right];
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
	//		}
	//	}
	}
}