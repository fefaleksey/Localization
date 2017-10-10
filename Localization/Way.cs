using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Schema;

namespace Localization
{
	class Way
	{
		public static List<int> curent_way = new List<int>();
		private static int i=0, forbiddenDirection=3;

		private static int length = 64;

		public static void CurentWayInit()
		{
			curent_way.Clear();
			curent_way.Add(1);
			i = 0;
		}
		//localiz==true <=> мы локализовались
		public static int NextDirection(bool wayExist, bool localiz)
		{
			if (curent_way.Count == 0)
			{
				Console.WriteLine("Complete");
				return 0;
			}
			
			if (localiz)
			{
				//curent_way.RemoveAt(i);
				//--i;
				if (curent_way[i] == 4)
				{
					curent_way.RemoveAt(i);
					--i;
					return NextDirection(false, false); // return?????
					//return 5;//???????
				}
				else
				{
					curent_way[i]++;
					
					if (curent_way.Count == 1 && curent_way[0]==2)
					{
						//запрещенное направление мы поменяем только 1 раз!
						forbiddenDirection = 1;
						Robot.InitialDirection = 3;
					}
					
					if (curent_way[i] == forbiddenDirection)
					{
						curent_way[i]++;
					}
					
				}
				return curent_way[i];
			}
			
			if (wayExist)
			{
				if (curent_way.Count==length)
				{
					return NextDirection(false, false);
					//return curent_way[i];
				}
				if (forbiddenDirection == 1)
				{
					curent_way.Add(2);
					++i;
					return 2;
				}
				else
				{
					curent_way.Add(1);
					++i;
					return 1;
				}
			}
			else
			{
				if (curent_way[i] == 4)
				{
					curent_way.RemoveAt(i);
					--i;
					return NextDirection(false, false);//return ???
					//return 1;//curent_way[i];
				}
				else
				{
					++curent_way[i];
					
					if (curent_way.Count==1 && curent_way[0]==2)
					{
						forbiddenDirection = 1;
						Robot.InitialDirection = 3;
					}
					if (curent_way[i] == forbiddenDirection)
					{
						++curent_way[i];
					}
					
					return curent_way[i];
				}
			}
			Console.WriteLine("Way.cs NextDirection - bag");
			return -1;
		}
	}
}