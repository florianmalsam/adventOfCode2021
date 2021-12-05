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
        private readonly List<Line> lines;
        private int[,] ground;

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
            }).ToList();

            // only horizontal, vertical and 45° diagonal lines
            lines = lines.Where(line => 
                line.Start.X == line.End.X || line.Start.Y == line.End.Y ||
                line.Start.X - line.End.X == line.End.Y - line.Start.Y ||
                line.Start.X - line.End.X == line.Start.Y - line.End.Y
            ).ToList();

            //switch start and end if line has "wrong" direction
            foreach (var line in lines) SwitchStartAndEndIfNeccesary(line);

            // init array with max size of input
            ground = new int[ lines.Max(l => Math.Max(l.Start.X, l.End.X)) + 1, lines.Max(l => Math.Max(l.Start.Y, l.End.Y)) + 1 ];
        }

        /// <summary>
        /// draws all lines from the input and calculates the number of points where at least two lines overlap
        /// </summary>
        /// <returns>result in advent of code format</returns>
        internal int Calc()
        {
            lines.ForEach(l => l.DrawLine(ref ground));

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

        private static void SwitchStartAndEndIfNeccesary(Line line)
        {
            if ((line.Direction == Direction.Horizontal && line.End.Y < line.Start.Y) || line.End.X < line.Start.X)
            {
                var tmPoint = line.Start;
                line.Start = line.End;
                line.End = tmPoint;
            }
        }

        private class Line
        {
            public Direction Direction =>
                Start.X == End.X ? Direction.Horizontal :
                Start.Y == End.Y ? Direction.Vertical:
                Start.Y > End.Y ? Direction.DiagonalNegative : Direction.DiagonalPositive;

            public int Length => Direction == Direction.Vertical ? Math.Abs(End.X - Start.X) : Math.Abs(End.Y - Start.Y);

            public Point Start = new();
            public Point End = new();

            public void DrawLine(ref int[,] ground)
            {
                if (Direction == Direction.Horizontal)
                {
                    for (int i = 0; i <= Length; i++)
                        ground[Start.X, Start.Y + i]++;
                }

                if (Direction == Direction.Vertical)
                {
                    for (int i = 0; i <= Length; i++)
                        ground[Start.X + i, Start.Y]++;
                }

                if(Direction == Direction.DiagonalPositive)
                {
                    for (int i = 0; i <= Length; i++)
                        ground[Start.X + i, Start.Y + i]++;
                }

                if(Direction == Direction.DiagonalNegative)
                {
                    for (int i = 0; i <= Length; i++)
                        ground[Start.X + i, Start.Y - i]++;
                }
            }

        }

        internal enum Direction
        {
            Horizontal, Vertical, DiagonalPositive, DiagonalNegative
        }

        record Point
        {
            public int X, Y;
        }
    }
}
