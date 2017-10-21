using System.Collections.Generic;
/*
namespace Localization
{
	//TODO: Сделать этот класс, убрать все гипотезы из map
    class Hypothesis
    {
	    public List<List<int>> Hypothesis = new List<List<int>>();
		
	    public void Hypothesis3(int direction, bool beginWay, Motion Motion, Robot Robot)
		{
			int i, quantity = Robot.Sensors[0] + Robot.Sensors[1] + Robot.Sensors[2] + Robot.Sensors[3];

			for (i = 0; i < Hypothesis[0].Count; ++i)
			{
				var newDir = Motion.GetNewDir(Hypothesis[2][i], direction, beginWay);
				var fl = true;
				switch (newDir)
				{
					case Down:
					{
						//int i, quantity = _sensors[0] + _sensors[1] + _sensors[2] + _sensors[3];
						//var fl = true;
						int x = Hypothesis[0][i], y = Hypothesis[1][i];

						if (x + 1 < Height && _map[x + 1, y, 0] == quantity)
						{
							if (_map[x, y, Down] == 0 &&
							    CheckWalls(x + 1, y, Down, Robot))
							{
								//++_quantityOfWays;
								//var x = ++start[2][i];
								Hypothesis[0][i]++;
								Hypothesis[2][i] = Down; //ToDownDir(hypothesis[2][i]);
								fl = false;
							}
						}
						break;
					}
					case Left:
					{
						//int i, quantity = _sensors[0] + _sensors[1] + _sensors[2] + _sensors[3];
						//var fl = true;
						int x = Hypothesis[0][i], y = Hypothesis[1][i];

						if (y > 0 && _map[x, y - 1, 0] == quantity)
						{
							if (_map[x, y, Left] == 0 && CheckWalls(x, y - 1, Left, Robot))
							{
								//++_quantityOfWays;
								//var x = ++start[2][i];
								Hypothesis[1][i]--;
								Hypothesis[2][i] = Left; //ToDownDir(hypothesis[2][i]);
								fl = false;
							}
						}
						break;
					}
					case Up:
					{
						//int i, quantity = _sensors[0] + _sensors[1] + _sensors[2] + _sensors[3];
						//var fl = true;
						int x = Hypothesis[0][i], y = Hypothesis[1][i];

						if (x > 0 && _map[x - 1, y, 0] == quantity)
						{
							if (_map[x, y, Up] == 0 && CheckWalls(x - 1, y, Up, Robot))
							{
								//++_quantityOfWays;
								//var x = ++start[2][i];
								Hypothesis[0][i]--;
								Hypothesis[2][i] = Up; //ToDownDir(hypothesis[2][i]);
								fl = false;
							}
						}
						break;
					}
					case Right:
					{
						//int i, quantity = _sensors[0] + _sensors[1] + _sensors[2] + _sensors[3];
						//var fl = true;
						int x = Hypothesis[0][i], y = Hypothesis[1][i];

						if (y + 1 < Wight && _map[x, y + 1, 0] == quantity)
						{
							if (_map[x, y, Right] == 0 && CheckWalls(x, y + 1, Right, Robot))
							{
								//++_quantityOfWays;
								//var x = ++start[2][i];
								Hypothesis[1][i]++;
								Hypothesis[2][i] = Right; //ToDownDir(hypothesis[2][i]);
								fl = false;
							}
						}
						break;
					}
				}
				/*
				if (!fl && Robot.InitialDirection==1)
				{
					if (Hypothesis[2][i] > 2) Hypothesis[2][i] -= 2;
					else Hypothesis[2][i] += 2;
				}
				*//*
				if (fl)
				{
					Hypothesis[0].RemoveAt(i);
					Hypothesis[1].RemoveAt(i);
					Hypothesis[2].RemoveAt(i);
					i--;
				}
			}
		}
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

	    private int ToDownDir(int direction, Robot robot)
	    {

		    if (way.CurentWay.Count == 1)
		    {
			    if (direction > 2) return direction - 2;
			    return direction + 2;
		    }
		    else return direction;
		    *//*
		    if (direction > 2) return direction - 2;
		    return direction + 2;
		    *//*
		    Console.WriteLine("Map.ToDownDir - bag");
		    return direction; //down == up
	    }

	    // текущее направление в абсолютных коорд/направление движения(куда едем?)
	    public int ChooseDir(int currentDirection, int newDirection, Way way)
	    {
		    if (newDirection == Up) return currentDirection;
		    if (newDirection == Right) return ToRightDir(currentDirection);
		    if (newDirection == Left) return ToLeftDir(currentDirection);
		    return ToDownDir(currentDirection, way);
	    }
    }
}*/