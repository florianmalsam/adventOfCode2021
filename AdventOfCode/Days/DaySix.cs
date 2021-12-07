using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    /// <summary>
    /// https://adventofcode.com/2021/day/6
    /// </summary>
    internal class DaySix
    {
        private readonly long[] days = new long[9];

        public DaySix()
        {
            foreach(var fish in File.ReadAllLines("Data/day6")[0].Split(',').Select(x => int.Parse(x)))
            {
                days[fish]++;
            }
        }

        /// <summary>
        /// simulates fish reproduction
        /// </summary>
        /// <returns>result in advent of code format</returns>
        internal long Calc(int c)
        {
            for (int d = 1; d <= c; d++)
            {
                Console.WriteLine(d);

                long x0 = days[0];

                days[0] = days[1];
                days[1] = days[2];
                days[2] = days[3];
                days[3] = days[4];
                days[4] = days[5];
                days[5] = days[6];
                days[6] = days[7] + x0;
                days[7] = days[8];
                days[8] = x0;
            }

            long count = 0;
            for (int i = 0; i < 9; i++)
            {
                count += days[i];
            }
            return count;
        }
    }
}
