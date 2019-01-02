using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace TravelingSanta2018
{
    public partial class TravelingSanta2018 : Form
    {
        private Pen pen = new Pen(Brushes.DarkGray);

        private ElasticConstruction elasticConstruction;
        private List<Position> positions;
        private List<Position> resultPath;

        private int maxX = 0;
        private int maxY = 0;

        private float xScale;
        private float yScale;

        private int offset;
        private int index = 0;

        public TravelingSanta2018()
        {
            InitializeComponent();


        }

        private void TravelingSanta2018_Load(object sender, EventArgs e)
        {
            //positions = GetPositions("cities.csv");
            DrawRandomPath((int)numericUpDown1.Value);
        }

        private List<Position> GetRandomPositions(int count)
        {
            List<Position> positions = new List<Position>();
            Random random = new Random(DateTime.Now.Millisecond);

            for (int i = 0; i < count; i++)
            {
                Position position = new Position
                {
                    ID = i,
                    X = random.Next(100),
                    Y = random.Next(100)
                };

                positions.Add(position);
            }

            return positions;
        }

        private List<Position> ConstructPath(List<Position> positions)
        {
            elasticConstruction = new ElasticConstruction();

            foreach (Position position in positions)
            {
                resultPath = elasticConstruction.AddToPath(position);
            }

            return resultPath;
        }

        private void InitializeMaxValues()
        {
            foreach (Position position in resultPath)
            {
                SaveMax(ref maxX, position.X);
                SaveMax(ref maxY, position.Y);
            }
        }

        private void SaveMax(ref int max, double position)
        {
            if (max < position)
                max = (int)position;
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

        private void TravelingSanta2018_Paint(object sender, PaintEventArgs e)
        {
            foreach (Position position in positions)
            {
                e.Graphics.DrawRectangle(pen, (int)(position.X * xScale) + offset, (int)(position.Y * yScale) + offset, 2, 2);
            }

            timer1.Enabled = true;
        }

        private void TravelingSanta2018_Resize(object sender, EventArgs e)
        {
            InitEvironmnetVariables();
            Invalidate();
        }

        private void InitEvironmnetVariables()
        {
            int boarder = 100;

            xScale = (Size.Width - 100) / (float)maxX;
            yScale = (Size.Height - 100) / (float)maxY;

            offset = boarder / 4;

            index = 0;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (index == resultPath.Count - 1)
            {
                timer1.Enabled = false;
                return;
            }

            Graphics g = CreateGraphics();

            g.DrawLine(pen, (float)(resultPath[index].X * xScale) + offset, (float)(resultPath[index].Y * yScale) + offset,
                         (float)(resultPath[index + 1].X * xScale) + offset, (float)(resultPath[index + 1].Y * yScale) + offset);

            index++;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            DrawRandomPath((int)numericUpDown1.Value);
        }

        private void DrawRandomPath(int n)
        {
            positions = GetRandomPositions(n);
            resultPath = ConstructPath(positions);

            InitializeMaxValues();
            InitEvironmnetVariables();

            Invalidate();
        }

        private void NumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            DrawRandomPath((int)((NumericUpDown)sender).Value);
        }
    }
}
