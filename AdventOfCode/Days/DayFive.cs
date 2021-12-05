using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    /// <summary>
    /// https://adventofcode.com/2021/day/5
    /// </summary>
    internal class DayFive
    {
        private readonly IEnumerable<Line> lines;
        private readonly int[,] ground;

        public DayFive()
        {
            List<string> inputLines = File.ReadAllLines("Data/day5").ToList();

            lines = inputLines.Select(line => new Line {
                Start = new Point {
                    X = int.Parse(line.Split("->")[0].Split(',')[0].Trim()),
                    Y = int.Parse(line.Split("->")[0].Split(',')[1].Trim())
                },
                End = new Point {
                    X = int.Parse(line.Split("->")[1].Split(',')[0].Trim()),
                    Y = int.Parse(line.Split("->")[1].Split(',')[1].Trim())
                }
            });

            // only horizontal and vertical lines
            lines = lines.Where(line => line.Start.X == line.End.X || line.Start.Y == line.End.Y).ToList();

            //switch start and end if line has "wrong" direction
            foreach (var line in lines) SwitchStartAndEndIfNeccesary(line);

            // init array with max size of input
            ground = new int[ lines.Max(l => Math.Max(l.Start.X, l.End.X)) + 1, lines.Max(l => Math.Max(l.Start.Y, l.End.Y)) + 1 ];
        }

        /// <summary>
        /// draws all lines from the input and calculates the number of points where at least two lines overlap
        /// </summary>
        /// <returns>result in advent of code format</returns>
        internal int CalcPart1()
        {
            foreach (var line in lines)
            {
                DrawLine(line);
            }

            return CalcOverlappingPoints();
        }

        private int CalcOverlappingPoints()
        {
            // calc result
            int sum = 0;
            foreach (var x in ground)
            {
                sum += x > 1 ? 1 : 0;
            }
            return sum;
        }

        private void DrawLine(Line line)
        {
            if (line.Start.X == line.End.X)
            {
                // horizontal line
                var lengthOfLine = Math.Abs(line.End.Y - line.Start.Y);
                for (int i= 0; i <= lengthOfLine; i++)
                    ground[line.Start.X, line.Start.Y + i ]++;
            }

            if (line.Start.Y == line.End.Y)
            {
                //vertical line
                var lengthOfLine = Math.Abs(line.End.X - line.Start.X);
                for (int i = 0; i <= lengthOfLine; i++)
                    ground[line.Start.X + i, line.Start.Y]++;
            }
        }

        private static void SwitchStartAndEndIfNeccesary(Line line)
        {
            if (line.End.Y - line.Start.Y < 0 || line.End.X - line.Start.X < 0)
            {
                var tmPoint = line.Start;
                line.Start = line.End;
                line.End = tmPoint;
            }
        }

        record Line
        {
            public Point Start = new();
            public Point End = new();

        }

        record Point
        {
            public int X, Y;
        }
    }
}
