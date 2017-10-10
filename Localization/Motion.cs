using System;
namespace Localization
{
	class Motion
	{
		public const int Down = 1;
		public const int Left = 2;
		public const int Up = 3;
		public const int Right = 4;

		private static int ToRightDir(int direction)
		{
			if (direction < 4) return direction + 1;
			return 1;
		}

		//Возвращает направление, если поворачиваем в соотв. сторону
		private static int ToLeftDir(int direction)
		{
			/*
			if (Robot.InitialDirection == Down)
			{
				if (direction > 1) return direction - 1;
				return 4;
			}
			else
			*/
			if (direction > 1) return direction - 1;
			return 4;
		}

		private static int ToDownDir(int direction)
		{

			if (Way.curent_way.Count == 1)
			{
				if (direction > 2) return direction - 2;
				return direction + 2;
			}
			else return direction;
			/*
			if (direction > 2) return direction - 2;
			return direction + 2;
			*/
			//Console.WriteLine("Map.ToDownDir - bag");
			//return direction; //down == up
		}

		// текущее направление в абсолютных коорд/направление движения(куда едем?)
		public static int GetNewDir(int currentDirection, int newDirection)
		{
			switch (newDirection)
			{
				case Up:
					return currentDirection;
				case Right:
					return ToRightDir(currentDirection);
				case Left:
					return ToLeftDir(currentDirection);
				case Down:
					return ToDownDir(currentDirection);
			}
			Console.WriteLine("Motion.GetNewDir - Bag");
			return -1;
		}
	}
}