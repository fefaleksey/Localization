namespace Localization
{
    class RobotSensors
    {
        //public int[,] Sensors = new int[QualitySensors, 4];
        public const int QualitySensors = 3; // количество клеток, на которых сенсоры работают адекватно
        private const int Down = 1;
        private const int Left = 2;
        private const int Up = 3;
        private const int Right = 4;

        public void SensorsRead(int x, int y, int direction, Robot robot, Map map)
        {
            var i = 0;
            //Down
            while (i < QualitySensors && x + 1 < map.Height)
            {
                var j = GetIndex(direction, Down);
                robot.Sensors[i, j] = map.map[x, y, Down];
                if (robot.Sensors[i, j] == 1) break;
                i++;
                x++;
            }
            i = 0;
            //Left
            while (i < QualitySensors && y > 0)
            {
                var j = GetIndex(direction, Left);
                robot.Sensors[i, j] = map.map[x, y, Left];
                if (robot.Sensors[i, j] == 1) break;
                i++;
                y--;
            }
            i = 0;
            //Up
            while (i < QualitySensors && x > 0)
            {
                var j = GetIndex(direction, Up);
                robot.Sensors[i, j] = map.map[x, y, Up];
                if (robot.Sensors[i, j] == 1) break;
                i++;
                x--;
            }
            i = 0;
            //Right
            while (i < QualitySensors && y + 1 < map.Widht)
            {
                var j = GetIndex(direction, Right);
                robot.Sensors[i, j] = map.map[x, y, Right];
                if (robot.Sensors[i, j] == 1) break;
                i++;
                y++;
            }
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