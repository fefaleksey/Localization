﻿using System;

namespace Localization
{
	public class Generate
	{
		//заменить Way.Length на как
		//private int[,] _directions = new int[(int) Math.Pow(2, Robot.RobotSensors.QualitySensors), ];
		/*
		public void SetHashtable(int key, int step, int value)
		{
			_directions[key, step] = value;
		}

		public int GetDirection(int key, int step)
		{
			return _directions[key, step];
		}
		*/
		public int HashtableLength(FinalWays finalWays)
		{
			var max = 0;
			var counter = 0;
			for (var i = 0; i < finalWays.Ways.Count; i++)
			{
				while (finalWays.Ways[i][counter]!=8888888)
				{
					counter++;
				}
				if (max < counter) max = counter;
				counter = 0;
			}
			return max;
		}
		
		public int[,] GenerateHashtable(FinalWays finalWays)
		{
			var directions = new int[(int) Math.Pow(2, Math.Pow(2,Robot.RobotSensors.QualitySensors)), HashtableLength(finalWays)];
			var robot = new Robot();
			var motion = new Motion();
			var map = new Map();
			var solutionForRobot = new SolutionForRobot();
			for (var i = 0; i < finalWays.Ways.Count; i++)
			{
				var x = finalWays.Ways[i][0];
				var y = finalWays.Ways[i][1];
				var initialdirection = finalWays.Ways[i][3];
				if(initialdirection==8888888) continue;
				var newDir = finalWays.Ways[i][2];
				robot.InitialDirection = 3;
				robot.RSensors.Read(x, y,newDir /*direction*/, robot, map);
				var step = 0;
				var fl = true;
				//var newDir = direction;
				var beginWay = true;
				robot.InitialDirection = initialdirection;
				//initialdirection = newDir;
				for (var j = 3; j < finalWays.Ways[i].Count && finalWays.Ways[i][j] != 8888888; j++)
				{	
					var value = robot.RSensors.GetSensorsValue(robot);
					directions[value, step] = finalWays.Ways[i][j];
					newDir = motion.GetNewDir(newDir, finalWays.Ways[i][j], beginWay);
					//initialdirection = newDir;
					//if (fl) robot.InitialDirection = initialdirection;
					switch (newDir)
					{
						case Map.Down:
						{
							++x;
							break;
						}
						case Map.Left:
						{
							--y;
							break;
						}
						case Map.Up:
						{
							--x;
							break;
						}
						case Map.Right:
						{
							++y;
							break;
						}
					}
					fl = false;
					step++;
					robot.RSensors.Read(x, y, newDir, robot, map);
					beginWay = false;
					if (solutionForRobot.Impasse(robot))
					{
						fl = true;
						beginWay = true;
						if (robot.InitialDirection == 1)
						{
							//solutionForRobot.CorrectDirectionsInHypothesis(robot, ref map);
							robot.InitialDirection = 3;
							newDir = solutionForRobot.OppositeDirection(newDir);
						}
						else
						{
							robot.InitialDirection = 1;
						}
						//newDir = solutionForRobot.OppositeDirection(newDir);
					}
				}
			}
			return directions;
		}
	}
}