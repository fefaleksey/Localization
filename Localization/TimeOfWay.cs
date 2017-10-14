using System.Collections.Generic;

namespace Localization
{
    class TimeOfWay
    {
        public const int Down = 1, Left = 2, Up = 3, Right = 4;
        
        public void GetTime(ref List<List<int>> ways)
        {
            for (var i = 0; i < ways.Count; i++)
            {
                var time=0;
                for (var j = 3; j < ways[i].Count; j++)
                {
                    if (ways[i][j] == Down || ways[i][j] == Up)
                    {
                        time++;
                    }
                    else
                    {
                        time += 2;
                    }
                }
                ways[i].Insert(3, time);
            }
        }
    }
}