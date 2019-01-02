using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace TravelingSanta2018
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Position> positions = GetPositions("cities.csv");

            List<Position> resultPath = new List<Position>
            {
                positions[0],
                positions[1],
                positions[2],
                positions[0]
            };

            for (int i = 3; i < positions.Count; i++)
            {
                AddToPath(resultPath, positions[i]);
                Console.WriteLine(i);
            }

            WritePath(resultPath, "result");
        }

        private static void AddToPath(List<Position> resultPath, Position position)
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

        private static double GetDistance(List<Position> positions)
        {
            double length = 0;

            for (int i = 0; i < positions.Count - 1; i++)
            {
                length += GetDistance(positions[i], positions[i + 1]); //also prime and 10th city costs as described
            }

            return length;
        }

        private static double GetDistance(Position position1, Position position2)
        {
            return Math.Sqrt(Math.Pow(position1.X - position2.X, 2) + Math.Pow(position1.Y - position2.Y, 2));
        }

        private static void WritePath(IEnumerable<Position> positions, string filename)
        {
            using (StreamWriter streamWriter = new StreamWriter(filename + ".csv"))
            {
                streamWriter.WriteLine("Path");

                foreach (Position position in positions)
                {
                    streamWriter.WriteLine(position.ID);
                }
            }
        }

        private static List<Position> GetPositions(string filename)
        {
            List<Position> postions = new List<Position>();

            using (StreamReader streamReader = new StreamReader(filename))
            {
                streamReader.ReadLine();

                while (!streamReader.EndOfStream)
                {
                    string[] line = streamReader.ReadLine().Split(',');

                    Position position = new Position
                    {
                        ID = int.Parse(line[0]),
                        X = double.Parse(line[1], CultureInfo.InvariantCulture),
                        Y = double.Parse(line[2], CultureInfo.InvariantCulture)
                    };

                    postions.Add(position);
                }
            }

            return postions;
        }
    }

    struct Position
    {
        public int ID { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
    }
}
