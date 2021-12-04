using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    /// <summary>
    /// https://adventofcode.com/2021/day/4
    /// </summary>
    internal class DayFour
    {
        // bingo numbers
        private readonly int[] input = new int[] { 42, 32, 13, 22, 91, 2, 88, 85, 53, 87,
            37, 33, 76, 98, 89, 19, 69, 9, 62, 21, 38, 49, 54, 81, 0, 26, 79, 36, 57, 18,
            4, 40, 31, 80, 24, 64, 77, 97, 70, 6, 73, 23, 20, 47, 45, 51, 74, 25, 95, 96,
            58, 92, 94, 11, 39, 63, 65, 99, 48, 83, 29, 34, 44, 75, 55, 17, 14, 56, 8, 82,
            59, 52, 46, 90, 5, 41, 60, 67, 16, 1, 15, 61, 71, 66, 72, 30, 28, 3, 43, 27,
            78, 10, 86, 7, 50, 35, 84, 12, 93, 68 };

        private readonly Dictionary<int[,], bool[,]> boards = new();

        public DayFour()
        {
            List<string> inputLines = File.ReadAllLines("Data/day4").ToList();

            // parse input to 5x5 playing boards
            for (int i = 0; i < 100; i++)
            {
                var tmpBoard = new int[5, 5];

                for (int z = 0; z < 5; z++)
                    for (int j = 0; j < 15; j += 3)
                    {
                        string numberAsString = inputLines[i * 6 + z].Substring(j, 2);
                        int numberAsInt = int.Parse(numberAsString);
                        tmpBoard[z, j / 3] = numberAsInt;
                    }

                boards.Add(tmpBoard, new bool[5, 5]);
            }
        }

        /// <summary>
        /// calculate the last bingo winner
        /// </summary>
        /// <returns>result in advent of code format</returns>
        /// <exception cref="Exception"></exception>
        internal int Calc()
        {
            for (int i = 0; i < input.Length; i++)
            {
                Play(input[i]);

                var lastWinner = RemoveWinners();

                // if the list of boards is empty, we found the last winner board
                if (!boards.Any() && lastWinner != null)
                {
                    return CalcAdventOfCodeResut(lastWinner.Value, input[i]);
                }
            }
            throw new Exception("Invalid input");
        }

        internal static int CalcAdventOfCodeResut(KeyValuePair<int[,], bool[,]> board, int lastAction)
        {
            int sum = 0;

            for (int z = 0; z < 5; z++)
                for (int u = 0; u < 5; u++)
                {
                    if (!board.Value[z, u])
                    {
                        sum += board.Key[z, u];
                    }
                }

            return sum * lastAction;
        }

        /// <summary>
        /// execute one play move
        /// </summary>
        /// <param name="number"></param>
        internal void Play(int number)
        {
            // mark matches on bingo boards
            foreach (var board in boards)
            {
                for (int z = 0; z < 5; z++)
                    for (int j = 0; j < 5; j++)
                    {
                        if (board.Key[z, j] == number)
                        {
                            board.Value[z, j] = true;
                        }
                    }
            }
        }

        /// <summary>
        /// removes winners
        /// </summary>
        /// <returns>last winner of round</returns>
        internal KeyValuePair<int[,], bool[,]>? RemoveWinners()
        {
            KeyValuePair<int[,], bool[,]>? lastWinner = null;

            //check for winners
            foreach (var board in boards)
            {
                for (int j = 0; j < 5; j++)
                {
                    if ((board.Value[j, 0] && board.Value[j, 1] && board.Value[j, 2] && board.Value[j, 3] && board.Value[j, 4]) ||
                        (board.Value[0, j] && board.Value[1, j] && board.Value[2, j] && board.Value[3, j] && board.Value[4, j]))
                    {
                        // bingo! remove winner from game.
                        boards.Remove(board.Key);
                        lastWinner = board;
                    }
                }
            }

            return lastWinner;
        }
    }
}
