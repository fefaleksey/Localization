using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Schema;

/* To do 09.10 !!
 *Переделать NumberOfSteps, используя данный класс
 * Протестить метод NextDirection
 * Написать метод принятия решения */
namespace Localization
{
	class Way
	{
		public static List<int> curent_way = new List<int>();
		private static int i=0, forbiddenDirection=3;

		private static int length = 10;

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
				//Добавить этот путь в Map.best_ways
				//Или заставить сделать это метод HandingHypothesis
				
				curent_way.RemoveAt(i);
				--i;
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
					
					if (curent_way.Count == 1)
					{
						//запрещенное направление мы поменяем только 1 раз!
						forbiddenDirection = 1;
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
					if (curent_way.Count==1)
					{
						forbiddenDirection = 1;
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