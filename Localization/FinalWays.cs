using System.Collections.Generic;
using System;

namespace Localization
{
    public class FinalWays
    {
        //Ways: координаты, путь, 8888888, координаты после локализации, 8888888, время
        public List<List<int>> Ways;
        public List<List<int>> FinalList;

        public FinalWays()
        {
            Ways = new List<List<int>>();
            FinalList = new List<List<int>>();
            var map = new Map();
            map.HypothesisInit();
            for (var i = 0; i < map.Hypothesis[0].Count; i++)
            {
                FinalList.Add(new List<int>());
                FinalList[i].Add(map.Hypothesis[0][i]);
                FinalList[i].Add(map.Hypothesis[1][i]);
                FinalList[i].Add(map.Hypothesis[2][i]);
            }
        }

        public void SetFinalList()
        {
            for (var i = 0; i < FinalList.Count; i++)
            {
                var j = Ways[i].Count - 1;
                FinalList[i].Add(Ways[i][j]);
            }
        }

        public void PrintResult()
        {
            for (var i = 0; i < FinalList.Count; i++)
            {
                Console.Write(i + ". ");
                for (var j = 0; j < FinalList[i].Count; j++)
                {
                    Console.Write(FinalList[i][j] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}