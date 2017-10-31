﻿using System;

namespace Localization
{
	//Проекции футамуры, принцип belodi

	public class Motion
	{
		public const int Down = 1;
		public const int Left = 2;
		public const int Up = 3;
		public const int Right = 4;

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

		private int ToDownDir(int direction, bool beginWay)
		{

			if (beginWay)
			{
				if (direction > 2) return direction - 2;
				return direction + 2;
			}
			else return direction;
		}

		// текущее направление в абсолютных коорд/направление движения(куда едем?)
		public int GetNewDir(int currentDirection, int newDirection, bool beginWay)
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
					return ToDownDir(currentDirection, beginWay);
			}
			Console.WriteLine("Motion.GetNewDir - Bag");
			return -1;
		}
	}
}