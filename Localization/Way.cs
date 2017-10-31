using System;
using System.Collections.Generic;

namespace Localization
{
	public class Way
	{
		public List<int> CurentWay = new List<int>();
		private int _i = 0, _forbiddenDirection = 3;

		public const int Length = 64;
		public bool BeginWay = true;

		public void CurentWayInit()
		{
			CurentWay.Clear();
			CurentWay.Add(1);
			_forbiddenDirection = 3;
			_i = 0;
		}

		//localiz==true <=> мы локализовались
		public int NextDirection(bool wayExist, bool localiz, Robot robot)
		{
			if (CurentWay.Count == 0)
			{
				return 0;
			}

			if (localiz)
			{
				if (CurentWay[_i] == 4)
				{
					CurentWay.RemoveAt(_i);
					--_i;
					return NextDirection(false, false, robot);
				}
				else
				{
					CurentWay[_i]++;

					if (CurentWay.Count == 1 && CurentWay[0] == 2)
					{
						//запрещенное направление мы поменяем только 1 раз!
						_forbiddenDirection = 1;
						robot.InitialDirection = 3;
					}

					if (CurentWay[_i] == _forbiddenDirection)
					{
						CurentWay[_i]++;
					}

				}
				return CurentWay[_i];
			}

			if (wayExist)
			{
				if (CurentWay.Count == Length)
				{
					return NextDirection(false, false, robot);
				}
				if (_forbiddenDirection == 1)
				{
					CurentWay.Add(2);
					++_i;
					return 2;
				}
				else
				{
					CurentWay.Add(1);
					++_i;
					return 1;
				}
			}
			else
			{
				if (CurentWay[_i] == 4)
				{
					CurentWay.RemoveAt(_i);
					--_i;
					return NextDirection(false, false, robot);
				}
				else
				{
					++CurentWay[_i];

					if (CurentWay.Count == 1 && CurentWay[0] == 2)
					{
						_forbiddenDirection = 1;
						robot.InitialDirection = 3;
					}
					if (CurentWay[_i] == _forbiddenDirection)
					{
						++CurentWay[_i];
					}

					return CurentWay[_i];
				}
			}
		}
	}
}
