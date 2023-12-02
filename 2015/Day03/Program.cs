using System.Diagnostics;
using Utilities;

namespace Day03
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
             * --- Day 3: Perfectly Spherical Houses in a Vacuum ---
                Santa is delivering presents to an infinite two-dimensional grid of houses.

                He begins by delivering a present to the house at his starting location, and then an elf at the North Pole calls him via radio and tells him where to move next. Moves are always exactly one house to the north (^), south (v), east (>), or west (<). After each move, he delivers another present to the house at his new location.

                However, the elf back at the north pole has had a little too much eggnog, and so his directions are a little off, and Santa ends up visiting some houses more than once. How many houses receive at least one present?

                For example:

                > delivers presents to 2 houses: one at the starting location, and one to the east.
                ^>v< delivers presents to 4 houses in a square, including twice to the house at his starting/ending location.
                ^v^v^v^v^v delivers a bunch of presents to some very lucky children at only 2 houses.
             */

            var stopwatch = new Stopwatch();
            string[] rows = AoCUtils.GetInput(stopwatch, day: 3, puzzle: 1, title: "Perfectly Spherical Houses in a Vacuum");

            var houseCoordsWithPresents = new List<Tuple<int, int>> { new Tuple<int, int>(0, 0) };
            int currentX = 0, currentY = 0;

            foreach (char arrow in rows[0])
            {
                switch (arrow)
                {
                    case '^':
                        currentY++; break;
                    case 'v':
                        currentY--; break;
                    case '>':
                        currentX++; break;
                    case '<':
                        currentX--; break;
                }

                if (!houseCoordsWithPresents.Contains(new Tuple<int, int>(currentX, currentY)))
                    houseCoordsWithPresents.Add(new Tuple<int, int>(currentX, currentY));
            }

            AoCUtils.WriteOutput(stopwatch, houseCoordsWithPresents.Count.ToString());

            /*
             * --- Part Two ---
                The next year, to speed up the process, Santa creates a robot version of himself, Robo-Santa, to deliver presents with him.

                Santa and Robo-Santa start at the same location (delivering two presents to the same starting house), then take turns moving based on instructions from the elf, who is eggnoggedly reading from the same script as the previous year.

                This year, how many houses receive at least one present?

                For example:

                ^v delivers presents to 3 houses, because Santa goes north, and then Robo-Santa goes south.
                ^>v< now delivers presents to 3 houses, and Santa and Robo-Santa end up back where they started.
                ^v^v^v^v^v now delivers presents to 11 houses, with Santa going one direction and Robo-Santa going the other.
             */

            rows = AoCUtils.GetInput(stopwatch, day: 3, puzzle: 2, title: "Perfectly Spherical Houses in a Vacuum");
            currentX = 0; currentY = 0;
            int currentRoboX = 0, currentRoboY = 0;

            houseCoordsWithPresents = new List<Tuple<int, int>> { new Tuple<int, int>(0, 0) };
            bool roboSanta = false;

            foreach (char arrow in rows[0])
            {
                switch (arrow)
                {
                    case '^':
                        if (roboSanta) currentRoboY++;
                            else currentY++; 
                        break;
                    case 'v':
                        if (roboSanta) currentRoboY--;
                            else currentY--;
                        break;
                    case '>':
                        if (roboSanta) currentRoboX++;
                            else currentX++;
                        break;
                    case '<':
                        if (roboSanta) currentRoboX--;
                            else currentX--;
                        break;
                }

                if (!houseCoordsWithPresents.Contains(new Tuple<int, int>
                    (roboSanta ? currentRoboX : currentX, roboSanta ? currentRoboY : currentY)))
                {
                    houseCoordsWithPresents.Add(new Tuple<int, int>
                        (roboSanta ? currentRoboX : currentX, roboSanta ? currentRoboY : currentY));
                }

                roboSanta = !roboSanta;
            }

            AoCUtils.WriteOutput(stopwatch, houseCoordsWithPresents.Count.ToString());
        }
    }
}