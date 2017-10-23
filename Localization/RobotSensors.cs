using System;

namespace Localization
{
    class RobotSensors
    {
        public int[,] Sensors = new int[3, 4];
        public const int QualitySensors = 3; // количество клеток, на которых сенсоры работают адекватно
        private const int Down = 1;
        private const int Left = 2;
        private const int Up = 3;
        private const int Right = 4;

        public void SensorsRead(int x, int y, int direction, Robot robot, Map map)
        {
            int xStart = x, yStart = y, directionStart = direction;
            var i = 0;
            //Down
            while (i < QualitySensors && x + 1 < map.Height)
            {
                var j = GetIndex(direction, Down);
                Sensors[i, j] = map.map[x, y, Down];
                if (Sensors[i, j] == 1) break;
                i++;
                x++;
            }
            i = 0;
            //Left
            while (i < QualitySensors && y > 0)
            {
                var j = GetIndex(direction, Left);
                Sensors[i, j] = map.map[x, y, Left];
                if (Sensors[i, j] == 1) break;
                i++;
                y--;
            }
            i = 0;
            //Up
            while (i < QualitySensors && x > 0)
            {
                var j = GetIndex(direction, Up);
                Sensors[i, j] = map.map[x, y, Up];
                if (Sensors[i, j] == 1) break;
                i++;
                x--;
            }
            i = 0;
            //Right
            while (i < QualitySensors && y + 1 < map.Wight)
            {
                var j = GetIndex(direction, Right);
                Sensors[i, j] = map.map[x, y, Right];
                if (Sensors[i, j] == 1) break;
                i++;
                y++;
            }
            
            Sensors[0, 2] = map.map[x, y, 1];
            Sensors[0, 3] = map.map[x, y, 2];
            Sensors[0, 0] = map.map[x, y, 3];
            Sensors[0, 1] = map.map[x, y, 4];
        }

        /// <summary>
        /// Only for method SensorRead
        /// </summary>
        /// <param name="direction"> absolute current direction </param>
        /// <param name="newDirection"></param>
        /// <returns> new index </returns>
        private int GetIndex(int direction, int newDirection)
        {
            if (newDirection == direction + 2 || newDirection == direction - 2)
                return 0;
            if (newDirection == direction)
                return 2;
            if (newDirection == direction + 1 || newDirection == direction - 3)
                return 3;
            return 1;
        }
    }
}