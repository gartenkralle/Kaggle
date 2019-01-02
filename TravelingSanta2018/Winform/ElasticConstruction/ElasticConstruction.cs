using System;
using System.Collections.Generic;

namespace TravelingSanta2018
{
    public class ElasticConstruction
    {
        private List<Position> resultPath;

        public ElasticConstruction()
        {
            resultPath = new List<Position>();
        }

        public List<Position> AddToPath(Position position)
        {
            if (resultPath.Count == 0)
            {
                resultPath.Add(position);
                resultPath.Add(position);
            }
            else
            {
                double minDetour = double.MaxValue;
                int insertIndex = -1;

                for (int i = 0; i < resultPath.Count - 1; i++)
                {
                    double oldLength = GetDistance(resultPath[i], resultPath[i + 1]);
                    double newLength = GetDistance(resultPath[i], position) + GetDistance(position, resultPath[i + 1]);

                    double detour = newLength - oldLength;

                    if (detour < minDetour)
                    {
                        minDetour = detour;
                        insertIndex = i + 1;
                    }
                }

                resultPath.Insert(insertIndex, position);
            }

            return resultPath;
        }

        private static double GetDistance(Position position1, Position position2)
        {
            return Math.Sqrt(Math.Pow(position1.X - position2.X, 2) + Math.Pow(position1.Y - position2.Y, 2));
        }

        private List<Position> GetConvexHull(List<Position> positions)
        {
            List<Position> result = new List<Position>();

            return result;
        }
    }

    public struct Position
    {
        public int ID { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
    }
}
