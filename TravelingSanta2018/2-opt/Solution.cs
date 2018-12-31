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

            double min = GetDistance(positions);
            int fileCount = 0;

            Random random = new Random();

            while (true)
            {
                int random1 = random.Next(1, positions.Count - 1);
                int random2 = random.Next(1, positions.Count - 1);

                Swap(positions, random1, random2);

                double length = GetDistance(positions);

                if (length <= min)
                {
                    min = length;

                    fileCount++;

                    if (fileCount % 1000 == 0)
                        WritePath(positions, fileCount);

                }
                else
                {
                    Swap(positions, random1, random2);
                }
            }
        }

        private static void Swap(List<Position> positions, int positionA, int positionB)
        {
            Position temp = positions[positionA];
            positions[positionA] = positions[positionB];
            positions[positionB] = temp;
        }

        private static double GetDistance(List<Position> positions)
        {
            double length = 0;

            for (int i = 0; i < positions.Count - 1; i++)
            {
                length += GetDistance(positions[i], positions[i + 1]); //also prime and 10th...
            }

            return length;
        }

        private static double GetDistance(Position position1, Position position2)
        {
            return Math.Sqrt(Math.Pow(position1.X - position2.X, 2) + Math.Pow(position1.Y - position2.Y, 2));
        }

        private static void WritePath(List<Position> positions, int fileCount)
        {
            using (StreamWriter streamWriter = new StreamWriter(fileCount + ".csv"))
            {
                streamWriter.WriteLine("Path");

                foreach (Position position in positions)
                {
                    streamWriter.WriteLine(position.ID);
                }

                streamWriter.WriteLine(0);
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
